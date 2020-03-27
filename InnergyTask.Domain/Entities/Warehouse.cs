using System.Collections.Generic;
using System.Linq;

namespace InnergyTask.Domain.Entities
{
	public class Warehouse
	{
		public Warehouse(string name) => Name = name;

		public string Name { get; private set; }
		public List<Supply> Supplies { get; private set; } = new List<Supply>();
		public long TotalQuantity => Supplies?.Sum(s => s.Quantity) ?? 0;

		public override bool Equals(object obj)
		{
			var other = obj as Warehouse;
			return other != null && other.Name.Equals(Name);
		}

		public override int GetHashCode()
		{
			return Name.GetHashCode();
		}

		public override string ToString()
		{
			return Name;
		}
	}
}