using System.Drawing;

namespace Kepler7.Core.Patrones.Builder
{
    // Lo que tienen en común todas las recetas: crear la nave y entregarla terminada.
    public abstract class NaveApoyoBuilderBase : INaveApoyoBuilder
    {
        // Se crea en Reiniciar(); el Astillero siempre lo llama antes que cualquier otro paso.
        protected NaveApoyo Nave { get; private set; } = null!;

        public abstract string NombreReceta { get; }
        public abstract string DescripcionReceta { get; }
        public abstract int Costo { get; }
        protected abstract string Simbolo { get; }
        protected abstract Color Color { get; }

        public void Reiniciar()
        {
            Nave = new NaveApoyo();
            Nave.Tipo = NombreReceta;
            Nave.Identificar("Nave " + NombreReceta, Simbolo, Color, Costo);
        }

        public abstract void ConstruirCasco();
        public abstract void InstalarMotor();
        public abstract void MontarArmamento();
        public abstract void InstalarModuloEspecial();

        public NaveApoyo ObtenerResultado()
        {
            // La vida se llena recién acá porque el blindaje final se conoce al terminar todos los pasos.
            NaveApoyo terminada = Nave;
            terminada.Vida = terminada.VidaMaxima;

            // Se suelta la referencia para que la próxima nave no reutilice esta por error.
            Nave = null!;
            return terminada;
        }

        public override string ToString()
        {
            return NombreReceta;
        }
    }
}
