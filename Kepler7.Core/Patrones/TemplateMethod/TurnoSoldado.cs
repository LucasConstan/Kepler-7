using System.Collections.Generic;
using Kepler7.Core.Modelo;

namespace Kepler7.Core.Patrones.TemplateMethod
{
    // Dispara al enemigo más cercano. EnemigosEnRango los devuelve ordenados por distancia, por eso usa el primero.
    public class TurnoSoldado : TurnoUnidad
    {
        private const int Alcance = 4;

        protected override Accion Decidir(Unidad unidad, Contexto contexto)
        {
            List<Enemigo> enemigos = contexto.Grilla.EnemigosEnRango(unidad.Carril, unidad.Columna, Alcance);
            if (enemigos.Count > 0)
            {
                return Accion.Atacar(enemigos[0], "dispara al enemigo más cercano");
            }

            return Accion.Esperar("no hay enemigos a su alcance");
        }
    }
}
