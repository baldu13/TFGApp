using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ClientApp
{
    [Activity(Label = "Gracias por participar")]
    public class FinActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.AgradecimientoLayout);

            FindViewById<Button>(Resource.Id.botonFin).Click += delegate
            {
                StartActivity(new Intent(this, typeof(MainActivity)));
            };
        }
    }
}