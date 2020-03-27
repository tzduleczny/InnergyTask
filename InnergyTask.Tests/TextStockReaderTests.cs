using InnergyTask.Domain.Entities;
using InnergyTask.Domain.StockReaders;
using System.IO;
using System.Text;
using Xunit;

namespace InnergyTask.Tests
{
	public class TextStockReaderTests
	{
		private Stock Read(string text)
		{
			using var streamReader = new StreamReader(new MemoryStream(Encoding.ASCII.GetBytes(text)));
			TextStockReader reader = new TextStockReader(() => streamReader.ReadLine());
			return reader.Read();
		}

		[Fact]
		public void IgnoreComment()
		{
			Stock stock = Read("# New materials");
			Assert.True(stock.Materials.Count == 0);
			Assert.True(stock.Warehouses.Count == 0);
		}

		[Fact]
		public void ReadProperLine_OneWarehouse()
		{
			Stock stock = Read("Cherry Hardwood Arched Door - PS;COM-100001;WH-A,5");
			Assert.True(stock.Materials.Count == 1);
			Assert.True(stock.Warehouses.Count == 1);
			Assert.True(stock.Materials[0].Supplies.Count == 1);
			Assert.True(stock.Warehouses[0].Supplies.Count == 1);
			Assert.Equal("Cherry Hardwood Arched Door - PS", stock.Materials[0].Name);
			Assert.Equal("COM-100001", stock.Materials[0].Id);
			Assert.Equal("WH-A", stock.Warehouses[0].Name);
			Assert.Equal(5, stock.Materials[0].Supplies[0].Quantity);
		}

		[Fact]
		public void ReadProperLine_MultipleWarehouses()
		{
			Stock stock = Read("Generic Wire Pull;COM-123906c;WH-A,10|WH-B,6|WH-C,2");
			Assert.True(stock.Materials.Count == 1);
			Assert.True(stock.Warehouses.Count == 3);
			Assert.True(stock.Materials[0].Supplies.Count == 3);
			Assert.True(stock.Warehouses[0].Supplies.Count == 1);
			Assert.True(stock.Warehouses[1].Supplies.Count == 1);
			Assert.True(stock.Warehouses[2].Supplies.Count == 1);
			Assert.Equal("Generic Wire Pull", stock.Materials[0].Name);
			Assert.Equal("COM-123906c", stock.Materials[0].Id);
			Assert.Equal("WH-A", stock.Warehouses[0].Name);
			Assert.Equal("WH-B", stock.Warehouses[1].Name);
			Assert.Equal("WH-C", stock.Warehouses[2].Name);
			Assert.Equal(10, stock.Materials[0].Supplies[0].Quantity);
			Assert.Equal(6, stock.Materials[0].Supplies[1].Quantity);
			Assert.Equal(2, stock.Materials[0].Supplies[2].Quantity);
		}

		[Fact]
		public void DuplicatedMaterial()
		{
			Stock stock = Read("MDF;COM-101734;WH-A,10\r\nMDF;COM-101734;WH-A,15");
			Assert.True(stock.Materials.Count == 1);
			Assert.True(stock.Warehouses.Count == 1);
			Assert.True(stock.Materials[0].Supplies.Count == 1);
			Assert.Equal(25, stock.Materials[0].Supplies[0].Quantity);
		}
	}
}