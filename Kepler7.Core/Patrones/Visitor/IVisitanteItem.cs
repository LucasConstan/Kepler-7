namespace Kepler7.Core.Patrones.Visitor
{
    /// <summary>
    /// VISITOR - Interfaz del visitante. Cada operación sobre los ítems es una clase que implementa
    /// esta interfaz, con un método por cada tipo de ítem.
    /// </summary>
    public interface IVisitanteItem
    {
        void Visitar(ItemArma item);
        void Visitar(ItemArmadura item);
        void Visitar(ItemModuloDefensa item);
    }
}
