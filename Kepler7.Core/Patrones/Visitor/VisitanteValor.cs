namespace Kepler7.Core.Patrones.Visitor
{
    // Suma cuánto se paga por los ítems que visita. Cada tipo se tasa distinto.
    // Va acumulando, así que sirve tanto para un ítem solo como para todo el inventario.
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
}
