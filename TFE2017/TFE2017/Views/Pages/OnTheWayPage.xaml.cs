using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFE2017.Core.Managers;
using TFE2017.Core.Models;
using TFE2017.Core.Views.Customs;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Permissions.Abstractions;
using ZXing.Net.Mobile.Forms;

namespace TFE2017.Core.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OnTheWayPage : ContentPage
    {
        private ZXingScannerView _scanView;

        public OnTheWayPage(string buidingId, string departId, string destinationId, bool useStairs, bool useLift)
        {
            InitializeComponent();
            
            InitVisual();

            var building = Task.Run(() => DataBaseManager.GetBuilding(buidingId)).Result;
            var path = Task.Run(() => DataBaseManager.GetPath(buidingId, departId, destinationId, useStairs, useLift)).Result;

        }

        private void InitVisual()
        {
        //    LabelTitre.Text = "Suivez la flèche";
        //    ButtonNext.Text = "Etape suivante";
        //    LabelNext.Text = "prochaine étape";
        }       

        private void ButtonPrevClicked(object sender, EventArgs e)
        {

        }

        private void ButtonNextClicked(object sender, EventArgs e)
        {

        }
    }
}