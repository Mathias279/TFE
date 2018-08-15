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

        static public async Task<List<Room>> GetAllRooms()
        {
            try
            {
                List<IRecord> queryResult = await RunQuery($"MATCH (n:ROOM) RETURN n");

                return new List<Room>();// ParserService.ToObjects(queryResult);

            }
            catch (Exception ex)
            {
#if DEBUG
                Debugger.Break();
#endif
                return null;
            }
        }

        static public async Task<List<IPlaceEntity>> GetPath(string a1, string a2, string a3)
        {
            try
            {
                ISession session = await Connect();
                List<IPlaceEntity> roomsList = new List<IPlaceEntity>();

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
                        roomsList.Add(new Room());
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