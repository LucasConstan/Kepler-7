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

        // Los logros que se consiguieron alguna vez (los de patrones se pueden volver a ganar en cada partida).
        public List<string> Logros { get; set; } = new List<string>();

        // Recompensas de logros que ya se ganaron y todavía no se cobraron con el botón del Hub.
        public int CargasDeLogro { get; set; }

        // Cuántas veces se consiguió cada logro de patrón en partidas que contaron. Se muestra en el Hub.
        public Dictionary<string, int> VecesPorLogro { get; set; } = new Dictionary<string, int>();

        // Lo que ya se pagó de la partida que se está jugando (logros, oleadas, enemigos).
        public PartidaEnCurso PartidaEnCurso { get; set; } = new PartidaEnCurso();

        // Mejoras que se compraron en la tienda y todavía no se usaron. Se gastan al empezar la próxima partida.
        // Si se carga un JSON viejo que no la tiene, queda la lista vacía de acá.
        public List<string> MejorasPendientes { get; set; } = new List<string>();

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

        // Revisa los logros que dependen de la XP y las monedas, y devuelve solo los que se desbloquearon recién.
        public List<string> RevisarLogros()
        {
            List<string> nuevos = new List<string>();
            Desbloquear(CatalogoLogros.PrimeraMision, Xp > 0, nuevos);
            Desbloquear(CatalogoLogros.Veterano, Nivel >= 3, nuevos);
            Desbloquear(CatalogoLogros.Ahorrista, Monedas >= 150, nuevos);
            return nuevos;
        }

        // Anota que un logro de patrón se consiguió alguna vez. Devuelve false si ya estaba anotado.
        public bool DesbloquearLogro(string logro)
        {
            if (Logros.Contains(logro))
            {
                return false;
            }

            Logros.Add(logro);
            return true;
        }

        public int VecesQueSeConsiguio(string logro)
        {
            if (!VecesPorLogro.ContainsKey(logro))
            {
                return 0;
            }
            return VecesPorLogro[logro];
        }

        public void SumarVez(string logro)
        {
            VecesPorLogro[logro] = VecesQueSeConsiguio(logro) + 1;
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
