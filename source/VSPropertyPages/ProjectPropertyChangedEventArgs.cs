using System;
using System.ComponentModel;

namespace VSPropertyPages
{
    public class ProjectPropertyChangedEventArgs : EventArgs
    {
        public string PropertyName { get; }
        public string OldValue { get; }
        public string NewValue { get; }

        public ProjectPropertyChangedEventArgs(string propertyName, string oldValue, string newValue)
        {
            PropertyName = propertyName;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
