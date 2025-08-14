using System.Data;

namespace Project.DataAccess;

public interface IDataMapper<T> where T : class, new()
{
    T Map(IDataRecord reader);
}