using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFE2017.Core;
using Xamarin.Forms;

namespace TFE2017
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();

            Navigation.PushAsync(new QrCodePage());
		}
	}
}
