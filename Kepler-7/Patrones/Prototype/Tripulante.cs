using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Kepler_7.Modelo;

namespace Kepler_7.Patrones.Prototype
{
    /// <summary>
    /// PROTOTYPE — Prototipo abstracto. Un tripulante sabe clonarse a sí mismo:
    /// copia superficial con MemberwiseClone + copia profunda de la lista de equipamiento.
    /// </summary>
    public abstract class Tripulante : Unidad, IPrototipo<Tripulante>
    {
        private List<Equipo> _equipamiento = new List<Equipo>();

        public string Rol { get; private set; }
        public int DanioBase { get; private set; }
        public int VidaBase { get; private set; }

        /// <summary>Id del tripulante del que se clonó (null si es un original).</summary>
        public int? ClonadoDeId { get; private set; }

        public IReadOnlyList<Equipo> Equipamiento => _equipamiento;

        public override int Danio => DanioBase + _equipamiento.Sum(e => e.BonusDanio);
        public override int VidaMaxima => VidaBase + _equipamiento.Sum(e => e.BonusVida);

        protected Tripulante(string rol, string simbolo, Color color, int danioBase, int vidaBase, int costo)
        {
            Rol = rol;
            Nombre = rol;
            Simbolo = simbolo;
            Color = color;
            DanioBase = danioBase;
            VidaBase = vidaBase;
            Costo = costo;
            Vida = vidaBase;
        }

        public void Equipar(Equipo equipo)
        {
            _equipamiento.Add(equipo);
            Vida += equipo.BonusVida;
        }

        public Tripulante Clonar()
        {
            // Copia superficial: copia todos los campos (rol, stats, color...) de una sola vez.
            var clon = (Tripulante)MemberwiseClone();

            // Copia profunda de lo mutable: sin esto, el clon y el original compartirían
            // la MISMA lista y equipar al clon también equiparía al original.
            clon._equipamiento = _equipamiento.Select(e => e.Clonar()).ToList();

            clon.AsignarNuevoId();
            clon.ClonadoDeId = Id;
            clon.Vida = clon.VidaMaxima;
            return clon;
        }

        public override string Descripcion()
        {
            var equipo = _equipamiento.Count == 0
                ? "sin equipo"
                : string.Join(", ", _equipamiento.Select(e => e.Nombre));
            return $"{Rol} | Daño {Danio} | Vida {Vida}/{VidaMaxima} | Equipo: {equipo}";
        }
    }

    public class Soldado : Tripulante
    {
        public Soldado() : base("Soldado", "So", Color.FromArgb(70, 130, 220), 12, 100, 50)
        {
            Equipar(new Equipo("Rifle estándar", 3, 0, 0));
        }
    }

    public class Medico : Tripulante
    {
        public Medico() : base("Médico", "Me", Color.FromArgb(60, 180, 110), 4, 80, 60)
        {
            Equipar(new Equipo("Kit médico", 0, 10, 0));
        }
    }

    public class Francotirador : Tripulante
    {
        public Francotirador() : base("Francotirador", "Fr", Color.FromArgb(150, 100, 220), 25, 60, 75)
        {
            Equipar(new Equipo("Mira térmica", 5, 0, 0));
        }
    }

    public class Comandante : Tripulante
    {
        public Comandante() : base("Comandante", "Co", Color.FromArgb(220, 180, 60), 10, 140, 90)
        {
            Equipar(new Equipo("Comunicador táctico", 2, 0, 0));
        }
    }
}
