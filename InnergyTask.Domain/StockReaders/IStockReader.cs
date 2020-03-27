using InnergyTask.Domain.Entities;

namespace InnergyTask.Domain.StockReaders
{
	public interface IStockReader
	{
		Stock Read();
	}
}