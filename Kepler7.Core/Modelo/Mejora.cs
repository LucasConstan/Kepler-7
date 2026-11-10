namespace Kepler7.Core.Modelo
{
    // Una mejora de la tienda del Hub. Se paga con monedas y sirve para una sola partida:
    // se usa al empezar la próxima y después se descarta.
    public class Mejora
    {
        public string Nombre { get; }
        public string Descripcion { get; }
        public int Costo { get; }

        public Mejora(string nombre, string descripcion, int costo)
        {
            Nombre = nombre;
            Descripcion = descripcion;
            Costo = costo;
        }

        // Así aparece en el combo de la tienda.
        public override string ToString()
        {
            return Nombre + " (" + Costo + " monedas)";
        }
    }
}
