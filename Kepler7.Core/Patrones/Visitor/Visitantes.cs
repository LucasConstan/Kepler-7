using Kepler7.Core.Patrones.State;

namespace Kepler7.Core.Patrones.Visitor
{
    /// <summary>VISITOR concreto - Descripción: arma el tipo y el efecto que se muestran en la tabla.</summary>
    public class VisitanteDescripcion : IVisitanteItem
    {
        private string _tipo = "";
        private string _efecto = "";

        public string ObtenerTipo()
        {
            return _tipo;
        }

        public string ObtenerEfecto()
        {
            return _efecto;
        }

        public void Visitar(ItemArma item)
        {
            _tipo = "Arma";
            _efecto = "+" + item.ObtenerBonusDanio() + " de daño";
        }

        public void Visitar(ItemArmadura item)
        {
            _tipo = "Armadura";
            _efecto = "+" + item.ObtenerBonusVida() + " de vida";
        }

        public void Visitar(ItemModuloDefensa item)
        {
            _tipo = "Módulo";
            _efecto = "Agrega " + item.ObtenerDefensa().ObtenerNombre() + " a la cadena";
        }
    }

    /// <summary>VISITOR concreto - Valor: suma cuánto se paga por los ítems que visita. Cada tipo se tasa distinto.</summary>
    public class VisitanteValor : IVisitanteItem
    {
        private int _total = 0;

        public int ObtenerTotal()
        {
            return _total;
        }

        public void Visitar(ItemArma item)
        {
            _total = _total + item.ObtenerBonusDanio() * 6;
        }

        public void Visitar(ItemArmadura item)
        {
            _total = _total + item.ObtenerBonusVida();
        }

        public void Visitar(ItemModuloDefensa item)
        {
            _total = _total + 60;
        }
    }

    /// <summary>VISITOR concreto - Equipar: se lo pone al héroe. Cada tipo de ítem se equipa distinto.</summary>
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

        public void Visitar(ItemModuloDefensa item)
        {
            _heroe.ObtenerCadena().Agregar(item.ObtenerDefensa());
            _inventario.Quitar(item);
        }
    }
}
