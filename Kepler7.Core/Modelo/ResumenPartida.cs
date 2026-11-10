using System.Collections.Generic;

namespace Kepler7.Core.Modelo
{
    // Una foto de cómo terminó la partida, para la pantalla final y para calcular la recompensa.
    // Se arma una sola vez al final, así los números no cambian mientras se muestran.
    public class ResumenPartida
    {
        public bool Victoria { get; }

        // Distingue una derrota de verdad de una estación apagada desde el Control central.
        public bool FueApagada { get; }

        public int OleadasSuperadas { get; }
        public int TotalOleadas { get; }
        public int EnemigosDerrotados { get; }
        public int DefensoresPerdidos { get; }
        public int TicksJugados { get; }
        public int CreditosFinales { get; }
        public int TripulantesPerdidos { get; }
        public List<string> SubsistemasCaidos { get; }

        public ResumenPartida(bool victoria, bool fueApagada, int oleadasSuperadas, int totalOleadas, int enemigosDerrotados,
                              int defensoresPerdidos, int ticksJugados, int creditosFinales, int tripulantesPerdidos,
                              List<string> subsistemasCaidos)
        {
            Victoria = victoria;
            FueApagada = fueApagada;
            OleadasSuperadas = oleadasSuperadas;
            TotalOleadas = totalOleadas;
            EnemigosDerrotados = enemigosDerrotados;
            DefensoresPerdidos = defensoresPerdidos;
            TicksJugados = ticksJugados;
            CreditosFinales = creditosFinales;
            TripulantesPerdidos = tripulantesPerdidos;
            SubsistemasCaidos = subsistemasCaidos;
        }
    }
}
