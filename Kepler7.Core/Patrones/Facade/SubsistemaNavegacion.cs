namespace Kepler7.Core.Patrones.Facade
{
    // Mantiene la órbita de la estación.
    internal class SubsistemaNavegacion
    {
        public string EstabilizarOrbita()
        {
            return "Navegación: órbita estabilizada.";
        }

        public string ManiobraEvasiva()
        {
            return "Navegación: maniobra evasiva para proteger el sector dañado.";
        }

        public string Apagar()
        {
            return "Navegación: propulsores en reposo.";
        }
    }
}
