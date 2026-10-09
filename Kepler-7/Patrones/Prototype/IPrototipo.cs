namespace Kepler_7.Patrones.Prototype
{
    /// <summary>
    /// PROTOTYPE — Interfaz del prototipo. Tipada (a diferencia de System.ICloneable, que devuelve object).
    /// </summary>
    public interface IPrototipo<T>
    {
        T Clonar();
    }
}
