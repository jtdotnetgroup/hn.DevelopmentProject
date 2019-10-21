using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using Oracle.ManagedDataAccess.Client;

namespace hn.AutoSyncLib.Common
{
    public class OracleDBHelper
    {
        public DbConnection conn { get; set; }

        private static OracleClientFactory factory = new OracleClientFactory();

        public OracleDBHelper(string conStr)
        {
            conn = factory.CreateConnection();
            conn.ConnectionString = conStr;
        }

        public int ExecuteNonQuery(string sql, Dictionary<string, object> pars)
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                var cmd = factory.CreateCommand();
                cmd.Connection = conn;
                cmd.CommandText = sql;

                foreach (var key in pars.Keys)
                {
                    cmd.Parameters.Add(new OracleParameter(key, pars[key]));
                }

                return cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.Out.WriteLineAsync(e.Message);
                Console.Out.WriteLineAsync("SQL:" + sql);
                throw;
            }

        }

        public object ExecuteScalar(string sql, Dictionary<string, object> pars)
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                var cmd = factory.CreateCommand();
                cmd.Connection = conn;
                cmd.CommandText = sql;

                foreach (var key in pars.Keys)
                {
                    cmd.Parameters.Add(new OracleParameter(key, pars[key]));
                }

                return cmd.ExecuteScalar();
            }
            catch (Exception e)
            {

                Console.Out.WriteLineAsync(e.Message);
                Console.Out.WriteLineAsync("SQL:" + sql);
                throw;
            }
        }

        public string GetInsertSql<T>()
        {
            var t = typeof(T);

            var pis = t.GetProperties().ToList();

            var tableAttr = t.CustomAttributes.FirstOrDefault(p => p.AttributeType == typeof(TableAttribute));
            var tableName = tableAttr.ConstructorArguments.First().Value;

            var strbuilder = new StringBuilder();
            strbuilder.AppendFormat("INSERT INTO {0} ", tableName);

            string fields = "(";
            string values = "VALUES (";

            pis.ForEach(p =>
            {
                string fieldName = p.Name;

                var column = p.CustomAttributes.SingleOrDefault(o => o.AttributeType == typeof(ColumnAttribute));

                if (column != null)
                {
                    fieldName = column.ConstructorArguments.First().Value.ToString();
                }

                fields += fieldName;

                values += ":" + fieldName;

                if (p == pis.Last())
                {
                    fields += ")";
                    values += ")";
                }
                else
                {
                    fields += ",";
                    values += ",";
                }
            });

            strbuilder.Append(fields);
            strbuilder.Append(values);

            return strbuilder.ToString();
        }

        public string GetUpdateSql<T>(string where)
        {
            var t = typeof(T);

            var pis = t.GetProperties().ToList();

            var tableAttr = t.CustomAttributes.FirstOrDefault(p => p.AttributeType == typeof(TableAttribute));
            var tableName = tableAttr.ConstructorArguments.First().Value;

            var strbuilder = new StringBuilder();
            strbuilder.AppendFormat("UPDATE {0} SET ", tableName);

            string fields = "";

            pis.ForEach(p =>
            {
                string fieldName = p.Name;

                var column = p.CustomAttributes.SingleOrDefault(o => o.AttributeType == typeof(ColumnAttribute));

                if (column != null)
                {
                    fieldName = column.ConstructorArguments.First().Value.ToString();
                }

                fields += fieldName + "=:" + fieldName;

                if (p == pis.Last())
                {
                    fields += " WHERE 1=1 ";
                }
                else
                {
                    fields += ",";
                }
            });

            strbuilder.Append(fields);
            strbuilder.Append(where);
            return strbuilder.ToString();
        }

        public bool Update<T>(T obj, string where)
        {
            var sql = GetUpdateSql<T>(where);
            var cmd = GetCommand(sql, obj);
            cmd.Connection = conn;

            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception e)
            {
                Console.Out.WriteLineAsync(e.Message);
                Console.Out.WriteLineAsync("SQL:" + sql);
                throw;
            }

        }

        public bool Insert<T>(T obj)
        {
            var sql = GetInsertSql<T>();
            var cmd = GetCommand(sql, obj);
            cmd.Connection = conn;

            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception e)
            {
                Console.Out.WriteLineAsync(e.Message);
                Console.Out.WriteLineAsync("SQL:" + sql);
                throw;
            }
        }

        private DbCommand GetCommand<T>(string sql, T par)
        {
            var cmd = factory.CreateCommand();
            cmd.CommandText = sql;
            cmd.Connection = conn;

            var t = typeof(T);
            var pis = t.GetProperties().ToList();

            pis.ForEach(p =>
            {
                var value = p.GetValue(par);
                if (value == null)
                    value = "";

                cmd.Parameters.Add(new OracleParameter(":" + p.Name, value));
            });

            return cmd;
        }

        private DbCommand GetCommand<T>(string sql, List<T> pars)
        {
            var cmd = factory.CreateCommand();
            cmd.CommandText = sql;
            cmd.Connection = conn;

            var t = typeof(T);
            var pis = t.GetProperties().ToList();

            pars.ForEach(par =>
            {
                pis.ForEach(p =>
                {
                    var value = p.GetValue(par);
                    if (value == null)
                        value = "";

                    cmd.Parameters.Add(new OracleParameter(":" + p.Name, value));
                });
            });


            return cmd;
        }


        public int Delete<T>(string where)
        {


            var t = typeof(T);

            var pis = t.GetProperties().ToList();

            var tableAttr = t.CustomAttributes.FirstOrDefault(p => p.AttributeType == typeof(TableAttribute));
            var tableName = tableAttr.ConstructorArguments.First().Value;

            Console.Out.WriteLineAsync($"清除{tableName}全表");
            LogHelper.LogInfo($"清除{tableName}全表");

            string sql = string.Format("DELETE FROM {0} WHERE 1=1 {1}", tableName, where);

            var cmd = factory.CreateCommand();
            cmd.CommandText = sql;
            cmd.Connection = conn;

            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                return cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.Out.WriteLineAsync(e.Message);
                Console.Out.WriteLineAsync("SQL:" + sql);
                throw;
            }

        }

        public DataTable Select(string sql)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            var cmd = factory.CreateCommand();
            cmd.CommandText = sql;
            cmd.Connection = conn;
            var da = factory.CreateDataAdapter();
            da.SelectCommand = cmd;
            DataTable table = new DataTable();
            try
            {
                da.Fill(table);
            }
            catch (Exception e)
            {
                Console.Out.WriteLineAsync($"SQL:{sql}\n异常：{e.Message}");
                throw;
            }

            return table;
        }

        public List<T> Select<T>(string sql) where T : new()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            var cmd = factory.CreateCommand();
            cmd.CommandText = sql;
            cmd.Connection = conn;
            var da = factory.CreateDataAdapter();
            da.SelectCommand = cmd;
            DataTable table = new DataTable();
            da.Fill(table);
            //反射获得泛型类信息
            Type t = typeof(T);
            //获得泛型类所有公共字段
            List<PropertyInfo> pisInfos = t.GetProperties().ToList();
            //最终返回的对象列表
            List<T> result = new List<T>();
            foreach (DataRow row in table.Rows)
            {
                T item = new T();
                foreach (DataColumn col in table.Columns)
                {
                    //这里假设字段名和数据库列名是一 一对应
                    //可能通过字段名与列名进行比较，相同则进行取值赋值操作
                    PropertyInfo pi = pisInfos.FirstOrDefault(p => p.Name == col.ColumnName);
                    if (pi != null)
                    {
                        object value = row[col.ColumnName];
                        pi.SetValue(item, value);
                    }
                }
                result.Add(item);
            }

            return result;
        }

        public bool BatchInsert<T>(List<T> data)
        {
            if (data == null || data.Count == 0)
            {
                return false;
            }
            string sql = "INSERT ALL \n";

            foreach (var row in data)
            {
                var str = GetInsertSql<T>().Replace("INSERT", "");
                sql += str + "\n";
            }

            sql += "SELECT 1 FROM DUAL";

            var cmd = GetCommand(sql, data);

            cmd.Connection = conn;

            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                var result = cmd.ExecuteNonQuery() > 0;
                conn.Close();
                return result;
            }
            catch (Exception e)
            {
                Console.Out.WriteLineAsync(e.Message);
                Console.Out.WriteLineAsync("SQL:" + sql);
                throw;
            }

            return false;
        }

        public bool BatchUpdate<T>(List<T> data, string where)
        {
            if (data == null || data.Count == 0)
            {
                return false;
            }
            var now = DateTime.Now;
            var sql = GetUpdateSql<T>(where);

            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                data.ForEach(row =>
                {
                    var cmd = GetCommand(sql, row);
                    cmd.Connection = conn;
                    cmd.ExecuteNonQuery();
                });
                var timespan = DateTime.Now - now;
                Console.Out.WriteLineAsync($"批量更新完成耗时：{timespan.Hours}时{timespan.Minutes}分{timespan.Seconds}秒，共更新{data.Count}条数据");
                return true;
            }
            catch (Exception e)
            {
                Console.Out.WriteLineAsync($"批量更新失败\n异常：{e.Message}");
                Console.Out.WriteLineAsync("SQL:" + sql);
                throw;
            }
        }
    }
}