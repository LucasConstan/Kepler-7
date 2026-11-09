namespace Kepler7.Core.Patrones.Visitor
{
    // Aumenta la vida máxima del héroe.
    public class ItemArmadura : ItemInventario
    {
        private int _bonusVida;

        public ItemArmadura(string nombre, int bonusVida) : base(nombre)
        {
            _bonusVida = bonusVida;
        }

        public int ObtenerBonusVida()
        {
            return _bonusVida;
        }

        public override void Aceptar(IVisitanteItem visitante)
        {
            visitante.Visitar(this);
        }
    }
}
