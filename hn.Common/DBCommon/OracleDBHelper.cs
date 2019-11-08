using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;

namespace hn.Common
{
    public class OracleDBHelper
    {
        private static readonly OracleClientFactory factory = new OracleClientFactory();

        private  string conStr { get; set; }

        public OracleDBHelper(string conStr)
        {
            this.conStr = conStr;
            conn = factory.CreateConnection();
            conn.ConnectionString = conStr;

        }

        public DbConnection conn { get; set; }

        /// <summary>
        /// 用于执行没有返回数据的SQL语句，如UPDATE或INSERT、DELETE类
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql, Dictionary<string, object> pars)
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                var cmd = factory.CreateCommand();
                cmd.Connection = conn;
                cmd.CommandText = sql;

                foreach (var key in pars.Keys) cmd.Parameters.Add(new OracleParameter(key, pars[key]));

                return cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                LogHelper.Error(e);
                LogHelper.Info("SQL:" + sql);
                throw;
            }
        }
        
        /// <summary>
        /// 执行SQL语并返回首行首列值，例如：SELECT COUNT()/MAX()/MIN()等
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        public object ExecuteScalar(string sql, Dictionary<string, object> pars)
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                var cmd = factory.CreateCommand();
                cmd.Connection = conn;
                cmd.CommandText = sql;

                foreach (var key in pars.Keys) cmd.Parameters.Add(new OracleParameter(key, pars[key]));

                return cmd.ExecuteScalar();
            }
            catch (Exception e)
            {
                LogHelper.Error(e);
                LogHelper.Info("SQL:" + sql);
                throw;
            }
        }

        /// <summary>
        /// 根据实体对象获取插入SQL语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public string GetInsertSql<T>()
        {
            var t = typeof(T);

            var pis = t.GetProperties().Where(p =>
                p.GetCustomAttributes(true).Count(pi => pi.GetType() == typeof(NotMappedAttribute)) == 0).ToList();

            var tableAttr =
                t.GetCustomAttributes(true)
                    .FirstOrDefault(p => p.GetType() == typeof(TableAttribute)) as TableAttribute;
            var tableName = tableAttr.Name;

            var strbuilder = new StringBuilder();
            strbuilder.AppendFormat("INSERT INTO {0} ", tableName);

            var fields = "(";
            var values = " VALUES (";

            pis.ForEach(p =>
            {
                var fieldName = p.Name;

                if (p.GetCustomAttributes(true).SingleOrDefault(o => o.GetType() == typeof(ColumnAttribute)) is
                    ColumnAttribute column)
                    fieldName = column.Name;

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

        /// <summary>
        /// 根据实体对像获取UPDATE语句
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="where">更新条件</param>
        /// <returns></returns>
        public string GetUpdateSql<T>(string where)
        {
            var t = typeof(T);

            var pis = t.GetProperties().Where(p =>
                p.GetCustomAttributes(true).Count(pi => pi.GetType() == typeof(NotMappedAttribute)) == 0).ToList();

            var tableAttr =
                t.GetCustomAttributes(true)
                    .FirstOrDefault(p => p.GetType() == typeof(TableAttribute)) as TableAttribute;
            var tableName = tableAttr.Name;

            var strbuilder = new StringBuilder();
            strbuilder.AppendFormat("UPDATE {0} SET ", tableName);

            var fields = "";

            pis.ForEach(p =>
            {
                var fieldName = p.Name;

                if (p.GetCustomAttributes(true).SingleOrDefault(o => o.GetType() == typeof(ColumnAttribute)) is
                    ColumnAttribute column)
                    fieldName = column.Name;

                fields += fieldName + "=:" + p.Name;

                if (p == pis.Last())
                    fields += " WHERE 1=1 ";
                else
                    fields += ",";
            });

            strbuilder.Append(fields);
            strbuilder.Append(where);
            return strbuilder.ToString();
        }

        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="obj">实体数据</param>
        /// <param name="where">更新条件</param>
        /// <returns></returns>
        public bool Update<T>(T obj, string where)
        {
            var sql = GetUpdateSql<T>(where);
            var cmd = GetCommand(sql, obj);
            cmd.Connection = conn;

            try
            {
                if (conn.State==ConnectionState.Closed) conn.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception e)
            {
                LogHelper.Error(e);
                LogHelper.Info("SQL:" + sql);
                throw;
            }
        }

        /// <summary>
        /// 更新实体数据，更新条件为实体类型的Key特性字段
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="obj">实体数据</param>
        /// <returns></returns>
        public bool Update<T>(T obj)
        {
            var t = typeof(T);
            var pis = t.GetProperties();

            var keyFieldName = "";
            PropertyInfo keyPropertyInfo = null;

            foreach (var pi in pis)
            {
                var keyAttr = pi.GetCustomAttributes(true).Count(p => p is KeyAttribute) == 1;
                if (keyAttr)
                {
                    keyFieldName = pi.Name;

                    var fieldName = pi.Name;

                    if (pi.GetCustomAttributes(true).SingleOrDefault(o => o.GetType() == typeof(ColumnAttribute)) is
                        ColumnAttribute column)
                        keyFieldName = column.Name;

                    keyPropertyInfo = pi;

                    break;
                }
            }

            if (string.IsNullOrEmpty(keyFieldName)) throw new ArgumentException(string.Format("{0}类未指定Key字段", t.Name));

            var where = string.Format(" AND {0}=:{1}", keyFieldName, keyPropertyInfo.Name);

            var sql = GetUpdateSql<T>(where);

            var cmd = GetCommand(sql, obj);
            cmd.Connection = conn;

            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception e)
            {
                LogHelper.Error(e);
                LogHelper.Info("SQL:" + sql);
                throw;
            }
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="obj">插入的实体数据</param>
        /// <returns></returns>
        public bool Insert<T>(T obj)
        {
            var start = DateTime.Now;
            var sql = GetInsertSql<T>();
            var cmd = GetCommand(sql, obj);
            cmd.Connection = conn;

            try
            {
                LogHelper.Info($"插入数据【{typeof(T)}】");
               if(conn.State==ConnectionState.Closed) conn.Open();

               var result = cmd.ExecuteNonQuery();
                conn.Close();
                
                LogHelper.Info($"插入完成，耗时【{(DateTime.Now-start).TotalMilliseconds}】毫秒");


               return result > 0;
            }
            catch (Exception e)
            {
                LogHelper.Error(e);
                LogHelper.Info("SQL:" + sql);
                throw;
            }
        }

        /// <summary>
        /// 根据实体类型获取DBCommand
        /// </summary>
        /// <typeparam name="T">实体数据类型</typeparam>
        /// <param name="sql">要执行的SQL语句</param>
        /// <param name="par">替换进SQL的实际数据</param>
        /// <returns></returns>
        private DbCommand GetCommand<T>(string sql, T par)
        {
            var cmd = factory.CreateCommand();
            cmd.CommandText = sql;
            cmd.Connection = conn;

            var t = typeof(T);
            var pis = t.GetProperties().Where(p =>
                p.GetCustomAttributes(true).Count(pi => pi.GetType() == typeof(NotMappedAttribute)) == 0).ToList();

            pis.ForEach(p =>
            {
                var value = p.GetValue(par, null);
                if (value == null)
                    value = "";

                cmd.Parameters.Add(new OracleParameter(":" + p.Name, value));
            });

            return cmd;
        }

        /// <summary>
        /// 根据实体类型获取DBCommand
        /// </summary>
        /// <typeparam name="T">实体数据类型</typeparam>
        /// <param name="sql">要执行的SQL语句</param>
        /// <param name="par">替换进SQL的实际数据</param>
        /// <returns></returns>
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

            var tableAttr =
                t.GetCustomAttributes(true)
                    .FirstOrDefault(p => p.GetType() == typeof(TableAttribute)) as TableAttribute;
            var tableName = tableAttr.Name;

            var sql = string.Format("DELETE FROM {0} WHERE 1=1 {1}", tableName, where);

            var cmd = factory.CreateCommand();
            cmd.CommandText = sql;
            cmd.Connection = conn;

            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

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
            if (conn.State == ConnectionState.Closed) conn.Open();
            var cmd = factory.CreateCommand();
            cmd.CommandText = sql;
            cmd.Connection = conn;
            var da = factory.CreateDataAdapter();
            da.SelectCommand = cmd;
            var table = new DataTable();
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
            var t = typeof(T);
            //获得泛型类所有公共字段
            var pisInfos = t.GetProperties().ToList();
            //最终返回的对象列表
            var result = new List<T>();
            foreach (DataRow row in table.Rows)
            {
                var item = new T();
                foreach (DataColumn col in table.Columns)
                {
                    //这里假设字段名和数据库列名是一 一对应
                    //可能通过字段名与列名进行比较，相同则进行取值赋值操作
                    var pi = pisInfos.FirstOrDefault(p =>
                    {
                        if (p.Name.ToUpper() == col.ColumnName.ToUpper()) return true;
                        //如果列名与字段不是一一对应的，则反射字段Column特性，获取Column的Name值与列名进行比较

                        return p.GetCustomAttributes(true).FirstOrDefault(f => f.GetType() == typeof(ColumnAttribute)) is ColumnAttribute attr && attr.Name.ToUpper() == col.ColumnName.ToUpper();
                    });
                    if (pi != null)
                    {
                        var value = row[col.ColumnName];
                        if (value != null && !string.IsNullOrEmpty(value.ToString())) pi.SetValue(item, value, null);
                    }
                }

                result.Add(item);
            }

            return result;
        }

        public List<T> Select<T>(string sql) where T : new()
        {
            if (conn.State == ConnectionState.Closed) conn.Open();
            var cmd = factory.CreateCommand();
            cmd.CommandText = sql;
            cmd.Connection = conn;
            var da = factory.CreateDataAdapter();
            da.SelectCommand = cmd;
            var table = new DataTable();
            da.Fill(table);

            var result = DataTableToList<T>(table);

            return result;
        }

        public T Get<T>(string sql) where T : new()
        {
            if (conn.State == ConnectionState.Closed) conn.Open();
            var cmd = factory.CreateCommand();
            cmd.CommandText = sql;
            cmd.Connection = conn;
            var da = factory.CreateDataAdapter();
            da.SelectCommand = cmd;
            var table = new DataTable();
            da.Fill(table);

            var result = DataTableToList<T>(table).FirstOrDefault();

            return result;
        }

        public bool BatchInsert<T>(List<T> data)
        {
            var start = DateTime.Now;
            if (data == null || data.Count == 0) return false;
            var builder=new StringBuilder();
            builder.Append("INSERT ALL \n");
            var str = GetInsertSql<T>().Replace("INSERT", "");

            data.ForEach(p =>
            {
                builder.Append($"{str}\n");
            });

            builder.Append( " SELECT 1 FROM DUAL");

            var sql = builder.ToString();
            var cmd = GetCommand(sql, data);

            cmd.Connection = conn;

            try
            {
                LogHelper.Info($"批量插入数据【{typeof(T).Name}】【{data.Count}】条");
                if (conn.State == ConnectionState.Closed) conn.Open();

                var result = cmd.ExecuteNonQuery() ;
                conn.Close();
                var timespan = DateTime.Now - start;
                LogHelper.Info($"批量插入完成，耗时【{timespan.TotalMilliseconds}】毫秒");
                return result>0;
            }
            catch (Exception e)
            {
                LogHelper.Error(e);
                LogHelper.Info($"插入失败数据：\r\n{JsonConvert.SerializeObject(data)}\r\n");
                return false;
            }
        }

        public bool BatchUpdate<T>(List<T> data, string where)
        {
            if (data == null || data.Count == 0) return false;
            var now = DateTime.Now;
            var sql = GetUpdateSql<T>(where);

            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

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
            var tableAttr =
                t.GetCustomAttributes(true)
                    .FirstOrDefault(p => p.GetType() == typeof(TableAttribute)) as TableAttribute;
            var tableName = tableAttr == null ? t.Name : tableAttr.Name;

            var builder = new StringBuilder();
            builder.Append("SELECT * FROM ");
            builder.Append(tableName);
            builder.Append(" Where 1=1");

            return builder.ToString();
        }

        public T Get<T>(object id) where T : new()
        {
            if (conn.State == ConnectionState.Closed) conn.Open();
            var sql = GetSelectSql<T>();
            var t = typeof(T);
            var pis = t.GetProperties();

            var keyFieldName = "";

            foreach (var pi in pis)
            {
                var keyAttr = pi.GetCustomAttributes(true).FirstOrDefault(p => p.GetType() == typeof(KeyAttribute));

                if (keyAttr != null)
                {
                    keyFieldName = pi.Name;
                    break;
                }
            }

            if (string.IsNullOrEmpty(keyFieldName)) throw new ArgumentException(string.Format("{0}类未指定Key字段", t.Name));

            sql += string.Format(" AND {0}=", keyFieldName) + id;

            var result = Select<T>(sql).FirstOrDefault();

            return result;
        }

        public List<T> GetAll<T>() where T : new()
        {
            var sql = GetSelectSql<T>();
            return Select<T>(sql);
        }

        private DbCommand GetCommand<T>(T where)
        {
            var sql = GetSelectSql<T>();
            var builder = new StringBuilder();
            builder.Append(sql);

            var t = where.GetType();
            var pis = t.GetProperties().Where(p =>
                p.GetCustomAttributes(true).Count(pi => pi.GetType() == typeof(NotMappedAttribute)) == 0);
            var cmd = factory.CreateCommand();

            foreach (var pi in pis)
            {
                var value = pi.GetValue(where, null);
                if (value != null)
                {
                    var fieldName = pi.Name;

                    if (pi.GetCustomAttributes(true).FirstOrDefault(p => p.GetType() == typeof(ColumnAttribute)) is
                        ColumnAttribute attr) fieldName = attr.Name;

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
            var sql = GetSelectSql<T>();
            var builder = new StringBuilder();
            builder.Append(sql);
            builder.Append(" WHERE 1=1 ");

            var t = wherer.GetType();
            var pis = t.GetProperties();

            foreach (var pi in pis)
            {
                var value = pi.GetValue(wherer, null);
                if (value != null)
                {
                    var fieldName = pi.Name;

                    if (pi.GetCustomAttributes(true).FirstOrDefault(p => p.GetType() == typeof(ColumnAttribute)) is
                        ColumnAttribute attr) fieldName = attr.Name;

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
            if (conn.State == ConnectionState.Closed) conn.Open();

            var cmd = GetCommand(condiction);
            var da = factory.CreateDataAdapter();
            da.SelectCommand = cmd;

            var data = new DataTable();
            da.Fill(data);

            var result = DataTableToList<T>(data);

            return result;
        }
    }
}