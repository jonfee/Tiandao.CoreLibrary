using System;
using System.Runtime.Serialization;

namespace Tiandao.Options.Configuration
{
#if !CORE_CLR

#endif
	//[Serializable]
	public class OptionConfigurationException : Exception
    {
		#region 私有字段

		private string _fileName;

		#endregion

		#region 公共属性

		public string FileName
		{
			get
			{
				return _fileName;
			}
			internal set
			{
				_fileName = value;
			}
		}

		#endregion

		#region 构造方法

		internal OptionConfigurationException()
		{

		}

		public OptionConfigurationException(string message) : base(message)
		{

		}

		public OptionConfigurationException(string message, Exception innerException) : base(message, innerException)
		{

		}

//		protected OptionConfigurationException(SerializationInfo info, StreamingContext context) : base(info, context)
//		{
//			_fileName = info.GetString("FileName");
//		}

		#endregion

		#region 重写方法

//		public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
//		{
//			base.GetObjectData(info, context);
//
//			info.AddValue("FileName", _fileName);
//		}
		
		#endregion
	}
}