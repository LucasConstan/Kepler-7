using System.Collections.Generic;

namespace Kepler7.Core.Modelo
{
    // Lo que el Hub ya le pagó a la partida que se está jugando. Se guarda dentro del progreso
    // (y no en la partida guardada) a propósito: si se retoma un guardado viejo y se vuelve a jugar
    // una oleada o a ganar un logro, no se cobra dos veces.
    public class PartidaEnCurso
    {
        public string Id { get; set; } = "";

        // Logros ganados en esta partida, y los que todavía esperan la primera oleada para acreditarse.
        public List<string> Logros { get; set; } = new List<string>();
        public List<string> LogrosSinAcreditar { get; set; } = new List<string>();

        // La oleada más alta que ya se pagó, y cuántos enemigos derrotados ya se pagaron.
        public int OleadasPagadas { get; set; }
        public int EnemigosPagados { get; set; }

        // Lo que lleva ganado en total, para el mensaje del final.
        public int Xp { get; set; }
        public int Monedas { get; set; }
    }
}
