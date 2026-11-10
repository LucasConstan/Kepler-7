using System.Collections.Generic;

namespace Kepler7.Core.Modelo
{
    // La foto de una partida entre oleadas, lista para pasar a JSON. Solo tiene datos:
    // la arma Partida.CrearGuardado() y la vuelve a armar Partida.DesdeGuardado().
    // Las propiedades tienen get y set públicos porque así las necesita System.Text.Json.
    public class PartidaGuardada
    {
        // Identifica la partida, para que el progreso sepa qué ya se cobró de ella.
        public string Id { get; set; } = "";
        public string Fecha { get; set; } = "";

        // La oleada que toca jugar al retomar.
        public int NumeroOleada { get; set; }
        public int Creditos { get; set; }
        public int Tick { get; set; }
        public int EnemigosDerrotados { get; set; }
        public int DefensoresPerdidos { get; set; }

        // Un valor por subsistema, en el orden de los carriles.
        public List<int> Integridad { get; set; } = new List<int>();
        public List<int> TripulantesPorCarril { get; set; } = new List<int>();
        public int TripulantesGenerados { get; set; }

        // La IA de la próxima oleada: el jugador la pudo haber cambiado.
        public string EstrategiaOleada { get; set; } = "";

        // Los defensores del tablero y los de la reserva (estos con Carril -1). El héroe también va acá.
        public List<UnidadGuardada> Unidades { get; set; } = new List<UnidadGuardada>();

        // Lo que tiene equipado el héroe. Un nombre vacío quiere decir que no tiene nada en ese lugar.
        public string ArmaHeroe { get; set; } = "";
        public int BonusArmaHeroe { get; set; }
        public string ArmaduraHeroe { get; set; } = "";
        public int BonusArmaduraHeroe { get; set; }

        // La cadena de defensas del héroe en su orden: "Blindaje", "Escudo" o "Evasion".
        public List<string> DefensasHeroe { get; set; } = new List<string>();

        public List<ItemGuardado> Inventario { get; set; } = new List<ItemGuardado>();
    }
}
