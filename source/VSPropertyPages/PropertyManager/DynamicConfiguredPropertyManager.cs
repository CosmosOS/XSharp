using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.ProjectSystem;

namespace VSPropertyPages
{
    public class DynamicConfiguredPropertyManager : IPropertyManager
    {
        public event EventHandler<ProjectPropertyChangingEventArgs> PropertyChanging;
        public event EventHandler<ProjectPropertyChangedEventArgs> PropertyChanged;
        public event EventHandler ConfigurationsChanged;

        private UnconfiguredProject _unconfiguredProject;
        private IProjectThreadingService _projectThreadingService;

        private IReadOnlyCollection<ConfiguredProject> _configuredProjects;

        public DynamicConfiguredPropertyManager(
            UnconfiguredProject unconfiguredProject,
            IReadOnlyCollection<ConfiguredProject> configuredProjects)
        {
            _unconfiguredProject = unconfiguredProject ?? throw new ArgumentNullException(nameof(unconfiguredProject));
            _projectThreadingService = _unconfiguredProject.ProjectService.Services.ThreadingPolicy;

            _configuredProjects = configuredProjects ?? throw new ArgumentNullException(nameof(configuredProjects));

            if (configuredProjects.Count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(configuredProjects));
            }
        }

        public Task UpdateConfigurationsAsync(IReadOnlyCollection<ConfiguredProject> configuredProjects)
        {
            _configuredProjects = configuredProjects;
            ConfigurationsChanged?.Invoke(this, EventArgs.Empty);

            return Task.CompletedTask;
        }

        public Task<string> GetPropertyAsync(string propertyName) =>
            _configuredProjects.First().Services.ProjectPropertiesProvider.GetCommonProperties()
            .GetEvaluatedPropertyValueAsync(propertyName);

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

            foreach (var configuredProject in _configuredProjects)
            {
                var properties = configuredProject.Services.ProjectPropertiesProvider.GetCommonProperties();
                await properties.SetPropertyValueAsync(
                    propertyName, value, configuredProject.ProjectConfiguration.Dimensions).ConfigureAwait(false);
            }

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
