using Plugin.Compass;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFE2017.Core.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TFE2017.Core
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PositionPage : ContentPage
	{
        private List<string> _listDirections;
        private int _degres;
        PositionEntity _pos1;
        int _indexDirection;

        public PositionPage()
        {
            try
            {
                InitializeComponent();
                _listDirections = new List<string>() { "N", "NE", "E", "SE", "S", "SO", "O", "NO" };
                _degres = 0;

                CrossCompass.Current.CompassChanged += (s, e) =>
                {
                    _degres = (int)(e.Heading) + 1;// so always > 0

                LabelHeading.Text = _degres.ToString() + "°";
                    _indexDirection = (_degres + 22) / 45;
                    LabelDirection.Text = _listDirections[_indexDirection];
                };

                CrossCompass.Current.Start();

                BuildingEntity ephec = new BuildingEntity();
                ephec.FloorsList[0] = new FloorEntity();
            }
            catch(Exception ex)
            {
                Debugger.Break();
            }
        }
	}
}