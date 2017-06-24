using System;
using System.Net;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace ClientApp
{
    [Activity(Label = "ExperimentsUC", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            EditText usuario = FindViewById<EditText>(Resource.Id.usuarioInput);
            EditText pass = FindViewById<EditText>(Resource.Id.passInput);
            TextView error = FindViewById<TextView>(Resource.Id.textError);
            Button button = FindViewById<Button>(Resource.Id.botonLog);

            button.Click += delegate {
                string txt = "";
                if (hayInternet())
                {
                    model.Experimento exp = ServerConnection.logeaExperimento(usuario.Text, pass.Text);
                    //TODO dependiendo del tipo e id
                    Intent intent = null;
                    switch (exp.Tipo)
                    {
                        case -1:
                            txt = "Error al conectar";
                            break;
                        case 0:
                            txt = "Credenciales incorrectos";
                            break;
                        case 1:
                            intent = new Intent(this, typeof(BeautyContestActivity));
                            intent.SetFlags(ActivityFlags.NewTask);
                            intent.PutExtra("USUARIO", usuario.Text);
                            intent.PutExtra("RONDAS", exp.Rondas);
                            intent.PutExtra("RONDA", 1);
                            StartActivity(intent);
                            break;
                        case 2:
                            intent = new Intent(this, typeof(FondosActivity));
                            intent.SetFlags(ActivityFlags.NewTask);
                            intent.PutExtra("USUARIO", usuario.Text);
                            intent.PutExtra("RONDAS", exp.Rondas);
                            intent.PutExtra("RONDA", 1);
                            intent.PutExtra("IDEXP", exp.Id);
                            StartActivity(intent);
                            break;
                    }
                }else
                {
                    txt = "No hay conexion a Internet";
                }
                error.Text= txt;
            };
        }

        private static bool hayInternet()
        {
            try
            {
                new WebClient().OpenRead("http://www.google.com");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

