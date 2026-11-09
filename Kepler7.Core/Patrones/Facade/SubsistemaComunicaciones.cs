using Kepler7.Core.Modelo;

namespace Kepler7.Core.Patrones.Facade
{
    // Los avisos a la tripulación y a la Tierra.
    internal class SubsistemaComunicaciones
    {
        private readonly Partida _partida;

        public SubsistemaComunicaciones(Partida partida)
        {
            _partida = partida;
        }

        public string AnunciarOleada()
        {
            return "Comunicaciones: alerta general, se acerca la oleada \"" + _partida.OleadaActual.Nombre + "\".";
        }

        public string EnviarSOS()
        {
            return "Comunicaciones: SOS enviado a la Tierra.";
        }

        public string Apagar()
        {
            return "Comunicaciones: último mensaje enviado, antenas desconectadas.";
        }
    }
}
