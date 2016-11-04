using System;
using System.Text;

namespace Tiandao.Options.Profiles
{
    public class ProfileComment : ProfileItem
	{
		#region 私有字段

		private StringBuilder _text;

		#endregion

		#region 公共属性

		public string Text
		{
			get
			{
				return _text.ToString();
			}
		}

		public string[] Lines
		{
			get
			{
				return _text.ToString().Split('\r', '\n');
			}
		}

		public override ProfileItemType ItemType
		{
			get
			{
				return ProfileItemType.Comment;
			}
		}

		#endregion

		#region 构造方法

		public ProfileComment(string text, int lineNumber = -1) : base(lineNumber)
		{
			if(string.IsNullOrEmpty(text))
				_text = new StringBuilder();
			else
				_text = new StringBuilder(text);
		}

		#endregion

		#region 公共方法

		public void Append(string text)
		{
			if(string.IsNullOrEmpty(text))
				return;

			_text.Append(text);
		}

		public void AppendFormat(string format, params object[] args)
		{
			if(string.IsNullOrEmpty(format))
				return;

			_text.AppendFormat(format, args);
		}

		public void AppendLine(string text)
		{
			if(text == null)
				_text.AppendLine();
			else
				_text.AppendLine(text);
		}

		#endregion
	}
}
