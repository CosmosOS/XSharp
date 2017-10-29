using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;

using VSPropertyPages.Sample.PropertyPages;

namespace VSPropertyPages.Sample
{
    [Guid(PackageGuid)]
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [ProvideObject(typeof(WpfPropertyPage))]
    [ProvideObject(typeof(WinFormsPropertyPage))]
    public sealed class VSPropertyPagesSample : Package
    {
        /// <summary>
        /// The GUID for this package.
        /// </summary>
        public const string PackageGuid = "e137b1f2-b898-4df2-bde2-01ba35c08c46";
    }
}
