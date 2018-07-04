using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using TFE2017.Core.Models;

namespace TFE2017.Core.Managers
{
    class DatabaseManager
    {
        public const string DataBaseFile   = "dbBuildings.db3";
        public const Environment.SpecialFolder DataBaseFolder = Environment.SpecialFolder.Personal;

        public string DataDasePath;

        public SQLite.SQLiteConnection DataBase;
        

        public void Init()
        {
            DataDasePath = Path.Combine(Environment.GetFolderPath(DataBaseFolder),DataBaseFile);            
        }


        private bool IsDbPresent()
        {
            return File.Exists(DataDasePath);
        }
            
        public bool CreateDatabase()
        {
            try
            {
                
                DataBase = new SQLite.SQLiteConnection(DataDasePath);

                DataBase.CreateTable<BuildingEntity>();

                DataBase.CreateTable<DoorEntity>();

                DataBase.CreateTable<RoomEntity>();
                return true;

            }
            catch (Exception ex)
            {
#if DEBUG
                Debugger.Break();
#endif
                return false;
            }
        }
        private void InsertBuilding(BuildingEntity building)
        {

            DataBase.Insert(building);
            
        }
    }
}
