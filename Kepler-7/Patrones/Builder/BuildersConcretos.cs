using System.Drawing;

namespace Kepler_7.Patrones.Builder
{
    /// <summary>BUILDER concreto — Exploradora: rápida, poco blindaje y poco daño por disparo.</summary>
    public class NaveExploradoraBuilder : NaveApoyoBuilderBase
    {
        public override string NombreReceta => "Exploradora";
        public override string DescripcionReceta => "Rápida: dispara 3 veces por tick, pero tiene poco blindaje y daño.";
        public override int Costo => 100;
        protected override string Simbolo => "NE";
        protected override Color Color => Color.FromArgb(40, 200, 220);

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

    /// <summary>BUILDER concreto — Combate: tanque, mucho blindaje y daño fuerte pero lento.</summary>
    public class NaveCombateBuilder : NaveApoyoBuilderBase
    {
        public override string NombreReceta => "Combate";
        public override string DescripcionReceta => "Tanque: mucho blindaje y daño fuerte, pero dispara 1 vez por tick.";
        public override int Costo => 180;
        protected override string Simbolo => "NC";
        protected override Color Color => Color.FromArgb(220, 70, 70);

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
            Nave.Blindaje += 80;
            Nave.AgregarComponente("Escudo deflector (+80 blindaje)");
        }
    }

    /// <summary>BUILDER concreto — Carga: defensa modesta, pero genera recursos extra en cada oleada.</summary>
    public class NaveCargaBuilder : NaveApoyoBuilderBase
    {
        public override string NombreReceta => "Carga";
        public override string DescripcionReceta => "Logística: defensa modesta, pero da +40 créditos en cada oleada.";
        public override int Costo => 120;
        protected override string Simbolo => "NK";
        protected override Color Color => Color.FromArgb(230, 150, 40);

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
