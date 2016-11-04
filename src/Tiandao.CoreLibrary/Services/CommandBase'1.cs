﻿using System;
using System.Collections.Generic;

namespace Tiandao.Services
{
	/// <summary>
	/// 提供实现<see cref="ICommand"/>接口功能的基类，建议需要完成<see cref="ICommand"/>接口功能的实现者从此类继承。
	/// </summary>
	/// <typeparam name="TContext">指定命令的执行上下文类型。</typeparam>
	public abstract class CommandBase<TContext> : CommandBase, ICommand<TContext>, IPredication<TContext> where TContext : CommandContextBase
	{
		#region 构造方法

		protected CommandBase() : base(null, true)
		{
		}

		protected CommandBase(string name) : base(name, true)
		{
		}

		protected CommandBase(string name, bool enabled) : base(name, enabled)
		{
		}

		#endregion

		#region 公共方法

		/// <summary>
		/// 判断命令是否可被执行。
		/// </summary>
		/// <param name="context">判断命令是否可被执行的上下文对象。</param>
		/// <returns>如果返回真(true)则表示命令可被执行，否则表示不可执行。</returns>
		/// <remarks>
		///		<para>本方法为虚拟方法，可由子类更改基类的默认实现方式。</para>
		///		<para>如果<seealso cref="CommandBase.Predication"/>属性为空(null)，则返回<seealso cref="CommandBase.Enabled"/>属性值；否则返回由<seealso cref="CommandBase.Predication"/>属性指定的断言对象的断言方法的值。</para>
		/// </remarks>
		public virtual bool CanExecute(TContext context)
		{
			return base.CanExecute(context);
		}

		/// <summary>
		/// 执行命令。
		/// </summary>
		/// <param name="context">执行命令的上下文对象。</param>
		/// <returns>返回执行的返回结果。</returns>
		/// <remarks>
		///		<para>本方法的实现中首先调用<seealso cref="CommandBase.CanExecute"/>方法，以确保阻止非法的调用。</para>
		/// </remarks>
		public object Execute(TContext context)
		{
			return base.Execute(context);
		}

		#endregion

		#region 抽象方法

		protected virtual TContext CreateContext(object parameter, IDictionary<string, object> items)
		{
			return parameter as TContext;
		}

		protected abstract void OnExecute(TContext context);

		#endregion

		#region 重写方法

		protected override bool CanExecute(object parameter)
		{
			var context = parameter as TContext;

			if(context == null)
				context = this.CreateContext(parameter, null);

			return this.CanExecute(context);
		}

		protected override object OnExecute(object parameter)
		{
			var context = parameter as TContext;

			if(context == null)
				context = this.CreateContext(parameter, null);

			//执行具体的命令操作
			this.OnExecute(context);

			return context.Result;
		}

		#endregion

		#region 显式实现

		bool IPredication<TContext>.Predicate(TContext context)
		{
			return this.CanExecute(context);
		}

		#endregion
	}
}