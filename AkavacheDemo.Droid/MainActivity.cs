﻿using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Akavache;
using AkavacheDemo.Common;
using AndroidHUD;
using Splat;

namespace AkavacheDemo.Droid
{
    [Activity(Label = "AkavacheDemo.Droid", MainLauncher = true, Icon = "@drawable/icon", Theme="@android:style/Theme.Holo.Light")]
    public class MainActivity : Activity
    {
        int count = 1;
        DataCache _cache;

        Webservice _service;
        protected override void OnCreate(Bundle bundle)
        {

            BlobCache.ApplicationName = "AkavacheDemo";
            _service = new Webservice();
            _cache = new DataCache(_service);

            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.btnFindAirport);
            Button saveButton = FindViewById<Button>(Resource.Id.btnSave);
            Button getImageButtom = FindViewById<Button>(Resource.Id.btnImage);
            ImageView imView = FindViewById<ImageView>(Resource.Id.airportImage);
            EditText airportCode = FindViewById<EditText>(Resource.Id.txtAirportCode);
            TextView code = FindViewById<TextView>(Resource.Id.lblCode);
            TextView name = FindViewById<TextView>(Resource.Id.lblName);
            TextView location = FindViewById<TextView>(Resource.Id.lblLocation);
            EditText comment = FindViewById<EditText>(Resource.Id.txtComment);
            button.Click += async delegate
            {
                AndHUD.Shared.Show(this, "Searching", -1, MaskType.Black);
                if(!string.IsNullOrWhiteSpace(airportCode.Text))
                {
                    //get the data
                    var airport = await _cache.GetAirport(airportCode.Text);

                    AndHUD.Shared.Dismiss();

                    //set the data
                    if(airport != null)
                    {
                        code.Text = airport.code;
                        name.Text = airport.name;
                        location.Text = airport.location;
                        comment.Text = airport.comments;
                    }
                    else
                    {
                        AndHUD.Shared.ShowErrorWithStatus(this, string.Format("{0} Not Found",airportCode.Text),
                            MaskType.Black, TimeSpan.FromSeconds(3));
                    }
                }

            };

            saveButton.Click += async delegate
            {
                AndHUD.Shared.Show(this, "Saving", -1, MaskType.Black);
                if(!string.IsNullOrWhiteSpace(code.Text))
                {
                    var airportToSave = new Airport()
                    {
                        code=code.Text,
                        location = location.Text,
                        name = name.Text,
                        comments = comment.Text
                    };
                    await _cache.StoreAirport(airportToSave).ContinueWith(p => AndHUD.Shared.Dismiss())
                        .ContinueWith(d => AndHUD.Shared.ShowSuccess(this, "Saved", MaskType.Black, TimeSpan.FromSeconds(2)));
                }
            };

            getImageButtom.Click += async delegate
            {
                AndHUD.Shared.Show(this, "Getting Image", -1, MaskType.Black);
                var image = await _cache.GetAnImage();
                imView.SetImageDrawable(image.ToNative());

                AndHUD.Shared.Dismiss();
            };
        }
    }
}


