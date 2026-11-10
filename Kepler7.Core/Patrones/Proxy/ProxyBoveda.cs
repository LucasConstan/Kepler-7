using System;
using System.Collections.Generic;
using Kepler7.Core.Modelo;

namespace Kepler7.Core.Patrones.Proxy
{
    // Se pone delante de la Bóveda real y hace dos cosas antes de dejar pasar el pedido:
    // 1) Protección: solo deja retirar monedas desde cierto nivel.
    // 2) Registro: anota en el log de la Bóveda cada operación, se haga o no.
    public class ProxyBoveda : IBoveda
    {
        public const int NivelParaRetirar = 2;

        private readonly Boveda _bovedaReal;
        private readonly Progreso _progreso;
        private readonly List<string> _log;

        public ProxyBoveda(Boveda bovedaReal, Progreso progreso, List<string> log)
        {
            _bovedaReal = bovedaReal;
            _progreso = progreso;
            _log = log;
        }

        // Consultar no tiene restricciones ni se anota: solo se reenvía.
        public int ConsultarSaldo()
        {
            return _bovedaReal.ConsultarSaldo();
        }

        public bool Depositar(int cantidad)
        {
            bool ok = _bovedaReal.Depositar(cantidad);
            if (ok)
            {
                Anotar("Depósito de " + cantidad + " monedas · saldo " + _bovedaReal.ConsultarSaldo());
            }
            else
            {
                Anotar("Depósito rechazado: cantidad inválida (" + cantidad + ")");
            }

            return ok;
        }

        public bool Retirar(int cantidad)
        {
            // Control de acceso: si no tiene el nivel, el pedido ni siquiera llega a la Bóveda real.
            if (_progreso.Nivel < NivelParaRetirar)
            {
                Anotar("ACCESO DENEGADO: retiro de " + cantidad + " monedas con nivel " + _progreso.Nivel
                    + " (se necesita nivel " + NivelParaRetirar + ")");
                return false;
            }

            bool ok = _bovedaReal.Retirar(cantidad);
            if (ok)
            {
                Anotar("Retiro de " + cantidad + " monedas · saldo " + _bovedaReal.ConsultarSaldo());
            }
            else
            {
                Anotar("Retiro rechazado: saldo insuficiente o cantidad inválida (" + cantidad + ")");
            }

            return ok;
        }

        private void Anotar(string texto)
        {
            _log.Add("[" + DateTime.Now.ToString("dd/MM HH:mm:ss") + "] " + texto);
        }
    }
}
