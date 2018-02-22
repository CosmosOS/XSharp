using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.ProjectSystem;

namespace VSPropertyPages
{
    public interface IPropertyManager
    {
        event EventHandler<ProjectPropertyChangingEventArgs> PropertyChanging;
        event EventHandler<ProjectPropertyChangedEventArgs> PropertyChanged;
        event EventHandler ConfigurationsChanged;

        Task UpdateConfigurationsAsync(IReadOnlyCollection<ConfiguredProject> configuredProjects);

        Task<string> GetPropertyAsync(string propertyName);
        Task SetPropertyAsync(string propertyName, string value);

        Task<string> GetPathPropertyAsync(string propertyName, bool isRelative);
        Task SetPathPropertyAsync(string propertyName, string value, bool isRelative);

        Task<bool> IsDirtyAsync();
        Task<bool> ApplyAsync();
    }
}
