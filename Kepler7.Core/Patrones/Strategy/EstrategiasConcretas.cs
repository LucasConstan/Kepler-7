using System.Collections.Generic;
using Kepler7.Core.Modelo;

namespace Kepler7.Core.Patrones.Strategy
{
    /// <summary>STRATEGY concreta — Agresiva: va directo al núcleo y solo se frena si le tapan el paso.</summary>
    public class IAAgresiva : IEstrategiaIA
    {
        public string Nombre => "Agresiva";
        public string Sigla => "AG";
        public string Descripcion => "Avanza siempre hacia el núcleo; ataca solo si algo le bloquea el camino.";

        public Accion DecidirAccion(Enemigo enemigo, Contexto contexto)
        {
            if (contexto.Grilla.PuedeAvanzar(enemigo))
                return Accion.Avanzar("va directo al núcleo");

            var objetivos = contexto.Grilla.DefensoresEnRango(enemigo.Carril, enemigo.Columna, enemigo.Alcance);
            if (objetivos.Count > 0)
                return Accion.Atacar(objetivos[0], "tiene el paso bloqueado");

            return Accion.Esperar("otro enemigo le tapa el paso");
        }
    }

    /// <summary>STRATEGY concreta — Defensiva: se detiene a pelear apenas tiene un objetivo y avanza con cautela.</summary>
    public class IADefensiva : IEstrategiaIA
    {
        public string Nombre => "Defensiva";
        public string Sigla => "DF";
        public string Descripcion => "Dispara apenas tiene un defensor al alcance; si no, avanza un paso cada dos ticks.";

        public Accion DecidirAccion(Enemigo enemigo, Contexto contexto)
        {
            var objetivos = contexto.Grilla.DefensoresEnRango(enemigo.Carril, enemigo.Columna, enemigo.Alcance);
            if (objetivos.Count > 0)
                return Accion.Atacar(objetivos[0], "se detiene a pelear");

            if (contexto.Tick % 2 == 0 && contexto.Grilla.PuedeAvanzar(enemigo))
                return Accion.Avanzar("avanza con cautela");

            return Accion.Esperar("espera antes de avanzar");
        }
    }

    /// <summary>STRATEGY concreta — Aleatoria: elige al azar entre lo que puede hacer.</summary>
    public class IAAleatoria : IEstrategiaIA
    {
        public string Nombre => "Aleatoria";
        public string Sigla => "AL";
        public string Descripcion => "Elige al azar entre atacar, avanzar, cambiar de carril o esperar.";

        public Accion DecidirAccion(Enemigo enemigo, Contexto contexto)
        {
            var opciones = new List<Accion>();

            var objetivos = contexto.Grilla.DefensoresEnRango(enemigo.Carril, enemigo.Columna, enemigo.Alcance);
            if (objetivos.Count > 0) opciones.Add(Accion.Atacar(objetivos[0], "le tocó atacar"));

            if (contexto.Grilla.PuedeAvanzar(enemigo)) opciones.Add(Accion.Avanzar("le tocó avanzar"));

            foreach (int vecino in new[] { enemigo.Carril - 1, enemigo.Carril + 1 })
            {
                if (contexto.Grilla.EstaLibre(vecino, enemigo.Columna))
                    opciones.Add(Accion.CambiarCarril(vecino, "le tocó cambiar de carril"));
            }

            opciones.Add(Accion.Esperar("le tocó esperar"));
            return opciones[contexto.Azar.Next(opciones.Count)];
        }
    }

    /// <summary>STRATEGY concreta — Calculadora: busca el carril menos defendido y ataca al defensor más débil.</summary>
    public class IACalculadora : IEstrategiaIA
    {
        public string Nombre => "Calculadora";
        public string Sigla => "CA";
        public string Descripcion => "Se cambia al carril vecino con menos defensa y ataca al defensor más débil.";

        public Accion DecidirAccion(Enemigo enemigo, Contexto contexto)
        {
            // 1) ¿Algún carril vecino tiene menos de la mitad de defensa que el mío? Me voy para ahí.
            int poderActual = contexto.Grilla.PoderDefensivo(enemigo.Carril);
            int mejorCarril = enemigo.Carril;
            int mejorPoder = poderActual;
            foreach (int vecino in new[] { enemigo.Carril - 1, enemigo.Carril + 1 })
            {
                if (!contexto.Grilla.EstaLibre(vecino, enemigo.Columna)) continue;

                int poder = contexto.Grilla.PoderDefensivo(vecino);
                if (poder * 2 < poderActual && poder < mejorPoder)
                {
                    mejorCarril = vecino;
                    mejorPoder = poder;
                }
            }
            if (mejorCarril != enemigo.Carril)
                return Accion.CambiarCarril(mejorCarril, $"{Estacion.Subsistemas[mejorCarril]} está menos defendido");

            // 2) Si tiene defensores a su alcance, ataca al que tenga menos vida.
            var objetivos = contexto.Grilla.DefensoresEnRango(enemigo.Carril, enemigo.Columna, enemigo.Alcance);
            if (objetivos.Count > 0)
            {
                Unidad masDebil = objetivos[0];
                foreach (var objetivo in objetivos)
                {
                    if (objetivo.Vida < masDebil.Vida) masDebil = objetivo;
                }
                return Accion.Atacar(masDebil, "ataca al defensor más débil");
            }

            // 3) Si no, sigue avanzando.
            if (contexto.Grilla.PuedeAvanzar(enemigo)) return Accion.Avanzar("sigue avanzando");
            return Accion.Esperar("otro enemigo le tapa el paso");
        }
    }

    /// <summary>Las estrategias disponibles (no guardan estado, así que se comparten).</summary>
    public static class EstrategiasIA
    {
        public static readonly IEstrategiaIA Agresiva = new IAAgresiva();
        public static readonly IEstrategiaIA Defensiva = new IADefensiva();
        public static readonly IEstrategiaIA Aleatoria = new IAAleatoria();
        public static readonly IEstrategiaIA Calculadora = new IACalculadora();

        public static readonly IEstrategiaIA[] Todas = { Agresiva, Defensiva, Aleatoria, Calculadora };
    }
}
