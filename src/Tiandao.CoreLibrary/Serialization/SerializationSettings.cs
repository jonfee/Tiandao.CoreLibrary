using System;
using System.Collections.Generic;

namespace Tiandao.Serialization
{
    public class SerializationSettings : ComponentModel.NotifyObject
	{
		#region 私有字段

		private int _maximumDepth;
		private SerializationBehavior _serializationBehavior;
		private SerializationMembers _serializationMembers;

		#endregion

		#region 公共属性

		public int MaximumDepth
		{
			get
			{
				return _maximumDepth;
			}
			set
			{
				var newValue = Math.Max(-1, value);
				var changed = _maximumDepth != newValue;

				_maximumDepth = newValue;

				if(changed)
					this.OnPropertyChanged(() => this.MaximumDepth);
			}
		}

		public SerializationBehavior SerializationBehavior
		{
			get
			{
				return _serializationBehavior;
			}
			set
			{
				this.SetPropertyValue(() => this.SerializationBehavior, ref _serializationBehavior, value);
			}
		}

		public SerializationMembers SerializationMembers
		{
			get
			{
				return _serializationMembers;
			}
			set
			{
				this.SetPropertyValue(() => this.SerializationMembers, ref _serializationMembers, value);
			}
		}

		#endregion

		#region 构造方法

		public SerializationSettings()
		{
			_maximumDepth = -1;
			_serializationMembers = Serialization.SerializationMembers.All;
		}

		public SerializationSettings(int maximumDepth, Serialization.SerializationMembers serializationMembers)
		{
			this.MaximumDepth = maximumDepth;
			this.SerializationMembers = serializationMembers;
		}
		
		#endregion
	}
}
