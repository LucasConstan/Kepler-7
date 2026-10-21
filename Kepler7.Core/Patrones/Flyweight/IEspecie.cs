using System.Drawing;

namespace Kepler7.Core.Patrones.Flyweight
{
    /// <summary>
    /// FLYWEIGHT - Interfaz del flyweight. Es el estado INTRÍNSECO: los datos que son
    /// iguales para todos los tripulantes de una misma especie y por eso se comparten.
    /// </summary>
    public interface IEspecie
    {
        string Nombre { get; }
        Color Color { get; }
        string PlanetaOrigen { get; }
        string Dieta { get; }
    }
}
