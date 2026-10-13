using System.Collections.Generic;
using System.Linq;
using Kepler7.Core.Patrones.Builder;
using Kepler7.Core.Patrones.Strategy;
using Kepler7.Core.Patrones.TemplateMethod;

namespace Kepler7.Core.Modelo
{
    public enum FasePartida
    {
        Preparacion,   // el jugador arma y despliega sus defensas
        Oleada,        // los ticks avanzan y los enemigos atacan
        Victoria,
        Derrota
    }

    /// <summary>
    /// Una partida completa: tablero, economía, oleadas y el bucle de ticks.
    /// No sabe nada de formularios: la interfaz solo llama a AvanzarTick() desde un Timer.
    /// </summary>
    public class Partida
    {
        public const int IntegridadMaxima = 100;
        public const int CarrilesCaidosParaPerder = 3;

        private readonly List<Oleada> _oleadas;
        private readonly int[] _integridad = new int[Grilla.Carriles];
        private readonly List<SpawnEnemigo> _porAparecer = new List<SpawnEnemigo>();
        private readonly Contexto _contexto;
        private int _indiceOleada;
        private int _tickOleada;

        public Estacion Estacion { get; }
        public Grilla Grilla { get; } = new Grilla();
        public HistorialTurnos Historial { get; } = new HistorialTurnos();
        public Random Azar { get; }
        public FasePartida Fase { get; private set; } = FasePartida.Preparacion;
        public int Tick { get; private set; }
        public int EnemigosDerrotados { get; private set; }

        // Mensajes importantes de la partida (inicio de oleada, bajas, golpes al núcleo...).
        public event Action<string>? Mensaje;

        public Partida(int creditosIniciales, int? semilla = null)
        {
            Estacion = new Estacion(creditosIniciales);
            Azar = semilla.HasValue ? new Random(semilla.Value) : new Random();
            _oleadas = Campania.CrearOleadas();
            _contexto = new Contexto(this);
            for (int i = 0; i < _integridad.Length; i++) _integridad[i] = IntegridadMaxima;
        }

        public Oleada OleadaActual => _oleadas[_indiceOleada];
        public int NumeroOleada => _indiceOleada + 1;
        public int TotalOleadas => _oleadas.Count;

        public int IntegridadDelCarril(int carril) => _integridad[carril];

        public int CarrilesCaidos => _integridad.Count(i => i == 0);

        // Créditos extra por oleada que dan las naves de carga desplegadas.
        public int BonusRecursosPorOleada =>
            Grilla.Defensores().OfType<NaveApoyo>().Sum(n => n.BonusRecursos);

        // --- Preparación ---

        public bool Desplegar(Unidad unidad, int carril, int columna)
        {
            if (Fase != FasePartida.Preparacion) return false;
            if (!Estacion.Reserva.Contains(unidad) || !Grilla.EsZonaDefensa(columna)) return false;
            if (!Grilla.Colocar(unidad, carril, columna)) return false;

            Estacion.QuitarDeReserva(unidad);
            return true;
        }

        public bool IniciarOleada()
        {
            if (Fase != FasePartida.Preparacion) return false;

            _tickOleada = 0;
            _porAparecer.Clear();
            _porAparecer.AddRange(OleadaActual.Spawns);
            Fase = FasePartida.Oleada;
            Avisar($"Oleada {NumeroOleada}/{TotalOleadas}: {OleadaActual.Nombre} (IA {OleadaActual.Estrategia.Nombre})");
            Avisar(OleadaActual.Historia);
            return true;
        }

        // --- El bucle del juego ---

        public void AvanzarTick()
        {
            if (Fase != FasePartida.Oleada) return;

            Tick++;
            _tickOleada++;

            HacerAparecerEnemigos();

            // 1) Juegan los defensores (el Comandante primero, así su bonus llega a los demás).
            var defensores = Grilla.Defensores().OrderBy(d => d.Turno.Prioridad).ToList();
            foreach (var defensor in defensores)
            {
                if (defensor.EstaViva) defensor.Turno.EjecutarTurno(defensor, _contexto);
            }

            // 2) Juegan los enemigos, empezando por los que están más cerca del núcleo.
            var enemigos = Grilla.Enemigos().OrderBy(e => e.Columna).ToList();
            foreach (var enemigo in enemigos)
            {
                if (Fase != FasePartida.Oleada) break;
                if (enemigo.EstaViva && enemigo.Columna >= 0) enemigo.Turno.EjecutarTurno(enemigo, _contexto);
            }

            // 3) Fin del tick: los bonus temporales se terminan.
            foreach (var unidad in Grilla.Unidades()) unidad.BonusDanio = 0;

            if (Fase == FasePartida.Oleada && _porAparecer.Count == 0 && Grilla.Enemigos().Count == 0)
                TerminarOleada();
        }

