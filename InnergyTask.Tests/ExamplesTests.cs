using InnergyTask.Domain.Entities;
using InnergyTask.Domain.StockReaders;
using InnergyTask.Domain.StockWriters;
using System.IO;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace InnergyTask.Tests
{
	public class ExamplesTests : XUnitBase
	{
		private const string ResourceLocation = "InnergyTask.Tests.Examples";

		public ExamplesTests(ITestOutputHelper output) : base(output)
		{
		}

		[Fact]
		public void Example1()
		{
			using var inputReader = new StreamReader(GetResourceStream($"{ResourceLocation}.Example1_in.txt"));
			IStockReader reader = new TextStockReader(() => inputReader.ReadLine());

			StringBuilder output = new StringBuilder();
			IStockWriter writer = new TextStockWriter(line =>
			{
				output.AppendLine(line);
				Output.WriteLine(line);
			});

			Stock stock = reader.Read();
			writer.Write(stock);

			string expected = GetResourceText($"{ResourceLocation}.Example1_out.txt");
			Assert.Equal(expected, output.ToString());
		}
	}
}