using Acr.UserDialogs;
using Neo4j.Driver.V1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TFE2017.Core.Managers
{
    class DataBaseManager
    {
        private ISession Connect()
        {
            IDriver driver = GraphDatabase.Driver("bolt://hobby-ohlpjagjjjpngbkejmmoojbl.dbs.graphenedb.com:24786", AuthTokens.Basic("Math279", "b.Ge9EvCbZwWWH.DWYwHYCkL6ycEu18"), Config.Builder.WithEncryptionLevel(EncryptionLevel.Encrypted).ToConfig());
            return driver.Session();
        }

        public async Task<List<string>> GetAllNodesNames()
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
    }
}
