using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TFE2017.Core.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DestinationPage : ContentPage
	{
		public DestinationPage (string query)
		{
            try
            {
                InitializeComponent();

                //recherche par nom
                //arborescence


            }
            catch (Exception ex)
            {
#if DEBUG
                Debugger.Break();
#endif
            }
		}
	}
}