using System;
using System.Collections.Generic;

namespace Kepler7.Core.Patrones.Bridge
{
    // Patrón Bridge (comunicaciones con la Tierra): esta es la "abstracción", es decir, QUÉ se manda
    // (texto, alerta o emergencia) y con qué formato. No cifra nada: se lo pide a su ICifrado, que es el puente.
    // El cifrado se puede cambiar en cualquier momento, así cualquier mensaje se combina con cualquier cifrado.
    public abstract class Mensaje
    {
        protected ICifrado Cifrado { get; private set; }

        protected Mensaje(ICifrado cifrado)
        {
            Cifrado = cifrado;
        }

        public abstract string Tipo { get; }
        public abstract string Prioridad { get; }
        public abstract string Frecuencia { get; }

        public void CambiarCifrado(ICifrado nuevo)
        {
            Cifrado = nuevo;
        }

        public ICifrado ObtenerCifrado()
        {
            return Cifrado;
        }

        // Cada tipo de mensaje arma su texto a su manera (encabezado, formato, datos extra).
        protected abstract string Formatear(string contenido);

        // Arma el texto, lo cifra con el cifrado que tenga en este momento y simula que la Tierra lo descifra.
        public Transmision Enviar(string contenido)
        {
            string texto = Formatear(contenido);
            string cifrado = Cifrado.Cifrar(texto);
            string recibido = Cifrado.Descifrar(cifrado);

            List<string> pasos = new List<string>();
            pasos.Add("1. " + Tipo + " armado (prioridad " + Prioridad + ")");
            pasos.Add("2. Cifrado con " + Cifrado.Nombre + ": " + texto.Length + " caracteres → " + cifrado.Length);
            pasos.Add("3. Transmitido por " + Frecuencia);
            pasos.Add("4. La Tierra lo descifra con " + Cifrado.Nombre + " y lo lee");

            return new Transmision(DateTime.Now, Tipo, Cifrado.Nombre, texto, cifrado, recibido, pasos);
        }
    }
}
