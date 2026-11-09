using System;

namespace Kepler7.Core.Patrones.ChainOfResponsibility
{
    // Absorbe daño hasta agotar su carga. Es la única defensa que se gasta,
    // por eso es la única que hace algo en Recargar.
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
}
