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
    [Activity(Label = "Beauty Contest")]
    public class BeautyContestActivity : Activity
    {
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.BeautyContestActivityLayout);

            //elementos del layout a utilizar
            EditText selector = FindViewById<EditText>(Resource.Id.selector);
            Button confirmar = FindViewById<Button>(Resource.Id.confirmar);

            string txtRonda = "Ronda "+Intent.Extras.Get("RONDA");
            FindViewById<TextView>(Resource.Id.textViewRonda).Text= txtRonda;

            confirmar.Click += delegate {
                float num = float.Parse(selector.Text);
                string user = (string)Intent.Extras.Get("USUARIO");
                ServerConnection.enviaResultadoBeautyContest(num, user, Convert.ToInt32(Intent.Extras.Get("RONDA")));

                Intent intent = new Intent(this, typeof(ResultadosBeautyContest));
                intent.SetFlags(ActivityFlags.NewTask);
                intent.PutExtra("USUARIO", user);
                intent.PutExtra("RONDAS", Convert.ToInt32(Intent.Extras.Get("RONDAS")));
                intent.PutExtra("RONDA", Convert.ToInt32(Intent.Extras.Get("RONDA")));
                StartActivity(intent);
            };
        }
    }
}