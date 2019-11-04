using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using log4net;
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
                LogHelper.Error(e);
                LogHelper.Info("SQL:" + sql);
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

                LogHelper.Error(e);
                LogHelper.Info("SQL:" + sql);
                throw;
            }
        }

        public string GetInsertSql<T>()
        {
            var t = typeof(T);
            
            var pis = t.GetProperties().Where(p=>p.GetCustomAttributes().Count(pi => pi.GetType() == typeof(NotMappedAttribute)) == 0).ToList();

            var tableAttr = t.GetCustomAttributes(true).FirstOrDefault(p => p.GetType() == typeof(TableAttribute)) as TableAttribute;
            var tableName = tableAttr.Name;

            var strbuilder = new StringBuilder();
            strbuilder.AppendFormat("INSERT INTO {0} ", tableName);

            string fields = "(";
            string values = "VALUES (";

            pis.ForEach(p =>
            {
                    string fieldName = p.Name;

                    if (p.GetCustomAttributes(true).SingleOrDefault(o => o.GetType() == typeof(ColumnAttribute)) is
                        ColumnAttribute column)
                    {
                        fieldName = column.Name;
                    }

                    fields += fieldName;

                    values += ":" + p.Name;

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

            var pis = t.GetProperties().Where(p => p.GetCustomAttributes().Count(pi => pi.GetType() == typeof(NotMappedAttribute)) == 0).ToList();

            var tableAttr = t.GetCustomAttributes(true).FirstOrDefault(p => p.GetType() == typeof(TableAttribute)) as TableAttribute;
            var tableName = tableAttr.Name;

            var strbuilder = new StringBuilder();
            strbuilder.AppendFormat("UPDATE {0} SET ", tableName);

            string fields = "";

            pis.ForEach(p =>
            {

                    string fieldName = p.Name;

                    if (p.GetCustomAttributes(true).SingleOrDefault(o => o.GetType() == typeof(ColumnAttribute)) is
                        ColumnAttribute column)
                    {
                        fieldName = column.Name;
                    }

                    fields += fieldName + "=:" + p.Name;

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
                LogHelper.Error(e);
                LogHelper.Info("SQL:" + sql);
                throw;
            }

        }

        public bool Update<T>(T obj)
        {
            var t = typeof(T);
            var pis = t.GetProperties();

            string keyFieldName = "";

            foreach (var pi in pis)
            {
                var keyAttr = pi.GetCustomAttributes(true).Count(p => p is KeyAttribute) == 1;
                if (keyAttr)
                {
                    keyFieldName = pi.Name;
                    break;
                }
            }

            if (string.IsNullOrEmpty(keyFieldName))
            {
                throw new ArgumentException(string.Format("{0}类未指定Key字段", t.Name));
            }
            string where = string.Format(" AND {0}=:{1}", keyFieldName, keyFieldName);
            string sql = GetUpdateSql<T>(where);

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
                LogHelper.Error(e);
                LogHelper.Info("SQL:" + sql);
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
                LogHelper.Error(e);
                LogHelper.Info("SQL:" + sql);
                throw;
            }
        }

        private DbCommand GetCommand<T>(string sql, T par)
        {
            var cmd = factory.CreateCommand();
            cmd.CommandText = sql;
            cmd.Connection = conn;

            var t = typeof(T);
            var pis = t.GetProperties().Where(p=>p.GetCustomAttributes().Count(pi=>pi.GetType()==typeof(NotMappedAttribute))==0).ToList();

            pis.ForEach(p =>
            {
                var value = p.GetValue(par, null);
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
                    var value = p.GetValue(par, null);
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

            var tableAttr = t.GetCustomAttributes(true).FirstOrDefault(p => p.GetType() == typeof(TableAttribute)) as TableAttribute;
            var tableName = tableAttr.Name;

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
                LogHelper.Error(e);
                LogHelper.Info("SQL:" + sql);
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
                LogHelper.Info($"SQL:{sql}\n异常：{e.Message}");
                throw;
            }

            return table;
        }

        private List<T> DataTableToList<T>(DataTable table) where T : new()
        {
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
                    PropertyInfo pi = pisInfos.FirstOrDefault(p =>
                    {
                        if (p.Name.ToUpper() == col.ColumnName.ToUpper())
                        {
                            return true;
                        }
                        //如果列名与字段不是一一对应的，则反身字段Column特，获取Column的Name值与列名进行比较
                        var attr =
                            p.GetCustomAttributes(true).FirstOrDefault(f => f.GetType() == typeof(ColumnAttribute)) as
                                ColumnAttribute;

                        return attr != null && attr.Name.ToUpper() == col.ColumnName.ToUpper();

                    });
                    if (pi != null)
                    {
                        object value = row[col.ColumnName];
                        if (value != null && !string.IsNullOrEmpty(value.ToString()))
                        {
                            pi.SetValue(item, value, null);
                        }
                    }
                }
                result.Add(item);
            }

            return result;
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

            var result = DataTableToList<T>(table);

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
                LogHelper.Error(e);
                LogHelper.Info("SQL:" + sql);
                throw;
            }

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
                LogHelper.Info($"批量更新完成耗时：{timespan.Hours}时{timespan.Minutes}分{timespan.Seconds}秒，共更新{data.Count}条数据");
                return true;
            }
            catch (Exception e)
            {
                LogHelper.Info($"批量更新失败\n异常：{e.Message}");
                LogHelper.Info("SQL:" + sql);
                throw;
            }
        }

        public string GetSelectSql<T>()
        {
            var t = typeof(T);
            var tableAttr = t.GetCustomAttributes(true).FirstOrDefault(p => p.GetType() == typeof(TableAttribute)) as TableAttribute;
            string tableName = tableAttr == null ? t.Name : tableAttr.Name;

            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT * FROM ");
            builder.Append(tableName);
            builder.Append(" Where 1=1");

            return builder.ToString();
        }

        public T Get<T>(object id) where T : new()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            string sql = GetSelectSql<T>();
            var t = typeof(T);
            var pis = t.GetProperties();

            string keyFieldName = "";

            foreach (var pi in pis)
            {
                var keyAttr = pi.GetCustomAttributes(true).FirstOrDefault(p => p.GetType() == typeof(KeyAttribute));

                if (keyAttr != null)
                {
                    keyFieldName = pi.Name;
                    break;
                }
            }

            if (string.IsNullOrEmpty(keyFieldName))
            {
                throw new ArgumentException(string.Format("{0}类未指定Key字段", t.Name));
            }

            sql += string.Format(" AND {0}=", keyFieldName) + id.ToString();

            var result = Select<T>(sql).FirstOrDefault();

            return result;

        }

        public List<T> GetAll<T>() where T : new()
        {
            string sql = GetSelectSql<T>();
            return Select<T>(sql);
        }

        private DbCommand GetCommand<T>(T where)
        {
            string sql = GetSelectSql<T>();
            StringBuilder builder = new StringBuilder();
            builder.Append(sql);

            var t = where.GetType();
            var pis = t.GetProperties().Where(p=>p.GetCustomAttributes().Count(pi=>pi.GetType()==typeof(NotMappedAttribute))==0);
            var cmd = factory.CreateCommand();

            foreach (var pi in pis)
            {
                var value = pi.GetValue(where, null);
                if (value != null)
                {
                    string fieldName = pi.Name;

                    if (pi.GetCustomAttributes(true).FirstOrDefault(p => p.GetType() == typeof(ColumnAttribute)) is ColumnAttribute attr)
                    {
                        fieldName = attr.Name;
                    }

                    cmd.Parameters.Add(new OracleParameter(pi.Name, value));

                    builder.Append(" AND ");
                    builder.Append(fieldName);
                    builder.Append("=:");
                    builder.Append(pi.Name);

                }
            }
            sql = builder.ToString();
            cmd.CommandText = sql;
            cmd.Connection = conn;
            return cmd;
        }

        public string GetSelectSql<T>(object wherer)
        {
            string sql = GetSelectSql<T>();
            StringBuilder builder = new StringBuilder();
            builder.Append(sql);
            builder.Append(" WHERE 1=1 ");

            var t = wherer.GetType();
            var pis = t.GetProperties();

            foreach (var pi in pis)
            {
                var value = pi.GetValue(wherer, null);
                if (value != null)
                {
                    string fieldName = pi.Name;

                    if (pi.GetCustomAttributes(true).FirstOrDefault(p => p.GetType() == typeof(ColumnAttribute)) is ColumnAttribute attr)
                    {
                        fieldName = attr.Name;
                    }

                    builder.Append(" AND ");
                    builder.Append(fieldName);
                    builder.Append("=:");
                    builder.Append(pi.Name);

                }
            }

            return builder.ToString();
        }

        public List<T> GetWhere<T>(T condiction) where T : new()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            var cmd = GetCommand(condiction);
            var da = factory.CreateDataAdapter();
            da.SelectCommand = cmd;

            DataTable data = new DataTable();
            da.Fill(data);

            var result = DataTableToList<T>(data);

            return result;

        }
    }
}