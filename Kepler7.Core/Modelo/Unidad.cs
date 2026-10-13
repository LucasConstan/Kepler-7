using System.Drawing;
using Kepler7.Core.Patrones.TemplateMethod;

namespace Kepler7.Core.Modelo
{
    /// <summary>
    /// Cualquier cosa que ocupa una celda del tablero: tripulantes, naves de apoyo y enemigos.
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

        // Posición en la grilla (-1 si todavía no está colocada). La asigna la Grilla.
        public int Carril { get; internal set; } = -1;
        public int Columna { get; internal set; } = -1;

        // Daño extra que dura un solo tick (lo reparte el Comandante).
        public int BonusDanio { get; internal set; }

        public abstract int Danio { get; }
        public abstract int VidaMaxima { get; }

        // TEMPLATE METHOD: cada tipo de unidad trae su propio turno.
        public abstract TurnoUnidad Turno { get; }

        public virtual bool EsEnemigo => false;
        public bool EstaViva => Vida > 0;
        public int DanioTotal => Danio + BonusDanio;
        public string NombreCompleto => Nombre + " #" + Id;

        // Enganche para el patrón State: un personaje aturdido no puede actuar.
        public virtual bool PuedeActuar => EstaViva;

        // Enganche para Chain of Responsibility: acá el daño pasaría por la cadena de defensas.
        // Devuelve el daño que realmente se recibió.
        public virtual int RecibirDanio(int cantidad)
        {
            int real = Math.Min(Math.Max(cantidad, 0), Vida);
            Vida -= real;
            return real;
        }

        public int Curar(int cantidad)
        {
            int real = Math.Min(Math.Max(cantidad, 0), VidaMaxima - Vida);
            Vida += real;
            return real;
        }

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
