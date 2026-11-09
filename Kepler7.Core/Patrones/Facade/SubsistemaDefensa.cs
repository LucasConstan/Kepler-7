using Kepler7.Core.Modelo;

namespace Kepler7.Core.Patrones.Facade
{
    // Arma a los defensores desplegados.
    internal class SubsistemaDefensa
    {
        private readonly Partida _partida;

        public SubsistemaDefensa(Partida partida)
        {
            _partida = partida;
        }

        public string ArmarDefensas()
        {
            return "Defensa: " + _partida.Grilla.Defensores().Count + " defensores armados y en posición.";
        }

        public string PriorizarSector(int carril)
        {
            return "Defensa: prioridad de fuego en " + Estacion.Subsistemas[carril] + ".";
        }

        public string Apagar()
        {
            return "Defensa: armas desactivadas.";
        }
    }
}
