using Kepler7.Core.Modelo;

namespace Kepler7.Core.Patrones.Facade
{
    // Los subsistemas son "internal" a propósito: desde Kepler7.Game no se pueden usar,
    // así el formulario está obligado a pasar por el ControlCentral.
    internal class SubsistemaEnergia
    {
        private readonly Partida _partida;

        public SubsistemaEnergia(Partida partida)
        {
            _partida = partida;
        }

        public string EncenderReactor()
        {
            return "Energía: reactor al 100%.";
        }

        public string DesviarEnergiaA(int carril)
        {
            int reparado = _partida.RepararNucleo(carril, 30);
            return "Energía: desvía potencia al núcleo de " + Estacion.Subsistemas[carril] + " (+" + reparado + " de integridad).";
        }

        public string Apagar()
        {
            return "Energía: reactor apagado.";
        }
    }
}
