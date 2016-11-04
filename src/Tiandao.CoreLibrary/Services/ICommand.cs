using System;
using System.Collections.Generic;

namespace Tiandao.Services
{
	/// <summary>
	/// 扩展命令接口。
	/// </summary>
	public interface ICommand
    {
		#region 事件定义

		/// <summary>
		/// 在 <seealso cref="Enabled"/> 属性发生改变之后发生。
		/// </summary>
		event EventHandler EnabledChanged;

		/// <summary>
		/// 在 <seealso cref="Execute"/> 方法执行之前发生。可以通过事件参数取消后续的执行。
		/// </summary>
		event EventHandler<CommandExecutedEventArgs> Executed;

		/// <summary>
		/// 在 <seealso cref="Execute"/> 方法执行之后发生。
		/// </summary>
		event EventHandler<CommandExecutingEventArgs> Executing;

		#endregion

		#region 属性定义

		/// <summary>
		/// 获取命令的名称。
		/// </summary>
		string Name
		{
			get;
		}

		/// <summary>
		/// 获取或设置一个值，该值指示命令是否可以执行。
		/// </summary>
		bool Enabled
		{
			get;
			set;
		}

		#endregion

		#region 方法定义

		/// <summary>
		/// 判断当前命令能否依据给定的选项和参数执行。
		/// </summary>
		/// <param name="parameter">判断命令能否执行的参数对象。</param>
		/// <returns>返回能否执行的结果。</returns>
		bool CanExecute(object parameter);

		/// <summary>
		/// 执行命令。
		/// </summary>
		/// <param name="parameter">执行命令的参数对象。</param>
		/// <returns>返回执行的返回结果。</returns>
		/// <remarks>
		///		<para>对实现着的要求：应该在本方法的实现中首先调用<see cref="CanExecute"/>方法，以确保阻止非法的调用。</para>
		/// </remarks>
		object Execute(object parameter);

		#endregion
	}
}