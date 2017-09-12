using System;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.Threading;

namespace XSharp.ProjectSystem.VS.PropertyPages
{
    public abstract class PropertyPageViewModel : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private PropertyManager mPropertyManager;

        public PropertyPageViewModel(UnconfiguredProject aUnconfiguredProject)
        {
            mPropertyManager = new PropertyManager(aUnconfiguredProject);
        }

        private void OnPropertyChanged(string aPropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(aPropertyName));
        }

        private void OnPropertyChanged<T>(T aPropertyValue, T aValue, string aPropertyName)
        {
            if (!aPropertyValue.Equals(aValue))
            {
                OnPropertyChanged(aPropertyName);
            }
        }

        public bool PropertiesChanged => (mPropertyManager?.PropertiesChanged).GetValueOrDefault(false);

        public void Dispose() => mPropertyManager.Dispose();

        protected string GetProperty(string aPropertyName) => mPropertyManager.GetProperty(aPropertyName);

        protected void SetProperty(string aPropertyName, string aValue)
        {
            if (mPropertyManager.GetProperty(aPropertyName) != aValue)
            {
                mPropertyManager.SetProperty(aPropertyName, aValue);
                OnPropertyChanged(aPropertyName);
            }
        }

        public ImmutableDictionary<string, string> GetProperties() => mPropertyManager.GetProperties();

        public Task ApplyAsync() => mPropertyManager.ApplyChangesAsync();
    }
}
