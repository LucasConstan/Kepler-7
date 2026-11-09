using Kepler7.Core.Patrones.ChainOfResponsibility;

namespace Kepler7.Core.Patrones.Visitor
{
    // Agrega una defensa a la cadena del héroe.
    public class ItemModuloDefensa : ItemInventario
    {
        private Defensa _defensa;

        public ItemModuloDefensa(string nombre, Defensa defensa) : base(nombre)
        {
            _defensa = defensa;
        }

        public Defensa ObtenerDefensa()
        {
            return _defensa;
        }

        public override void Aceptar(IVisitanteItem visitante)
        {
            visitante.Visitar(this);
        }
    }
}
