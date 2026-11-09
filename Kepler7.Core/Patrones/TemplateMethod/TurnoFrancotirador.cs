using System.Collections.Generic;
using Kepler7.Core.Modelo;

namespace Kepler7.Core.Patrones.TemplateMethod
{
    // No busca al más cercano sino al enemigo con más vida.
    public class TurnoFrancotirador : TurnoUnidad
    {
        protected override Accion Decidir(Unidad unidad, Contexto contexto)
        {
            // Usar el ancho de la grilla como alcance es la forma de ver todo el carril.
            List<Enemigo> enemigos = contexto.Grilla.EnemigosEnRango(unidad.Carril, unidad.Columna, Grilla.Columnas);
            if (enemigos.Count == 0)
            {
                return Accion.Esperar("no hay enemigos en el carril");
            }

            Enemigo masFuerte = enemigos[0];
            for (int i = 0; i < enemigos.Count; i++)
            {
                if (enemigos[i].Vida > masFuerte.Vida)
                {
                    masFuerte = enemigos[i];
                }
            }

            return Accion.Atacar(masFuerte, "apunta al enemigo con más vida");
        }
    }
}
