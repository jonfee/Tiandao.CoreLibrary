# 概述

CoreLibrary 类库提供了.NET开发的常用功能集以及一些相对于.NET BCL中进行了功能强化的实现。采用C#语言开发，支持跨平台可运行在 Windows、Windows Phone 8.x 以及 Linux(Mono) 等平台中。

-----

# 更新日志

## V1.2.0
* 调整针对 NET45/NET46/CORE 的支持。

## V1.1.0
* 扩展随机数生成类。

## V1.0.0
* 升级至 .NET Core RTM 版本。

## V1.1.5-beta
* 修复 ApplicationDirectory 为 Null 的问题。
* 修复 AssemblyManager 类获取程序集名称的问题。

## V1.1.4-beta
* 基于 .NET Core RC2 升级了部份代码。

## V1.1.3-beta
* 修正 Location Equals Bug。
* 新增一个多边形电子围栏监测函数。

## V1.1.2-beta
* 新增资源文件读取类(ResourceUtility)
* 优化枚举工具类，读取 Description 时，优先解析资源表达式。

## V1.1.1-beta
* 新增一个服务依赖表述类。
* 新增与程序集加载相关的类。

## V1.1.0-beta
* 新增对 Microsoft 的INI文件格式的各项操作。

## V1.0.9-beta
* 增加对内存缓存的测试。
* 修复配置多个同类型日志记录器失效的错误。
* 修复日志文件匹配错乱的问题。
* 修复文件查找模式的错误。
* 为 IDictionary<,> 增加一个 TryGetValue(...) 扩展方法。
* 增强 SetValue(...) 方法的功能。 
* 增加一个键值对的结构。
* 增加两个判断数值类型的方法。
* 对不存在的成员抛出的异常改为：ArgumentException -> KeyNotFoundException
* 完善对单值类型的判断。
* 完善转换的功能。
* 为服务容器增加一个获取指定名称的服务类型的方法。

## V1.0.8-beta
* 增加一个对服务容器的测试案例。
* 重构服务存储的逻辑，功能更强大了对于继承者而言变得更易用了。
* 修复服务条目的隐性逻辑错误。
* 增加一个特性，使得服务的查找可以跨服务容器。 
* 为 IServiceProvider 接口增加一个 Storage 属性。
* 移除服务容器的扩展类，这些扩展功能由服务容器的新增成员实现了。 
* 为服务容器增加了几个解析方法。
* 增加对路径的测试。
* 调整路径解析的逻辑。确保无效的方案名始终返回空。 
* 调整文件系统默认方案的属性逻辑。
* 调整 FileSystem 部分代码。

## V1.0.7.3-beta
* 修复 LoggerHandlerElement 类 Properties 属性获取问题。

## V1.0.7.2-beta
* 修复获取自定义 Attribute 的 Bug。

## V1.0.7.1-beta
* 修复 IApplicationModule 无法释放的Bug。

## V1.0.7-beta
* 添加与异常处理相关的类。
* 添加一些基于 Type 类的实用扩展方法。

## V1.0.6-beta
* 添加 Caching 命名空间，包含与缓存相关的类。
* 添加 LBS 命名空间，包含与位置服务相关的类。
* 添加与日期处理相关的实用类(DateTimeUtility)。
* 补充基于 Object、Guid、String、Type等类的扩展方法。

## V1.0.5-beta
* 新增 Diagnostics 命名空间，包含与日至诊断相关的类。
* 新增 IO 命名空间，包含与 IO 相关的类。
* 新增 Services 命名空间，包含与服务命令相关的类。
* 新增 Text 命名空间，包含与文本处理相关的类。
* 重大调整：使得 CoreLibrary 在 .NET Framework(DNX451) 与 .NET Core(DNXCORE50) 并存。

## V1.0.4-beta
* 新增 Options 命名空间，包含与配置相关的类。
* 新增应用程序上下文基类(ApplicationContextBase)。

## V1.0.3-beta
* 新增 Serialization 命名空间，新增若干与序列化相关的类。

## V1.0.2-beta
* 添加位于 Collections 命名空间下的常用集合类，Collection、NamedCollection。
* 添加与队列(Queue)相关的类。
* 添加与层级分类(Category)相关的类。
* 添加一个线程安全的对象缓存类(ObjectCache)。
* 添加一个线程安全的通用对象池的相关功能类（ObjectPool）。
* 添加基于字符串进行扩展的实用类(StringExtension)，后续版本会继续迭代该类。

## V1.0.1-beta
* 添加与枚举相关的类，包含：枚举描述项(EnumEntry)，枚举辅助(EnumUtility)等。
* 添加与类型转换相关的类，包含：枚举转换器、GUID转换器、字符编码转换器、IP地址转换器。
* 添加实用转换类(Converter)，包含类型转换、成员动态获取、成员动态赋值、字节文本处理等功能。
* 添加基于 System.Type 进行扩展的实用类(TypeExtension)。
