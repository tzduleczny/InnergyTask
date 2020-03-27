using System.Collections.Generic;

namespace InnergyTask.Domain.Entities
{
	public class Material
	{
		public Material(string id, string name)
			=> (Id, Name) = (id, name);

		public string Id { get; private set; }
		public string Name { get; private set; }
		public List<Supply> Supplies { get; private set; } = new List<Supply>();

		public override bool Equals(object obj)
		{
			var other = obj as Material;
			return other != null && other.Id.Equals(Id);
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}

		public override string ToString()
		{
			return Id;
		}
	}
}