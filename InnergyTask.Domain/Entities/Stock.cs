using System.Collections.Generic;

namespace InnergyTask.Domain.Entities
{
	public class Stock
	{
		public List<Material> Materials { get; private set; } = new List<Material>();
		public List<Warehouse> Warehouses { get; private set; } = new List<Warehouse>();
	}
}