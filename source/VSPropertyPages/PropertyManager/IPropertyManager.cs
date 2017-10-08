using System;
using System.Threading.Tasks;

namespace VSPropertyPages
{
    public interface IPropertyManager
    {
        event EventHandler<ProjectPropertyChangingEventArgs> PropertyChanging;
        event EventHandler<ProjectPropertyChangedEventArgs> PropertyChanged;

        Task<string> GetPropertyAsync(string propertyName);
        Task SetPropertyAsync(string propertyName, string value);

        Task<bool> IsDirtyAsync();
        Task<bool> ApplyAsync();
    }
}
