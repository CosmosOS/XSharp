using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.VisualStudio.ProjectSystem;

namespace VSPropertyPages
{
    public abstract class PropertyPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<ProjectPropertyChangedEventArgs> ProjectPropertyChanged;
        public event EventHandler<ProjectPropertyChangingEventArgs> ProjectPropertyChanging;

        private IPropertyManager _propertyManager;
        private IProjectThreadingService _projectThreadingService;

        protected PropertyPageViewModel(
            IPropertyManager propertyManager,
            IProjectThreadingService projectThreadingService)
        {
            _propertyManager = propertyManager;
            _projectThreadingService = projectThreadingService;

            _propertyManager.PropertyChanged += PropertyManager_PropertyChanged;
            _propertyManager.PropertyChanging += PropertyManager_PropertyChanging;
            _propertyManager.ConfigurationsChanged += PropertyManager_ConfigurationsChanged;
        }

        public Task<string> GetPropertyAsync(string propertyName) =>
            _propertyManager.GetPropertyAsync(propertyName);

        public Task<string> GetPathPropertyAsync(string propertyName, bool isRelative) =>
            _propertyManager.GetPathPropertyAsync(propertyName, isRelative);

        /// <summary>
        /// Sets a project property.
        /// <para />
        /// If no <paramref name="changedProperties"/> are specified, all properties will be considered as changed.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        /// <param name="value">The property value.</param>
        /// <param name="changedProperties">The names of properties that may change after setting the project property.</param>
        /// <returns>An awaitable <seealso cref="Task"/>.</returns>
        public async Task SetPropertyAsync(string propertyName, string value, params string[] changedProperties)
        {
            await _propertyManager.SetPropertyAsync(propertyName, value);
            OnPropertyChanged(changedProperties);
        }

        /// <summary>
        /// Sets a project property.
        /// <para />
        /// If no <paramref name="changedProperties"/> are specified, all properties will be considered as changed.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        /// <param name="value">The property value.</param>
        /// <param name="changedProperties">The names of properties that may change after setting the project property.</param>
        /// <returns>An awaitable <seealso cref="Task"/>.</returns>
        public async Task SetPathPropertyAsync(string propertyName, string value, bool isRelative, params string[] changedProperties)
        {
            await _propertyManager.SetPathPropertyAsync(propertyName, value, isRelative);
            OnPropertyChanged(changedProperties);
        }

        public Task<bool> IsDirtyAsync() => _propertyManager.IsDirtyAsync();
        public Task<bool> ApplyAsync() => _propertyManager.ApplyAsync();

        protected string GetProperty(string propertyName) =>
            WaitForAsync(() => GetPropertyAsync(propertyName));

        protected string GetPathProperty(string propertyName, bool isRelative) =>
            WaitForAsync(() => GetPathPropertyAsync(propertyName, isRelative));

        /// <summary>
        /// If no <paramref name="changedProperties"/> are specified, all properties will be considered as changed.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        /// <param name="value">The property value.</param>
        /// <param name="changedProperties">The names of properties that may change after setting the project property.</param>
        /// <returns>An awaitable <seealso cref="Task"/>.</returns>
        protected void SetProperty(string propertyName, string value, params string[] changedProperties) =>
            WaitForAsync(() => SetPropertyAsync(propertyName, value, changedProperties));

        protected void SetPathProperty(string propertyName, string value, bool isRelative, params string[] changedProperties) =>
            WaitForAsync(() => SetPathPropertyAsync(propertyName, value, isRelative, changedProperties));

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void WaitForAsync(Func<Task> asyncFunc) => _projectThreadingService.ExecuteSynchronously(asyncFunc);

        private T WaitForAsync<T>(Func<Task<T>> asyncFunc) => _projectThreadingService.ExecuteSynchronously(asyncFunc);

        private void OnPropertyChanged(params string[] propertyNames)
        {
            if (PropertyChanged != null)
            {
                if (propertyNames.Length > 0)
                {
                    foreach (var property in propertyNames)
                    {
                        OnPropertyChanged(property);
                    }
                }
                else
                {
                    OnPropertyChanged(String.Empty);
                }
            }
        }

        private void PropertyManager_PropertyChanged(object sender, ProjectPropertyChangedEventArgs e) =>
            ProjectPropertyChanged?.Invoke(this, e);

        private void PropertyManager_PropertyChanging(object sender, ProjectPropertyChangingEventArgs e) =>
            ProjectPropertyChanging?.Invoke(this, e);

        private void PropertyManager_ConfigurationsChanged(object sender, EventArgs e) => OnPropertyChanged(String.Empty);
    }
}
