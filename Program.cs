using DNCTool.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNCTool
{
	class Program
	{
		static async Task Main(string[] args)
		{
			HttpServer httpServer = new HttpServer(new string[] { "http://*:9988/" });
			Process.Start("http://localhost:9988");	// Mở liên kết bằng trình duyệt mặc định
			await httpServer.StartAsync();
			Console.ReadKey();
		}
	}
}
