using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace RepoWebIT
{
   public class Mapper<T> where T : new()
    {
        private Dictionary<string, string> _mappings { get; set; }

        public Mapper()
        {
            _mappings = CreateMap();
        }

        public T Map(IDataRecord record)
        {
            var item = Activator.CreateInstance<T>();
            var itemType = item.GetType();


            
            foreach (var map in _mappings)
            {
                var prop = itemType.GetProperty(map.Key);

                if (record[map.Value] != DBNull.Value)
                {
                    prop.SetValue(item, record[map.Value], null);
                }
                
            }

            return item;
        }


        public List<T> MapList(IDataReader reader)
        {
            var list = new List<T>();

            while (reader.Read())
            {
                list.Add(Map(reader));
            }

            reader.Close();
            return list;
        }


        public Dictionary<string, string> CreateMap()
        {
            var mappings = new Dictionary<string, string>();
            var props = typeof(T).GetProperties().Where(p => p.CanWrite);


            foreach (var prop in props)
            {
                mappings.Add(prop.Name, prop.Name);
            }

            return mappings;
        }

    }
}


