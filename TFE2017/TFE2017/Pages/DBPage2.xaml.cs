using Neo4j.Driver.V1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TFE2017.Core.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DBPage2 : ContentPage
    {
        public IDriver Neo4jDB;


        public DBPage2()
        {
            InitializeComponent();

            Connect();

            test();
        }

        private void Connect()
        {
            Neo4jDB = GraphDatabase.Driver("bolt://hobby-ohlpjagjjjpngbkejmmoojbl.dbs.graphenedb.com:24786", AuthTokens.Basic("Math279", "b.Ge9EvCbZwWWH.DWYwHYCkL6ycEu18"), Config.Builder.WithEncryptionLevel(EncryptionLevel.Encrypted).ToConfig());
        }

        public void test() { 
            var session = Neo4jDB.Session();
            using (var tx = session.BeginTransaction())
            {
                //tx.Run("CREATE (n:Person {name:'Bob'})");
                var result = tx.Run("match (n) return n").ToList();
                foreach (var element in result)
                {

                }
                tx.Success();
            }
            Debugger.Break();

            //Label1.TextColor = Color.Green;
        }
        public void GetPath(string origine, string destination)
        {
            
        }
    }
}