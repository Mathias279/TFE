using Neo4j.Driver.V1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
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
        private static TimeSpan _timeout = new TimeSpan(0, 1, 0);
        //private static Config _config = 
        private static IDriver _driver = GraphDatabase.Driver(
            _urlConnect,
            AuthTokens.Basic(_urlUser, _urlPassword),
            Config.Builder.
                WithConnectionAcquisitionTimeout(_timeout).
                WithConnectionIdleTimeout(_timeout).
                WithConnectionTimeout(_timeout).
                ToConfig());

        static public List<Room> GetAllRooms(string buildinId)
        {
            try
            {
                string query = $" MATCH p = shortestPath((b:Building {{ Id : { buildinId } }})-[*]-(room:Room))" +
                    $" RETURN room" +
                    $" ORDER BY room.Name";
                using (var session = _driver.Session())
                {
                    return session.ReadTransaction(tx =>
                    {
                        var result = tx.Run(query);
                        List<Room> rooms = new List<Room>();
                        foreach (var res in result)
                        {
                            var node = res["room"].As<INode>();
                            var id = node["Id"].As<string>();
                            var name = node["Name"].As<string>();
                            rooms.Add(new Room(id, name));
                        }
                        tx.Success();
                        tx.Dispose();
                        return rooms;
                    });
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Debugger.Break();
#endif
            }
            return new List<Room>();
        }

        static public Building GetBuilding(string buildingId)
        {
            try
            {
                string query = $" MATCH (b:Building {{ Id : { buildingId } }})" +
                   $" RETURN b";
                using (var session = _driver.Session())
                {
                    return session.ReadTransaction(tx =>
                    {
                        var result = tx.Run(query);
                        var node = result.Single()["b"].As<INode>();
                        var id = node["Id"].As<string>();
                        var name = node["Name"].As<string>();
                        var angle = node["Angle"].As<string>();
                        tx.Success();
                        tx.Dispose();
                        return new Building(id, name, angle);
                    });
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Debugger.Break();
#endif
            }
            return new Building();
        }

        static public Building GetRoom(string roomId)
        {
            try
            {
                string query = $" MATCH (r:Room {{ Id : { roomId } }})" +
                   $" RETURN r";
                using (var session = _driver.Session())
                {
                    return session.ReadTransaction(tx =>
                    {
                        var result = tx.Run(query);
                        var node = result.Single()["r"].As<INode>();
                        var id = node["Id"].As<string>();
                        var name = node["Name"].As<string>();
                        var angle = node["Angle"].As<string>();
                        tx.Success();
                        tx.Dispose();
                        return new Building(id, name, angle);
                    });
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Debugger.Break();
#endif
            }
            return new Building();
        }

        static public int GetLevel(string roomId)
        {
            try
            {
                string query = $" MATCH (r:Room {{ Id : { roomId } }})-[rel]-(n)" +
                   $" RETURN rel";
                using (var session = _driver.Session())
                {
                    return session.ReadTransaction(tx =>
                    {
                        var result = tx.Run(query);
                        var relation = result.Single()["r"].As<IRelationship>();
                        int x = Int32.Parse(relation["X"].As<string>());
                        int y = Int32.Parse(relation["Y"].As<string>());
                        int z = Int32.Parse(relation["Z"].As<string>());
                        tx.Success();
                        tx.Dispose();
                        return z;
                    });
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Debugger.Break();
#endif
            }
            return 0;
        }

        static public List<IPlaceEntity> GetPath(string buildinId, string beginId, string endId, bool useStairs, bool useLift)
        {
            var allPath = new List<IPath>();

            // get min len
            int minRel = int.MaxValue;

            string preferences = "";
            if (!useStairs)
                preferences = $" WHERE NONE (s IN nodes(path) WHERE s:Staircase) ";
            else if (!useLift)
                preferences = $" WHERE NONE (l IN nodes(path) WHERE l:Lift)";

            string query = $" MATCH path = " +
                $"shortestPath((beginning: Room {{ Id: { beginId} }} ) - [*0..15] - (destination: Room {{ Id: { endId} }} )) " +
                $" {preferences} " +
                $" RETURN length(path) as minlen";

            try
            {
                using (var session = _driver.Session())
                {
                    session.ReadTransaction(tx =>
                    {
                        var result = tx.Run(query);
                        minRel = result.Single()["minlen"].As<int>();
                        tx.Success();
                        tx.Dispose();
                    });
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Debugger.Break();
#endif
            }

            //get A Lot of paths
            try
            {
                for (int testLen = minRel; testLen < minRel + 5; testLen++)
                {
                    query = $" MATCH path = " +
                        $"(beginning: Room {{ Id: { beginId} }} ) - [*{testLen}] - (destination: Room {{ Id: { endId} }} ) " +
                        $" {preferences} " +
                        $" RETURN path" +
                        $" LIMIT 5";
                    using (var session = _driver.Session())
                    {
                        session.ReadTransaction(tx =>
                        {
                            try
                            {
                                var result = tx.Run(query);
                                foreach (var res in result)
                                {
                                    var path = res["path"].As<IPath>();
                                    allPath.Add(path);
                                }
                                tx.Success();
                            }
                            catch (Exception ex)
                            {
                                tx.Failure();
                            }
                            tx.Dispose();
                        });
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Debugger.Break();
#endif
            }

            //get len for each path and best path
            double shortestDist = double.MaxValue;
            IPath bestPath = allPath.First();

            try
            {
                foreach (IPath path in allPath)
                {
                    double pathDist = 0;
                    PositionEntity previousPos = new PositionEntity(path.Relationships[0]["X"].As<double>(), 
                        path.Relationships[0]["Y"].As<double>(), path.Relationships[0]["Z"].As<double>());

                    foreach (IRelationship rel in path.Relationships)
                    {
                        PositionEntity thisPos = new PositionEntity(rel["X"].As<double>(),
                            rel["Y"].As<double>(), rel["Z"].As<double>());
                        pathDist += MapManager.GetDistance(previousPos, thisPos);

                        if (rel.Type == "STAIRS")
                            pathDist += 10;
                        else if (rel.Type == "LIFT")
                            pathDist += 1;

                        if (pathDist > shortestDist)
                            break;
                        else
                            previousPos = thisPos;
                    }

                    if (pathDist < shortestDist)
                    {
                        shortestDist = pathDist;
                        bestPath = path;
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Debugger.Break();
#endif
            }

            int endLoop = bestPath.Relationships.Count();

            List<IPlaceEntity> myPath = new List<IPlaceEntity>();

            // get entry
            try
            {
                query = $"MATCH (b: Building) - [e: ENTRY] - (begin: Room {{ Id: {beginId} }} ) " +
                    " RETURN e";
                using (var session = _driver.Session())
                {
                    session.ReadTransaction(tx =>
                    {
                        var result = tx.Run(query);
                        var relation = result.First()["e"].As<IRelationship>();
                        int x = Int32.Parse(relation["X"].As<string>());
                        int y = Int32.Parse(relation["Y"].As<string>());
                        int z = Int32.Parse(relation["Z"].As<string>());
                        tx.Success();
                        tx.Dispose();
                        Door entry = new Door(new PositionEntity(x, y, z));
                        myPath.Add(entry);
                    });
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Debugger.Break();
#endif
            }

            //get final path
            try
            {
                for (int i = 0; i < endLoop; i++)
                {
                    Room nod = new Room(bestPath.Nodes[i]["Id"].As<string>(), "");
                    if (bestPath.Nodes[i].Labels.Contains("Room"))
                        nod.Name = bestPath.Nodes[i]["Name"].As<string>();
                    else
                        nod.Name = bestPath.Nodes[i].Labels.First();
                    myPath.Add(nod);

                    Door rel = new Door(new PositionEntity(bestPath.Relationships[i]["X"].As<double>(),
                        bestPath.Relationships[i]["Y"].As<double>(),
                        bestPath.Relationships[i]["Z"].As<double>()));
                    myPath.Add(rel);
                }

                Room n = new Room(bestPath.Nodes[endLoop]["Id"].As<string>(), "");
                if (bestPath.Nodes[endLoop].Labels.Contains("Room"))
                    n.Name = bestPath.Nodes[endLoop]["Name"].As<string>();
                else
                    n.Name = bestPath.Nodes[endLoop].Labels.First();
                myPath.Add(n);

                return myPath;
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

