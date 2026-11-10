using System.Collections.Generic;

namespace Kepler7.Core.Modelo
{
    // El progreso del jugador en el Hub: XP, nivel, monedas y logros.
    // Las propiedades tienen get y set públicos porque así las necesita System.Text.Json
    // para pasarlas a JSON y volver a armarlas al cargar.
    public class Progreso
    {
        public const int XpPorNivel = 100;

        public int Xp { get; set; }

        // Las monedas solo deberían cambiar a través de la Bóveda (Proxy).
        public int Monedas { get; set; }

        public List<string> Logros { get; set; } = new List<string>();

        // El nivel se calcula con la XP: 0-99 es nivel 1, 100-199 es nivel 2, etc.
        // Como no tiene set, al cargar el JSON se ignora y se vuelve a calcular.
        public int Nivel
        {
            get { return Xp / XpPorNivel + 1; }
        }

        // Suma XP y devuelve true si con eso subió de nivel.
        public bool GanarXp(int cantidad)
        {
            int nivelAnterior = Nivel;
            Xp = Xp + cantidad;
            return Nivel > nivelAnterior;
        }

        // Revisa cada logro y devuelve solo los que se desbloquearon recién.
        public List<string> RevisarLogros()
        {
            List<string> nuevos = new List<string>();
            Desbloquear("Primera misión", Xp > 0, nuevos);
            Desbloquear("Veterano (nivel 3)", Nivel >= 3, nuevos);
            Desbloquear("Ahorrista (150 monedas)", Monedas >= 150, nuevos);
            return nuevos;
        }

        private void Desbloquear(string logro, bool cumplido, List<string> nuevos)
        {
            if (cumplido && !Logros.Contains(logro))
            {
                Logros.Add(logro);
                nuevos.Add(logro);
            }
        }
    }
}
