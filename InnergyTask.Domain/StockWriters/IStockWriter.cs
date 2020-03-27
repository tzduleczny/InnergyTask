using InnergyTask.Domain.Entities;

namespace InnergyTask.Domain.StockWriters
{
	public interface IStockWriter
	{
		void Write(Stock stock);
	}
}