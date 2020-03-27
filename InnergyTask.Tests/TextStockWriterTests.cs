using InnergyTask.Domain.Entities;
using InnergyTask.Domain.StockWriters;
using System.Text;
using Xunit;

namespace InnergyTask.Tests
{
	public class TextStockWriterTests
	{
		private string Write(Stock stock)
		{
			StringBuilder sb = new StringBuilder();
			IStockWriter writer = new TextStockWriter(line => sb.AppendLine(line));
			writer.Write(stock);
			return sb.ToString();
		}

		[Fact]
		public void OrderWarehousesByQuantityDesc()
		{
			var m1 = new Material("id1", "name1");
			var w1 = new Warehouse("w1");
			var w2 = new Warehouse("w2");
			var w3 = new Warehouse("w3");
			var s1 = new Supply(m1, w1, 2);
			var s2 = new Supply(m1, w2, 1);
			var s3 = new Supply(m1, w3, 3);
			w1.Supplies.Add(s1);
			w2.Supplies.Add(s2);
			w3.Supplies.Add(s3);

			Stock stock = new Stock();
			stock.Materials.Add(m1);
			stock.Warehouses.AddRange(new[] { w1, w2, w3 });

			string result = Write(stock);
			string expected =
				"w3 (total 3)\r\n" +
				"id1: 3\r\n\r\n" +
				"w1 (total 2)\r\n" +
				"id1: 2\r\n\r\n" +
				"w2 (total 1)\r\n" +
				"id1: 1\r\n";

			Assert.Equal(expected, result);
		}

		[Fact]
		public void OrderWarehousesByNameDesc()
		{
			var m1 = new Material("id1", "name1");
			var w1 = new Warehouse("w1");
			var w2 = new Warehouse("w2");
			var w3 = new Warehouse("w3");
			var s1 = new Supply(m1, w1, 1);
			var s2 = new Supply(m1, w2, 1);
			var s3 = new Supply(m1, w3, 1);
			w1.Supplies.Add(s1);
			w2.Supplies.Add(s2);
			w3.Supplies.Add(s3);

			Stock stock = new Stock();
			stock.Materials.Add(m1);
			stock.Warehouses.AddRange(new[] { w1, w2, w3 });

			string result = Write(stock);
			string expected =
				"w3 (total 1)\r\n" +
				"id1: 1\r\n\r\n" +
				"w2 (total 1)\r\n" +
				"id1: 1\r\n\r\n" +
				"w1 (total 1)\r\n" +
				"id1: 1\r\n";

			Assert.Equal(expected, result);
		}

		[Fact]
		public void OrderMaterialsById()
		{
			var m1 = new Material("id1", "nane1");
			var m2 = new Material("id2", "nane2");
			var m3 = new Material("id3", "nane3");
			var w1 = new Warehouse("w1");
			var s1 = new Supply(m1, w1, 3);
			var s2 = new Supply(m2, w1, 1);
			var s3 = new Supply(m3, w1, 2);
			w1.Supplies.Add(s1);
			w1.Supplies.Add(s2);
			w1.Supplies.Add(s3);

			Stock stock = new Stock();
			stock.Materials.AddRange(new[] { m2, m3, m1 });
			stock.Warehouses.AddRange(new[] { w1 });

			string result = Write(stock);
			string expected =
				"w1 (total 6)\r\n" +
				"id1: 3\r\n" +
				"id2: 1\r\n" +
				"id3: 2\r\n";

			Assert.Equal(expected, result);
		}

		[Fact]
		public void SeparateWarehousesByEmptyLine()
		{
			var m1 = new Material("id1", "name1");
			var w1 = new Warehouse("w1");
			var w2 = new Warehouse("w2");
			var s1 = new Supply(m1, w1, 1);
			var s2 = new Supply(m1, w2, 1);
			w1.Supplies.Add(s1);
			w2.Supplies.Add(s2);

			Stock stock = new Stock();
			stock.Materials.Add(m1);
			stock.Warehouses.AddRange(new[] { w1, w2 });

			string result = Write(stock);
			string expected =
				"w2 (total 1)\r\n" +
				"id1: 1\r\n\r\n" +
				"w1 (total 1)\r\n" +
				"id1: 1\r\n";

			Assert.Equal(expected, result);
		}
	}
}