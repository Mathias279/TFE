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
using TFE2017.Core.Services; 

namespace TFE2017.Core.Managers
{
    static class DataBaseManager
    {
        private static string _urlConnect = "bolt://hobby-ohlpjagjjjpngbkejmmoojbl.dbs.graphenedb.com:24786";
        private static string _urlUser = "Math279";
        private static string _urlPassword = "b.Ge9EvCbZwWWH.DWYwHYCkL6ycEu18";

        private static IDriver _driver;

        static private async Task<ISession> Connect()
        {
            try
            {
                _driver = GraphDatabase.Driver(_urlConnect, AuthTokens.Basic(_urlUser, _urlPassword), Config.Builder.WithEncryptionLevel(EncryptionLevel.Encrypted).ToConfig());
                return _driver.Session();
            }
            catch (Exception ex)
            {
#if DEBUG
                Debugger.Break();
#endif
                return null;
            }
        }

        static private async Task Disconnect()
        {
            try
            {
                _driver.Dispose();
                await _driver.CloseAsync();
            }
            catch (Exception ex)
            {
#if DEBUG
                Debugger.Break();
#endif
            }
        }

        public static async Task<List<IRecord>> RunQuery(string query)
        {
            try
            {
                ISession session = await Connect();
                IStatementResult result;

                if (!(session is null))
                {

                    using (var tx = await session.BeginTransactionAsync())
                    {
                        result = tx.Run(query);

                        tx.Success();
                        tx.Dispose();
                    }
                    if (result.Any())
                    {
                        return result.ToList();
                    }
                    else
                        return null;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
#if DEBUG
                Debugger.Break();
#endif
                return null;
            }
            finally
            {
                Disconnect();
            }
        }

        static public async Task<List<Room>> GetAllRooms(string buildinId)
        {
            try
            {
                string query = $" MATCH (b:Building {{ Id : { buildinId } }}),( r:Room), p = shortestPath((b)-[*]-(r))" +
                    $" RETURN r.Id , r.Name "+
                    $" ORDER BY r.Name";
                List<IRecord> queryResult = await RunQuery(query);

                List<Room> listRooms = new List<Room>();
                foreach (IRecord record in queryResult)
                {
                    var id = record.Values[record.Keys[0]].ToString();
                    var name = record.Values[record.Keys[1]].ToString();
                    listRooms.Add(new Room(id,name));
                }

                return listRooms;

            }
            catch (Exception ex)
            {
#if DEBUG
                Debugger.Break();
#endif
                return null;
            }
        }

        static public async Task<List<IPlaceEntity>> GetPath(string buildinId, string beginId, string endId, bool noStairs = false, bool noLifts = false)
        {
            try
            {
                ISession session = await Connect();
                List<IPlaceEntity> roomsList = new List<IPlaceEntity>();

                int floor = 0;
                string name = "nom";

                string query = $" MATCH path = shortestPath((beginning:Room {{ Id: {beginId} }} ) - [*0..20] - (destination:Room {{ Id : {endId} }} ))";

                if (noStairs)
                    query += $" WHERE NONE (n IN nodes(path WHERE n:Staircase)";
                if (noStairs && noLifts)
                    query += $" And ";
                if (noLifts)
                    query += $" WHERE NONE (n IN nodes(path WHERE n:Lift)";

                query += $" RETURN path as shortestPath, reduce(link = 0, destination IN relationships(path) | link + 1) AS totallinks" +
                $" ORDER BY totallinks ASC" +
                $" LIMIT 1";

                var queryFull = query;

                using (var tx = session.BeginTransaction())
                {
                    var result = tx.Run(query).ToList();
                    foreach (var element in result)
                    {
                        roomsList.Add(element.As<Room>());
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
                return new List<IPlaceEntity>();
            }
        }
    }
}