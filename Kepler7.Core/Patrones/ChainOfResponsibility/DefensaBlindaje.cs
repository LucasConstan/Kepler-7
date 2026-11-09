using System;

namespace Kepler7.Core.Patrones.ChainOfResponsibility
{
    // Le saca un poco a cada golpe, sin límite de usos.
    public class DefensaBlindaje : Defensa
    {
        public override string ObtenerNombre()
        {
            return "Blindaje (-3)";
        }

        protected override void Aplicar(Golpe golpe)
        {
            // El Math.Min evita que un golpe de menos de 3 termine con daño negativo.
            int reducido = Math.Min(3, golpe.ObtenerDanioRestante());
            golpe.ReducirDanio(reducido);
            golpe.AgregarPaso("Blindaje: resta " + reducido + ", quedan " + golpe.ObtenerDanioRestante());
        }
    }
}
