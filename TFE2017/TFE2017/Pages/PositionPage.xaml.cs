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

        public PositionPage()
        {
            InitializeComponent();

            _listDirections = new List<string>(){ "N","NE","E","SE","S","SO","O","NO" };

            PositionEntity pos1 =  new PositionEntity() { PosX = 10, PosY = 10};

            LabelPosition.Text = pos1.ToString();




            CrossCompass.Current.CompassChanged += (s, e) =>
            {
                Debug.WriteLine("*** Compass Heading = {0}", e.Heading);

                LabelHeading.Text = ((int)(e.Heading)).ToString() + "°";

                LabelDirection.Text = _listDirections[(int)((e.Heading + 22.5) / 45)];

            };

            CrossCompass.Current.Start();



        }
	}
}