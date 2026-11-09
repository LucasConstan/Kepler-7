using System.Collections.Generic;
using Kepler7.Core.Modelo;

namespace Kepler7.Core.Patrones.Strategy
{
    // Va derecho al núcleo y solo pelea cuando alguien le tapa el camino.
    public class IAAgresiva : IEstrategiaIA
    {
        public string Nombre
        {
            get { return "Agresiva"; }
        }

        public string Sigla
        {
            get { return "AG"; }
        }

        public string Descripcion
        {
            get { return "Avanza siempre hacia el núcleo; ataca solo si algo le bloquea el camino."; }
        }

        public Accion DecidirAccion(Enemigo enemigo, Contexto contexto)
        {
            if (contexto.Grilla.PuedeAvanzar(enemigo))
            {
                return Accion.Avanzar("va directo al núcleo");
            }

            List<Unidad> objetivos = contexto.Grilla.DefensoresEnRango(enemigo.Carril, enemigo.Columna, enemigo.Alcance);
            if (objetivos.Count > 0)
            {
                return Accion.Atacar(objetivos[0], "tiene el paso bloqueado");
            }

            // Si llegó hasta acá, lo que tiene adelante no es un defensor: es otro enemigo.
            return Accion.Esperar("otro enemigo le tapa el paso");
        }
    }
}
