﻿using System;
using System.IO;
using System.ComponentModel;
using System.Collections.Generic;
using System.Reflection;

namespace Tiandao.ComponentModel
{
	#if !CORE_CLR
	public class ApplicationContextBase : MarshalByRefObject, INotifyPropertyChanged
	#else
	public class ApplicationContextBase : INotifyPropertyChanged
	#endif
	{
		#region 单例字段

		private volatile static ApplicationContextBase _current;

		#endregion

		#region 事件声明

		public event CancelEventHandler Exiting;
		public event EventHandler<ApplicationEventArgs> Starting;
		public event EventHandler<ApplicationEventArgs> Started;
		public event EventHandler<ApplicationEventArgs> Initializing;
		public event EventHandler<ApplicationEventArgs> Initialized;
		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region 单例属性

		/// <summary>
		/// 获取当前应用程序的<see cref="ApplicationContextBase"/>上下文。
		/// </summary>
		public static ApplicationContextBase Current
		{
			get
			{
				return _current;
			}
			protected set
			{
				if(value == null)
					throw new ArgumentNullException();

				_current = value;
			}
		}

		#endregion

		#region 同步变量

		protected readonly object SyncRoot = new object();

		#endregion

		#region 成员字段

		private string _applicationId;
		private string _title;
		private string _description;
		private ICollection<IApplicationModule> _modules;
		private Options.ISettingsProvider _settings;

		#endregion

		#region 公共属性

		/// <summary>
		/// 获取或设置当前应用程序唯一代号。
		/// </summary>
		/// <remarks>
		///		<para>注意：本属性一旦被设置则不能被更改。如果已经设置过本属性(即该属性值不为空或全空格字符)，则不能再次设置本属性否则将抛出<see cref="InvalidOperationException"/>异常。</para>
		/// </remarks>
		public string ApplicationId
		{
			get
			{
				return _applicationId;
			}
			set
			{
				if(string.IsNullOrWhiteSpace(value))
					throw new ArgumentNullException();

				if(!string.IsNullOrWhiteSpace(_applicationId))
					throw new InvalidOperationException("The ApplicationId has specified already.");

				if(string.Equals(_applicationId, value.Trim(), StringComparison.Ordinal))
					return;

				//设置对应的成员字段值
				_applicationId = value.Trim();

				//激发“PropertyChanged”事件
				this.OnPropertyChanged("ApplicationId");
			}
		}

		/// <summary>
		/// 获取当前应用程序的根目录。
		/// </summary>
		public virtual string ApplicationDirectory
		{
			get
			{
#if !CORE_CLR
				return AppDomain.CurrentDomain.BaseDirectory;
#else
				return AppContext.BaseDirectory;
#endif
			}
		}

		/// <summary>
		/// 获取或设置当前应用程序的标题。
		/// </summary>
		public string Title
		{
			get
			{
				return _title;
			}
			set
			{
				if(string.Equals(_title, value, StringComparison.Ordinal))
					return;

				//设置成员变量
				_title = value ?? string.Empty;

				//激发“PropertyChanged”事件
				this.OnPropertyChanged("Title");
			}
		}

		/// <summary>
		/// 获取或设置当前应用程序的描述文本。
		/// </summary>
		public string Description
		{
			get
			{
				return _description;
			}
			set
			{
				if(string.Equals(_description, value, StringComparison.Ordinal))
					return;

				//设置成员变量
				_description = value ?? string.Empty;

				//激发“PropertyChanged”事件
				this.OnPropertyChanged("Description");
			}
		}

		/// <summary>
		/// 获取当前应用程序的自定义设置提供程序。
		/// </summary>
		public Options.ISettingsProvider Settings
		{
			get
			{
				if(_settings == null)
					System.Threading.Interlocked.CompareExchange(ref _settings, this.OptionManager.Settings, null);

				return _settings;
			}
			protected set
			{
				_settings = value;
			}
		}

		/// <summary>
		/// 获取当前应用程序的选项管理。
		/// </summary>
		public Options.OptionManager OptionManager
		{
			get
			{
				return Options.OptionManager.Default;
			}
		}

		/// <summary>
		/// 获取当前应用程序的默认选项配置。
		/// </summary>
		public virtual Options.Configuration.OptionConfiguration Configuration
		{
			get
			{
				return null;
			}
		}

		/// <summary>
		/// 获取当前应用程序的服务管理对象。
		/// </summary>
		public virtual Services.IServiceProviderFactory ServiceFactory
		{
			get
			{
				return Services.ServiceProviderFactory.Instance;
			}
		}

		/// <summary>
		/// 获取当前应用程序的模块集合。
		/// </summary>
		public virtual ICollection<IApplicationModule> Modules
		{
			get
			{
				if(_modules == null)
					System.Threading.Interlocked.CompareExchange(ref _modules, new Collections.Collection<IApplicationModule>(), null);

				return _modules;
			}
		}

		#endregion

		#region 构造方法

		protected ApplicationContextBase()
		{
		}

		protected ApplicationContextBase(string applicationId)
		{
			if(string.IsNullOrWhiteSpace(applicationId))
				throw new ArgumentNullException(nameof(applicationId));

			//初始化属性变量
			_applicationId = applicationId;
		}

		#endregion

		#region 公共方法

		/// <summary>
		/// 确认指定的当前应用程序的相对目录是否存在，如果不存在则依次创建它们，并返回其对应的完整路径。
		/// </summary>
		/// <param name="relativePath">相对于应用程序根目录的相对路径，可使用'/'或'\'字符作为相对路径的分隔符。</param>
		/// <returns>如果<paramref name="relativePath"/>参数为空或者全空白字符则返回应用程序根目录(即<see cref="ApplicationDirectory"/>属性值。)，否则返回其相对路径的完整路径。</returns>
		public string EnsureDirectory(string relativePath)
		{
			string fullPath = this.ApplicationDirectory;

			if(string.IsNullOrWhiteSpace(relativePath))
				return fullPath;

			var parts = relativePath.Split('/', '\\', Path.DirectorySeparatorChar);

			foreach(var part in parts)
			{
				if(string.IsNullOrWhiteSpace(part))
					continue;

				fullPath = Path.Combine(fullPath, part);

				if(!Directory.Exists(fullPath))
					Directory.CreateDirectory(fullPath);
			}

			return fullPath;
		}

		#endregion

		#region 重写方法

		public override string ToString()
		{
			return string.Format("[{0}] {1}", this.ApplicationId, this.Title);
		}

		#endregion

		#region 激发事件

		protected void OnPropertyChanged(string propertyName)
		{
			if(string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");

			this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}

		protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			var propertyChanged = this.PropertyChanged;

			if(propertyChanged != null)
				propertyChanged(this, e);
		}

		protected virtual void OnExiting(CancelEventArgs args)
		{
			var exiting = this.Exiting;

			if(exiting != null)
				exiting(null, args);
		}

		protected virtual void OnStarting(ApplicationEventArgs args)
		{
			var starting = this.Starting;

			if(starting != null)
				starting(null, args);
		}

		protected virtual void OnStarted(ApplicationEventArgs args)
		{
			var started = this.Started;

			if(started != null)
				started(null, args);
		}

		protected virtual void OnInitializing(ApplicationEventArgs args)
		{
			var initializing = this.Initializing;

			if(initializing != null)
				initializing(this, args);
		}

		protected virtual void OnInitialized(ApplicationEventArgs args)
		{
			var initialized = this.Initialized;

			if(initialized != null)
				initialized(this, args);
		}

		#endregion
	}
}
