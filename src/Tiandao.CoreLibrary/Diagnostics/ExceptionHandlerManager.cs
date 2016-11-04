using System;
using System.Collections.Generic;

namespace Tiandao.Diagnostics
{
	/// <summary>
	/// 提供了异常处理的管理功能，是异常处理的入口。
	/// </summary>
	public sealed class ExceptionHandlerManager
	{
		#region 常量定义

		public const int UnknownError = -1;
		public const int UnhandledError = 100;

		#endregion

		#region 事件定义

		public event EventHandler<FailureHandleEventArgs> Error;
		public event EventHandler<ExceptionHandlingEventArgs> Handling;
		public event EventHandler<ExceptionHandledEventArgs> Handled;

		#endregion

		#region 静态字段

		public static readonly ExceptionHandlerManager Instance = new ExceptionHandlerManager();

		#endregion

		#region 私有字段

		private readonly List<IExceptionHandler> _handlers;

		#endregion

		#region 构造方法

		private ExceptionHandlerManager()
		{
			_handlers = new List<IExceptionHandler>();
		}

		#endregion

		#region 公共属性

		/// <summary>
		/// 当前所支持的异常处理程序集合。
		/// </summary>
		public IList<IExceptionHandler> Handlers
		{
			get
			{
				return _handlers;
			}
		}

		#endregion

		#region 公共方法

		/// <summary>
		/// 从内部的异常处理程序树中查找最接近于指定异常对象的处理程序，然后指派该处理程序处理指定的异常对象。
		/// </summary>
		/// <param name="exception">要处理的指定异常对象。</param>
		/// <returns>处理成功则返回真(True)，否则返回假(False)。</returns>
		/// <exception cref="ExceptionHandlingException">在异常处理程序树中没有找到合适的异常处理程序。</exception>
		/// <remarks>如果参数<paramref name="exception"/>为空引用（在 Visual Basic 中为 Nothing），则返回真(True)。</remarks>
		public bool Handle(Exception exception, object context)
		{
			if(exception == null)
				return true;

			//声明“Error”事件参数对象
			FailureHandleEventArgs args = null;

			try
			{
				foreach(IExceptionHandler handler in _handlers)
				{
					bool canHandle = handler.CanHandle(exception.GetType());

					if(canHandle)
					{
						//构建“Handling”事件参数对象
						ExceptionHandlingEventArgs handlingArgs = new ExceptionHandlingEventArgs(handler, exception);

						//激发“Handling”事件
						this.OnHandling(handlingArgs);

						//定义异常处理器处理的后续异常对象
						Exception continuedException = null;

						//如果事件处理参数不取消后续处理则执行处理器的异常处理方法
						if(!handlingArgs.Cancel)
						{
							continuedException = handler.Handle(exception, context);
						}

						//激发“Handled”事件
						this.OnHandled(new ExceptionHandledEventArgs(handler, exception));

						//返回处理成功
						if(continuedException == null)
						{
							return true;
						}
						else
						{
							exception = continuedException;
						}
					}
				}
			}
			catch(Exception ex)
			{
				//构建“Error”事件参数对象
				args = new FailureHandleEventArgs(ex, UnknownError);
			}

			//构建“Error”事件参数对象
			if(args == null)
			{
				args = new FailureHandleEventArgs(exception, UnhandledError);
			}

			//激发“Error”事件
			this.OnError(args);

			//返回事件参数对象的“Handled”属性值
			return args.Handled;
		}

		#endregion

		#region 激发事件

		private void OnError(FailureHandleEventArgs e)
		{
			if(this.Error != null)
			{
				this.Error(this, e);
			}
		}

		private void OnHandled(ExceptionHandledEventArgs e)
		{
			if(this.Handled != null)
			{
				this.Handled(this, e);
			}
		}

		private void OnHandling(ExceptionHandlingEventArgs e)
		{
			if(this.Handling != null)
			{
				this.Handling(this, e);
			}
		}

		#endregion
	}
}
