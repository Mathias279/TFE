using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using ZXing.Mobile;
using System.Threading.Tasks;
using Android;
using Plugin.Permissions;

namespace TFE2017.Droid
{
    [Activity(Label = "TFE2017", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivityOldPerm : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private bool _verbose;

        protected async override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            global::Xamarin.Forms.Forms.Init(this, bundle);


            _verbose = false;
#if DEBUG
            _verbose = true;
#endif
            bool gotPermission = true;
            if (ShouldShowRequestPermissionRationale(Manifest.Permission.Camera))
            {
                 await TryToGetPermissions();
            }
            LoadApplication(new App());
        }

        #region RuntimePermissions

        const int RequestLocationId = 0;
        readonly string[] PermissionsGroupLocation =
            {
                Manifest.Permission.Camera
            };

        async Task TryToGetPermissions()
        {
            if (_verbose)
                Toast.MakeText(this, "SDK version = " + (int)Build.VERSION.SdkInt, ToastLength.Short).Show();

            if ((int)Build.VERSION.SdkInt >= 23)
            {
                if (_verbose)
                    Toast.MakeText(this, "SDK version >= 23", ToastLength.Short).Show();
                await GetPermissionsAsync();
            }
            else
            {
                if (_verbose)
                    Toast.MakeText(this, "SDK version < 23", ToastLength.Short).Show();
            }
        }
                
        async Task GetPermissionsAsync()
        {
            const string CameraPermission = Manifest.Permission.Camera;
            if (CheckSelfPermission(CameraPermission) == (int)Permission.Granted)
            {
                //TODO change the message to show the permissions name
                if (_verbose)
                    Toast.MakeText(this, "Camera permissions granted", ToastLength.Short).Show();
            }
            else
            {
                if (_verbose)
                    Toast.MakeText(this, "Camera permissions not granted", ToastLength.Short).Show();
            }

            if (ShouldShowRequestPermissionRationale(CameraPermission))
            {
                //set alert for executing the task
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("Permissions Needed");
                alert.SetMessage("The application need special permissions to continue");
                alert.SetPositiveButton("Request Permissions", (senderAlert, args) =>
                {
                    RequestPermissions(PermissionsGroupLocation, RequestLocationId);
                });

                alert.SetNegativeButton("Cancel", (senderAlert, args) =>
                {
                    Toast.MakeText(this, "Cancelled!", ToastLength.Short).Show();
                });

                Dialog dialog = alert.Create();
                dialog.Show();
            }
            RequestPermissions(PermissionsGroupLocation, RequestLocationId);
        }
        
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
         {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            switch (requestCode)
            {
                case RequestLocationId:
                    {
                        if (grantResults[0] == (int)Android.Content.PM.Permission.Granted)
                        {
                            Toast.MakeText(this, "Special permissions granted", ToastLength.Short).Show();
                        }
                        else
                        {
                            //Permission Denied :(
                            Toast.MakeText(this, "Special permissions denied", ToastLength.Short).Show();
                        }
                    }
                    break;
            }
            //base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        #endregion

    }
}

