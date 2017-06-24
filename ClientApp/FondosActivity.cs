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
    [Activity(Label = "Fondo Público y Privado")]
    public class FondosActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.FondosLayout);

            Button confirmar = FindViewById<Button>(Resource.Id.confirmarFondos);
            TextView textPublico = FindViewById<TextView>(Resource.Id.textViewFPu);
            TextView textPrivado = FindViewById<TextView>(Resource.Id.textViewFPr);
            SeekBar seekbar = FindViewById<SeekBar>(Resource.Id.seekBar);

            string txtRonda = "Ronda " + Intent.Extras.Get("RONDA");
            FindViewById<TextView>(Resource.Id.textViewRondaFondos).Text = txtRonda;

            seekbar.ProgressChanged += delegate {
                float result = (seekbar.Progress / 100f);
                textPublico.Text = "Fondo público: " + result + "€";
                textPrivado.Text = "Fondo privado: " + (200-result) + "€";
            };

            confirmar.Click += delegate {
                float num = (seekbar.Progress / 100f);
                string user = (string)Intent.Extras.Get("USUARIO");
                ServerConnection.enviaResultadoFondos(num, 200 - num, user, Convert.ToInt32(Intent.Extras.Get("RONDA")));

                Intent intent = new Intent(this, typeof(ResultadoFondos));
                intent.SetFlags(ActivityFlags.NewTask);
                intent.PutExtra("USUARIO", user);
                intent.PutExtra("RONDAS", Convert.ToInt32(Intent.Extras.Get("RONDAS")));
                intent.PutExtra("RONDA", Convert.ToInt32(Intent.Extras.Get("RONDA")));
                intent.PutExtra("PUBLICO", num);
                intent.PutExtra("PRIVADO", 200-num);
                intent.PutExtra("IDEXP", Convert.ToInt32(Intent.Extras.Get("IDEXP")));
                if(Convert.ToInt32(Intent.Extras.Get("RONDA")) == 1)
                {
                    intent.PutExtra("ACUMULADO", 0f);
                } else
                {
                    intent.PutExtra("ACUMULADO", Convert.ToSingle(Intent.Extras.Get("ACUMULADO")));
                }
                StartActivity(intent);
            };
        }
    }
}