using System;
using Microsoft.VisualStudio.ProjectSystem.VS.Properties;

namespace VSPropertyPages
{
    public class PropertyPageMetadata : IPageMetadata
    {
        public string Name { get; }

        public Guid PageGuid { get; }

        public int PageOrder { get; }

        public bool HasConfigurationCondition { get; }

        public PropertyPageMetadata(string name, string pageGuid, int pageOrder, bool hasConfigurationCondition)
            : this(name, Guid.Parse(pageGuid), pageOrder, hasConfigurationCondition)
        {
        }

        public PropertyPageMetadata(string name, Guid pageGuid, int pageOrder, bool hasConfigurationCondition)
        {
            Name = name;
            PageGuid = pageGuid;
            PageOrder = pageOrder;
            HasConfigurationCondition = hasConfigurationCondition;
        }
    }
}
