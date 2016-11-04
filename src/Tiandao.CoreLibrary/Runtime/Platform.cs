using System;

#if !CORE_CLR
#else
using System.Runtime.InteropServices;
#endif

namespace Tiandao.Runtime
{
    public static class Platform
    {
	    public static bool IsLinux
	    {
		    get
		    {
#if !CORE_CLR
			    return Environment.OSVersion.Platform == PlatformID.Unix;
#else
				return RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
#endif
		    }
	    }

	    public static bool IsMacOSX
		{
			get
			{
#if !CORE_CLR
				return Environment.OSVersion.Platform == PlatformID.MacOSX;
#else
				return RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
#endif
			}
		}

	    public static bool IsWindows
	    {
		    get
		    {
#if !CORE_CLR
				return Environment.OSVersion.Platform == PlatformID.Win32NT;
#else
				return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
#endif
			}
		}
    }
}
