using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.ProjectSystem.Properties;

namespace VSPropertyPages
{
    public class RuleBasedPropertyManager : IPropertyManager
    {
        public event EventHandler<ProjectPropertyChangingEventArgs> PropertyChanging;
        public event EventHandler<ProjectPropertyChangedEventArgs> PropertyChanged;
        public event EventHandler ConfigurationsChanged;

        private UnconfiguredProject _unconfiguredProject;
        private IProjectThreadingService _projectThreadingService;

        private IRule _rule;

        public RuleBasedPropertyManager(UnconfiguredProject unconfiguredProject, IRule rule)
        {
            _unconfiguredProject = unconfiguredProject ?? throw new ArgumentNullException(nameof(unconfiguredProject));
            _projectThreadingService = _unconfiguredProject.ProjectService.Services.ThreadingPolicy;

            _rule = rule;
        }

        public Task UpdateConfigurationsAsync(IReadOnlyCollection<ConfiguredProject> configuredProjects) => Task.CompletedTask;

        public async Task<string> GetPropertyAsync(string propertyName) =>
            await _rule.GetPropertyValueAsync(propertyName).ConfigureAwait(false);

        public async Task SetPropertyAsync(string propertyName, string value)
        {
            await _projectThreadingService.SwitchToUIThread();
            PropertyChanging?.Invoke(this, new ProjectPropertyChangingEventArgs(propertyName));

            var oldValue = await _rule.GetPropertyValueAsync(propertyName).ConfigureAwait(false);

            if (!String.Equals(oldValue, value, StringComparison.Ordinal))
            {
                await _rule.GetProperty(propertyName).SetValueAsync(value).ConfigureAwait(false);

                await _projectThreadingService.SwitchToUIThread();
                PropertyChanged?.Invoke(this, new ProjectPropertyChangedEventArgs(propertyName, oldValue, value));
            }
        }

        public async Task<string> GetPathPropertyAsync(string propertyName, bool isRelative)
        {
            var value = await GetPropertyAsync(propertyName).ConfigureAwait(false);
            return isRelative ? _unconfiguredProject.MakeRelative(value) : _unconfiguredProject.MakeRooted(value);
        }

        public Task SetPathPropertyAsync(string propertyName, string value, bool isRelative) =>
            isRelative ? SetPropertyAsync(propertyName, _unconfiguredProject.MakeRelative(value))
            : SetPropertyAsync(propertyName, _unconfiguredProject.MakeRooted(value));

        public Task<bool> IsDirtyAsync() => _unconfiguredProject.GetIsDirtyAsync();

        public async Task<bool> ApplyAsync()
        {
            await _unconfiguredProject.SaveAsync().ConfigureAwait(false);
            await _unconfiguredProject.SaveUserFileAsync().ConfigureAwait(false);

            return true;
        }
    }
}
