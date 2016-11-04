using System;
using System.IO;
using System.Threading;
using System.Collections.Concurrent;

namespace Tiandao.Diagnostics
{
	public abstract class FileLogger : ILogger
	{
		#region 常量定义

		private const int KB = 1024;
		private const int FILE_SIZE = 1024;

		#endregion

		#region 私有字段

		private string _filePath;
		private int _fileSize;
		private readonly object _syncRoot;
		private readonly ConcurrentQueue<LogEntry> _queue;

		#endregion

		#region 公共属性

		public string FilePath
		{
			get
			{
				return _filePath;
			}
			set
			{
				_filePath = value;
			}
		}

		/// <summary>
		/// 获取或设置日志文件的大小，单位为KB，默认为1MB。
		/// </summary>
		public int FileSize
		{
			get
			{
				return _fileSize;
			}
			set
			{
				_fileSize = value;
			}
		}

		#endregion

		#region 构造方法

		protected FileLogger()
		{
			_fileSize = FILE_SIZE;
			_syncRoot = new object();
			_queue = new ConcurrentQueue<LogEntry>();
		}

		protected FileLogger(string filePath, int fileSize = FILE_SIZE)
		{
			if(string.IsNullOrWhiteSpace(filePath))
				throw new ArgumentNullException("filePath");

			_filePath = filePath.Trim();
			_fileSize = Math.Max(fileSize, 0);
			_syncRoot = new object();
			_queue = new ConcurrentQueue<LogEntry>();
		}

		#endregion

		#region 日志方法

		public void Log(LogEntry entry)
		{
			if(entry == null)
				return;

			var filePath = this.ResolveSequence(entry);

			if(string.IsNullOrWhiteSpace(filePath))
				throw new InvalidOperationException("Unspecified path of the log file.");

			//将日志实体加入内存队列中
			_queue.Enqueue(entry);

			//从线程池拉出一个后台线程进行具体的日志记录操作
			ThreadPool.QueueUserWorkItem(this.LogFile, filePath);
		}

		private void LogFile(object filePath)
		{
			Stream stream = null;

			try
			{
				//当前线程获取日志写入锁
				Monitor.Enter(_syncRoot);

				//以写模式打开日志文件
				stream = new FileStream((string)filePath, FileMode.Append, FileAccess.Write, FileShare.Read);
				LogEntry entry;

				//从日志队列中取出一条日志信息
				while(_queue.TryDequeue(out entry))
				{
					//将当前日志信息写入日志文件流
					this.WriteLog(entry, stream);
				}
			}
			finally
			{
				//如果当前线程是日志写入线程
				if(Monitor.IsEntered(_syncRoot))
				{
					//关闭日志文件流
					if(stream != null)
						stream.Dispose();

					//释放日志写入锁
					Monitor.Exit(_syncRoot);
				}
			}
		}

		#endregion

		#region 抽象方法

		protected abstract void WriteLog(LogEntry entry, Stream output);

		#endregion

		#region 虚拟方法

		protected virtual string GetFilePath(LogEntry entry)
		{
			var filePath = string.Empty;

			if(string.IsNullOrWhiteSpace(_filePath))
				filePath = string.IsNullOrWhiteSpace(entry.Source) ? string.Empty : entry.Source + ".log";
			else
				filePath = Logger.TemplateManager.Evaluate<string>(_filePath.Trim(), entry);

			if(string.IsNullOrWhiteSpace(filePath))
				return null;

			filePath = filePath.Replace((Path.DirectorySeparatorChar == '/' ? '\\' : '/'), Path.DirectorySeparatorChar).Trim();

			if(filePath[0] == '/' || filePath[0] == '\\')
			{
				filePath = Path.Combine(Path.GetPathRoot(ComponentModel.ApplicationContextBase.Current.ApplicationDirectory), filePath.Substring(1));

				if(!Directory.Exists(Path.GetDirectoryName(filePath)))
					Directory.CreateDirectory(Path.GetDirectoryName(filePath));

				return filePath;
			}

			string directoryPath;

			if(filePath.StartsWith("~/") || filePath.StartsWith("~\\"))
				directoryPath = ComponentModel.ApplicationContextBase.Current.EnsureDirectory(Path.GetDirectoryName(filePath.Substring(2)));
			else
				directoryPath = ComponentModel.ApplicationContextBase.Current.EnsureDirectory(Path.GetDirectoryName(filePath));

			return Path.Combine(directoryPath, Path.GetFileName(filePath));
		}

		#endregion

		#region 私有方法

		private string ResolveSequence(LogEntry entry)
		{
			const string PATTERN = @"(?<no>\d+)";
			const string SEQUENCE = "{sequence}";

			var maximum = 0;
			var result = string.Empty;
			var filePath = this.GetFilePath(entry);

			if(string.IsNullOrEmpty(filePath) || (!filePath.Contains(SEQUENCE)))
				return filePath;

			if(this.FileSize < 1)
				return filePath.Replace(SEQUENCE, string.Empty);

			var fileName = System.IO.Path.GetFileName(filePath);
			var infos = IO.LocalFileSystem.Instance.Directory.GetFiles(System.IO.Path.GetDirectoryName(filePath), fileName.Replace(SEQUENCE, "|" + PATTERN + "|"), false);
			var pattern = string.Empty;
			int index = 0, position = 0;

			while((index = fileName.IndexOf(SEQUENCE, index)) >= 0)
			{
				if(index > 0)
					pattern += IO.LocalFileSystem.LocalDirectoryProvider.EscapePattern(fileName.Substring(position, index - position));

				pattern += PATTERN;
				index += SEQUENCE.Length;
				position = index;
			}

			if(position < fileName.Length)
				pattern += IO.LocalFileSystem.LocalDirectoryProvider.EscapePattern(fileName.Substring(position));

			//设置正则匹配模式为完整匹配
			if(pattern != null && pattern.Length > 0)
				pattern = "^" + pattern + "$";

			foreach(var info in infos)
			{
				var match = System.Text.RegularExpressions.Regex.Match(info.Name, pattern);

				if(match.Success)
				{
					var number = int.Parse(match.Groups["no"].Value);

					if(number > maximum)
					{
						maximum = number;

						if(info.Size < this.FileSize * KB)
							result = info.Path.Url;
					}
				}
			}

			if(string.IsNullOrEmpty(result))
				return filePath.Replace(SEQUENCE, (maximum + 1).ToString());

			return result;
		}

		#endregion
	}
}
