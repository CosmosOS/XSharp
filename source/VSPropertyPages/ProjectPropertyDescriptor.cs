using System;
using System.ComponentModel;
using Microsoft;

namespace VSPropertyPages
{
    internal class ProjectPropertyDescriptor : PropertyDescriptor
    {
        public override Type ComponentType => null;

        public override bool IsReadOnly => false;

        public override Type PropertyType => typeof(string);

        public ProjectPropertyDescriptor(string propertyName)
            : base(propertyName, Array.Empty<Attribute>())
        {
        }

        public override bool CanResetValue(object component) => false;

        public override object GetValue(object component)
        {
            Assumes.Fail("This method should not get called!");
            return null;
        }

        public override void ResetValue(object component) =>
            Assumes.Fail("This method should not get called!");

        public override void SetValue(object component, object value) =>
            Assumes.Fail("This method should not get called!");

        public override bool ShouldSerializeValue(object component) => true;
    }
}
