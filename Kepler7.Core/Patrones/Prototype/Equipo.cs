using System.Collections.Generic;

namespace Kepler7.Core.Patrones.Prototype
{
    // Una pieza de equipamiento. También se clona: cada tripulante clonado recibe su propia copia.
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

        public Equipo Clonar()
        {
            return (Equipo)MemberwiseClone();
        }

        public override string ToString()
        {
            string bonus = "";
            if (BonusDanio > 0)
            {
                bonus = "+" + BonusDanio + " daño";
            }

            if (BonusVida > 0)
            {
                // Solo hace falta la coma si ya había algo escrito antes.
                if (bonus != "")
                {
                    bonus = bonus + ", ";
                }
                bonus = bonus + "+" + BonusVida + " vida";
            }

            return Nombre + " (" + bonus + ") - " + Costo + " cr";
        }

        // El equipamiento que se puede comprar para los tripulantes de la reserva.
        public static List<Equipo> Catalogo()
        {
            List<Equipo> catalogo = new List<Equipo>();
            catalogo.Add(new Equipo("Rifle de plasma", 10, 0, 40));
            catalogo.Add(new Equipo("Blindaje táctico", 0, 50, 35));
            catalogo.Add(new Equipo("Implante de reflejos", 5, 20, 45));
            catalogo.Add(new Equipo("Granadas EMP", 15, 0, 55));
            return catalogo;
        }
    }
}
