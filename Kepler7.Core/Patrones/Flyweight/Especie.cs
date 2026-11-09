using System.Drawing;

namespace Kepler7.Core.Patrones.Flyweight
{
    // El objeto que se comparte. Lo crea la FabricaEspecies, así que hay uno por especie para todos
    // los tripulantes. La única excepción son las copias que se hacen para medir la comparación sin Flyweight.
    public class Especie : IEspecie
    {
        // Cuenta cuántos objetos Especie se crearon en total (sirve para demostrar el patrón).
        public static int InstanciasCreadas { get; private set; }

        public string Nombre { get; }
        public Color Color { get; }
        public string PlanetaOrigen { get; }
        public string Dieta { get; }

        public Especie(string nombre, Color color, string planetaOrigen, string dieta)
        {
            Nombre = nombre;
            Color = color;
            PlanetaOrigen = planetaOrigen;
            Dieta = dieta;
            InstanciasCreadas = InstanciasCreadas + 1;
        }
    }
}
