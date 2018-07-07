using System;
using System.Collections.Generic;
using System.Text;
using TFE2017.Core.Managers;
using TFE2017.Core.Models;

namespace TFE2017.Core.Managers.Interfaces
{
    interface IDataBaseManager
    {
        bool CreateDatabase();
        void InsertBuilding(BuildingEntity building);
        
    }

}
}
