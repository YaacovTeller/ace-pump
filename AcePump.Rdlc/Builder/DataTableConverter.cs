using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace AcePump.Rdlc.Builder
{
    public class DataTableConverter
    {
        public static DataTable FromEnumerable<TEntity>(IEnumerable<TEntity> source)
        {
            DataTableConverter<TEntity> converter = new DataTableConverter<TEntity>(source);

            return converter.GetTable();
        }

        public static DataTable FromObject<TEntity>(TEntity source)
        {
            List<TEntity> list = new List<TEntity>();

            if (source != null)
                list.Add(source);

            return FromEnumerable(list);
        }
    }

    internal class DataTableConverter<TEntity>
    {
        private DataTable Table { get; set; }
        private IEnumerable<TEntity> Source { get; set; }

        public DataTableConverter(IEnumerable<TEntity> source)
        {
            this.Source = source;
            this.Table = new DataTable();
        }

        public DataTable GetTable()
        {
            Type entityType = typeof(TEntity);
            DataTablePropertyAcessor[] entityProperties = GetPropertyAccessors(entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance));

            // define table structure based on type
            foreach (DataTablePropertyAcessor entityProperty in entityProperties)
                Table.Columns.Add(entityProperty.Name, entityProperty.DataTableType);

            // add rows
            foreach (TEntity item in Source)
            {
                object[] valueArray = new object[entityProperties.Length - 1 + 1];

                for (int i = 0; i <= entityProperties.Length - 1; i++)
                    valueArray[i] = entityProperties[i].GetDataTableValue(item);

                Table.Rows.Add(valueArray);
            }

            return Table;
        }

        private DataTablePropertyAcessor[] GetPropertyAccessors(PropertyInfo[] propertyInfos)
        {
            List<DataTablePropertyAcessor> accessors = new List<DataTablePropertyAcessor>();

            foreach (PropertyInfo info in propertyInfos)
                accessors.Add(new DataTablePropertyAcessor(info));

            return accessors.ToArray();
        }

        private class DataTablePropertyAcessor
        {
            private readonly PropertyInfo _PropertyInfo;

            public string Name
            {
                get
                {
                    return _PropertyInfo.Name;
                }
            }

            private bool ConvertNullable;

            private readonly Type _DataTableType;
            public Type DataTableType
            {
                get
                {
                    return _DataTableType;
                }
            }

            public DataTablePropertyAcessor(PropertyInfo propertyInfo)
            {
                _PropertyInfo = propertyInfo;

                if (_PropertyInfo.PropertyType.IsGenericType && _PropertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    _DataTableType = Nullable.GetUnderlyingType(_PropertyInfo.PropertyType);
                    ConvertNullable = true;
                }
                else
                {
                    _DataTableType = _PropertyInfo.PropertyType;
                    ConvertNullable = false;
                }
            }

            public object GetDataTableValue(object from)
            {
                if (!ConvertNullable)
                    return _PropertyInfo.GetValue(from, null);
                else
                {
                    object nullableValue = _PropertyInfo.GetValue(from, null);

                    return nullableValue;
                }
            }
        }
    }
}
