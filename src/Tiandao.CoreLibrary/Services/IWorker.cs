using System;
using System.Collections.Generic;

namespace Tiandao.Services
{
	/// <summary>
	/// 关于工作器的接口。
	/// </summary>
	public interface IWorker
    {
		#region 事件定义

		/// <summary>
		/// 表示状态发生了改变。
		/// </summary>
		event EventHandler<WorkerStateChangedEventArgs> StateChanged;

		#endregion

		#region 属性定义

		/// <summary>
		/// 获取当前工作器的名称。
		/// </summary>
		string Name
		{
			get;
		}

		/// <summary>
		/// 获取当前工作器的状态。
		/// </summary>
		WorkerState State
		{
			get;
		}

		/// <summary>
		/// 获取或设置是否禁用工作器。
		/// </summary>
		bool Disabled
		{
			get;
			set;
		}

		/// <summary>
		/// 获取工作器是否允许暂停和继续。
		/// </summary>
		bool CanPauseAndContinue
		{
			get;
		}

		#endregion

		#region 方法定义

		/// <summary>
		/// 启动工作器。
		/// </summary>
		/// <param name="args">启动的参数。</param>
		void Start(params string[] args);

		/// <summary>
		/// 停止工作器。
		/// </summary>
		/// <param name="args">停止的参数。</param>
		void Stop(params string[] args);

		/// <summary>
		/// 暂停工作器。
		/// </summary>
		void Pause();

		/// <summary>
		/// 恢复工作器，继续运行。
		/// </summary>
		void Resume();

		#endregion
	}
}
