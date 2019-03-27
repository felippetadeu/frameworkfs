using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Framework.ClassManipulations
{
    public static class DataReaderManipulation
    {
        public static T DataReaderToClass<T>(IDataReader dr, bool skipRead = false)
        {
            T obj = default(T);
            var readed = false;
            if (!skipRead)
                readed = dr.Read();

            if (!skipRead && !readed)
                return obj;

            obj = Activator.CreateInstance<T>();
            foreach (PropertyInfo prop in obj.GetType().GetProperties())
            {
                if (ClassManipulation.IsDbColumn<T>(prop) && !object.Equals(dr[prop.Name], DBNull.Value))
                {
                    prop.SetValue(obj, Convert.ChangeType(dr[prop.Name], GetType(prop.PropertyType)), null);
                }
            }

            return obj;
        }

        public static List<T> DataReaderMapToList<T>(IDataReader dr)
        {
            List<T> list = new List<T>();
            while (dr.Read())
            {
                list.Add(DataReaderToClass<T>(dr, true));
            }
            return list;
        }

        public static Type GetType(Type type)
        {
            if (Nullable.GetUnderlyingType(type) != null)
            {
                return Nullable.GetUnderlyingType(type);
            }
            else
            {
                return type;
            }
        }
    }
}
