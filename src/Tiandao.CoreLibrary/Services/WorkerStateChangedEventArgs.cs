using System;
using System.Collections.Generic;

namespace Tiandao.Services
{
    public class WorkerStateChangedEventArgs : EventArgs
	{	
		#region 私有字段

		private string _actionName;
		private WorkerState _state;
		private Exception _exception;

		#endregion

		#region 公共属性

		public string ActionName
		{
			get
			{
				return _actionName;
			}
		}

		public WorkerState State
		{
			get
			{
				return _state;
			}
		}

		public Exception Exception
		{
			get
			{
				return _exception;
			}
		}

		#endregion

		#region 构造方法

		public WorkerStateChangedEventArgs(string actionName, WorkerState state) : this(actionName, state, null)
		{
		}

		public WorkerStateChangedEventArgs(string actionName, WorkerState state, Exception exception)
		{
			if(string.IsNullOrWhiteSpace(actionName))
				throw new ArgumentNullException("actionName");

			_actionName = actionName.Trim();
			_state = state;
			_exception = exception;
		}

		#endregion
    }
}