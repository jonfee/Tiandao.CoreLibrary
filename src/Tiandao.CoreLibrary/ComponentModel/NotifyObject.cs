using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Tiandao.ComponentModel
{
    public class NotifyObject : INotifyPropertyChanged
	{
		#region 事件声明

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region 私有字段

		private ConcurrentDictionary<string, object> _properties;

		#endregion

		#region 保护属性

		protected bool HasProperties
		{
			get
			{
				return _properties != null && _properties.Count > 0;
			}
		}

		protected IDictionary<string, object> Properties
		{
			get
			{
				if(_properties == null)
					System.Threading.Interlocked.CompareExchange(ref _properties, new ConcurrentDictionary<string, object>(), null);

				return _properties;
			}
		}

		#endregion

		#region 保护方法

		protected T GetPropertyValue<T>(string propertyName, T defaultValue = default(T))
		{
			if(string.IsNullOrWhiteSpace(propertyName))
				throw new ArgumentNullException(nameof(propertyName));

			var properties = this.Properties;
			object value;

			if(properties.TryGetValue(propertyName.Trim(), out value))
				return (T)value;

			var property = this.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

			if(property == null)
				throw new InvalidOperationException(string.Format("The '{0}' property is not exists.", propertyName));

			//返回属性的默认值
			return this.GetPropertyDefaultValue(property, defaultValue);
		}

		protected T GetPropertyValue<T>(Expression<Func<T>> propertyExpression, T defaultValue = default(T))
		{
			if(propertyExpression == null)
				throw new ArgumentNullException(nameof(propertyExpression));

			var property = this.GetPropertyInfo<T>(propertyExpression);

			if(property == null)
				throw new ArgumentException("Invalid expression of the argument", "propertyExpression");

			var properties = this.Properties;
			object value;

			if(properties.TryGetValue(property.Name, out value))
				return (T)value;

			//返回属性的默认值
			return this.GetPropertyDefaultValue(property, defaultValue);
		}

		protected void SetPropertyValue(string propertyName, object value)
		{
			if(string.IsNullOrWhiteSpace(propertyName))
				throw new ArgumentNullException(nameof(propertyName));

			var properties = this.Properties;

			properties[propertyName.Trim()] = value;

			this.OnPropertyChanged(propertyName);
		}

		protected void SetPropertyValue<T>(Expression<Func<T>> propertyExpression, T value)
		{
			if(propertyExpression == null)
				throw new ArgumentNullException(nameof(propertyExpression));

			var property = this.GetPropertyInfo<T>(propertyExpression);

			if(property == null)
				throw new ArgumentException("Invalid expression of the argument", "propertyExpression");

			var properties = this.Properties;

			properties[property.Name] = value;

			this.OnPropertyChanged(property.Name);
		}

		protected void SetPropertyValue<T>(string propertyName, ref T target, T value)
		{
			if(object.ReferenceEquals(target, value))
				return;

			target = value;
			this.OnPropertyChanged(propertyName);
		}

		protected void SetPropertyValue<T>(Expression<Func<T>> propertyExpression, ref T target, T value)
		{
			if(object.ReferenceEquals(target, value))
				return;

			if(propertyExpression == null)
				throw new ArgumentNullException(nameof(propertyExpression));

			var property = this.GetPropertyInfo<T>(propertyExpression);

			if(property == null)
				throw new ArgumentException("Invalid expression of the argument", "propertyExpression");

			target = value;
			this.OnPropertyChanged(property.Name);
		}
		
		#endregion

		#region 激发事件

		protected void OnPropertyChanged(string propertyName)
		{
			this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}

		protected void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
		{
			if(propertyExpression == null)
				throw new ArgumentNullException(nameof(propertyExpression));

			var property = this.GetPropertyInfo<T>(propertyExpression);

			if(property == null)
				throw new ArgumentException("Invalid expression of the argument", "propertyExpression");

			this.OnPropertyChanged(property.Name);
		}

		protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
		{
			var handler = this.PropertyChanged;

			if(handler != null)
				handler(this, args);
		}
		
		#endregion

		#region 私有方法

		private PropertyInfo GetPropertyInfo<T>(Expression<Func<T>> propertyExpression)
		{
			if(propertyExpression == null)
				throw new ArgumentNullException(nameof(propertyExpression));

			var memberExpression = propertyExpression.Body as MemberExpression;

			if(memberExpression == null)
				throw new ArgumentException("Invalid expression of the argument.", "propertyExpression");

			return memberExpression.Member as PropertyInfo;
		}

		private T GetPropertyDefaultValue<T>(PropertyInfo property, T defaultValue)
		{
			if(property == null)
				throw new ArgumentNullException("property");

			var attribute = property.GetCustomAttribute<DefaultValueAttribute>();

			if(attribute != null)
				return Common.Converter.ConvertValue<T>(attribute.Value);

			return defaultValue;
		}

		#endregion
	}
}