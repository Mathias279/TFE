using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using TFE2017.Core.Managers;
using TFE2017.Core.Pages;
using Xamarin.Forms;

namespace TFE2017
{
	public partial class App : Application
	{
		public App ()
		{
            try
            {
                InitializeComponent();
                Init();
                MainPage = new NavigationPage(new MainPage());
            }
            catch (Exception ex)
            {
#if DEBUG
                Debugger.Break();
#endif
            }
        }

        private void Init()
        {
            //DatabaseManager
        }

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
