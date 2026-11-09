namespace Kepler7.Core.Patrones.TemplateMethod
{
    // Una línea del historial: qué hizo una unidad, en qué tick y en qué etapa de su turno.
    public class EntradaTurno
    {
        public int Tick { get; }
        public string Unidad { get; }
        public EtapaTurno Etapa { get; }
        public string Texto { get; }

        public EntradaTurno(int tick, string unidad, EtapaTurno etapa, string texto)
        {
            Tick = tick;
            Unidad = unidad;
            Etapa = etapa;
            Texto = texto;
        }

        public override string ToString()
        {
            // Estas dos llevan tilde al mostrarse, por eso no alcanza con Etapa.ToString().
            string etapa;
            if (Etapa == EtapaTurno.Decision)
            {
                etapa = "Decisión";
            }
            else if (Etapa == EtapaTurno.Accion)
            {
                etapa = "Acción";
            }
            else
            {
                etapa = Etapa.ToString();
            }

            return "[t" + Tick + "] " + Unidad + " · " + etapa + ": " + Texto;
        }
    }
}
