namespace Kepler7.Core.Patrones.ChainOfResponsibility
{
    /// <summary>CHAIN OF RESPONSIBILITY concreto - Evasión: a veces esquiva el golpe entero.</summary>
    public class DefensaEvasion : Defensa
    {
        private Random _azar;

        public DefensaEvasion(Random azar)
        {
            _azar = azar;
        }

        public override string ObtenerNombre()
        {
            return "Evasión (20%)";
        }

        protected override void Aplicar(Golpe golpe)
        {
            if (_azar.Next(100) < 20)
            {
                golpe.ReducirDanio(golpe.ObtenerDanioRestante());
                golpe.AgregarPaso("Evasión: esquiva el golpe");
            }
            else
            {
                golpe.AgregarPaso("Evasión: falla");
            }
        }
    }

    /// <summary>CHAIN OF RESPONSIBILITY concreto - Escudo: absorbe daño hasta agotar su carga.</summary>
    public class DefensaEscudo : Defensa
    {
        private const int CargaMaxima = 20;
        private int _carga = CargaMaxima;

        public override string ObtenerNombre()
        {
            return "Escudo (" + _carga + "/" + CargaMaxima + ")";
        }

        public override void Recargar()
        {
            _carga = CargaMaxima;
        }

        protected override void Aplicar(Golpe golpe)
        {
            int absorbido = Math.Min(_carga, golpe.ObtenerDanioRestante());
            _carga = _carga - absorbido;
            golpe.ReducirDanio(absorbido);
            golpe.AgregarPaso("Escudo: absorbe " + absorbido + ", queda en " + _carga);
        }
    }

    /// <summary>CHAIN OF RESPONSIBILITY concreto - Blindaje: le resta un poco a cada golpe.</summary>
    public class DefensaBlindaje : Defensa
    {
        public override string ObtenerNombre()
        {
            return "Blindaje (-3)";
        }

        protected override void Aplicar(Golpe golpe)
        {
            int reducido = Math.Min(3, golpe.ObtenerDanioRestante());
            golpe.ReducirDanio(reducido);
            golpe.AgregarPaso("Blindaje: resta " + reducido + ", quedan " + golpe.ObtenerDanioRestante());
        }
    }
}
