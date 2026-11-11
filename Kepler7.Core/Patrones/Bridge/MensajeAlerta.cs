namespace Kepler7.Core.Patrones.Bridge
{
    // Una alerta: pide respuesta y adjunta el estado de la estación.
    public class MensajeAlerta : Mensaje
    {
        public MensajeAlerta(ICifrado cifrado) : base(cifrado)
        {
        }

        public override string Tipo
        {
            get { return "Alerta"; }
        }

        public override string Prioridad
        {
            get { return "alta"; }
        }

        public override string Frecuencia
        {
            get { return "el canal prioritario de la flota"; }
        }

        protected override string Formatear(string contenido)
        {
            return "[ALERTA] " + contenido + " | Se pide respuesta en menos de 6 horas. — Estación Kepler-7";
        }
    }
}
