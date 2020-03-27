namespace InnergyTask.Domain.Entities
{
	public class Supply
	{
		public Supply(Material material, Warehouse warehouse, long quantity = 0)
			=> (Material, Warehouse, Quantity) = (material, warehouse, quantity);

		public Material Material { get; private set; }
		public Warehouse Warehouse { get; private set; }
		public long Quantity { get; set; }

		public override string ToString()
		{
			return $"{Material}|{Warehouse}|{Quantity}";
		}
	}
}