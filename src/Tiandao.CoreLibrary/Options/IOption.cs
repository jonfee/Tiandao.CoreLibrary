using System;
using System.ComponentModel;

namespace Tiandao.Options
{
	/// <summary>
	/// 表示选项的基本功能定义，由 <seealso cref="Option"/> 类实现。
	/// </summary>
	/// <remarks>建议选项的实现者从 <see cref="Option"/> 基类继承。</remarks>
	public interface IOption
    {
		#region 事件定义

		event EventHandler Changed;
		event EventHandler Applied;
		event EventHandler Resetted;
		event ComponentModel.CancelEventHandler Applying;
		event ComponentModel.CancelEventHandler Resetting;

		#endregion

		#region 属性定义

		object OptionObject
		{
			get;
		}

		IOptionView View
		{
			get;
			set;
		}

		IOptionViewBuilder ViewBuilder
		{
			get;
			set;
		}

		IOptionProvider Provider
		{
			get;
		}

		bool IsDirty
		{
			get;
		}

		#endregion

		#region 方法定义

		void Reset();

		void Apply();

		#endregion
	}
}