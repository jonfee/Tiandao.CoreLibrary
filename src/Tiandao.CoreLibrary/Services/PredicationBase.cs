using System;

namespace Tiandao.Services
{
    public abstract class PredicationBase<T> : IPredication<T>, IMatchable<string>
	{
		#region 私有字段

		private string _name;

		#endregion

		#region 公共属性

		public virtual string Name
		{
			get
			{
				return _name;
			}
		}

		#endregion

		#region 构造方法

		protected PredicationBase(string name)
		{
			if(string.IsNullOrWhiteSpace(name))
				throw new ArgumentNullException(nameof(name));

			_name = name.Trim();
		}

		#endregion

		#region 断言方法

		public abstract bool Predicate(T parameter);

		bool IPredication.Predicate(object parameter)
		{
			return this.Predicate(this.ConvertParameter(parameter));
		}

		#endregion

		#region 虚拟方法

		protected virtual T ConvertParameter(object parameter)
		{
			return Common.Converter.ConvertValue<T>(parameter);
		}

		#endregion

		#region 服务匹配

		public virtual bool IsMatch(string parameter)
		{
			return string.Equals(this.Name, parameter, StringComparison.OrdinalIgnoreCase);
		}

		bool IMatchable.IsMatch(object parameter)
		{
			return this.IsMatch(parameter as string);
		}

		#endregion

		#region 重写方法

		public override string ToString()
		{
			return string.Format("{0} ({1})", this.Name, this.GetType());
		}

		#endregion
	}
}
