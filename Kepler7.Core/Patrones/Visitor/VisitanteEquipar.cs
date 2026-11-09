using Kepler7.Core.Patrones.State;

namespace Kepler7.Core.Patrones.Visitor
{
    // Le pone el ítem al héroe. El arma y la armadura reemplazan a la anterior, que vuelve al inventario.
    public class VisitanteEquipar : IVisitanteItem
    {
        private Heroe _heroe;
        private Inventario _inventario;

        public VisitanteEquipar(Heroe heroe, Inventario inventario)
        {
            _heroe = heroe;
            _inventario = inventario;
        }

        public void Visitar(ItemArma item)
        {
            ItemArma? anterior = _heroe.EquiparArma(item);
            _inventario.Quitar(item);
            if (anterior != null)
            {
                _inventario.Agregar(anterior);
            }
        }

        public void Visitar(ItemArmadura item)
        {
            ItemArmadura? anterior = _heroe.EquiparArmadura(item);
            _inventario.Quitar(item);
            if (anterior != null)
            {
                _inventario.Agregar(anterior);
            }
        }

        // Un módulo no reemplaza nada: se suma al final de la cadena de defensas.
        public void Visitar(ItemModuloDefensa item)
        {
            _heroe.ObtenerCadena().Agregar(item.ObtenerDefensa());
            _inventario.Quitar(item);
        }
    }
}
