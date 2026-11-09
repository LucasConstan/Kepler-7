using System.Collections.Generic;
using Kepler7.Core.Modelo;

namespace Kepler7.Core.Patrones.Strategy
{
    // Prefiere pelear antes que avanzar, y cuando avanza lo hace de a poco.
    public class IADefensiva : IEstrategiaIA
    {
        public string Nombre
        {
            get { return "Defensiva"; }
        }

        public string Sigla
        {
            get { return "DF"; }
        }

        public string Descripcion
        {
            get { return "Dispara apenas tiene un defensor al alcance; si no, avanza un paso cada dos ticks."; }
        }

        public Accion DecidirAccion(Enemigo enemigo, Contexto contexto)
        {
            List<Unidad> objetivos = contexto.Grilla.DefensoresEnRango(enemigo.Carril, enemigo.Columna, enemigo.Alcance);
            if (objetivos.Count > 0)
            {
                return Accion.Atacar(objetivos[0], "se detiene a pelear");
            }

            // Solo se mueve en los ticks pares, por eso tarda más en llegar que las otras IA.
            if (contexto.Tick % 2 == 0 && contexto.Grilla.PuedeAvanzar(enemigo))
            {
                return Accion.Avanzar("avanza con cautela");
            }

            return Accion.Esperar("espera antes de avanzar");
        }
    }
}
