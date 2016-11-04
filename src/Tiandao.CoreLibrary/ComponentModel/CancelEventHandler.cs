using System;
using System.ComponentModel;

namespace Tiandao.ComponentModel
{
	/// <summary>
	/// 表示处理可取消事件的方法。
	/// </summary>
	/// <param name="sender">事件源。</param><param name="e">包含事件数据的 <see cref="System.ComponentModel.CancelEventArgs"/>。</param>
	//	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public delegate void CancelEventHandler(object sender, CancelEventArgs e);
}