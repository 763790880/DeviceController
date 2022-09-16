//需增加数据库初始化功能
//需增加ORM框架
//需设计数据库模型
using Microsoft.Data.Sqlite;
using System.Data;
using System.Data.SQLite;
using System.Reflection;

namespace SqlLiteHelper
{
    partial class SQLiteHelper : ISQLiteHelper
    {
        private static string path => GetPath();
        private static string connectionString = string.Format(@"Data Source={0}", $"{path}\\Sqlite.db");

        /// <summary>
        /// 适合增删改操作，返回影响条数
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql, params SQLiteParameter[] parameters)
        {
            using (SqliteConnection conn = new SqliteConnection(connectionString))
            {
                using (SqliteCommand comm = conn.CreateCommand())
                {
                    try
                    {
                        conn.Open();
                        comm.CommandText = sql;
                        if(parameters!=null)
                            comm.Parameters.AddRange(parameters);
                        return comm.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        if (conn != null && conn.State != ConnectionState.Closed)
                            conn.Close();
                    }

                }
            }
        }

        public static string GetPath()
        {
            var location = Assembly.GetEntryAssembly().Location;
            var directory = Path.GetDirectoryName(location);
            return directory;
        }
        /// <summary>
        /// 查询操作，返回查询结果中的第一行第一列的值
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public object ExecuteScalar(string sql, params SQLiteParameter[] parameters)
        {
            using SqliteConnection conn = new SqliteConnection(connectionString);
            using SqliteCommand comm = conn.CreateCommand();
            try
            {
                conn.Open();
                comm.CommandText = sql;
                comm.Parameters.AddRange(parameters);
                return comm.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (conn != null && conn.State != ConnectionState.Closed)
                    conn.Close();
            }
        }


        /// <summary>
        /// 执行ExecuteReader
        /// </summary>
        /// <param name="sqlText">SQL</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public SqliteDataReader ExecuteReader(string sql, params SQLiteParameter[] parameters)
        {
            try
            {
                //SqlDataReader要求，它读取数据的时候有，它独占它的SqlConnection对象，而且SqlConnection必须是Open状态
                using SqliteConnection conn = new SqliteConnection(connectionString);//不要释放连接，因为后面还需要连接打开状态
                SqliteCommand cmd = conn.CreateCommand();
                conn.Open();
                cmd.CommandText = sql;
                cmd.Parameters.AddRange(parameters);
                //CommandBehavior.CloseConnection当SqlDataReader释放的时候，顺便把SqlConnection对象也释放掉
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        /// <summary>
        /// Adapter调整，查询操作，返回DataTable
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public List<T> ExecuteDataTable<T>(string sql, params SQLiteParameter[] parameters) where T : class, new()
        {
            using SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, connectionString);
            DataTable dt = new DataTable();
            adapter.SelectCommand.Parameters.AddRange(parameters);
            adapter.Fill(dt);
            return DataTableToLists<T>(dt);

        }

        /// <summary>
        /// DataTable 转换为List 集合
        /// </summary>
        /// <typeparam name="TResult">类型</typeparam>
        /// <param name="dt">DataTable</param>
        /// <returns></returns>
        public List<TResult> DataTableToLists<TResult>(DataTable dt) where TResult : class, new()
        {
            //创建一个属性的列表
            List<PropertyInfo> prlist = new List<PropertyInfo>();
            //获取TResult的类型实例  反射的入口
            Type t = typeof(TResult);
            //获得TResult 的所有的Public 属性 并找出TResult属性和DataTable的列名称相同的属性(PropertyInfo) 并加入到属性列表
            Array.ForEach<PropertyInfo>(t.GetProperties(), p => { if (dt.Columns.IndexOf(p.Name) != -1) prlist.Add(p); });
            //创建返回的集合
            List<TResult> oblist = new List<TResult>();

            foreach (DataRow row in dt.Rows)
            {
                //创建TResult的实例
                TResult ob = new TResult();
                //找到对应的数据  并赋值
                prlist.ForEach(p => { if (row[p.Name] != DBNull.Value) p.SetValue(ob, row[p.Name], null); });
                //放入到返回的集合中.
                oblist.Add(ob);
            }
            return oblist;
        }


    }
}