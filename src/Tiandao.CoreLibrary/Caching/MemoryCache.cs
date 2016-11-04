using System;
using System.Collections.Generic;
#if CORE_CLR
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
#endif

namespace Tiandao.Caching
{
	public class MemoryCache : ICache, Common.IDisposableObject, Common.IAccumulator
	{
		#region 单例字段

		public static readonly MemoryCache Default = new MemoryCache("Tiandao.Caching.MemoryCache");

		#endregion

		#region 事件声明

		public event EventHandler<Common.DisposedEventArgs> Disposed;

		public event EventHandler<CacheChangedEventArgs> Changed;

		#endregion

		#region 私有字段

		private ICacheCreator _creator;
		private string _name;

#if !CORE_CLR
		private System.Runtime.Caching.MemoryCache _innerCache;
#else
		private Microsoft.Extensions.Caching.Memory.MemoryCache _innerCache;
#endif

		#endregion

		#region 公共属性

		public string Name
		{
			get
			{
				return _name;
			}
		}

		public long Count
		{
			get
			{
#if !CORE_CLR
				return _innerCache.GetCount();
#else
				return _innerCache.Count;
#endif
			}
		}

		public ICacheCreator Creator
		{
			get
			{
				return _creator;
			}
			set
			{
				_creator = value;
			}
		}

		public bool IsDisposed
		{
			get
			{
				return _innerCache == null;
			}
		}

		#endregion

		#region 构造方法

		public MemoryCache(string name) : this(name, null)
		{
		}

		public MemoryCache(string name, ICacheCreator creator)
		{
			if(string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentNullException("name");
			}

			_creator = creator;
			_name = name;

#if !CORE_CLR
			_innerCache = new System.Runtime.Caching.MemoryCache(name);
#else
			_innerCache = new Microsoft.Extensions.Caching.Memory.MemoryCache(new MemoryCacheOptions());
#endif
		}

		#endregion

		#region 公共方法

		public bool Exists(string key)
		{
#if !CORE_CLR
			return _innerCache.Contains(key);
#else
			object result;

			return _innerCache.TryGetValue(key, out result);
#endif
		}

		public TimeSpan? GetDuration(string key)
		{
			throw new NotSupportedException();
		}

		public void SetDuration(string key, TimeSpan duration)
		{
			throw new NotSupportedException();
		}

		public T GetValue<T>(string key)
		{
			return Common.Converter.ConvertValue<T>(this.GetValue(key));
		}

		public object GetValue(string key)
		{
			var creator = this.Creator;

			if(creator == null || this.Exists(key))
			{
				return _innerCache.Get(key);
			}

			return this.GetValue(key, _ =>
			{
				TimeSpan duration;
				var value = creator.Create(this.Name, _, out duration);
				return new Tuple<object, TimeSpan>(value, duration);
			});
		}

		public object GetValue(string key, Func<string, Tuple<object, TimeSpan>> valueCreator)
		{
			if(valueCreator == null)
			{
				return _innerCache.Get(key);
			}

			var result = valueCreator(key);

#if !CORE_CLR
			return _innerCache.AddOrGetExisting(key, result.Item1, new System.Runtime.Caching.CacheItemPolicy()
			{
				SlidingExpiration = result.Item2,
				RemovedCallback = this.OnRemovedCallback,
			});
#else
			if(!this.Exists(key))
			{
				var options = new MemoryCacheEntryOptions();
				options.SlidingExpiration = result.Item2;
				options.RegisterPostEvictionCallback(this.OnPostEvictionCallback);

				return _innerCache.Set(key, result.Item1, options);
			}
			else
			{
				return _innerCache.Get(key);
			}
#endif
		}

		public object GetValue(string key, Func<string, Tuple<object, DateTime>> valueCreator)
		{
			if(valueCreator == null)
			{
				return _innerCache.Get(key);
			}

			var result = valueCreator(key);

#if !CORE_CLR
			return _innerCache.AddOrGetExisting(key, result.Item1, new System.Runtime.Caching.CacheItemPolicy()
			{
				AbsoluteExpiration = result.Item2 > DateTime.Now ? result.Item2 : System.Runtime.Caching.ObjectCache.InfiniteAbsoluteExpiration,
				RemovedCallback = this.OnRemovedCallback,
			});
#else
			if(!this.Exists(key))
			{
				var options = new MemoryCacheEntryOptions();
				options.AbsoluteExpiration = result.Item2 > DateTime.Now ? result.Item2 : DateTimeOffset.MaxValue;
				options.RegisterPostEvictionCallback(this.OnPostEvictionCallback);

				return _innerCache.Set(key, result.Item1, options);
			}
			else
			{
				return _innerCache.Get(key);
			}
#endif
		}

		public bool SetValue(string key, object value)
		{
			return this.SetValue(key, value, TimeSpan.Zero);
		}

		public bool SetValue(string key, object value, TimeSpan duration, bool requiredNotExists = false)
		{
			if(requiredNotExists)
			{
				var exists = this.Exists(key);

				if(exists)
				{
					return false;
				}
			}

#if !CORE_CLR
			if(duration == TimeSpan.Zero)
			{
				_innerCache.Set(key, value, new System.Runtime.Caching.CacheItemPolicy()
				{
					AbsoluteExpiration = System.Runtime.Caching.ObjectCache.InfiniteAbsoluteExpiration,
					RemovedCallback = this.OnRemovedCallback,
				});
			}
			else
			{
				_innerCache.Set(key, value, new System.Runtime.Caching.CacheItemPolicy()
				{
					SlidingExpiration = duration,
					RemovedCallback = this.OnRemovedCallback,
				});
			}
#else
			var options = new MemoryCacheEntryOptions();

			if(duration == TimeSpan.Zero)
			{
				options.AbsoluteExpiration = DateTimeOffset.MaxValue;
			}
			else
			{
				options.SlidingExpiration = duration;
			}

			options.RegisterPostEvictionCallback(this.OnPostEvictionCallback);

			_innerCache.Set(key, value, options);
#endif

			return true;
		}

