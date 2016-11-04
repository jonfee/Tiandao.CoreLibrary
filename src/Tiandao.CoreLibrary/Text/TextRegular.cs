﻿using System;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace Tiandao.Text
{
    public class TextRegular : ITextRegular
	{
		#region 私有字段

		private readonly Regex _regex;

		#endregion

		#region 构造方法

		public TextRegular(string pattern)
		{
			if(string.IsNullOrWhiteSpace(pattern))
				throw new ArgumentNullException("pattern");

			_regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase, TimeSpan.FromSeconds(3));
		}

		#endregion

		#region 公共方法

		public bool IsMatch(string text)
		{
			if(text == null)
				return false;

			try
			{
				return _regex.IsMatch(text);
			}
			catch
			{
				return false;
			}
		}

		public bool IsMatch(string text, out string result)
		{
			result = null;

			if(text == null)
				return false;

			Match match;

			try
			{
				match = _regex.Match(text);
			}
			catch
			{
				return false;
			}

			if(match.Success)
			{
				var group = match.Groups["value"];

				if(group == null)
				{
					result = match.Value;
				}
				else
				{
					foreach(Capture capture in group.Captures)
					{
						result += capture.Value;
					}
				}
			}

			return match.Success;
		}

		#endregion

		#region 显式实现

		bool Services.IMatchable.IsMatch(object parameter)
		{
			if(parameter == null)
				return false;

			return this.IsMatch(parameter as string);
		}

		#endregion

		#region 嵌套子类

		public static class Web
		{
			/// <summary>获取电子邮箱(Email)地址的文本验证器。</summary>
			public static readonly TextRegular Email = new TextRegular(@"^\s*(?<value>[A-Za-z0-9]([-_\.]?[A-Za-z0-9]+)*@([A-Za-z0-9]+([-_]?[A-Za-z0-9]+)*)(\.[A-Za-z0-9]+([-_]?[A-Za-z0-9]+)*)*\.[A-Za-z]+)\s*$");
		}

		public static class Uri
		{
			#region 正则文本
			/*
^\s*(?<value>
(
	(?<scheme>[A-Za-z]+)://
)?
(?<host>
	(?<domain>[A-Za-z0-9]+([-_][A-Za-z0-9]+)*)
	(\.(?<domain>[A-Za-z0-9]+([-_][A-Za-z0-9]+)*))+
)
(:(?<port>\d{1,5}))?
(?<path>
	(?<segment>
		/[^\s/:\*\?\&]*
	)*
)?
(?<query>
	\?
	(
		(?<parameter>
			(?<parameter_name>[A-Za-z0-9]+([-_][A-Za-z0-9]+)*)
			(=(?<parameter_value>[^\?\&\s]*))?
		)
		\&?
	)*
)?
(?<fragment>
	\#[^\#\s]*
)?
)\s*$
			 */
			private const string URI_PATTERN = @"^\s*(?<value>((?<scheme>${scheme})://)?(?<host>(?<domain>[A-Za-z0-9]+([-_][A-Za-z0-9]+)*)(\.(?<domain>[A-Za-z0-9]+([-_][A-Za-z0-9]+)*))+)(:(?<port>\d{1,5}))?(?<path>(?<segment>/[^\s/:\*\?\&]*)*)?(?<query>\?((?<parameter>(?<parameter_name>[A-Za-z0-9]+([-_][A-Za-z0-9]+)*)(=(?<parameter_value>[^\?\&\s]*))?)\&?)*)?(?<fragment>\#[^\#\s]*)?)\s*$";
			
			#endregion

			#region 静态变量

			private static readonly ConcurrentDictionary<string, TextRegular> _instances = new ConcurrentDictionary<string, TextRegular>(StringComparer.OrdinalIgnoreCase);
			
			#endregion

			#region 单例字段

			/// <summary>获取任意协议的URL文本验证器。</summary>
			public static readonly TextRegular Url = new TextRegular(URI_PATTERN.Replace("${scheme}", "[A-Za-z]+"));

			/// <summary>获取Http协议的URL文本验证器。</summary>
			public static readonly TextRegular Http = new TextRegular(URI_PATTERN.Replace("${scheme}", "http[s]?"));

			/// <summary>获取Ftp协议的URL文本验证器。</summary>
			public static readonly TextRegular Ftp = new TextRegular(URI_PATTERN.Replace("${scheme}", "ftp"));
			
			#endregion

			#region 静态方法

			public static TextRegular GetRegular(string scheme)
			{
				if(string.IsNullOrWhiteSpace(scheme))
					throw new ArgumentNullException("scheme");

				return _instances.GetOrAdd(scheme.Trim().ToLowerInvariant(), key => new TextRegular(URI_PATTERN.Replace("${scheme}", key)));
			}

			#endregion
		}

		public static class Chinese
		{
			/// <summary>获取中国手机号码的文本验证器。</summary>
			public static readonly TextRegular Cellphone = new TextRegular(@"^\s*((\+|00)86\s*[-\.]?)?\s*(?<value>1\d{2})(?<separator>(\s*)|(-?))(?<value>\d{4})(?<separator>(\s*)|(-?))(?<value>\d{4})\s*$");

			/// <summary>获取中国固定电话号码的文本验证器。</summary>
			public static readonly TextRegular Telephone = new TextRegular(@"^\s*(?<value>0\d{2,4})?(?<separator>(\s*)|(-?))(?<value>\d{4})(?<separator>(\s*)|(-?))(?<value>\d{3,4})\s*$");

			/// <summary>获取中国身份证号码的文本验证器。</summary>
			public static readonly TextRegular IdentityNo = new TextRegular(@"^\s*(?<value>\d{6})?(?<separator>(\s*)|(-?))(?<value>\d{8})(?<separator>(\s*)|(-?))(?<value>(\d{4})|(\d{3}[A-Za-z]))\s*$");

			/// <summary>获取中国邮政编码的文本验证器。</summary>
			public static readonly TextRegular PostalCode = new TextRegular(@"^\s*(?<value>\d{6})\s*$");
		}

		#endregion
	}
}