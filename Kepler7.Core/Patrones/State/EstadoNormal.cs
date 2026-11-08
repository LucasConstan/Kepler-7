using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kepler7.Core.Patrones.State
{
    public class EstadoNormal : IEstadoHeroe
    {
        public string ObtenerNombre()
        {
            return "Normal";
        }

        public bool PuedeActuar()
        {
            return true;
        }

        public int ModificarDanio(int danioBase)
        {
            return danioBase;
        }

        public void AlIniciarTurno(Heroe heroe)
        {
        }

        public void AlRecibirDanio(Heroe heroe, int danioRecibido)
        {
            if (heroe.TieneVidaBaja())
            {
                heroe.CambiarEstado(new EstadoEnfurecido());
            }
            else if (danioRecibido >= 10)
            {
                heroe.CambiarEstado(new EstadoAturdido());
            }
        }
    }
}