		public bool SetValue(string key, object value, DateTime expires, bool requiredNotExists = false)
		{
			if(requiredNotExists)
			{
				var exists = this.Exists(key);

				if(exists)
				{
					return false;
				}
			}

#if !CORE_CLR
			if(expires < DateTime.Now)
			{
				_innerCache.Set(key, value, new System.Runtime.Caching.CacheItemPolicy()
				{
					AbsoluteExpiration = System.Runtime.Caching.ObjectCache.InfiniteAbsoluteExpiration,
					RemovedCallback = this.OnRemovedCallback,
				});
			}
			else
			{
				_innerCache.Set(key, value, new System.Runtime.Caching.CacheItemPolicy()
				{
					AbsoluteExpiration = expires.ToUniversalTime(),
					RemovedCallback = this.OnRemovedCallback,
				});
			}
#else
			var options = new MemoryCacheEntryOptions();
			options.AbsoluteExpiration = expires < DateTime.Now ? DateTimeOffset.MaxValue : expires.ToUniversalTime();
			options.RegisterPostEvictionCallback(this.OnPostEvictionCallback);

			_innerCache.Set(key, value, options);
#endif
			return true;
		}

		public bool Rename(string key, string newKey)
		{
			if(string.IsNullOrWhiteSpace(newKey))
			{
				throw new ArgumentNullException("newKey");
			}

#if !CORE_CLR
			var orignalValue = _innerCache.Remove(key);

			if(orignalValue != null)
				_innerCache.Add(newKey, orignalValue, System.Runtime.Caching.ObjectCache.InfiniteAbsoluteExpiration);

			return orignalValue != null;
#else
			object orignalValue;

			if(_innerCache.TryGetValue(key, out orignalValue))
			{
				_innerCache.Remove(key);
				_innerCache.Set(newKey, orignalValue, new MemoryCacheEntryOptions
				{
					AbsoluteExpiration = DateTimeOffset.MaxValue
				});
			}

			return orignalValue != null;
#endif
		}

		public bool Remove(string key)
		{
#if !CORE_CLR
			return _innerCache.Remove(key) != null;
#else
			object orignalValue;

			if(_innerCache.TryGetValue(key, out orignalValue))
			{
				_innerCache.Remove(key);
			}

			return orignalValue != null;
#endif
		}

		public void Clear()
		{
#if !CORE_CLR
			_innerCache.Trim(100);
#endif
			//System.Runtime.Caching.MemoryCache 没有Clear方法，它的Trim()也不一定会回收缓存项
			throw new NotSupportedException();
		}

		#endregion

		#region 缓存事件

#if !CORE_CLR

		private void OnRemovedCallback(System.Runtime.Caching.CacheEntryRemovedArguments args)
		{
			this.OnChanged(new CacheChangedEventArgs(this.ConvertReason(args.RemovedReason), args.CacheItem.Key, args.CacheItem.Value));
		}

#else

		private void OnPostEvictionCallback(object key, object value, EvictionReason reason, object state)
		{
			if(reason == EvictionReason.Removed)
			{
				this.OnChanged(new CacheChangedEventArgs(this.ConvertReason(reason), key.ToString(), value));
			}
		}

#endif

		#endregion

		#region 激发事件

		protected virtual void OnChanged(CacheChangedEventArgs args)
		{
			var changed = this.Changed;

			if(changed != null)
			{
				changed(this, args);
			}
		}

		#endregion

		#region 私有方法

#if !CORE_CLR

		private CacheChangedReason ConvertReason(System.Runtime.Caching.CacheEntryRemovedReason reason)
		{
			switch(reason)
			{
				case System.Runtime.Caching.CacheEntryRemovedReason.Expired:
					return CacheChangedReason.Expired;
				case System.Runtime.Caching.CacheEntryRemovedReason.Removed:
					return CacheChangedReason.Removed;
				case System.Runtime.Caching.CacheEntryRemovedReason.ChangeMonitorChanged:
					return CacheChangedReason.Dependented;
				default:
					return CacheChangedReason.None;
			}
		}

#else

		private CacheChangedReason ConvertReason(EvictionReason reason)
		{
			switch(reason)
			{
				case EvictionReason.Expired:
					return CacheChangedReason.Expired;
				case EvictionReason.Removed:
					return CacheChangedReason.Removed;
				default:
					return CacheChangedReason.None;
			}
		}

#endif

		#endregion

		#region 处置方法

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			var cache = System.Threading.Interlocked.Exchange(ref _innerCache, null);

			if(cache == null)
			{
				return;
			}

			cache.Dispose();

			var disposed = this.Disposed;

			if(disposed != null)
			{
				disposed(this, new Common.DisposedEventArgs(disposing));
			}
		}

		#endregion

		#region 递增接口

		public long Increment(string key, int interval = 1)
		{
			var value = this.GetValue(key);

			if(value == null)
			{
				this.SetValue(key, interval);
				return interval;
			}

			long number;

			if(!Common.Converter.TryConvertValue<long>(value, out number))
			{
				throw new InvalidOperationException();
			}

			number += interval;

			this.SetValue(key, number);

			return number;
		}

		public long Decrement(string key, int interval = 1)
		{
			return this.Increment(key, -interval);
		}

		#endregion
	}
}