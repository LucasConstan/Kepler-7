namespace Kepler7.Core.Patrones.Visitor
{
    // Arma el tipo y el efecto que se muestran en la tabla del inventario.
    // Guarda solo lo del último ítem visitado, por eso se usa uno nuevo para cada ítem.
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
}
