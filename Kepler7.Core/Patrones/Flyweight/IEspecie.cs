using System.Drawing;

namespace Kepler7.Core.Patrones.Flyweight
{
    // Patrón Flyweight: esto es el estado intrínseco, los datos que son iguales
    // para todos los tripulantes de una misma especie y por eso se pueden compartir.
    public interface IEspecie
    {
        string Nombre { get; }
        Color Color { get; }
        string PlanetaOrigen { get; }
        string Dieta { get; }
    }
}
