using System.Drawing;

namespace Kepler_7.Patrones.Builder
{
    /// <summary>
    /// Base común de los builders concretos: maneja la nave en construcción
    /// y la entrega terminada (con la vida al máximo).
    /// </summary>
    public abstract class NaveApoyoBuilderBase : INaveApoyoBuilder
    {
        // Se crea en Reiniciar(); el director siempre lo llama antes que cualquier otro paso.
        protected NaveApoyo Nave { get; private set; } = null!;

        public abstract string NombreReceta { get; }
        public abstract string DescripcionReceta { get; }
        public abstract int Costo { get; }
        protected abstract string Simbolo { get; }
        protected abstract Color Color { get; }

        public void Reiniciar()
        {
            Nave = new NaveApoyo { Tipo = NombreReceta };
            Nave.Identificar("Nave " + NombreReceta, Simbolo, Color, Costo);
        }

        public abstract void ConstruirCasco();
        public abstract void InstalarMotor();
        public abstract void MontarArmamento();
        public abstract void InstalarModuloEspecial();

        public NaveApoyo ObtenerResultado()
        {
            var terminada = Nave;
            terminada.Vida = terminada.VidaMaxima;
            Nave = null!;
            return terminada;
        }

        public override string ToString() => NombreReceta;
    }
}
