using Kepler7.Core.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kepler7.Core.Patrones.State;

namespace Kepler7.Core.Patrones.TemplateMethod
{
    public class TurnoHeroe : TurnoUnidad
    {
        protected override string Inicio(Unidad unidad, Contexto contexto)
        {
            Heroe heroe = (Heroe)unidad;
            heroe.ObtenerEstado().AlIniciarTurno(heroe);
            return "empieza su turno (estado " + heroe.ObtenerEstado().ObtenerNombre() + ")";
        }

        protected override Accion Decidir(Unidad unidad, Contexto contexto)
        {
            List<Enemigo> enemigos = contexto.Grilla.EnemigosEnRango(unidad.Carril, unidad.Columna, 4);
            if (enemigos.Count > 0)
            {
                return Accion.Atacar(enemigos[0], "dispara al enemigo más cercano");
            }
            return Accion.Esperar("no hay enemigos a su alcance");
        }
    }
}
