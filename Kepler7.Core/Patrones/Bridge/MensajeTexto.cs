namespace Kepler7.Core.Patrones.Bridge
{
    // Un mensaje común: el contenido tal cual, con la firma de la estación.
    public class MensajeTexto : Mensaje
    {
        public MensajeTexto(ICifrado cifrado) : base(cifrado)
        {
        }

        public override string Tipo
        {
            get { return "Texto"; }
        }

        public override string Prioridad
        {
            get { return "normal"; }
        }

        public override string Frecuencia
        {
            get { return "el canal de datos de rutina"; }
        }

        protected override string Formatear(string contenido)
        {
            return "[TEXTO] " + contenido + " — Estación Kepler-7";
        }
    }
}
