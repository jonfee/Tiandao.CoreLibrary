using System;
using System.Collections.Generic;

namespace Tiandao.Diagnostics
{
	/// <summary>
	/// 定义了异常处理的通用功能。
	/// </summary>
	public interface IExceptionHandler
	{
		/// <summary>
		/// 判断当前异常处理器是否支持指定的异常类型。
		/// </summary>
		/// <param name="exceptionType">要判断的异常类型。</param>
		/// <returns>支持指定的异常类型则返回真(True)，否则返回假(False)。</returns>
		bool CanHandle(Type exceptionType);

		/// <summary>
		/// 处理指定的异常。
		/// </summary>
		/// <param name="exception">要处理的异常对象。</param>
		/// <param name="context">请求上下文信息。</param>
		/// <returns>如果当前处理器已经对参数<paramref name="exception"/>指定的异常对象，处理完毕则返回为空，如果当前异常处理器还需要后续的其他处理器对返回的新异常对象继续处理的话，则返回一个新异常。</returns>
		Exception Handle(Exception exception, object context);

		/// <summary>
		/// 获取当前异常处理程序支持的所能处理的异常列表。
		/// </summary>
		IList<Type> CanHandledExceptionTypes
		{
			get;
		}
	}
}
