using System.Drawing;
using Kepler7.Core.Patrones.TemplateMethod;

namespace Kepler7.Core.Patrones.Prototype
{
    public class Medico : Tripulante
    {
        public override TurnoUnidad Turno { get; } = new TurnoMedico();

        public Medico() : base("Médico", "Me", Color.FromArgb(60, 180, 110), 4, 80, 60)
        {
            Equipar(new Equipo("Kit médico", 0, 10, 0));
        }
    }
}
