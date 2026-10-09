using System.Collections.Generic;

namespace Kepler_7.Patrones.Prototype
{
    /// <summary>
    /// Pieza de equipamiento de un tripulante. También es un prototipo: cada tripulante
    /// clonado recibe su propia copia, así que modificar el equipo del clon no toca al original.
    /// </summary>
    public class Equipo : IPrototipo<Equipo>
    {
        public string Nombre { get; }
        public int BonusDanio { get; }
        public int BonusVida { get; }
        public int Costo { get; }

        public Equipo(string nombre, int bonusDanio, int bonusVida, int costo)
        {
            Nombre = nombre;
            BonusDanio = bonusDanio;
            BonusVida = bonusVida;
            Costo = costo;
        }

        public Equipo Clonar() => (Equipo)MemberwiseClone();

        public override string ToString()
        {
            var bonus = new List<string>();
            if (BonusDanio > 0) bonus.Add($"+{BonusDanio} daño");
            if (BonusVida > 0) bonus.Add($"+{BonusVida} vida");
            return $"{Nombre} ({string.Join(", ", bonus)}) — {Costo} cr";
        }

        /// <summary>Equipamiento que se puede comprar para los tripulantes de la reserva.</summary>
        public static List<Equipo> Catalogo()
        {
            return new List<Equipo>
            {
                new Equipo("Rifle de plasma", 10, 0, 40),
                new Equipo("Blindaje táctico", 0, 50, 35),
                new Equipo("Implante de reflejos", 5, 20, 45),
                new Equipo("Granadas EMP", 15, 0, 55),
            };
        }
    }
}
