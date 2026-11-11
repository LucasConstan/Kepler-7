namespace Kepler7.Core.Patrones.Bridge
{
    // Una emergencia: va en mayúsculas, con MAYDAY y la posición de la estación para que la encuentren.
    public class MensajeEmergencia : Mensaje
    {
        public MensajeEmergencia(ICifrado cifrado) : base(cifrado)
        {
        }

        public override string Tipo
        {
            get { return "Emergencia"; }
        }

        public override string Prioridad
        {
            get { return "máxima"; }
        }

        public override string Frecuencia
        {
            get { return "todas las frecuencias a la vez"; }
        }

        protected override string Formatear(string contenido)
        {
            return "[EMERGENCIA] MAYDAY MAYDAY MAYDAY · " + contenido.ToUpperInvariant()
                + " · POSICIÓN: ÓRBITA DE KEPLER-7, SECTOR 7G — Estación Kepler-7";
        }
    }
}
