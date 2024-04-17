using Android;
using Android.App;
using Android.Content.PM;
using Android.Nfc;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Core.App;
using Com.Xreader.Helper;
using Google.Android.Material.Snackbar;
using Newtonsoft.Json.Linq;
using System;
using System.Threading;
using static Android.Graphics.ColorSpace;

namespace AppTest
{


    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {

        int REQUEST_LOCATION = 1000;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            var requiredPermissions = new String[] { Manifest.Permission.AccessFineLocation, Manifest.Permission.BluetoothScan, Manifest.Permission.BluetoothAdvertise, Manifest.Permission.BluetoothConnect };

            ActivityCompat.RequestPermissions(this, requiredPermissions, REQUEST_LOCATION);

            string nameid = "bluetooth-rodinbell";

            RdUHFModule mrdUHFModule = new RdUHFModule(this);

            Button btn_create = FindViewById<Button>(Resource.Id.btn_create);
            btn_create.Click += (s, e) =>
            {
                //var res = mxreaderHelper.CreateReaderHub(nameid);
                var res = mrdUHFModule.CreateReader(nameid);
                Console.WriteLine("debug == " + res);
            };

            Button btn_connect = FindViewById<Button>(Resource.Id.btn_connect);
            btn_connect.Click += (s, e) =>
            {
                JObject jobj = new JObject
                {
                    {"method", "connectDevice"},

                };
                string connectstr = jobj.ToString();

                Thread thread = new Thread(() =>
                {
                    Console.WriteLine("connect ===" + Thread.CurrentThread.ManagedThreadId);
                    var res = mrdUHFModule.CallReaderUhf(nameid, connectstr);
                    Console.WriteLine("connect End == " +res);
                });
                thread.Start();
 

            };

            Button btn_getFirm = FindViewById<Button>(Resource.Id.btn_getFirm);
            btn_getFirm.Click += (s, e) =>
            {
                JObject jobj = new JObject
                {
                    {"method", "getFirmwareVersion"},
                };
                string getfirmstr = jobj.ToString();
                Console.WriteLine("debug == " + getfirmstr);
                var res = mrdUHFModule.CallReaderUhf(nameid, getfirmstr);
                Console.WriteLine("debug == " + res);
            };

            Button btn_inventory = FindViewById<Button>(Resource.Id.btn_inventory);
            btn_inventory.Click += (s, e) =>
            {
                JObject jobj = new JObject
                {
                    {"method", "customInventory"},
                };

                string inventorystr = jobj.ToString();
                Console.WriteLine("debug == " + inventorystr);
                var res = mrdUHFModule.CallReaderUhf(nameid, inventorystr);
                Console.WriteLine("debug == " + res);
            };

            Button btn_close = FindViewById<Button>(Resource.Id.btn_close);
            btn_close.Click += (s, e) =>
            {
                mrdUHFModule.DestroyReader(nameid);
            };

        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

        }

    }
}