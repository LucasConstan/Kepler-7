using System.Drawing;

namespace Kepler7.Core.Patrones.Builder
{
    // El tanque: mucho blindaje y golpes fuertes, pero un solo disparo por tick.
    public class NaveCombateBuilder : NaveApoyoBuilderBase
    {
        public override string NombreReceta
        {
            get { return "Combate"; }
        }

        public override string DescripcionReceta
        {
            get { return "Tanque: mucho blindaje y daño fuerte, pero dispara 1 vez por tick."; }
        }

        public override int Costo
        {
            get { return 180; }
        }

        protected override string Simbolo
        {
            get { return "NC"; }
        }

        protected override Color Color
        {
            get { return Color.FromArgb(220, 70, 70); }
        }

        public override void ConstruirCasco()
        {
            Nave.Blindaje = 220;
            Nave.AgregarComponente("Casco reforzado de titanio (blindaje 220)");
        }

        public override void InstalarMotor()
        {
            Nave.DisparosPorTick = 1;
            Nave.AgregarComponente("Motor de fusión pesado (1 disparo por tick)");
        }

        public override void MontarArmamento()
        {
            Nave.PotenciaDeFuego = 22;
            Nave.AgregarComponente("Cañones de riel dobles (daño 22)");
        }

        public override void InstalarModuloEspecial()
        {
            // Se suma al blindaje del casco, por eso este paso tiene que ir después de ConstruirCasco.
            Nave.Blindaje = Nave.Blindaje + 80;
            Nave.AgregarComponente("Escudo deflector (+80 blindaje)");
        }
    }
}
