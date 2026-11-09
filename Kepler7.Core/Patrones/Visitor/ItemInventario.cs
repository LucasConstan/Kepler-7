namespace Kepler7.Core.Patrones.Visitor
{
    // Un ítem solo guarda sus datos y sabe aceptar a un visitante.
    // Las operaciones (describir, tasar, equipar) viven en los visitantes.
    public abstract class ItemInventario
    {
        private string _nombre;

        protected ItemInventario(string nombre)
        {
            _nombre = nombre;
        }

        public string ObtenerNombre()
        {
            return _nombre;
        }

        public abstract void Aceptar(IVisitanteItem visitante);
    }
}
