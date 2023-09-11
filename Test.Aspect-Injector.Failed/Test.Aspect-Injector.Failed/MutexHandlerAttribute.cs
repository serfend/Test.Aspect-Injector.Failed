using AspectInjector.Broker;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;
using Common.Attribute;
using System.Threading;

namespace ProjectCommon.Attribute
{
	/// <summary>
	/// 使得单个方法同一时间只执行n次
	/// [MutexHandlerAttribute]
	/// void Foo(){}
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	[Aspect(AspectInjector.Broker.Scope.Global)]
	[Injection(typeof(MutexHandlerAttribute))]
	public class MutexHandlerAttribute : AttributeBase
	{
		/// <summary>
		/// 区分单个方法阻塞名称
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 最多同时能有多少个执行
		/// </summary>
		public int MaxCount { get; set; }
		/// <summary>
		/// 同步方法的描述
		/// </summary>
		public string? Description { get; set; }
		/// <summary>
		/// Inportant:Aspect need a argumentless ctor.
		/// </summary>
		public MutexHandlerAttribute() : this(null, 1, null) { }
		public MutexHandlerAttribute(string? name = null, int max_count = 1, string? description = null)
		{
			Name = name ?? "default";
			MaxCount = max_count;
			Description = description;
		}
		[Advice(Kind.Around, Targets = Target.Method)]
		public object OnEnter([Argument(Source.Name)] string name,
			[Argument(Source.Arguments)] object[] args,
			[Argument(Source.Type)] Type hostType,
			[Argument(Source.Target)] Func<object[], object> target,
			[Argument(Source.Triggers)] System.Attribute[] triggers)
		{
			var mutex = new Mutex(false, Name);
			mutex.WaitOne();
			var result = target(args);
			mutex.ReleaseMutex();
			return result;
		}
	}
}
