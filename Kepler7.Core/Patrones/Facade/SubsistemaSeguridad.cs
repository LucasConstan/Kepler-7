using Kepler7.Core.Modelo;

namespace Kepler7.Core.Patrones.Facade
{
    // Las compuertas y los sectores de la estación.
    internal class SubsistemaSeguridad
    {
        public string SellarCompuertas()
        {
            return "Seguridad: compuertas exteriores selladas.";
        }

        public string AislarSector(int carril)
        {
            return "Seguridad: sector " + Estacion.Subsistemas[carril] + " aislado del resto.";
        }

        public string Apagar()
        {
            return "Seguridad: todas las compuertas bloqueadas.";
        }
    }
}
