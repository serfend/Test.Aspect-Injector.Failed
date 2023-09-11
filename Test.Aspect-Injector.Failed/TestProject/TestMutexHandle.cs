using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectCommon.Attribute;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestProject.Common.MutexHandler
{
	[TestClass]
	public class TestMutexHandle
	{
		private int counter = 0;
		[MutexHandler(Name = nameof(TestMutexHandle))]
		public void TestMethod(int delay = 1000)
		{
			counter++;
			Thread.Sleep(delay);
			counter--;
		}
		[TestMethod]
		public void TestMutex()
		{
			ThreadPool.SetMaxThreads(20, (int)1e6);
			var callback = new WaitCallback((x) =>
			{
				TestMethod(50);
			});
			for (var i = 0; i < 10; i++)
			{
				ThreadPool.QueueUserWorkItem(callback);
			}
			Thread.Sleep(20);
			Assert.IsTrue(counter == 1, $"应同时最多运行一个任务，实际为:{counter}");
			Thread.Sleep(1000);
			Assert.IsTrue(counter == 0, $"全部任务运行完后应归零，实际为:{counter}");
		}
	}
}
