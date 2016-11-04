using System;
using System.Collections.Generic;

namespace Tiandao.Services.Composition
{
    public interface IExecutionPipelineSelector
    {
		IEnumerable<ExecutionPipeline> SelectPipelines(IExecutionContext context, IEnumerable<ExecutionPipeline> pipelines);
	}
}