using System;

namespace Kepler7.Core.Patrones.ChainOfResponsibility
{
    // A veces esquiva el golpe entero. Cuando lo logra, el daño queda en cero y la cadena termina acá.
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
}
