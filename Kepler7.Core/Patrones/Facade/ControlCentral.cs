using System.Collections.Generic;
using Kepler7.Core.Modelo;

namespace Kepler7.Core.Patrones.Facade
{
    // Patrón Facade: el operador solo ve tres operaciones simples y acá se coordina
    // a los seis subsistemas en el orden correcto. Cada método devuelve la lista de pasos
    // para que la interfaz los muestre, porque el Core no escribe nada en pantalla.
    public class ControlCentral
    {
        private readonly Partida _partida;
        private readonly SubsistemaEnergia _energia;
        private readonly SubsistemaSoporteVital _soporteVital;
        private readonly SubsistemaNavegacion _navegacion = new SubsistemaNavegacion();
        private readonly SubsistemaComunicaciones _comunicaciones;
        private readonly SubsistemaSeguridad _seguridad = new SubsistemaSeguridad();
        private readonly SubsistemaDefensa _defensa;

        // La emergencia se puede usar una vez por oleada.
        public bool EmergenciaDisponible { get; private set; }

        public ControlCentral(Partida partida)
        {
            _partida = partida;
            _energia = new SubsistemaEnergia(partida);
            _soporteVital = new SubsistemaSoporteVital(partida);
            _comunicaciones = new SubsistemaComunicaciones(partida);
            _defensa = new SubsistemaDefensa(partida);
        }

        // Prepara todos los subsistemas y arranca la oleada.
        public List<string> IniciarTurno()
        {
            List<string> pasos = new List<string>();
            if (_partida.Fase != FasePartida.Preparacion)
            {
                pasos.Add("Control central: no se puede iniciar un turno ahora.");
                return pasos;
            }

            pasos.Add("Control central: iniciando turno...");
            pasos.Add(_energia.EncenderReactor());
            pasos.Add(_soporteVital.VerificarOxigeno());
            pasos.Add(_navegacion.EstabilizarOrbita());
            pasos.Add(_comunicaciones.AnunciarOleada());
            pasos.Add(_seguridad.SellarCompuertas());
            pasos.Add(_defensa.ArmarDefensas());

            _partida.IniciarOleada();
            EmergenciaDisponible = true;
            pasos.Add("Control central: oleada " + _partida.NumeroOleada + " en curso.");
            return pasos;
        }

        // Busca el subsistema más dañado y coordina a todos para salvarlo.
        public List<string> EntrarEnEmergencia()
        {
            List<string> pasos = new List<string>();
            if (_partida.Fase != FasePartida.Oleada)
            {
                pasos.Add("Control central: el modo emergencia solo se usa durante una oleada.");
                return pasos;
            }

            if (!EmergenciaDisponible)
            {
                pasos.Add("Control central: el modo emergencia ya se usó en esta oleada.");
                return pasos;
            }

            int carril = CarrilMasDaniado();
            if (carril == -1)
            {
                // No la marca como usada: si todavía no hay daño, se puede pedir más tarde.
                pasos.Add("Control central: ningún subsistema está dañado, no hace falta la emergencia.");
                return pasos;
            }

            EmergenciaDisponible = false;
            pasos.Add("Control central: ¡EMERGENCIA en " + Estacion.Subsistemas[carril] + "!");
            pasos.Add(_comunicaciones.EnviarSOS());
            pasos.Add(_seguridad.AislarSector(carril));
            pasos.Add(_energia.DesviarEnergiaA(carril));
            pasos.Add(_soporteVital.CurarDefensores(carril));
            pasos.Add(_navegacion.ManiobraEvasiva());
            pasos.Add(_defensa.PriorizarSector(carril));
            return pasos;
        }

        // Apaga los subsistemas en orden inverso al encendido y termina la partida.
        public List<string> ApagarEstacion()
        {
            List<string> pasos = new List<string>();
            pasos.Add("Control central: apagando la estación...");
            pasos.Add(_defensa.Apagar());
            pasos.Add(_seguridad.Apagar());
            pasos.Add(_comunicaciones.Apagar());
            pasos.Add(_navegacion.Apagar());
            pasos.Add(_soporteVital.Apagar());
            pasos.Add(_energia.Apagar());

            _partida.Apagar();
            EmergenciaDisponible = false;
            return pasos;
        }

        // El carril con menos integridad que todavía no cayó (-1 si todos están sanos).
        private int CarrilMasDaniado()
        {
            int peor = -1;
            for (int carril = 0; carril < Grilla.Carriles; carril++)
            {
                int integridad = _partida.IntegridadDelCarril(carril);
                if (integridad != 0 && integridad != Partida.IntegridadMaxima)
                {
                    if (peor == -1 || integridad < _partida.IntegridadDelCarril(peor))
                    {
                        peor = carril;
                    }
                }
            }

            return peor;
        }
    }
}
