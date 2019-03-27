using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace Framework.ClassManipulations
{
    public static class ClassManipulation
    {
        public static string GetTableName<T>() where T: new()
        {
            var table = (TableAttribute) Attribute.GetCustomAttributes(typeof(T), typeof(TableAttribute)).FirstOrDefault();
            if (table != null)
            {
                return table.Name;
            }
            return string.Empty;
        }

        public static string GetIdentityColumn<T>() where T : new()
        {
            var properties = typeof(T).GetProperties();

            var idProp = GetIdentityProp<T>();

            if (idProp != null)
            {
                return idProp.Name;
            }

            return string.Empty;
        }

        public static PropertyInfo GetIdentityProp<T>() where T : new()
        {
            var properties = typeof(T).GetProperties();

            foreach (var item in properties)
            {
                if (Attribute.IsDefined(item, typeof(KeyAttribute)))
                {
                    return item;
                }
            }
            return null;
        }

        public static List<string> GetColumns<T>() where T : new()
        {
            List<string> columns = new List<string>();

            var properties = typeof(T).GetProperties();

            foreach (var item in properties)
            {
                if (Attribute.IsDefined(item, typeof(ColumnAttribute)))
                {
                    columns.Add(item.Name);
                }
            }

            return columns;
        }

        public static PropertyInfo GetColumn<T>(string propName)
        {
            return typeof(T).GetProperty(propName);
        }

        public static bool IsDbColumn<T>(PropertyInfo prop)
        {
            return Attribute.IsDefined(prop, typeof(ColumnAttribute)) || Attribute.IsDefined(prop, typeof(KeyAttribute));
        }
    }
}
