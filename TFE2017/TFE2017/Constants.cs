using System;
using System.Collections.Generic;
using System.Text;

namespace TFE2017.Core
{
    public static class Constants
    {
        public readonly static string Url = "https://play.google.com/store/apps/details";
        public readonly static string UrlField0 = "id=com.Slack";
        public readonly static string UrlField1Key = "buildingId";
        public readonly static string UrlField2Key = "entryId";

        public static string DataBaseQuery(string param1, string param2)
        {
            return $"" +
            $"MATCH path = shortestPath((beginning)-[*1..100]-(destination))" +
            $"  WHERE beginning.Id = {param1}" +
            $"  AND destination.Id = {param2}" +
            $"RETURN path as shortestPath, REDUCE(link = 0, destination IN relationships(path) | link+1) AS totallinksORDER BY totallinks ASC" +
            $"LIMIT 1";
        }

    }
}