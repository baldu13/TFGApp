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
    [Activity(Label = "ResultadoFondos")]
    public class ResultadoFondos : Activity
    {
        float totalUsuario;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.ResultadosFondosLayout);

            TextView numResultados = FindViewById<TextView>(Resource.Id.textViewNumResultados);
            TextView fpu = FindViewById<TextView>(Resource.Id.textViewFPu);
            TextView fpr = FindViewById<TextView>(Resource.Id.textViewFPr);
            TextView total = FindViewById<TextView>(Resource.Id.textViewTotalPublico);
            TextView resumen = FindViewById<TextView>(Resource.Id.textViewResumen);
            TextView ganancias = FindViewById<TextView>(Resource.Id.textViewGananciasAcumuladas);
            Button continuar = FindViewById<Button>(Resource.Id.continuarResultados2);
            Button actualizar = FindViewById<Button>(Resource.Id.actualizarResultados2);

            loadResultados(numResultados, fpu, fpr, total, resumen, ganancias, continuar, actualizar);


            actualizar.Click += delegate
            {
                loadResultados(numResultados, fpu, fpr, total, resumen, ganancias, continuar, actualizar);
            };


            continuar.Click += delegate
            {
                if (Convert.ToInt32(Intent.Extras.Get("RONDA")) < Convert.ToInt32(Intent.Extras.Get("RONDAS")))
                {
                    Intent intent = new Intent(this, typeof(FondosActivity));
                    intent.SetFlags(ActivityFlags.NewTask);
                    intent.PutExtra("USUARIO", (string)Intent.Extras.Get("USUARIO"));
                    intent.PutExtra("RONDAS", Convert.ToInt32(Intent.Extras.Get("RONDAS")));
                    intent.PutExtra("RONDA", Convert.ToInt32(Intent.Extras.Get("RONDA")) + 1);
                    intent.PutExtra("IDEXP", Convert.ToInt32(Intent.Extras.Get("IDEXP")));
                    float newAcumulado = Convert.ToSingle(Intent.Extras.Get("ACUMULADO")) + totalUsuario;
                    intent.PutExtra("ACUMULADO", Convert.ToSingle(newAcumulado));
                    StartActivity(intent);
                }
                else
                {
                    StartActivity(new Intent(this, typeof(FinActivity)));
                }
            };
        }

        private void loadResultados(TextView results, TextView fpu, TextView fpr, TextView total, TextView resumen, TextView gananciasAcumuladas, Button continuar, Button actualizar)
        {
            string user = (string)Intent.Extras.Get("USUARIO");
            model.Resultado[] resultados = ServerConnection.resultadosExperimento(user);
            IList<model.Resultado> resultadosFilteredPublico = new List<model.Resultado>();
            float publicoPropio = Convert.ToSingle(Intent.Extras.Get("PUBLICO"));
            float privadoPropio = Convert.ToSingle(Intent.Extras.Get("PRIVADO"));
            //Filtramos los de la ronda actual
            foreach (model.Resultado r in resultados)
            {
                //Comprobamos que sea de esta ronda y sea publico
                if (r.Ronda == Convert.ToInt32(Intent.Extras.Get("RONDA")) && r.Etiqueta.Equals("Fondo Publico"))
                {
                    resultadosFilteredPublico.Add(r);
                }
            }
            //Calculos
            float totalPublico = resultadosFilteredPublico.Sum(x => x.ValorNumerico);
            int numParticipantes = resultados.Length / (2 * Convert.ToInt32(Intent.Extras.Get("RONDA")));

            //Set resultados en la interfaz
            int totalResultados = ServerConnection.getTotalParticipantes(Convert.ToInt32(Intent.Extras.Get("IDEXP")))*(Convert.ToInt32(Intent.Extras.Get("RONDA")))/ (Convert.ToInt32(Intent.Extras.Get("RONDAS")));
            float[] ratios = ServerConnection.getRatiosExperimento(Convert.ToInt32(Intent.Extras.Get("IDEXP")));

            if (totalResultados > resultados.Length / 2)
            {
                results.Text = "Esperando resultados... " + resultados.Length / 2 + "/" + totalResultados;
                fpu.Visibility = ViewStates.Invisible;
                fpr.Visibility = ViewStates.Invisible;
                total.Visibility = ViewStates.Invisible;
                resumen.Visibility = ViewStates.Invisible;
                gananciasAcumuladas.Visibility = ViewStates.Invisible;
                results.Visibility = ViewStates.Visible;

                continuar.Enabled = false;
                actualizar.Enabled = true;
            }
            else
            {
                fpu.Text = "Tu aportación al Fondo público: " + publicoPropio + "€";
                fpr.Text = "Tu aportación al Fondo privado: " + privadoPropio + "€";
                total.Text = "Aportación media al fondo público: " + (totalPublico / numParticipantes) + "€";
                totalUsuario = (ratios[1] * privadoPropio + ratios[0] * (totalPublico / numParticipantes));
                resumen.Text = "Tus ganancias: " + totalUsuario + "€";
                gananciasAcumuladas.Text = "Tus ganancias acumuladas: " + (Convert.ToSingle(Intent.Extras.Get("ACUMULADO"))+totalUsuario);
                fpu.Visibility = ViewStates.Visible;
                fpr.Visibility = ViewStates.Visible;
                total.Visibility = ViewStates.Visible;
                resumen.Visibility = ViewStates.Visible;
                gananciasAcumuladas.Visibility = ViewStates.Visible;
                results.Visibility = ViewStates.Invisible;

                continuar.Enabled = true;
                actualizar.Enabled = false;
            }
        }
    }
}