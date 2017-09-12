using System;
using Microsoft.VisualStudio.ProjectSystem.VS.Properties;

namespace XSharp.ProjectSystem.VS.PropertyPages
{
    internal class PropertyPageMetadata : IPageMetadata
    {
        public string Name
        {
            get;
        }

        public Guid PageGuid
        {
            get;
        }

        public int PageOrder
        {
            get;
        }

        public bool HasConfigurationCondition
        {
            get;
        }

        public PropertyPageMetadata(string aName, Guid aPageGuid, int aPageOrder, bool aHasConfigurationCondition)
        {
            if (aPageGuid == Guid.Empty)
            {
                throw new ArgumentException(null, nameof(aPageGuid));
            }

            Name = aName;
            PageGuid = aPageGuid;
            PageOrder = aPageOrder;
            HasConfigurationCondition = aHasConfigurationCondition;
        }
    }
}
