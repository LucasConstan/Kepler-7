using System.Drawing;

namespace Kepler_7.Modelo
{
    /// <summary>
    /// Cualquier cosa que ocupa una celda del tablero (tripulantes, naves de apoyo y, más adelante, enemigos).
    /// </summary>
    public abstract class Unidad
    {
        private static int _siguienteId = 1;

        public int Id { get; private set; }
        public string Nombre { get; protected set; } = "";
        public string Simbolo { get; protected set; } = "";
        public Color Color { get; protected set; }
        public int Costo { get; protected set; }
        public int Vida { get; protected internal set; }

        public abstract int Danio { get; }
        public abstract int VidaMaxima { get; }

        public string NombreCompleto => Nombre + " #" + Id;

        protected Unidad()
        {
            AsignarNuevoId();
        }

        protected void AsignarNuevoId()
        {
            Id = _siguienteId++;
        }

        public abstract string Descripcion();

        public override string ToString() => NombreCompleto;
    }
}
