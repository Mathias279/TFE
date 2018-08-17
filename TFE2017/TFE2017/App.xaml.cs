using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFE2017.Core.Managers;
using TFE2017.Core.Views.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TFE2017
{
	public partial class App : Application
    {
        public static string BuildingId { get; set; }
        public static string EntryId { get; set; }
        public static string DestinationId { get; set; }

        public App ()
		{
            try
            {
                InitializeComponent();
                MainPage = new NavigationPage(new MainPage());
            }
            catch (Exception ex)
            {
#if DEBUG
                Debugger.Break();
#endif
            }
        }

        public static async Task<bool> CheckPermission(Permission permission)
        {
            try
            {
                if (Device.RuntimePlatform != Device.Android)
                {
                    return true;
                }
                else
                {
                    var status = await CrossPermissions.Current.CheckPermissionStatusAsync(permission);
                    if (status != PermissionStatus.Granted)
                    {
                        var results = await CrossPermissions.Current.RequestPermissionsAsync(permission);
                        if (results.ContainsKey(permission))
                        {
                            status = results[permission];
                        }
                    }
                    if (status == PermissionStatus.Granted)
                    {
                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Debugger.Break();
#endif
                return false;
            }
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
