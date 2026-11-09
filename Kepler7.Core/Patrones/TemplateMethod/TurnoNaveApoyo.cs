using System.Collections.Generic;
using Kepler7.Core.Modelo;
using Kepler7.Core.Patrones.Builder;

namespace Kepler7.Core.Patrones.TemplateMethod
{
    // Dispara varias veces por tick. Cuántas depende de cómo la armó el Builder.
    public class TurnoNaveApoyo : TurnoUnidad
    {
        private const int Alcance = 5;

        protected override Accion Decidir(Unidad unidad, Contexto contexto)
        {
            List<Enemigo> enemigos = contexto.Grilla.EnemigosEnRango(unidad.Carril, unidad.Columna, Alcance);
            if (enemigos.Count > 0)
            {
                return Accion.Atacar(enemigos[0], "abre fuego contra el más cercano");
            }

            return Accion.Esperar("no hay enemigos a su alcance");
        }

        protected override string Actuar(Unidad unidad, Accion accion, Contexto contexto)
        {
            if (accion.Tipo != TipoAccion.Atacar)
            {
                return base.Actuar(unidad, accion, contexto);
            }

            if (!(unidad is NaveApoyo))
            {
                return base.Actuar(unidad, accion, contexto);
            }

            NaveApoyo nave = (NaveApoyo)unidad;
            List<string> resultados = new List<string>();
            Unidad? objetivo = accion.Objetivo;

            for (int i = 0; i < nave.DisparosPorTick; i++)
            {
                // Si el objetivo ya cayó, busca el siguiente enemigo a su alcance.
                if (objetivo == null || !objetivo.EstaViva)
                {
                    List<Enemigo> enemigos = contexto.Grilla.EnemigosEnRango(nave.Carril, nave.Columna, Alcance);
                    if (enemigos.Count > 0)
                    {
                        objetivo = enemigos[0];
                    }
                    else
                    {
                        objetivo = null;
                    }
                }

                if (objetivo == null)
                {
                    break;
                }

                // Cada disparo pega por separado. No se usa DanioTotal porque ese ya suma todos los disparos juntos.
                int real = contexto.AplicarDanio(objetivo, nave.PotenciaDeFuego + nave.BonusDanio);
                resultados.Add("dispara a " + objetivo.NombreCompleto + " (-" + real + " PV)");
            }

            if (resultados.Count == 0)
            {
                return "no pudo disparar";
            }

            string texto = resultados[0];
            for (int i = 1; i < resultados.Count; i++)
            {
                texto = texto + "; " + resultados[i];
            }
            return texto;
        }
    }
}
