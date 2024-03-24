using System.Data.SqlClient;
using System.Reflection;

namespace ParkingManagementApp.Common.Utilities
{
    public static class Utilities
    {
        public static List<T> DataReaderMapToList<T>(SqlDataReader dr)
        {
            List<T> list = new List<T>();
            T obj = default(T);
            while (dr.Read())
            {
                obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    if (!object.Equals(dr[prop.Name], DBNull.Value))
                    {
                        prop.SetValue(obj, dr[prop.Name], null);
                    }
                }
                list.Add(obj);
            }
            return list;
        }
  
        public static T ConvertValue<T>(object value)
        {
            Type t = typeof(T);
            Type u = Nullable.GetUnderlyingType(t);
            if (u != null)
            {
                if (value == null) return default(T);
                return (T)Convert.ChangeType(value, u);
            }
            if (value is IConvertible && !(typeof(T).IsEnum))
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            if (typeof(T).IsEnum && value.GetType().Name == "String")
            {
                var intValue = -1;
                var isNumeric = int.TryParse(value.ToString(), out intValue);
                if (isNumeric) value = intValue;
                return (T)value;
            }
            return (T)value;
        }
    }
}
