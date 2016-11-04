using System;
using System.Collections;
using System.Collections.Generic;

namespace Tiandao.Services
{
    public class CommandOptionCollection : IDictionary<string, object>
	{
		#region 私有字段

		private ICommand _command;
		private IDictionary<string, object> _items;

		#endregion

		#region 公共属性

		public int Count
		{
			get
			{
				return _items.Count;
			}
		}

		public object this[string key]
		{
			get
			{
				object value;

				if(_items.TryGetValue(key, out value))
					return value;

				var option = CommandHelper.GetOption(_command, key);

				if(option == null)
					throw new CommandOptionException(key);

				return option.DefaultValue;
			}
			set
			{
				_items[key] = this.ValidateOptionValue(key, value);
			}
		}

		public ICollection<string> Keys
		{
			get
			{
				return _items.Keys;
			}
		}

		public ICollection<object> Values
		{
			get
			{
				return _items.Values;
			}
		}

		#endregion

		#region 构造方法

		public CommandOptionCollection(ICommand command) : this(command, (IDictionary)null)
		{
		}

		public CommandOptionCollection(ICommand command, IDictionary options)
		{
			if(command == null)
				throw new ArgumentNullException("command");

			_command = command;

			if(options == null || options.Count == 0)
				_items = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
			else
			{
				_items = new Dictionary<string, object>(options.Count, StringComparer.OrdinalIgnoreCase);

				foreach(DictionaryEntry entry in options)
				{
					this[entry.Key.ToString()] = entry.Value;
				}
			}
		}

		public CommandOptionCollection(ICommand command, IDictionary<string, object> options)
		{
			if(command == null)
				throw new ArgumentNullException("command");

			_command = command;

			if(options == null || options.Count == 0)
				_items = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
			else
			{
				_items = new Dictionary<string, object>(options.Count, StringComparer.OrdinalIgnoreCase);

				foreach(var entry in options)
				{
					this[entry.Key] = entry.Value;
				}
			}
		}

		#endregion

		#region 公共方法

		public bool Contains(string name)
		{
			return _items.ContainsKey(name);
		}

		public bool TryGetValue(string name, out object value)
		{
			return _items.TryGetValue(name, out value);
		}

		public bool TryGetValue<T>(string name, out T value)
		{
			object result;

			if(_items.TryGetValue(name, out result))
			{
				value = (T)result;
				return true;
			}
			else
			{
				value = default(T);
				return false;
			}
		}

		#endregion

		#region 显式实现

		void IDictionary<string, object>.Add(string key, object value)
		{
			_items.Add(key, this.ValidateOptionValue(key, value));
		}

		bool IDictionary<string, object>.ContainsKey(string key)
		{
			return _items.ContainsKey(key);
		}

		bool IDictionary<string, object>.Remove(string key)
		{
			return _items.Remove(key);
		}

		bool IDictionary<string, object>.TryGetValue(string key, out object value)
		{
			return _items.TryGetValue(key, out value);
		}

		bool ICollection<KeyValuePair<string, object>>.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item)
		{
			var value = this.ValidateOptionValue(item.Key, item.Value);
			_items.Add(item.Key, value);
		}

		void ICollection<KeyValuePair<string, object>>.Clear()
		{
			_items.Clear();
		}

		bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> item)
		{
			return _items.Contains(item);
		}

		void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
		{
			_items.CopyTo(array, arrayIndex);
		}

		bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item)
		{
			return _items.Remove(item);
		}

		#endregion

		#region 私有方法

		private object ValidateOptionValue(string name, object value)
		{
			var option = CommandHelper.GetOption(_command, name);

			if(option == null)
				throw new CommandOptionException(name);

			if(option.Type != null)
			{
				if(option.Converter != null)
					return option.Converter.ConvertTo(value, option.Type);

				object result;

				if(Common.Converter.TryConvertValue(value, option.Type, out result))
					return result;

				throw new CommandOptionValueException(name, value);
			}

			return value;
		}

		#endregion

		#region 枚举遍历

		IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
		{
			return _items.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return _items.GetEnumerator();
		}

		#endregion
	}
}
