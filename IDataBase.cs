using System.Data;

namespace viewer3d
{
	/* Интерфейс для работы с БД */
	public interface IDataBase
	{
		DataSet RequestToData(string sql);
		int Insert(string sql);
    }
}
