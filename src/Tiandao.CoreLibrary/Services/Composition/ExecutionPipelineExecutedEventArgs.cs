using System;
using System.Collections.Generic;

namespace Tiandao.Services.Composition
{
    public class ExecutionPipelineExecutedEventArgs : ExecutionEventArgs<IExecutionPipelineContext>
	{
		public ExecutionPipelineExecutedEventArgs(IExecutionPipelineContext context) : base(context)
		{

		}
	}
}