﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace Tiandao.Services
{
	public class ServiceStorage : ServiceStorageBase, ICollection, ICollection<ServiceEntry>
	{
		#region 私有字段

		private List<ServiceEntry> _entries;
		private ConcurrentDictionary<string, ServiceEntry> _namedEntries;

		#endregion

		#region 公共属性

		public override int Count
		{
			get
			{
				return _entries.Count;
			}
		}

		#endregion

		#region 构造方法

		public ServiceStorage(IServiceProvider provider) : this(provider, Services.Matcher.Default)
		{
		}

		public ServiceStorage(IServiceProvider provider, IMatcher matcher) : base(provider, matcher)
		{
			_entries = new List<ServiceEntry>();
			_namedEntries = new ConcurrentDictionary<string, ServiceEntry>(StringComparer.OrdinalIgnoreCase);
		}

		#endregion

		#region 公共方法

		public override void Add(ServiceEntry entry)
		{
			if(entry == null)
				throw new ArgumentNullException("entry");

			if(!string.IsNullOrWhiteSpace(entry.Name))
				_namedEntries[entry.Name] = entry;

			_entries.Add(entry);
		}

		public override void Clear()
		{
			_namedEntries.Clear();
			_entries.Clear();
		}

		public override ServiceEntry Remove(string name)
		{
			if(string.IsNullOrWhiteSpace(name))
				return null;

			ServiceEntry entry = null;

			if(_namedEntries.TryRemove(name, out entry))
				_entries.Remove(entry);

			return entry;
		}

		public override ServiceEntry Get(string name)
		{
			if(string.IsNullOrWhiteSpace(name))
				return null;

			ServiceEntry namedEntry;

			//首先从命名项的字典中查找指定名称的服务项
			if(_namedEntries.TryGetValue(name, out namedEntry))
				return namedEntry;

			//调用基类的查找逻辑
			return base.Get(name);
		}

		public override IEnumerator<ServiceEntry> GetEnumerator()
		{
			foreach(var entry in _entries)
				yield return entry;
		}

		#endregion

		#region 显式实现

		void ICollection.CopyTo(Array array, int index)
		{
			for(int i = index; i < array.Length; index++)
			{
				array.SetValue(_entries[i - index], i);
			}
		}

		bool ICollection.IsSynchronized
		{
			get
			{
				return true;
			}
		}

		private object _syncRoot;

		object ICollection.SyncRoot
		{
			get
			{
				if(_syncRoot == null)
					System.Threading.Interlocked.CompareExchange(ref _syncRoot, new object(), null);

				return _syncRoot;
			}
		}

		bool ICollection<ServiceEntry>.Contains(ServiceEntry item)
		{
			return _entries.Contains(item);
		}

		void ICollection<ServiceEntry>.CopyTo(ServiceEntry[] array, int arrayIndex)
		{
			_entries.CopyTo(array, arrayIndex);
		}

		bool ICollection<ServiceEntry>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		bool ICollection<ServiceEntry>.Remove(ServiceEntry item)
		{
			throw new NotSupportedException();
		}

		#endregion
	}
}
