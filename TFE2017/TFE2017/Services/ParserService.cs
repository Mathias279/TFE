using Neo4j.Driver.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFE2017.Core.Models.Abstract;

namespace TFE2017.Core.Services
{
    static class ParserService
    {
        public static async Task<List<IPlaceEntity>> ToObjects(List<IRecord> records)
        {
            try
            {
                if (!(records.Any()))
                    return null;
                else
                {

                    List<IPlaceEntity> result = new List<IPlaceEntity>();
                    foreach (IRecord record in records)
                    {
                        result.Add(await ToObject(record));
                    }
                    return result;
                }

            }
            catch (Exception ex)
            {
                return new List<IPlaceEntity>();
            }
        }

        public static async Task<IPlaceEntity> ToObject(IRecord record)
        {
            var key1 = record.Keys[0];
            var value1 = record[key1];
            var key2 = value1;
            return null;
        }

        public static async Task<Object> ToRoom(IRecord record)
        {
            var type = record.Keys;
            return new object();
        }
    }
}