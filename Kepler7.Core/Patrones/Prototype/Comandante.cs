using System.Drawing;
using Kepler7.Core.Patrones.TemplateMethod;

namespace Kepler7.Core.Patrones.Prototype
{
    // No confundir con el héroe, que también se llama "Comandante" pero es la clase Heroe.
    public class Comandante : Tripulante
    {
        public override TurnoUnidad Turno { get; } = new TurnoComandante();

        public Comandante() : base("Comandante", "Co", Color.FromArgb(220, 180, 60), 10, 140, 90)
        {
            Equipar(new Equipo("Comunicador táctico", 2, 0, 0));
        }
    }
}
