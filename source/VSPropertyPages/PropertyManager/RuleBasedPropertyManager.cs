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
        private IRule _rule;

        public RuleBasedPropertyManager(UnconfiguredProject unconfiguredProject, IRule rule)
        {
            _unconfiguredProject = unconfiguredProject;
            _rule = rule;
        }

        public Task UpdateConfigurationsAsync(IReadOnlyCollection<ConfiguredProject> configuredProjects) => Task.CompletedTask;

        public async Task<string> GetPropertyAsync(string propertyName) =>
            await _rule.GetPropertyValueAsync(propertyName);

        public async Task SetPropertyAsync(string propertyName, string value)
        {
            PropertyChanging?.Invoke(this, new ProjectPropertyChangingEventArgs(propertyName));

            var oldValue = await _rule.GetPropertyValueAsync(propertyName);

            if (!String.Equals(oldValue, value, StringComparison.Ordinal))
            {
                await _rule.GetProperty(propertyName)?.SetValueAsync(value);
                PropertyChanged?.Invoke(this, new ProjectPropertyChangedEventArgs(propertyName, oldValue, value));
            }
        }

        public async Task<string> GetPathPropertyAsync(string propertyName, bool isRelative)
        {
            var value = await GetPropertyAsync(propertyName);
            return isRelative ? _unconfiguredProject.MakeRelative(value) : _unconfiguredProject.MakeRooted(value);
        }

        public Task SetPathPropertyAsync(string propertyName, string value, bool isRelative) =>
            isRelative ? SetPropertyAsync(propertyName, _unconfiguredProject.MakeRelative(value))
            : SetPropertyAsync(propertyName, _unconfiguredProject.MakeRooted(value));

        public Task<bool> IsDirtyAsync() => _unconfiguredProject.GetIsDirtyAsync();

        public async Task<bool> ApplyAsync()
        {
            await _unconfiguredProject.SaveAsync();
            await _unconfiguredProject.SaveUserFileAsync();

            return true;
        }
    }
}
