﻿using System;
using System.Collections.Generic;

namespace Tiandao.Services.Composition
{
    public interface IExecutionContext
    {
		/// <summary>
		/// 获取处理本次执行请求的执行器。
		/// </summary>
		IExecutor Executor
		{
			get;
		}

		/// <summary>
		/// 获取处理本次执行请求的输入参数。
		/// </summary>
		object Parameter
		{
			get;
		}

		/// <summary>
		/// 获取本次执行中发生的异常。
		/// </summary>
		Exception Exception
		{
			get;
		}

		/// <summary>
		/// 获取扩展属性集是否有内容。
		/// </summary>
		/// <remarks>
		///		<para>在不确定扩展属性集是否含有内容之前，建议先使用该属性来检测。</para>
		/// </remarks>
		bool HasExtendedProperties
		{
			get;
		}

		/// <summary>
		/// 获取可用于在本次执行过程中在各处理模块之间组织和共享数据的键/值集合。
		/// </summary>
		IDictionary<string, object> ExtendedProperties
		{
			get;
		}

		/// <summary>
		/// 获取或设置本次执行的返回结果。
		/// </summary>
		object Result
		{
			get;
			set;
		}
	}
}
