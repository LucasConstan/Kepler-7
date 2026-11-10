using Kepler7.Core.Modelo;

namespace Kepler7.Core.Patrones.Proxy
{
    // La Bóveda real: guarda las monedas del jugador. Solo controla que las cuentas den bien;
    // no pregunta quién la usa ni anota nada (eso lo hace el ProxyBoveda).
    // Las monedas viven en el Progreso, así se guardan en el JSON junto con la XP y los logros.
    public class Boveda : IBoveda
    {
        private readonly Progreso _progreso;

        public Boveda(Progreso progreso)
        {
            _progreso = progreso;
        }

        public int ConsultarSaldo()
        {
            return _progreso.Monedas;
        }

        public bool Depositar(int cantidad)
        {
            if (cantidad <= 0)
            {
                return false;
            }

            _progreso.Monedas = _progreso.Monedas + cantidad;
            return true;
        }

        public bool Retirar(int cantidad)
        {
            if (cantidad <= 0 || cantidad > _progreso.Monedas)
            {
                return false;
            }

            _progreso.Monedas = _progreso.Monedas - cantidad;
            return true;
        }
    }
}
