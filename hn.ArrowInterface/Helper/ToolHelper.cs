using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace hn.ArrowInterface.Helper
{
    public static class ToolHelper
    {
        #region 对象转成字典
        /// <summary>
        /// 对象转换为字典
        /// </summary>
        /// <param name="obj">待转化的对象</param>
        /// <returns></returns>
        public static Dictionary<string, object> ObjectToMap(this object obj)
        {
            Dictionary<string, object> map = new Dictionary<string, object>();

            Type t = obj.GetType(); // 获取对象对应的类， 对应的类型

            PropertyInfo[] pi = t.GetProperties(BindingFlags.Public | BindingFlags.Instance); // 获取当前type公共属性

            foreach (PropertyInfo p in pi)
            {
                MethodInfo m = p.GetGetMethod();

                if (m != null && m.IsPublic)
                {
                    // 进行判NULL处理
                    if (m.Invoke(obj, new object[] { }) != null)
                    {
                        map.Add(p.Name, m.Invoke(obj, new object[] { }).ToString()); // 向字典添加元素
                    }
                }
            }
            return map;
        }
        #endregion
    }
}
