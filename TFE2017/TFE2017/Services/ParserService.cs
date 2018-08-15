using Neo4j.Driver.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFE2017.Core.Services
{
    static class ParserService
    {
        public static async Task<Object> ToObjects(List<IRecord> records)
        {
            try
            {
                if (!(records.Any()))
                    return null;
                else
                {

                    List<object> result = new List<object>();
                    foreach (IRecord record in records)
                    {
                        result.Add(await ToObject(record));
                    }
                    return result;
                }

            }
            catch (Exception ex)
            {
                return new object();
            }
        }

        public static async Task<Object> ToObject(IRecord record)
        {
            var type = record.Keys;
            return new object();
        }

        public static async Task<Object> ToRoom(IRecord record)
        {
            var type = record.Keys;
            return new object();
        }
    }
}