using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.Threading;

namespace XSharp.ProjectSystem.VS.PropertyPages
{
    // READING PROJECT PROPERTIES
    // To read the project properties, a project subscription should be used, as the user may edit the project
    // file while the property page is open, and the property page will be notified, so the values can be updated.
    // Project subscriptions: https://github.com/Microsoft/VSProjectSystem/blob/master/doc/automation/subscribe_to_project_data.md
    // The example shows how to subscribe to rule changes, but we subscribe to project changes,
    // using IProjectSubscriptionService.ProjectSource
    //
    // WRITING PROJECT PROPERTIES
    // To apply project property changes, a write lock should be acquired, and the project acquired through the lock
    // shouldn't be used outside the lock.
    // Acquiring a lock for writing: https://github.com/Microsoft/VSProjectSystem/blob/master/doc/automation/obtaining_the_MSBuild.Project_from_CPS.md
    //
    internal class PropertyManager : IDisposable
    {
        private UnconfiguredProject mUnconfiguredProject;
        private IProjectLockService mProjectLockService;
        private IDisposable mSubscriptionDisposable;

        private ImmutableDictionary<string, string> mProjectFileProperties;
        private Dictionary<string, string> mProperties;

        public PropertyManager(UnconfiguredProject aUnconfiguredProject)
        {
            mProperties = new Dictionary<string, string>();

            mUnconfiguredProject = aUnconfiguredProject;
            mProjectLockService = mUnconfiguredProject.ProjectService.Services.ProjectLockService;

            var xSubscriptionService = mUnconfiguredProject.Services.ActiveConfiguredProjectSubscription;
            var xReceivingBlock = new ActionBlock<IProjectVersionedValue<IProjectSnapshot>>(ProjectUpdateAsync);
            mSubscriptionDisposable = xSubscriptionService.ProjectSource.SourceBlock.LinkTo(
                xReceivingBlock, new DataflowLinkOptions() { PropagateCompletion = true });

            // wait before binds
            while (mProjectFileProperties == null) ;
        }

        private Task ProjectUpdateAsync(IProjectVersionedValue<IProjectSnapshot> aUpdate)
        {
            var xOldDefaultProperties = mProjectFileProperties;

            var xProjectInstance = aUpdate.Value.ProjectInstance;
            mProjectFileProperties = xProjectInstance.Properties.ToImmutableDictionary(p => p.Name, p => p.EvaluatedValue);
            
            if (xOldDefaultProperties != null)
            {
                foreach (var xPropertyName in mProperties.Keys.ToList())
                {
                    if (xOldDefaultProperties.ContainsKey(xPropertyName))
                    {
                        var xOldValue = xOldDefaultProperties[xPropertyName];
                        var xNewValue = mProjectFileProperties[xPropertyName];

                        if (xOldValue == mProperties[xPropertyName] && xOldValue != xNewValue)
                        {
                            // value wasn't changed in the property page, but it was updated in the project file
                            mProperties[xPropertyName] = xNewValue;
                        }
                    }
                }
            }

            return TplExtensions.CompletedTask;
        }

        private bool PropertyChanged(KeyValuePair<string, string> aProperty)
        {
            if (mProjectFileProperties.TryGetValue(aProperty.Key, out var xDefaultValue))
            {
                if (xDefaultValue == aProperty.Value)
                {
                    return false;
                }
            }
            else
            {
                if (String.IsNullOrEmpty(aProperty.Value))
                {
                    return false;
                }
            }

            return true;
        }
        
        public bool PropertiesChanged
        {
            get
            {
                if (mProjectFileProperties != null)
                {
                    foreach (var xProperty in mProperties)
                    {
                        if (PropertyChanged(xProperty))
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }

        public void Dispose()
        {
            mSubscriptionDisposable.Dispose();
        }

        public string GetProperty(string aPropertyName)
        {
            if (!mProperties.TryGetValue(aPropertyName, out var xValue))
            {
                mProjectFileProperties.TryGetValue(aPropertyName, out xValue);
            }

            return xValue ?? String.Empty;
        }

        public string GetPathProperty(string aPropertyName) => mUnconfiguredProject.MakeRooted(GetProperty(aPropertyName));

        public void SetProperty(string aPropertyName, string aValue)
        {
            mProperties[aPropertyName] = aValue; 
        }

        public void SetPathProperty(string aPropertyName, string aValue) => SetProperty(aPropertyName, mUnconfiguredProject.MakeRelative(aValue));

        public ImmutableDictionary<string, string> GetProperties()
        {
            return mProperties.ToImmutableDictionary();
        }

        public async Task ApplyChangesAsync()
        {
            var xConfiguredProject = await mUnconfiguredProject.GetSuggestedConfiguredProjectAsync();

            using (var xAccess = await mProjectLockService.WriteLockAsync())
            {
                var xProject = await xAccess.GetProjectAsync(xConfiguredProject);
                await xAccess.CheckoutAsync(xConfiguredProject.UnconfiguredProject.FullPath);

                foreach (var xProperty in mProperties)
                {
                    if (PropertyChanged(xProperty))
                    {
                        xProject.SetProperty(xProperty.Key, xProperty.Value);
                    }
                }

                xProject.Save();
            }
        }
    }
}
