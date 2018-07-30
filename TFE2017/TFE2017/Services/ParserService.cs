using Neo4j.Driver.V1;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TFE2017.Core.Services
{
    static class ParserService
    {
        public static async Task<Object> ToObject(List<IRecord> records)
        {
            try
            {
                switch (records.Count)
                {
                    case (0):
                        return null;
                    case (1):
                        return ToObject(records[0]);
                    default:
                        List<object> result = new List<object>();
                        foreach (IRecord record in records)
                        {
                            result.Add(await ToObject(record));
                        }
                        return result;             }

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
    }
}
