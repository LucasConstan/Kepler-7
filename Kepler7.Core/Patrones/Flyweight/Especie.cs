using System.Drawing;

namespace Kepler7.Core.Patrones.Flyweight
{
    /// <summary>
    /// FLYWEIGHT concreto - Una especie. Solo la crea la FabricaEspecies,
    /// así que en todo el juego hay un único objeto por especie.
    /// </summary>
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
            InstanciasCreadas++;
        }
    }
}
