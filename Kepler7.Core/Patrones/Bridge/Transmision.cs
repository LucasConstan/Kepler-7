using System;
using System.Collections.Generic;

namespace Kepler7.Core.Patrones.Bridge
{
    // El resultado de enviar un mensaje: qué se mandó, cómo viajó y qué leyó la Tierra.
    // La interfaz lo usa para mostrar el envío y el historial.
    public class Transmision
    {
        public DateTime Hora { get; }
        public string TipoMensaje { get; }
        public string NombreCifrado { get; }
        public string TextoOriginal { get; }
        public string TextoCifrado { get; }
        public string TextoRecibido { get; }
        public IReadOnlyList<string> Pasos { get; }

        public Transmision(DateTime hora, string tipoMensaje, string nombreCifrado, string textoOriginal,
                           string textoCifrado, string textoRecibido, List<string> pasos)
        {
            Hora = hora;
            TipoMensaje = tipoMensaje;
            NombreCifrado = nombreCifrado;
            TextoOriginal = textoOriginal;
            TextoCifrado = textoCifrado;
            TextoRecibido = textoRecibido;
            Pasos = pasos;
        }

        // True si la Tierra leyó exactamente lo que salió de la estación.
        public bool LlegoIntacto
        {
            get { return TextoOriginal == TextoRecibido; }
        }

        public override string ToString()
        {
            return "[" + Hora.ToString("HH:mm:ss") + "] " + TipoMensaje + " + " + NombreCifrado;
        }
    }
}
