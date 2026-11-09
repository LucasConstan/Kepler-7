using System.Collections.Generic;
using Kepler7.Core.Modelo;

namespace Kepler7.Core.Patrones.Strategy
{
    // Junta todo lo que puede hacer en este momento y después elige una opción al azar.
    public class IAAleatoria : IEstrategiaIA
    {
        public string Nombre
        {
            get { return "Aleatoria"; }
        }

        public string Sigla
        {
            get { return "AL"; }
        }

        public string Descripcion
        {
            get { return "Elige al azar entre atacar, avanzar, cambiar de carril o esperar."; }
        }

        public Accion DecidirAccion(Enemigo enemigo, Contexto contexto)
        {
            List<Accion> opciones = new List<Accion>();

            List<Unidad> objetivos = contexto.Grilla.DefensoresEnRango(enemigo.Carril, enemigo.Columna, enemigo.Alcance);
            if (objetivos.Count > 0)
            {
                opciones.Add(Accion.Atacar(objetivos[0], "le tocó atacar"));
            }

            if (contexto.Grilla.PuedeAvanzar(enemigo))
            {
                opciones.Add(Accion.Avanzar("le tocó avanzar"));
            }

            // EstaLibre da false si el carril está fuera del tablero, así que los bordes no son problema.
            int carrilDeArriba = enemigo.Carril - 1;
            if (contexto.Grilla.EstaLibre(carrilDeArriba, enemigo.Columna))
            {
                opciones.Add(Accion.CambiarCarril(carrilDeArriba, "le tocó cambiar de carril"));
            }

            int carrilDeAbajo = enemigo.Carril + 1;
            if (contexto.Grilla.EstaLibre(carrilDeAbajo, enemigo.Columna))
            {
                opciones.Add(Accion.CambiarCarril(carrilDeAbajo, "le tocó cambiar de carril"));
            }

            // Esperar siempre se puede, así que la lista nunca queda vacía.
            opciones.Add(Accion.Esperar("le tocó esperar"));

            int posicionElegida = contexto.Azar.Next(opciones.Count);
            return opciones[posicionElegida];
        }
    }
}
