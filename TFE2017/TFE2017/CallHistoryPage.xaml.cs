using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TFE2017.Core
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CallHistoryPage : ContentPage
	{
		public CallHistoryPage ()
		{
			InitializeComponent ();


		}

        async void OnErase(object sender, EventArgs e)
        {
            App.PhoneNumbers.Clear();
            InitializeComponent();
        }
	}
}