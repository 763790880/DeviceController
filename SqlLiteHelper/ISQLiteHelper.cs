using Microsoft.Data.Sqlite;
using System.Data.SQLite;

namespace SqlLiteHelper
{
    public interface ISQLiteHelper
    {
        int ExecuteNonQuery(string sql, params SQLiteParameter[] parameters);
        object ExecuteScalar(string sql, params SQLiteParameter[] parameters);
        SqliteDataReader ExecuteReader(string sql, params SQLiteParameter[] parameters);
        List<T> ExecuteDataTable<T>(string sql, params SQLiteParameter[] parameters) where T : class, new();
    }
}
