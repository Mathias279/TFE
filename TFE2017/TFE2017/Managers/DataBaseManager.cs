using Acr.UserDialogs;
using Neo4j.Driver.V1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFE2017.Core.Models;
using TFE2017.Core.Models.Abstract;
using Xamarin.Forms;

namespace TFE2017.Core.Managers
{
    static class DataBaseManager
    {
        static private ISession Connect()
        {
            IDriver driver = GraphDatabase.Driver("bolt://hobby-ohlpjagjjjpngbkejmmoojbl.dbs.graphenedb.com:24786", AuthTokens.Basic("Math279", "b.Ge9EvCbZwWWH.DWYwHYCkL6ycEu18"), Config.Builder.WithEncryptionLevel(EncryptionLevel.Encrypted).ToConfig());
            return driver.Session();
        }

        static public async Task<List<string>> GetAllNodesNames()
        {
            try
            {
                ISession session = Connect();
                List<string> roomsList = new List<string>();

                using (var tx = session.BeginTransaction())
                {
                    var result = tx.Run("match (n) return n.name").ToList();
                    foreach (var element in result)
                    {
                        roomsList.Add(element["n.name"].ToString());
                    }
                    tx.Success();
                }
                return roomsList;
            }
            catch (Exception ex)
            {
#if DEBUG
                Debugger.Break();
#endif
                return new List<string>();
            }
        }

        static public async Task<List<MapEntity>> GetPath(string a1,string a2,string a3)
        {
            try
            {
                ISession session = Connect();
                List<MapEntity> roomsList = new List<MapEntity>();

                int floor = 0;
                string name = "nom";

                string query = $"MATCH p = (n) - [*1..4] - (h) -[] - (r: ROOM) " +
                    $" WHERE n.floor = {floor} AND r.name = '{name}' " +
                    $" AND h.entryPoint " +
                    $" AND NOT n: Building " +
                    $" AND none(x IN nodes(p)" +
                    $" WHERE x: Building)" +
                    $" RETURN p AS shortestPath, reduce(link = 0, r IN relationships(p) | link + 1) AS totallinksORDER BY totallinks ASCLIMIT 1";

                using (var tx = session.BeginTransaction())
                {
                    var result = tx.Run(query).ToList();
                    foreach (var element in result)
                    {
                        roomsList.Add(new RoomEntity());
                        //roomsList.Add(element["n.name"].ToString());
                    }
                    tx.Success();
                }

                return roomsList;
            }
            catch (Exception ex)
            {
#if DEBUG
                Debugger.Break();
#endif
                return new List<MapEntity>();
            }
        }
    }
}
