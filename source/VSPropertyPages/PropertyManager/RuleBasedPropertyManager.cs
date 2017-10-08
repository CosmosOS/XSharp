using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.ProjectSystem.Properties;

namespace VSPropertyPages
{
    public class RuleBasedPropertyManager : IPropertyManager
    {
        private UnconfiguredProject _unconfiguredProject;
        private IRule _rule;

        public event EventHandler<ProjectPropertyChangingEventArgs> PropertyChanging;
        public event EventHandler<ProjectPropertyChangedEventArgs> PropertyChanged;

        public RuleBasedPropertyManager(UnconfiguredProject unconfiguredProject, IRule rule)
        {
            _unconfiguredProject = unconfiguredProject;
            _rule = rule;
        }

        public async Task<string> GetPropertyAsync(string propertyName) =>
            await _rule.GetPropertyValueAsync(propertyName);

        public async Task SetPropertyAsync(string propertyName, string value)
        {
            PropertyChanging?.Invoke(this, new ProjectPropertyChangingEventArgs(propertyName));

            var oldValue = await _rule.GetPropertyValueAsync(propertyName);

            if (oldValue != value)
            {
                await _rule.GetProperty(propertyName)?.SetValueAsync(value);
                PropertyChanged?.Invoke(this, new ProjectPropertyChangedEventArgs(propertyName, oldValue, value));
            }
        }

        public async Task<bool> IsDirtyAsync() => await _unconfiguredProject.GetIsDirtyAsync();

        public async Task<bool> ApplyAsync()
        {
            await _unconfiguredProject.SaveAsync();
            await _unconfiguredProject.SaveUserFileAsync();

            return true;
        }
    }
}
