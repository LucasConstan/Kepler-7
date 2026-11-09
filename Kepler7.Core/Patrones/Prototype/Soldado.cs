using System.Drawing;
using Kepler7.Core.Patrones.TemplateMethod;

namespace Kepler7.Core.Patrones.Prototype
{
    public class Soldado : Tripulante
    {
        public override TurnoUnidad Turno { get; } = new TurnoSoldado();

        public Soldado() : base("Soldado", "So", Color.FromArgb(70, 130, 220), 12, 100, 50)
        {
            Equipar(new Equipo("Rifle estándar", 3, 0, 0));
        }
    }
}
