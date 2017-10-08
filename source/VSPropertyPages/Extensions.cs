using Microsoft.VisualStudio;

namespace VSPropertyPages
{
    internal static class Extensions
    {
        public static int ToVSConstant(this bool value) => value ? VSConstants.S_OK : VSConstants.S_FALSE;
        public static bool ToBoolean(this int value) => value == 1;
        public static ProjectPropertyDescriptor ToProjectPropertyDescriptor(this string propertyName) =>
            new ProjectPropertyDescriptor(propertyName);
    }
}
