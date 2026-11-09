using System.Drawing;

namespace Kepler7.Core.Patrones.Builder
{
    // No se destaca peleando: su gracia son los créditos extra que da en cada oleada.
    public class NaveCargaBuilder : NaveApoyoBuilderBase
    {
        public override string NombreReceta
        {
            get { return "Carga"; }
        }

        public override string DescripcionReceta
        {
            get { return "Logística: defensa modesta, pero da +40 créditos en cada oleada."; }
        }

        public override int Costo
        {
            get { return 120; }
        }

        protected override string Simbolo
        {
            get { return "NK"; }
        }

        protected override Color Color
        {
            get { return Color.FromArgb(230, 150, 40); }
        }

        public override void ConstruirCasco()
        {
            Nave.Blindaje = 120;
            Nave.AgregarComponente("Casco de carga modular (blindaje 120)");
        }

        public override void InstalarMotor()
        {
            Nave.DisparosPorTick = 1;
            Nave.AgregarComponente("Motor de carga estándar (1 disparo por tick)");
        }

        public override void MontarArmamento()
        {
            Nave.PotenciaDeFuego = 6;
            Nave.AgregarComponente("Torreta defensiva (daño 6)");
        }

        public override void InstalarModuloEspecial()
        {
            Nave.BonusRecursos = 40;
            Nave.AgregarComponente("Módulo de extracción de minerales (+40 créditos por oleada)");
        }
    }
}
