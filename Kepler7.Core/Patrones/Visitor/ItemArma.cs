namespace Kepler7.Core.Patrones.Visitor
{
    // Aumenta el daño del héroe.
    public class ItemArma : ItemInventario
    {
        private int _bonusDanio;

        public ItemArma(string nombre, int bonusDanio) : base(nombre)
        {
            _bonusDanio = bonusDanio;
        }

        public int ObtenerBonusDanio()
        {
            return _bonusDanio;
        }

        // Este método parece igual en todos los ítems, pero acá "this" es un ItemArma,
        // y eso decide cuál de los Visitar se ejecuta.
        public override void Aceptar(IVisitanteItem visitante)
        {
            visitante.Visitar(this);
        }
    }
}
