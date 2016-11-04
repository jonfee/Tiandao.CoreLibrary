using System;
using System.Collections.Generic;

namespace Tiandao.Options
{
	internal static class OptionUtility
    {
		internal static bool ResolveOptionPath(string fullPath, out string path, out string name)
		{
			path = string.Empty;
			name = string.Empty;

			if(string.IsNullOrWhiteSpace(fullPath))
				return false;

			fullPath = fullPath.Trim().Trim('/').Trim();

			if(string.IsNullOrWhiteSpace(fullPath))
				return false;

			var parts = fullPath.Split('/');

			if(parts.Length > 1)
				path = string.Join("/", parts, 0, parts.Length - 1);
			else
				path = "/";

			name = parts[parts.Length - 1].Trim();

			return true;
		}
	}
}