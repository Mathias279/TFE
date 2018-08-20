using Neo4j.Driver.V1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TFE2017.Core.Models;
using TFE2017.Core.Models.Abstract;

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
                await Disconnect();
            }
        }

        static public async Task<List<Room>> GetAllRooms(string buildinId)
        {
            try
            {
                string query = $" MATCH (b:Building {{ Id : { buildinId } }}),( r:Room), p = shortestPath((b)-[*]-(r))" +
                    $" RETURN r.Id , r.Name " +
                    $" ORDER BY r.Name";
                List<IRecord> queryResult = await RunQuery(query);

                List<Room> listRooms = new List<Room>();
                foreach (IRecord record in queryResult)
                {
                    var id = record.Values[record.Keys[0]].ToString();
                    var name = record.Values[record.Keys[1]].ToString();
                    listRooms.Add(new Room(id, name));
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

        static public async Task<Building> GetBuilding(string buildingId)
        {
            try
            {
                string query = $" MATCH (b:Building) RETURN b.Id, b.Name, b.Angle ";
                List<IRecord> queryResult = await RunQuery(query);
                List<Building> buildings = new List<Building>();

                foreach (IRecord record in queryResult)
                {
                    var id = "0";
                    var name = "0";
                    var angle = "0";

                    if (!(record.Values[record.Keys[0]] is null))
                        id = record.Values[record.Keys[0]].ToString();
                    if (!(record.Values[record.Keys[1]] is null))
                        name = record.Values[record.Keys[1]].ToString();
                    if (!(record.Values[record.Keys[2]] is null))
                        angle = record.Values[record.Keys[2]].ToString();
                    buildings.Add(new Building(id, angle, name));
                }
                return buildings.FirstOrDefault(building => building.Id == buildingId);
            }
            catch (Exception ex)
            {
#if DEBUG
                Debugger.Break();
#endif
                return null;
            }
        }

        static public async Task<List<IPlaceEntity>> GetPath(string buildinId, string beginId, string endId, bool useStairs, bool useLift)
        {
            try
            {
                ISession session = await Connect();
                string query = $" MATCH path = shortestPath((beginning:Room {{ Id: {beginId} }} ) - [*0..20] - (destination:Room {{ Id : {endId} }} ))";
                if (!useStairs || !useLift)
                {
                    query += $" WHERE ";
                    if (!useStairs)
                        query += " NONE (s IN nodes(path) WHERE s:Staircase)";
                    if (!useStairs && !useLift)
                        query += $" AND ";
                    if (!useLift)
                        query += $" NONE (l IN nodes(path) WHERE l:Lift)";
                }
                var queryNodes = query + "UNWIND nodes(path) as n RETURN n";
                var queryRelations = query + "UNWIND relationships(path) as r RETURN r";
                List<IPlaceEntity> nodes = new List<IPlaceEntity>();
                List<IRecord> queryResult = await RunQuery(queryNodes);
                foreach (var element in queryResult)
                {
                    var key1 = element.Keys[0];
                    var obj = element[key1];
                    if (obj is INode)
                    {
                        INode node = (INode)obj;
                        if (node.Labels[0] == "Room")
                            nodes.Add(new Room(node.Properties["Id"].ToString(), node.Properties["Name"].ToString()));
                        else
                            nodes.Add(new Room("0", node.Labels[0]));
                    }
                }
                List<IPlaceEntity> relations = new List<IPlaceEntity>();
                queryResult = await RunQuery(queryRelations);
                foreach (var element in queryResult)
                {
                    var key1 = element.Keys[0];
                    var obj = element[key1];
                    if (obj is IRelationship)
                    {
                        IRelationship rel = (IRelationship)obj;
                        var pos = new PositionEntity(double.Parse(rel.Properties["X"].ToString()), double.Parse(rel.Properties["Y"].ToString()), double.Parse(rel.Properties["Z"].ToString()));
                        relations.Add(new Door(pos));
                    }
                }
                List<IPlaceEntity> path = new List<IPlaceEntity>();
                List<IPlaceEntity> entries = new List<IPlaceEntity>();
                var queryEntry = $"MATCH (b: Building) - [e: ENTRY] - (beginning: Room {{ Id: {beginId} }} ) " +
                    " unwind [e,e] as twin" +
                    " return twin";
                queryResult = await RunQuery(queryEntry);
                foreach (var element in queryResult)
                {
                    var key1 = element.Keys[0];
                    var obj = element[key1];
                    if (obj is IRelationship)
                    {
                        IRelationship rel = (IRelationship)obj;
                        var pos = new PositionEntity(double.Parse(rel.Properties["X"].ToString()), double.Parse(rel.Properties["Y"].ToString()), double.Parse(rel.Properties["Z"].ToString()));
                        entries.Add(new Door(pos));
                    }
                }
                if (entries.Count == 1)
                    path.Add(entries[0]);
                else
                    throw new Exception("entry error");
                if (relations.Any())
                {
                    for (int index = 0; index < nodes.Count-1; index++)
                    {
                        path.Add(nodes[index]);
                        path.Add(relations[index]);
                    }
                    path.Add(nodes.Last());
                }
                return path;
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