        private void HacerAparecerEnemigos()
        {
            foreach (var spawn in _porAparecer.ToList())
            {
                if (spawn.Tick > _tickOleada) continue;
                // Si la celda de entrada está ocupada, lo intenta de nuevo en el próximo tick.
                if (!Grilla.EstaLibre(spawn.Carril, Grilla.Columnas - 1)) continue;

                var enemigo = Enemigo.Crear(spawn.Tipo, OleadaActual.Estrategia);
                Grilla.Colocar(enemigo, spawn.Carril, Grilla.Columnas - 1);
                _porAparecer.Remove(spawn);
            }
        }

        private void TerminarOleada()
        {
            int premio = OleadaActual.Recompensa + BonusRecursosPorOleada;
            Estacion.Ganar(premio);
            foreach (var defensor in Grilla.Defensores()) defensor.Vida = defensor.VidaMaxima;
            Avisar($"Oleada {NumeroOleada} superada: +{premio} créditos y las unidades se reparan.");

            if (_indiceOleada == _oleadas.Count - 1)
            {
                Fase = FasePartida.Victoria;
                Avisar("¡VICTORIA! Kepler-7 está a salvo.");
            }
            else
            {
                _indiceOleada++;
                Fase = FasePartida.Preparacion;
            }
        }

        // --- Eventos del combate (los llama el Contexto) ---

        internal void Matar(Unidad unidad)
        {
            Grilla.Quitar(unidad);
            if (unidad is Enemigo enemigo)
            {
                Estacion.Ganar(enemigo.Recompensa);
                EnemigosDerrotados++;
                Avisar($"{enemigo.NombreCompleto} derrotado (+{enemigo.Recompensa} créditos)");
            }
            else
            {
                Avisar($"{unidad.NombreCompleto} fue destruido");
            }
        }

        internal void GolpearNucleo(Enemigo enemigo)
        {
            int carril = enemigo.Carril;
            int antes = _integridad[carril];
            _integridad[carril] = Math.Max(0, antes - enemigo.Danio * 2);
            Grilla.Quitar(enemigo);

            Avisar($"¡{enemigo.NombreCompleto} golpeó el núcleo de {Estacion.Subsistemas[carril]}! " +
                   $"Integridad {_integridad[carril]}/{IntegridadMaxima}");
            if (antes > 0 && _integridad[carril] == 0)
                Avisar($"¡El subsistema {Estacion.Subsistemas[carril]} cayó!");

            if (CarrilesCaidos >= CarrilesCaidosParaPerder)
            {
                Fase = FasePartida.Derrota;
                Avisar($"DERROTA: cayeron {CarrilesCaidos} subsistemas. Kepler-7 se perdió.");
            }
        }

        // --- Strategy: cambiar la IA durante la partida ---

        public void CambiarEstrategia(Enemigo enemigo, IEstrategiaIA nueva)
        {
            Avisar($"{enemigo.NombreCompleto}: IA {enemigo.Estrategia.Nombre} → {nueva.Nombre}");
            enemigo.Estrategia = nueva;
        }

        // Cambia la IA de todos los enemigos que están en el tablero y de los que todavía no aparecieron.
        public void CambiarEstrategiaATodos(IEstrategiaIA nueva)
        {
            Avisar($"Todos los enemigos pasan a IA {nueva.Nombre}");
            OleadaActual.Estrategia = nueva;
            foreach (var enemigo in Grilla.Enemigos()) enemigo.Estrategia = nueva;
        }

        private void Avisar(string texto)
        {
            Mensaje?.Invoke(texto);
        }
    }
}
