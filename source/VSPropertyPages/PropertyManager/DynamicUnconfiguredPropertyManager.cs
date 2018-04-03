using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.ProjectSystem.Properties;

namespace VSPropertyPages
{
    public class DynamicUnconfiguredPropertyManager : IPropertyManager
    {
        public event EventHandler<ProjectPropertyChangingEventArgs> PropertyChanging;
        public event EventHandler<ProjectPropertyChangedEventArgs> PropertyChanged;
        public event EventHandler ConfigurationsChanged;

        private UnconfiguredProject _unconfiguredProject;
        private IProjectThreadingService _projectThreadingService;

        private IProjectPropertiesProvider _projectPropertiesProvider;

        public DynamicUnconfiguredPropertyManager(
            UnconfiguredProject unconfiguredProject,
            IProjectPropertiesProvider projectPropertiesProvider)
        {
            _unconfiguredProject = unconfiguredProject ?? throw new ArgumentNullException(nameof(unconfiguredProject));
            _projectThreadingService = _unconfiguredProject.ProjectService.Services.ThreadingPolicy;

            _projectPropertiesProvider = projectPropertiesProvider;
        }

        public Task UpdateConfigurationsAsync(IReadOnlyCollection<ConfiguredProject> configuredProjects) => Task.CompletedTask;

        public Task<string> GetPropertyAsync(string propertyName) =>
            _projectPropertiesProvider.GetCommonProperties().GetEvaluatedPropertyValueAsync(propertyName);

        public async Task<string> GetPathPropertyAsync(string propertyName, bool isRelative)
        {
            var path = await GetPropertyAsync(propertyName).ConfigureAwait(false);
            return isRelative ? _unconfiguredProject.MakeRelative(path) : _unconfiguredProject.MakeRooted(path);
        }

        public async Task SetPropertyAsync(string propertyName, string value)
        {
            await _projectThreadingService.SwitchToUIThread();
            PropertyChanging?.Invoke(this, new ProjectPropertyChangingEventArgs(propertyName));

            var oldValue = await GetPropertyAsync(propertyName).ConfigureAwait(false);

            var properties = _projectPropertiesProvider.GetCommonProperties();
            await properties.SetPropertyValueAsync(propertyName, value).ConfigureAwait(false);

            await _projectThreadingService.SwitchToUIThread();
            PropertyChanged?.Invoke(this, new ProjectPropertyChangedEventArgs(propertyName, oldValue, value));
        }

        public Task SetPathPropertyAsync(string propertyName, string value, bool isRelative) =>
            SetPropertyAsync(propertyName, isRelative ? _unconfiguredProject.MakeRelative(value) : _unconfiguredProject.MakeRooted(value));

        public Task<bool> IsDirtyAsync() => _unconfiguredProject.GetIsDirtyAsync();

        public async Task<bool> ApplyAsync()
        {
            await _unconfiguredProject.SaveAsync().ConfigureAwait(false);
            return true;
        }
    }
}
