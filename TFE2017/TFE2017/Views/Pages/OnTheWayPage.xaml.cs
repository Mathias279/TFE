using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFE2017.Core.Managers;
using TFE2017.Core.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TFE2017.Core.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OnTheWayPage : ContentPage
    {
        public OnTheWayPage(string buidingId, string departId, string destinationId, bool useStairs, bool useLift)
        {
            InitializeComponent();

            var path = DataBaseManager.GetPath(buidingId, departId, departId, useStairs, useLift);

        }
    }
}