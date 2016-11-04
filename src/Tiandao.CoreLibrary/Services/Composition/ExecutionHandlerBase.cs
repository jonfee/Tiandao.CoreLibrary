﻿using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Tiandao.Services.Composition
{
#if !CORE_CLR
	public abstract class ExecutionHandlerBase : MarshalByRefObject, IExecutionHandler, INotifyPropertyChanged
#else
	public abstract class ExecutionHandlerBase : IExecutionHandler, INotifyPropertyChanged
#endif
	{
		#region 事件定义

		public event EventHandler EnabledChanged;
		public event PropertyChangedEventHandler PropertyChanged;
		public event EventHandler<ExecutionPipelineExecutedEventArgs> Executed;
		public event EventHandler<ExecutionPipelineExecutingEventArgs> Executing;

		#endregion

		#region 私有字段

		private string _name;
		private bool _enabled;
		private IPredication _predication;
		private ExecutionFilterCollection _filters;

		#endregion

		#region 公共属性

		/// <summary>
		/// 获取或设置处理程序的名称。
		/// </summary>
		public string Name
		{
			get
			{
				return _name;
			}
			protected set
			{
				if(string.IsNullOrWhiteSpace(value))
					throw new ArgumentNullException();

				if(string.Equals(_name, value.Trim(), StringComparison.Ordinal))
					return;

				_name = value.Trim();

				//激发“PropertyChanged”事件
				this.OnPropertyChanged("Name");
			}
		}

		/// <summary>
		/// 获取或设置当前处理程序的断言对象，该断言决定处理程序是否可用。
		/// </summary>
		public IPredication Predication
		{
			get
			{
				return _predication;
			}
			set
			{
				if(_predication == value)
					return;

				_predication = value;

				//激发“PropertyChanged”事件
				this.OnPropertyChanged("Predication");
			}
		}

		/// <summary>
		/// 获取或设置当前处理程序是否可用。
		/// </summary>
		public bool Enabled
		{
			get
			{
				return _enabled;
			}
			set
			{
				if(_enabled == value)
					return;

				_enabled = value;

				//激发“EnabledChanged”事件
				this.OnEnabledChanged(EventArgs.Empty);

				//激发“PropertyChanged”事件
				this.OnPropertyChanged("Enabled");
			}
		}

		public ExecutionFilterCollection Filters
		{
			get
			{
				if(_filters == null)
					System.Threading.Interlocked.CompareExchange(ref _filters, new ExecutionFilterCollection(), null);

				return _filters;
			}
		}

		#endregion

		#region 构造方法

		protected ExecutionHandlerBase()
		{
			_name = this.GetType().Name;
			_enabled = true;
		}

		protected ExecutionHandlerBase(string name)
		{
			if(string.IsNullOrWhiteSpace(name))
				throw new ArgumentNullException("name");

			_name = name.Trim();
			_enabled = true;
		}

		#endregion

		#region 公共方法

		public virtual bool CanHandle(IExecutionPipelineContext context)
		{
			//如果断言对象是空则返回是否可用变量的值
			if(_predication == null)
				return _enabled;

			//返回断言对象的断言测试的值
			return _enabled && _predication.Predicate(context);
		}

		public void Handle(IExecutionPipelineContext context)
		{
			//在执行之前首先判断是否可以执行
			if(!this.CanHandle(context))
				return;

			//创建“Executing”事件参数
			var executingArgs = new ExecutionPipelineExecutingEventArgs(context);

			//激发“Executing”事件
			this.OnExecuting(executingArgs);

			if(executingArgs.Cancel)
				return;

			//执行过滤器的前半截
			var filters = ExecutionUtility.InvokeFiltersExecuting(_filters, filter => filter.OnExecuting(context));

			//执行当前处理请求
			this.OnExecute(context);

			//执行过滤器的后半截
			ExecutionUtility.InvokeFiltersExecuted(filters, filter => filter.OnExecuted(context));

			//激发“Executed”事件
			this.OnExecuted(new ExecutionPipelineExecutedEventArgs(context));
		}

		#endregion

		#region 事件激发

		protected virtual void OnEnabledChanged(EventArgs e)
		{
			var enabledChanged = this.EnabledChanged;

			if(enabledChanged != null)
				enabledChanged(this, e);
		}

		protected virtual void OnExecuted(ExecutionPipelineExecutedEventArgs e)
		{
			var executed = this.Executed;

			if(executed != null)
				executed(this, e);
		}

		protected virtual void OnExecuting(ExecutionPipelineExecutingEventArgs e)
		{
			var executing = this.Executing;

			if(executing != null)
				executing(this, e);
		}

		protected void OnPropertyChanged(string propertyName)
		{
			this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}

		protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			var propertyChanged = this.PropertyChanged;

			if(propertyChanged != null)
				propertyChanged(this, e);
		}

		#endregion

		#region 抽象方法

		protected abstract void OnExecute(IExecutionPipelineContext context);

		#endregion
	}
}
