using System;
using System.Collections.Generic;

using Tiandao.Common;

namespace Tiandao.Diagnostics
{
	public abstract class ExceptionHandlerBase : IExceptionHandler
	{
		#region 私有字段

		private readonly List<Type> _canHandledExceptionTypes;

		#endregion

		#region 构造方法

		protected ExceptionHandlerBase(Type[] canHandledExceptionTypes)
		{
			if(canHandledExceptionTypes == null || canHandledExceptionTypes.Length <= 0)
				throw new ArgumentNullException("canHandledExceptionTypes");

			_canHandledExceptionTypes = new List<Type>();

			foreach(Type exceptionType in canHandledExceptionTypes)
			{
				if(exceptionType != typeof(Exception) && (!exceptionType.IsSubclassOf(typeof(Exception))))
					throw new ArgumentException();

				_canHandledExceptionTypes.Add(exceptionType);
			}
		}

		#endregion

		#region 公共属性

		public IList<Type> CanHandledExceptionTypes
		{
			get
			{
				return _canHandledExceptionTypes;
			}
		}

		#endregion

		#region 虚拟方法

		public virtual bool CanHandle(Type exceptionType)
		{
			if(exceptionType == null)
			{
				throw new ArgumentNullException("exceptionType");
			}

			foreach(Type type in _canHandledExceptionTypes)
			{
				if(type == exceptionType)
				{
					return true;
				}
			}

			return false;
		}

		#endregion

		#region 抽象方法

		public abstract Exception Handle(Exception exception, object context);

		#endregion
	}
}
