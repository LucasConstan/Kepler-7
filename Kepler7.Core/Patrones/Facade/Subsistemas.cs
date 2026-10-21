using System.Linq;
using Kepler7.Core.Modelo;

namespace Kepler7.Core.Patrones.Facade
{
    // FACADE - Subsistemas. Son "internal": desde Kepler7.Game no se pueden usar,
    // así el formulario está obligado a pasar por el ControlCentral.
    // Cada subsistema tiene sus propios métodos; la fachada sabe en qué orden llamarlos.

    /// <summary>Subsistema de Energía: el reactor. En emergencia desvía energía para reparar un núcleo.</summary>
    internal class SubsistemaEnergia
    {
        private readonly Partida _partida;

        public SubsistemaEnergia(Partida partida) => _partida = partida;

        public string EncenderReactor() => "Energía: reactor al 100%.";

        public string DesviarEnergiaA(int carril)
        {
            int reparado = _partida.RepararNucleo(carril, 30);
            return $"Energía: desvía potencia al núcleo de {Estacion.Subsistemas[carril]} (+{reparado} de integridad).";
        }

        public string Apagar() => "Energía: reactor apagado.";
    }

    /// <summary>Subsistema de Soporte Vital: oxígeno y atención médica de la tripulación.</summary>
    internal class SubsistemaSoporteVital
    {
        private readonly Partida _partida;

        public SubsistemaSoporteVital(Partida partida) => _partida = partida;

        public string VerificarOxigeno() =>
            $"Soporte Vital: oxígeno estable para {_partida.Grilla.Defensores().Count} defensores desplegados.";

        public string CurarDefensores(int carril)
        {
            int total = 0;
            foreach (var defensor in _partida.Grilla.Defensores().Where(d => d.Carril == carril))
                total += defensor.Curar(defensor.VidaMaxima);
            return $"Soporte Vital: equipos médicos curan a los defensores de {Estacion.Subsistemas[carril]} (+{total} PV).";
        }

        public string Apagar() => "Soporte Vital: tripulación evacuada a las cápsulas.";
    }

    /// <summary>Subsistema de Navegación: mantiene la órbita de la estación.</summary>
    internal class SubsistemaNavegacion
    {
        public string EstabilizarOrbita() => "Navegación: órbita estabilizada.";
        public string ManiobraEvasiva() => "Navegación: maniobra evasiva para proteger el sector dañado.";
        public string Apagar() => "Navegación: propulsores en reposo.";
    }

    /// <summary>Subsistema de Comunicaciones: avisos a la tripulación y a la Tierra.</summary>
    internal class SubsistemaComunicaciones
    {
        private readonly Partida _partida;

        public SubsistemaComunicaciones(Partida partida) => _partida = partida;

        public string AnunciarOleada() =>
            $"Comunicaciones: alerta general, se acerca la oleada \"{_partida.OleadaActual.Nombre}\".";

        public string EnviarSOS() => "Comunicaciones: SOS enviado a la Tierra.";
        public string Apagar() => "Comunicaciones: último mensaje enviado, antenas desconectadas.";
    }

    /// <summary>Subsistema de Seguridad: compuertas y sectores de la estación.</summary>
    internal class SubsistemaSeguridad
    {
        public string SellarCompuertas() => "Seguridad: compuertas exteriores selladas.";
        public string AislarSector(int carril) => $"Seguridad: sector {Estacion.Subsistemas[carril]} aislado del resto.";
        public string Apagar() => "Seguridad: todas las compuertas bloqueadas.";
    }

    /// <summary>Subsistema de Defensa: arma a los defensores desplegados.</summary>
    internal class SubsistemaDefensa
    {
        private readonly Partida _partida;

        public SubsistemaDefensa(Partida partida) => _partida = partida;

        public string ArmarDefensas() =>
            $"Defensa: {_partida.Grilla.Defensores().Count} defensores armados y en posición.";

        public string PriorizarSector(int carril) => $"Defensa: prioridad de fuego en {Estacion.Subsistemas[carril]}.";
        public string Apagar() => "Defensa: armas desactivadas.";
    }
}
