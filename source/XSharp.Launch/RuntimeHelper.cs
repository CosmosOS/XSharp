#if NETCOREAPP2_0
using System.Runtime.InteropServices;
#endif

namespace XSharp.Launch
{
    internal static class RuntimeHelper
    {
        public static bool IsWindows
        {
            get
            {
#if NETCOREAPP2_0
                return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
#else
                return true;
#endif
            }
        }

        public static bool IsOSX
        {
            get
            {
#if NETCOREAPP2_0
                return RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
#else
                return false;
#endif
            }
        }

        public static bool IsLinux
        {
            get
            {
#if NETCOREAPP2_0
                return RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
#else
                return false;
#endif
            }
        }
    }
}
