using InnergyTask.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InnergyTask.Domain.StockReaders
{
	public class TextStockReader : IStockReader
	{
		private readonly Func<string> _reader;

		public TextStockReader(Func<string> reader)
		{
			_reader = reader;
		}

		public string CommentMark { get; set; } = "#";
		public char MaterialDataSeparator { get; set; } = ';';
		public char SupplySeparator { get; set; } = '|';
		public char SupplyDataSeparator { get; set; } = ',';

		public Stock Read()
		{
			var materials = new Dictionary<string, Material>();
			var warehouses = new Dictionary<string, Warehouse>();

			string line;
			while (!string.IsNullOrWhiteSpace(line = _reader()))
			{
				if (line.StartsWith(CommentMark))
					continue;

				string[] data = line.Split(MaterialDataSeparator);
				string materialId = data[1];

				if (!materials.TryGetValue(materialId, out Material material))
					materials[materialId] = material = new Material(materialId, name: data[0]);

				ReadSupplies(data[2], material, warehouses);
			}

			var stock = new Stock();
			stock.Materials.AddRange(materials.Values);
			stock.Warehouses.AddRange(warehouses.Values);
			return stock;
		}

		protected virtual void ReadSupplies(string text, Material material, Dictionary<string, Warehouse> warehouses)
		{
			foreach (string data in text.Split(SupplySeparator))
			{
				string[] pair = data.Split(SupplyDataSeparator);
				string warehouseName = pair[0];
				long quantity = long.Parse(pair[1]);

				if (!warehouses.TryGetValue(warehouseName, out Warehouse warehouse))
					warehouse = warehouses[warehouseName] = new Warehouse(warehouseName);

				Supply supply = material.Supplies.FirstOrDefault(s =>
					s.Warehouse.Equals(warehouse) && s.Material.Equals(material));

				if (supply == null)
				{
					supply = new Supply(material, warehouse, quantity);
					material.Supplies.Add(supply);
					warehouse.Supplies.Add(supply);
				}
				else
					supply.Quantity += quantity;
			}
		}
	}
}