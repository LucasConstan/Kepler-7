using System.Drawing;
using Kepler7.Core.Patrones.TemplateMethod;

namespace Kepler7.Core.Patrones.Prototype
{
    public class Francotirador : Tripulante
    {
        public override TurnoUnidad Turno { get; } = new TurnoFrancotirador();

        public Francotirador() : base("Francotirador", "Fr", Color.FromArgb(150, 100, 220), 25, 60, 75)
        {
            Equipar(new Equipo("Mira térmica", 5, 0, 0));
        }
    }
}
