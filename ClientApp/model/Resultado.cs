using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClientApp.model
{
    public class Resultado
    {
        private string usuario;
        private int grupo;
        private int ronda;
        private string etiqueta;
        private float valorNumerico;
        private string valorTexto;
        private int totalResultados;

        public string Usuario
        {
            get
            {
                return usuario;
            }

            set
            {
                usuario = value;
            }
        }

        public int Grupo
        {
            get
            {
                return grupo;
            }

            set
            {
                grupo = value;
            }
        }

        public int Ronda
        {
            get
            {
                return ronda;
            }

            set
            {
                ronda = value;
            }
        }

        public string Etiqueta
        {
            get
            {
                return etiqueta;
            }

            set
            {
                etiqueta = value;
            }
        }

        public float ValorNumerico
        {
            get
            {
                return valorNumerico;
            }

            set
            {
                valorNumerico = value;
            }
        }

        public string ValorTexto
        {
            get
            {
                return valorTexto;
            }

            set
            {
                valorTexto = value;
            }
        }

        public int TotalResultados
        {
            get
            {
                return totalResultados;
            }

            set
            {
                totalResultados = value;
            }
        }
    }
}
