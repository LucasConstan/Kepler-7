using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kepler7.Core.Patrones.State
{
    // Pierde dos turnos. En el tercero ya juega, porque AlIniciarTurno corre antes de
    // preguntar si puede actuar, y ahí es donde sale de este estado.
    public class EstadoAturdido : IEstadoHeroe
    {
        private int _turnosRestantes = 2;

        public string ObtenerNombre()
        {
            return "Aturdido";
        }

        public bool PuedeActuar()
        {
            return false;
        }

        public int ModificarDanio(int danioBase)
        {
            return danioBase;
        }

        public void AlIniciarTurno(Heroe heroe)
        {
            if (_turnosRestantes > 0)
            {
                _turnosRestantes = _turnosRestantes - 1;
            }
            else if (heroe.TieneVidaBaja())
            {
                heroe.CambiarEstado(new EstadoEnfurecido());
            }
            else
            {
                heroe.CambiarEstado(new EstadoNormal());
            }
        }

        public void AlRecibirDanio(Heroe heroe, int danioRecibido)
        {
        }
    }
}
