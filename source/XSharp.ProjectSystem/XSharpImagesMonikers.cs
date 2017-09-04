using System;
using Microsoft.VisualStudio.Imaging.Interop;

namespace XSharp.ProjectSystem
{
    public static class XSharpImagesMonikers
    {
        private static readonly Guid ManifestGuid = new Guid("574acd99-2695-4bc8-80ba-3d69c894ab63");

        private const int ProjectIcon = 0;

        public static ImageMoniker ProjectIconImageMoniker
        {
            get
            {
                return new ImageMoniker { Guid = ManifestGuid, Id = ProjectIcon };
            }
        }
    }
}
