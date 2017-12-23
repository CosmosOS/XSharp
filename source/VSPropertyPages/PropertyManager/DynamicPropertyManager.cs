using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.Threading;

namespace VSPropertyPages
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
    public class DynamicPropertyManager : IPropertyManager
    {
        private UnconfiguredProject _unconfiguredProject;
        private IProjectLockService _projectLockService;

        private IDisposable _projectSubscriptionDisposable;

        private Dictionary<string, string> _persistedProperties;
        private Dictionary<string, string> _properties;

        private TaskCompletionSource<bool> _firstProjectUpdateCompletionSource = new TaskCompletionSource<bool>();

        public event EventHandler<ProjectPropertyChangingEventArgs> PropertyChanging;
        public event EventHandler<ProjectPropertyChangedEventArgs> PropertyChanged;

        public DynamicPropertyManager(UnconfiguredProject unconfiguredProject)
        {
            _unconfiguredProject = unconfiguredProject;
            _projectLockService = _unconfiguredProject.ProjectService.Services.ProjectLockService;

            _properties = new Dictionary<string, string>();

            var subscriptionService = _unconfiguredProject.Services.ActiveConfiguredProjectSubscription;
            var receivingBlock = new ActionBlock<IProjectVersionedValue<IProjectSnapshot>>(ProjectUpdateAsync);
            _projectSubscriptionDisposable = subscriptionService.ProjectSource.SourceBlock.LinkTo(
                receivingBlock, new DataflowLinkOptions() { PropagateCompletion = true });

            _unconfiguredProject.ProjectService.Services.ThreadingPolicy.ExecuteSynchronously(
                () => _firstProjectUpdateCompletionSource.Task);
        }

        private Task ProjectUpdateAsync(IProjectVersionedValue<IProjectSnapshot> update)
        {
            var project = update.Value.ProjectInstance;
            _persistedProperties = project.Properties.ToDictionary(p => p.Name, p => p.EvaluatedValue);

            _firstProjectUpdateCompletionSource.TrySetResult(true);

            return TplExtensions.CompletedTask;
        }

        public Task<string> GetPropertyAsync(string propertyName)
        {
            if (_properties.TryGetValue(propertyName, out var value))
            {
                return Task.FromResult(value);
            }

            if (_persistedProperties.TryGetValue(propertyName, out value))
            {
                return Task.FromResult(value);
            }

            return Task.FromResult(String.Empty);
        }

        public Task SetPropertyAsync(string propertyName, string value)
        {
            PropertyChanging?.Invoke(this, new ProjectPropertyChangingEventArgs(propertyName));

            var isPropertySet = _properties.TryGetValue(propertyName, out var oldValue);

            if (_persistedProperties.TryGetValue(propertyName, out var defaultValue))
            {
                if (String.Equals(defaultValue, value, StringComparison.Ordinal))
                {
                    if (isPropertySet)
                    {
                        _properties.Remove(propertyName);
                        PropertyChanged?.Invoke(this, new ProjectPropertyChangedEventArgs(
                            propertyName, oldValue, value));
                    }
                }
                else
                {
                    if (isPropertySet)
                    {
                        _properties[propertyName] = value;
                        PropertyChanged?.Invoke(this, new ProjectPropertyChangedEventArgs(
                            propertyName, oldValue, value));
                    }
                    else
                    {
                        _properties.Add(propertyName, value);
                        PropertyChanged?.Invoke(this, new ProjectPropertyChangedEventArgs(
                            propertyName, defaultValue, value));
                    }
                }
            }

            return Task.CompletedTask;
        }

        public async Task<string> GetPathPropertyAsync(string propertyName, bool isRelative)
        {
            var value = await GetPropertyAsync(propertyName);

            if (isRelative)
            {
                return _unconfiguredProject.MakeRelative(value);
            }
            else
            {
                return _unconfiguredProject.MakeRooted(value);
            }
        }

        public async Task SetPathPropertyAsync(string propertyName, string value, bool isRelative)
        {
            if (isRelative)
            {
                await SetPropertyAsync(propertyName, _unconfiguredProject.MakeRelative(value));
            }
            else
            {
                await SetPropertyAsync(propertyName, _unconfiguredProject.MakeRooted(value));
            }
        }

        public Task<bool> IsDirtyAsync()
        {
            foreach (var property in _properties)
            {
                if (!_persistedProperties.TryGetValue(property.Key, out var value)
                 || !String.Equals(property.Value, value, StringComparison.Ordinal))
                {
                    return TplExtensions.TrueTask;
                }
            }

            return TplExtensions.FalseTask;
        }

        public async Task<bool> ApplyAsync()
        {
            using (var projectWriteLock = await _projectLockService.WriteLockAsync())
            {
                var configuredProject = await _unconfiguredProject.GetSuggestedConfiguredProjectAsync();

                var project = await projectWriteLock.GetProjectAsync(configuredProject);
                await projectWriteLock.CheckoutAsync(_unconfiguredProject.FullPath);
                
                foreach (var property in _properties)
                {
                    project.SetProperty(property.Key, property.Value);
                }

                project.Save();
            }

            return true;
        }
    }
}
