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
    [Activity(Label = "Resultados")]
    public class ResultadosBeautyContest : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.ResultadosBeautyContestLayout);

            TextView numResultados = FindViewById<TextView>(Resource.Id.textViewResultados1);
            TextView media = FindViewById<TextView>(Resource.Id.textViewResultados2);
            TextView ganador = FindViewById<TextView>(Resource.Id.textViewResultados3);

            loadResultados(numResultados, media, ganador);


            FindViewById<Button>(Resource.Id.actualizarResultados).Click += delegate
            {
                loadResultados(numResultados, media, ganador);
            };


            FindViewById<Button>(Resource.Id.continuarResultados).Click += delegate
            {
                if (Convert.ToInt32(Intent.Extras.Get("RONDA")) < Convert.ToInt32(Intent.Extras.Get("RONDAS")))
                {
                    Intent intent = new Intent(this, typeof(BeautyContestActivity));
                    intent.SetFlags(ActivityFlags.NewTask);
                    intent.PutExtra("USUARIO", (string) Intent.Extras.Get("USUARIO"));
                    intent.PutExtra("RONDAS", Convert.ToInt32(Intent.Extras.Get("RONDAS")));
                    intent.PutExtra("RONDA", Convert.ToInt32(Intent.Extras.Get("RONDA")) + 1);
                    StartActivity(intent);
                }
                else
                {
                    StartActivity(new Intent(this, typeof(FinActivity)));
                }
            };
        }

        private  void loadResultados(TextView results, TextView media, TextView ganador)
        {
            string user = (string)Intent.Extras.Get("USUARIO");
            model.Resultado[] resultados = ServerConnection.resultadosExperimento(user);
            IList<model.Resultado> resultadosFiltered = new List<model.Resultado>();
            //Filtramos los de la ronda actual
            foreach(model.Resultado r in resultados)
            {
                if(r.Ronda == Convert.ToInt32(Intent.Extras.Get("RONDA")))
                {
                    resultadosFiltered.Add(r);
                }
            }
            //Calculos
            float mediaResultados = 0;
            model.Resultado mejorResultado = new model.Resultado();
            foreach(model.Resultado r in resultadosFiltered)
            {
                mediaResultados += r.ValorNumerico;
            }
            mediaResultados /= resultados.Length;

            float diff = 1000; //Numero muy grande
            foreach(model.Resultado r in resultadosFiltered)
            {
                if(diff > Math.Abs(r.ValorNumerico-mediaResultados))
                {
                    diff = Math.Abs(r.ValorNumerico - mediaResultados);
                    mejorResultado = r;
                }
            }
            
            //Set resultados en la interfaz
            results.Text = "Resultados: " + resultados.Length;
            media.Text = "Media: " + mediaResultados;
            ganador.Text = "Ganador: " + mejorResultado.Usuario + ", Valor: " + mejorResultado.ValorNumerico;
        }
    }
}