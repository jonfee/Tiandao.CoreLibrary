﻿using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Tiandao.Collections
{
	/// <summary>
	/// 提供了一个线程安全的通用对象池的相关功能。
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ObjectPool<T> : IDisposable where T : class
	{
		#region 私有字段

		private ConcurrentBag<T> _idles;
		private Func<T> _creator;
		private Action<T> _remover;
		private int _maximumLimit;
		private SemaphoreSlim _semaphore;

		#endregion

		#region 公共属性

		/// <summary>
		/// 获取对象池的可用空闲元素数量，负数表示未限定。
		/// </summary>
		public int Count
		{
			get
			{
				var idles = _idles;

				if(idles == null)
					throw new ObjectDisposedException(null);

				if(_semaphore == null || _maximumLimit < 1)
					return -1;

				return _maximumLimit - idles.Count;
			}
		}

		/// <summary>
		/// 获取对象池的最大容量，零表示不限制。
		/// </summary>
		public int MaximumLimit
		{
			get
			{
				return _maximumLimit;
			}
		}
		
		#endregion

		#region 构造方法

		protected ObjectPool() : this(0)
		{

		}

		protected ObjectPool(int maximumLimit)
		{
			_maximumLimit = Math.Max(maximumLimit, 0);
			_idles = new ConcurrentBag<T>();

			if(_maximumLimit > 0)
				_semaphore = new SemaphoreSlim(_maximumLimit, _maximumLimit);
		}

		/// <summary>
		/// 创建一个新的对象管理池。
		/// </summary>
		/// <param name="creator">对象的创建方法。</param>
		/// <param name="remover">对象移除时的回调，该参数值可以为空(null)。</param>
		public ObjectPool(Func<T> creator, Action<T> remover) : this(creator, remover, 0)
		{

		}

		/// <summary>
		/// 创建一个新的对象管理池。
		/// </summary>
		/// <param name="creator">对象的创建方法。</param>
		/// <param name="remover">对象移除时的回调，该参数值可以为空(null)。</param>
		/// <param name="maximumLimit">对象池的最大容量，如果小于一则表示不控制池的大小。</param>
		public ObjectPool(Func<T> creator, Action<T> remover, int maximumLimit)
		{
			if(creator == null)
				throw new ArgumentNullException(nameof(creator));

			_creator = creator;
			_remover = remover;
			_maximumLimit = Math.Max(maximumLimit, 0);
			_idles = new ConcurrentBag<T>();

			if(_maximumLimit > 0)
				_semaphore = new SemaphoreSlim(_maximumLimit, _maximumLimit);
		}
		
		#endregion

		#region 公共方法

		/// <summary>
		/// 从对象池中获取一个可用对象。
		/// </summary>
		public T GetObject()
		{
			var idles = _idles;

			if(idles == null)
				throw new ObjectDisposedException(null);

			T item;

			if(_semaphore != null)
				_semaphore.Wait();

			if(!idles.TryTake(out item))
				item = this.OnCreate();

			//如果获取或者创建的新项为空，则释放一个信号量并返回空值
			if(item == null)
			{
				if(_semaphore != null)
					_semaphore.Release();

				return null;
			}

			//回调取出方法
			this.OnTakeout(item);

			return item;
		}

		/// <summary>
		/// 将一个对象释放到池中。
		/// </summary>
		public void Release(T item)
		{
			var idles = _idles;

			if(idles == null)
				throw new ObjectDisposedException(null);

			if(item == null)
				throw new ArgumentNullException(nameof(item));

			//回调放入方法
			this.OnTakein(item);

			idles.Add(item);

			if(_semaphore != null)
				_semaphore.Release();
		}

		/// <summary>
		/// 清空对象池，该方法会依次调用池中空闲对象的<see cref="OnRemove"/>方法。
		/// </summary>
		public void Clear()
		{
			var idles = _idles;

			if(idles == null)
				throw new ObjectDisposedException(null);

			T item;

			while(idles.TryTake(out item))
			{
				this.OnRemove(item);
			}
		}
		
		#endregion

		#region 虚拟方法

		protected virtual T OnCreate()
		{
			if(_creator == null)
				return Activator.CreateInstance<T>();

			return _creator();
		}

		/// <summary>
		/// 表示将一个对象放入当前缓存池时该方法被调用。
		/// </summary>
		/// <param name="value">被放入的那个缓存项对象。</param>
		protected virtual void OnTakein(T value)
		{

		}

		/// <summary>
		/// 表示当从当前缓存池中取出一个缓存项时该方法被调用。
		/// </summary>
		/// <param name="value">被取出的那个缓存项对象。</param>
		protected virtual void OnTakeout(T value)
		{

		}

		/// <summary>
		/// 表示删除当前缓存池中的一个缓存项时该方法被调用。
		/// </summary>
		/// <param name="value">被删除的那个缓存项对象。</param>
		protected virtual void OnRemove(T value)
		{
			if(_remover != null)
				_remover(value);

			IDisposable disposable = value as IDisposable;

			if(disposable != null)
				disposable.Dispose();
		}
		
		#endregion

		#region 释放资源

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if(!disposing)
				return;

			Clear();

			var idles = System.Threading.Interlocked.Exchange(ref _idles, null);

			if(idles != null)
			{
				_idles = null;
				_creator = null;
				_remover = null;

				if(_semaphore != null)
				{
					_semaphore.Dispose();
					_semaphore = null;
				}
			}
		}
		
		#endregion
	}
}