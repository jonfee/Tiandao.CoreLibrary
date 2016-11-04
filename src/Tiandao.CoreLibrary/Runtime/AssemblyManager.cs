using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#if CORE_CLR
using System.Runtime.Loader;
using Microsoft.Extensions.DependencyModel;
#endif

namespace Tiandao.Runtime
{
    public static class AssemblyManager
    {
		public static IEnumerable<AssemblyName> GetAssemblyNames()
		{
#if !CORE_CLR
			return AppDomain.CurrentDomain.GetAssemblies().Select(p => p.GetName());
#else
			return DependencyContext.Default.CompileLibraries.Select(library => new AssemblyName(library.Name));
#endif
		}

		public static Assembly Load(AssemblyName assemblyName)
		{
#if !CORE_CLR
			return AppDomain.CurrentDomain.Load(assemblyName);
#else
			return Assembly.Load(assemblyName);
#endif
		}

		public static Assembly LoadFrom(string assemblyFile)
		{
#if !CORE_CLR
			return Assembly.LoadFrom(assemblyFile);
#else
			return AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyFile);
#endif
		}
	}
}
