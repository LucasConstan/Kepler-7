using System.Collections.Generic;
using Kepler7.Core.Modelo;

namespace Kepler7.Core.Patrones.Strategy
{
    // La más pensada de todas: compara la defensa de los carriles antes de moverse y elige bien a quién atacar.
    public class IACalculadora : IEstrategiaIA
    {
        public string Nombre
        {
            get { return "Calculadora"; }
        }

        public string Sigla
        {
            get { return "CA"; }
        }

        public string Descripcion
        {
            get { return "Se cambia al carril vecino con menos defensa y ataca al defensor más débil."; }
        }

        public Accion DecidirAccion(Enemigo enemigo, Contexto contexto)
        {
            // 1) ¿Algún carril vecino tiene menos de la mitad de defensa que el mío? Me voy para ahí.
            int poderActual = contexto.Grilla.PoderDefensivo(enemigo.Carril);
            int mejorCarril = enemigo.Carril;
            int mejorPoder = poderActual;

            int carrilDeArriba = enemigo.Carril - 1;
            if (contexto.Grilla.EstaLibre(carrilDeArriba, enemigo.Columna))
            {
                int poderDeArriba = contexto.Grilla.PoderDefensivo(carrilDeArriba);
                if (poderDeArriba * 2 < poderActual && poderDeArriba < mejorPoder)
                {
                    mejorCarril = carrilDeArriba;
                    mejorPoder = poderDeArriba;
                }
            }

            int carrilDeAbajo = enemigo.Carril + 1;
            if (contexto.Grilla.EstaLibre(carrilDeAbajo, enemigo.Columna))
            {
                int poderDeAbajo = contexto.Grilla.PoderDefensivo(carrilDeAbajo);
                if (poderDeAbajo * 2 < poderActual && poderDeAbajo < mejorPoder)
                {
                    mejorCarril = carrilDeAbajo;
                    mejorPoder = poderDeAbajo;
                }
            }

            if (mejorCarril != enemigo.Carril)
            {
                return Accion.CambiarCarril(mejorCarril, Estacion.Subsistemas[mejorCarril] + " está menos defendido");
            }

            // 2) Si tiene defensores a su alcance, ataca al que tenga menos vida.
            List<Unidad> objetivos = contexto.Grilla.DefensoresEnRango(enemigo.Carril, enemigo.Columna, enemigo.Alcance);
            if (objetivos.Count > 0)
            {
                // Los objetivos vienen del más cercano al más lejano, así que si hay empate se queda con el más cercano.
                Unidad masDebil = objetivos[0];
                for (int i = 0; i < objetivos.Count; i++)
                {
                    if (objetivos[i].Vida < masDebil.Vida)
                    {
                        masDebil = objetivos[i];
                    }
                }
                return Accion.Atacar(masDebil, "ataca al defensor más débil");
            }

            // 3) Si no, sigue avanzando.
            if (contexto.Grilla.PuedeAvanzar(enemigo))
            {
                return Accion.Avanzar("sigue avanzando");
            }

            return Accion.Esperar("otro enemigo le tapa el paso");
        }
    }
}
