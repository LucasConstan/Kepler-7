namespace Kepler7.Core.Patrones.Visitor
{
    /// <summary>VISITOR - Estructura de objetos. Guarda los ítems y deja que un visitante los recorra a todos.</summary>
    public class Inventario
    {
        private List<ItemInventario> _items = new List<ItemInventario>();

        public List<ItemInventario> ObtenerItems()
        {
            return _items;
        }

        public void Agregar(ItemInventario item)
        {
            _items.Add(item);
        }

        public void Quitar(ItemInventario item)
        {
            _items.Remove(item);
        }

        public void AceptarEnTodos(IVisitanteItem visitante)
        {
            foreach (ItemInventario item in _items)
            {
                item.Aceptar(visitante);
            }
        }
    }
}
