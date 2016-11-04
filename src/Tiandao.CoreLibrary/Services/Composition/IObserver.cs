using System;
using System.Collections.Generic;

namespace Tiandao.Services.Composition
{
	/// <summary>
	/// 提供用于接收基于推送的通知的机制的观察者。
	/// </summary>
	public interface IObserver
    {
		/// <summary>
		/// 初始化当前观察者。
		/// </summary>
		/// <param name="observable">被注册到的观察容器，即被消息提供者。</param>
		/// <remarks>
		///		<para>该方法由观察容器进行调用，通常当观察者被注册到容器中时被调用。</para>
		/// </remarks>
		void Initialize(IObservable observable);

		/// <summary>
		/// 向观察者提供新的信息。
		/// </summary>
		/// <param name="value">当前的通知信息。</param>
		void OnNotified(object value);

		/// <summary>
		/// 通知观察者，提供程序已完成发送基于推送的通知。
		/// </summary>
		/// <param name="value">当前的通知信息。</param>
		void OnCompleted(object value);
	}
}
