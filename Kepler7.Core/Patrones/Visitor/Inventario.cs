using System.Collections.Generic;

namespace Kepler7.Core.Patrones.Visitor
{
    // Guarda los ítems y deja que un visitante los recorra a todos.
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
            // El foreach va a propósito: si un visitante cambia la lista mientras se recorre, salta un error
            // en vez de saltearse ítems sin avisar. Por eso VisitanteEquipar, que quita ítems, se usa de a uno.
            foreach (ItemInventario item in _items)
            {
                item.Aceptar(visitante);
            }
        }
    }
}
