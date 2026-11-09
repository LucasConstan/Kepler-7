namespace Kepler7.Core.Patrones.Prototype
{
    // Patrón Prototype: un objeto que sabe hacer una copia de sí mismo.
    // Se usa esta interfaz en vez de ICloneable porque esa devuelve object y obliga a castear.
    public interface IPrototipo<T>
    {
        T Clonar();
    }
}
