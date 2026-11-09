using System.Collections.Generic;
using Kepler7.Core.Modelo;

namespace Kepler7.Core.Patrones.Facade
{
    // Se ocupa del oxígeno y de la atención médica de la tripulación.
    internal class SubsistemaSoporteVital
    {
        private readonly Partida _partida;

        public SubsistemaSoporteVital(Partida partida)
        {
            _partida = partida;
        }

        public string VerificarOxigeno()
        {
            return "Soporte Vital: oxígeno estable para " + _partida.Grilla.Defensores().Count + " defensores desplegados.";
        }

        public string CurarDefensores(int carril)
        {
            int total = 0;
            List<Unidad> defensores = _partida.Grilla.Defensores();
            for (int i = 0; i < defensores.Count; i++)
            {
                if (defensores[i].Carril == carril)
                {
                    // Curar no se pasa del máximo, así que pedirle la vida máxima es curarlo por completo.
                    total = total + defensores[i].Curar(defensores[i].VidaMaxima);
                }
            }

            return "Soporte Vital: equipos médicos curan a los defensores de " + Estacion.Subsistemas[carril] + " (+" + total + " PV).";
        }

        public string Apagar()
        {
            return "Soporte Vital: tripulación evacuada a las cápsulas.";
        }
    }
}
