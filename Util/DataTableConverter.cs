using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace APIDirecciones.Util
{
    public class DataTableConverter
    {
        public static List<T> ConvertToList<T>(DataTable table) where T : new()
        {
            var list = new List<T>();

            foreach (DataRow row in table.Rows)
            {
                var obj = new T();

                foreach (DataColumn column in table.Columns)
                {
                    var prop = typeof(T).GetProperty(column.ColumnName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (prop != null && row[column] != DBNull.Value)
                    {
                        prop.SetValue(obj, Convert.ChangeType(row[column], prop.PropertyType));
                    }
                }

                list.Add(obj);
            }

            return list;
        }
    }
}