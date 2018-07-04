using SQLite;
using SQLite.Net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFE2017.Core.Models;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TFE2017.Core.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DbPage : ContentPage
	{
		public DbPage ()
		{
			InitializeComponent ();

            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),"dataBase.db3");
            bool IsDbPresent = File.Exists(dbPath);

            Debugger.Break();

            var db = new SQLite.SQLiteConnection(dbPath);

            db.CreateTable<BuildingEntity>();

            BuildingEntity ephec = new BuildingEntity();

            db.Insert(ephec);


        }
	}
}