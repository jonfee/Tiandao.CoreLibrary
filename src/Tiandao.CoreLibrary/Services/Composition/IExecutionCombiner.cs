using System;
using System.Collections.Generic;

namespace Tiandao.Services.Composition
{
    public interface IExecutionCombiner
    {
		object Combine(IEnumerable<IExecutionPipelineContext> contexts);
	}
}