using InnergyTask.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InnergyTask.Domain.StockWriters
{
	public class TextStockWriter : IStockWriter
	{
		private Action<string> _writer;

		public TextStockWriter(Action<string> lineWriter)
		{
			_writer = lineWriter;
		}

		public Func<Warehouse, string> FormatWarehouse => w => $"{w.Name} (total {w.TotalQuantity})";
		public Func<Supply, string> FormatSupply => s => $"{s.Material.Id}: {s.Quantity}";

		public void Write(Stock stock)
		{
			int count = 0;
			foreach (Warehouse warehouse in OrderWarehouses(stock.Warehouses))
			{
				if (count++ > 0)
					_writer(string.Empty);

				_writer(FormatWarehouse(warehouse));
				foreach (Supply supply in OrderSupplies(warehouse.Supplies))
					_writer(FormatSupply(supply));
			}
		}

		protected virtual IEnumerable<Warehouse> OrderWarehouses(IEnumerable<Warehouse> warehouses)
			=> warehouses
				.OrderByDescending(w => w.TotalQuantity)
				.ThenByDescending(w => w.Name);

		protected virtual IEnumerable<Supply> OrderSupplies(IEnumerable<Supply> supplies)
			=> supplies.OrderBy(s => s.Material.Id);
	}
}