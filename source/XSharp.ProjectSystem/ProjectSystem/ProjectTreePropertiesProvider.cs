using System.ComponentModel.Composition;

using Microsoft.VisualStudio.ProjectSystem;

namespace XSharp.ProjectSystem
{
    [Export(typeof(IProjectTreePropertiesProvider))]
    [AppliesTo(ProjectCapability.XSharp)]
    internal class ProjectTreePropertiesProvider : IProjectTreePropertiesProvider
    {
        public void CalculatePropertyValues(IProjectTreeCustomizablePropertyContext propertyContext, IProjectTreeCustomizablePropertyValues propertyValues)
        {
            if (propertyValues.Flags.Contains(ProjectTreeFlags.Common.ProjectRoot))
            {
                propertyValues.Icon = XSharpImagesMonikers.ProjectTreeIcon.ToProjectSystemType();
            }
        }
    }
}
