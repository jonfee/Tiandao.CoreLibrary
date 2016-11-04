using System;
using System.Collections.Generic;

namespace Tiandao.Services.Composition
{
	/// <summary>
	/// 提供基于推送机制的通知提供者。
	/// </summary>
	public interface IObservable
    {
		/// <summary>
		/// 向当前通知提供者订阅，表示某观察器将要接收通知。
		/// </summary>
		/// <param name="observer">要接收通知的观察者对象。</param>
		void Subscribe(IObserver observer);

		/// <summary>
		/// 向当前通知提供者退订，表示某观察器取消接收通知。
		/// </summary>
		/// <param name="observer">要取消接收通知的观察者对象。</param>
		void Unsubscribe(IObservable observer);

		/// <summary>
		/// 通知所有观察者，即向已注册的所有观察者发送通知。
		/// </summary>
		/// <param name="value">发送的信息。</param>
		void Notify(object value);

		/// <summary>
		/// 通知观察者，提供程序已完成发送基于推送的通知。
		/// </summary>
		/// <param name="value">发送的信息。</param>
		void Complete(object value);
	}
}
