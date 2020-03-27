using InnergyTask.Domain.Entities;
using InnergyTask.Domain.StockReaders;
using InnergyTask.Domain.StockWriters;
using System;

namespace InnergyTask.Console
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

			IStockReader reader = new TextStockReader(() => System.Console.ReadLine());
			IStockWriter writer = new TextStockWriter(line => System.Console.WriteLine(line));

			Stock stock = reader.Read();
			writer.Write(stock);
		}

		private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			System.Console.WriteLine($"UnhandledException: {e.ExceptionObject}");
		}
	}
}