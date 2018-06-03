using System;
using System.Collections.Generic;
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
        public PositionPage()
        {
            InitializeComponent();


            new PositionEntity() { PosX = 10, PosY = 10};


		}
	}
}