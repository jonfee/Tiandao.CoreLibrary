using System;
using System.Collections.Generic;

namespace Tiandao.Services.Composition
{
    public class ExecutionHandler : ExecutionHandlerBase
	{
		#region 成员字段

		private ICommand _command;

		#endregion

		#region 公共属性

		public ICommand Command
		{
			get
			{
				return _command;
			}
			set
			{
				if(value == null)
					throw new ArgumentNullException();

				_command = value;
			}
		}

		#endregion

		#region 构造方法

		public ExecutionHandler()
		{

		}

		public ExecutionHandler(ICommand command)
		{
			if(command == null)
				throw new ArgumentNullException("command");

			_command = command;
			this.Name = command.Name;
		}

		#endregion

		#region 重写方法

		public override bool CanHandle(IExecutionPipelineContext context)
		{
			if(_command == null)
				return false;

			return base.CanHandle(context) && _command.CanExecute(context);
		}

		protected override void OnExecute(IExecutionPipelineContext context)
		{
			if(_command != null)
				context.Result = _command.Execute(context);
		}

		#endregion
	}
}
