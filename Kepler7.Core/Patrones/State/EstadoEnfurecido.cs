using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kepler7.Core.Patrones.State
{
    // Con poca vida pega más fuerte. Se le pasa cuando recupera vida.
    public class EstadoEnfurecido : IEstadoHeroe
    {
        public string ObtenerNombre()
        {
            return "Enfurecido";
        }

        public bool PuedeActuar()
        {
            return true;
        }

        public int ModificarDanio(int danioBase)
        {
            return danioBase + 7;
        }

        public void AlIniciarTurno(Heroe heroe)
        {
            if (!heroe.TieneVidaBaja())
            {
                heroe.CambiarEstado(new EstadoNormal());
            }
        }

        public void AlRecibirDanio(Heroe heroe, int danioRecibido)
        {
        }
    }
}
