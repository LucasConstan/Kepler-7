using System.Drawing;

namespace Kepler7.Core.Patrones.Builder
{
    // La nave rápida: dispara muchas veces, pero aguanta poco y cada disparo pega suave.
    public class NaveExploradoraBuilder : NaveApoyoBuilderBase
    {
        public override string NombreReceta
        {
            get { return "Exploradora"; }
        }

        public override string DescripcionReceta
        {
            get { return "Rápida: dispara 3 veces por tick, pero tiene poco blindaje y daño."; }
        }

        public override int Costo
        {
            get { return 100; }
        }

        protected override string Simbolo
        {
            get { return "NE"; }
        }

        protected override Color Color
        {
            get { return Color.FromArgb(40, 200, 220); }
        }

        public override void ConstruirCasco()
        {
            Nave.Blindaje = 60;
            Nave.AgregarComponente("Casco liviano de aleación (blindaje 60)");
        }

        public override void InstalarMotor()
        {
            Nave.DisparosPorTick = 3;
            Nave.AgregarComponente("Motor iónico de alta respuesta (3 disparos por tick)");
        }

        public override void MontarArmamento()
        {
            Nave.PotenciaDeFuego = 8;
            Nave.AgregarComponente("Cañón láser ligero (daño 8)");
        }

        public override void InstalarModuloEspecial()
        {
            Nave.AgregarComponente("Módulo de sensores de largo alcance");
        }
    }
}
