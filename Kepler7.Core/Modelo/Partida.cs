using System;
using System.Collections.Generic;
using Kepler7.Core.Patrones.Builder;
using Kepler7.Core.Patrones.Flyweight;
using Kepler7.Core.Patrones.Prototype;
using Kepler7.Core.Patrones.Strategy;
using Kepler7.Core.Patrones.TemplateMethod;
using Kepler7.Core.Patrones.State;
using Kepler7.Core.Patrones.ChainOfResponsibility;
using Kepler7.Core.Patrones.Visitor;

namespace Kepler7.Core.Modelo
{
    // Una partida completa: tablero, economía, oleadas y el bucle de ticks.
    // No sabe nada de formularios: la interfaz solo llama a AvanzarTick() desde un Timer.
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
        private Heroe _heroe;
        private Inventario _inventario;

        public Estacion Estacion { get; }
        public Grilla Grilla { get; } = new Grilla();
        public HistorialTurnos Historial { get; } = new HistorialTurnos();
        public Random Azar { get; }
        public FasePartida Fase { get; private set; } = FasePartida.Preparacion;
        public int Tick { get; private set; }
        public int EnemigosDerrotados { get; private set; }

        // Defensores propios destruidos (tripulantes, naves y el héroe). Se muestra en el resumen final.
        public int DefensoresPerdidos { get; private set; }

        // True si la partida terminó porque se apagó la estación, y no porque cayeron los subsistemas.
        public bool FueApagada { get; private set; }

        // FLYWEIGHT: la tripulación de la estación. Es el escudo de cada núcleo.
        public BibliotecaTripulantes Tripulacion { get; } = new BibliotecaTripulantes();

        // Cuántos tripulantes mueren por cada punto de daño que llega al núcleo.
        // Mueren rápido a propósito: la tripulación cae antes que el núcleo.
        // Era 30; con 50 un subsistema queda expuesto después de 3 golpes de Pirata en vez de 4.
        public const int TripulantesPorPuntoDeDanio = 50;
        public const int TripulantesPorEnvio = 250;
        public const int CostoEnvioTripulantes = 50;

        // Mensajes importantes de la partida (inicio de oleada, bajas, golpes al núcleo...).
        public event Action<string>? Mensaje;

        // La semilla es opcional: con la misma semilla el azar da siempre los mismos resultados.
        public Partida(int creditosIniciales, int? semilla = null)
        {
            Estacion = new Estacion(creditosIniciales);
            if (semilla.HasValue)
            {
                Azar = new Random(semilla.Value);
            }
            else
            {
                Azar = new Random();
            }

            _oleadas = Campania.CrearOleadas();
            _contexto = new Contexto(this);
            _heroe = new Heroe();
            Estacion.AgregarAReserva(_heroe);

            _inventario = new Inventario();
            _inventario.Agregar(new ItemArma("Rifle reforzado", 5));
            _inventario.Agregar(new ItemModuloDefensa("Módulo de evasión", new DefensaEvasion(Azar)));
            _inventario.Agregar(new ItemModuloDefensa("Módulo de escudo", new DefensaEscudo()));

            for (int i = 0; i < _integridad.Length; i++)
            {
                _integridad[i] = IntegridadMaxima;
            }
        }

        public Oleada OleadaActual
        {
            get { return _oleadas[_indiceOleada]; }
        }

        public int NumeroOleada
        {
            get { return _indiceOleada + 1; }
        }

        public int TotalOleadas
        {
            get { return _oleadas.Count; }
        }

        public Inventario ObtenerInventario()
        {
            return _inventario;
        }

        // VISITOR: un ítem nuevo por cada oleada superada.
        private void EntregarPremio()
        {
            ItemInventario? premio = null;

            switch (_indiceOleada)
            {
                case 0:
                    premio = new ItemArmadura("Armadura táctica", 50);
                    break;
                case 1:
                    premio = new ItemArma("Rifle de plasma", 10);
                    break;
                case 2:
                    premio = new ItemArmadura("Exoarmadura", 100);
                    break;
                case 3:
                    premio = new ItemArma("Cañón de riel", 20);
                    break;
            }

            if (premio != null)
            {
                _inventario.Agregar(premio);
                Avisar("Nuevo ítem en el inventario: " + premio.ObtenerNombre());
            }
        }

        public Heroe ObtenerHeroe()
        {
            return _heroe;
        }

        public int IntegridadDelCarril(int carril)
        {
            return _integridad[carril];
        }

        public int CarrilesCaidos
        {
            get
            {
                int caidos = 0;
                for (int i = 0; i < _integridad.Length; i++)
                {
                    if (_integridad[i] == 0)
                    {
                        caidos = caidos + 1;
                    }
                }
                return caidos;
            }
        }

        // Créditos extra por oleada que dan las naves de carga desplegadas.
        public int BonusRecursosPorOleada
        {
            get
            {
                int total = 0;
                List<Unidad> defensores = Grilla.Defensores();
                for (int i = 0; i < defensores.Count; i++)
                {
                    if (defensores[i] is NaveApoyo)
                    {
                        NaveApoyo nave = (NaveApoyo)defensores[i];
                        total = total + nave.BonusRecursos;
                    }
                }
                return total;
            }
        }

        // --- Preparación ---

        public bool Desplegar(Unidad unidad, int carril, int columna)
        {
            if (Fase != FasePartida.Preparacion)
            {
                return false;
            }

            if (!Estacion.Reserva.Contains(unidad) || !Grilla.EsZonaDefensa(columna))
            {
                return false;
            }

            if (!Grilla.Colocar(unidad, carril, columna))
            {
                return false;
            }

            Estacion.QuitarDeReserva(unidad);
            return true;
        }

        public bool IniciarOleada()
        {
            if (Fase != FasePartida.Preparacion)
            {
                return false;
            }

            _tickOleada = 0;
            _porAparecer.Clear();
            _porAparecer.AddRange(OleadaActual.Spawns);
            Fase = FasePartida.Oleada;
            Avisar("Oleada " + NumeroOleada + "/" + TotalOleadas + ": " + OleadaActual.Nombre + " (IA " + OleadaActual.Estrategia.Nombre + ")");
            Avisar(OleadaActual.Historia);
            return true;
        }

        // --- El bucle del juego ---

        public void AvanzarTick()
        {
            if (Fase != FasePartida.Oleada)
            {
                return;
            }

            Tick = Tick + 1;
            _tickOleada = _tickOleada + 1;

            HacerAparecerEnemigos();

            // 1) Juegan los defensores (el Comandante primero, así su bonus llega a los demás).
            List<Unidad> defensores = Grilla.Defensores();
            OrdenarPorPrioridad(defensores);
            for (int i = 0; i < defensores.Count; i++)
            {
                if (defensores[i].EstaViva)
                {
                    defensores[i].Turno.EjecutarTurno(defensores[i], _contexto);
                }
            }

            // 2) Juegan los enemigos, empezando por los que están más cerca del núcleo.
            List<Enemigo> enemigos = Grilla.Enemigos();
            OrdenarPorColumna(enemigos);
            for (int i = 0; i < enemigos.Count; i++)
            {
                // Un enemigo pudo haber terminado la partida al golpear un núcleo.
                if (Fase != FasePartida.Oleada)
                {
                    break;
                }

                if (enemigos[i].EstaViva && enemigos[i].Columna >= 0)
                {
                    enemigos[i].Turno.EjecutarTurno(enemigos[i], _contexto);
                }
            }

            // 3) Fin del tick: los bonus temporales se terminan.
            List<Unidad> unidades = Grilla.Unidades();
            for (int i = 0; i < unidades.Count; i++)
            {
                unidades[i].BonusDanio = 0;
            }

            if (Fase == FasePartida.Oleada && _porAparecer.Count == 0 && Grilla.Enemigos().Count == 0)
            {
                TerminarOleada();
            }
        }

        // Ordenamiento por inserción. Con igual prioridad se respeta el orden que ya tenían,
        // y eso importa: define quién juega primero. List.Sort no lo garantiza.
        private static void OrdenarPorPrioridad(List<Unidad> lista)
        {
            for (int i = 1; i < lista.Count; i++)
            {
                Unidad actual = lista[i];
                int j = i - 1;
                while (j >= 0 && lista[j].Turno.Prioridad > actual.Turno.Prioridad)
                {
                    lista[j + 1] = lista[j];
                    j = j - 1;
                }
                lista[j + 1] = actual;
            }
        }

        // Lo mismo que OrdenarPorPrioridad, pero para los enemigos según su columna.
        private static void OrdenarPorColumna(List<Enemigo> lista)
        {
            for (int i = 1; i < lista.Count; i++)
            {
                Enemigo actual = lista[i];
                int j = i - 1;
                while (j >= 0 && lista[j].Columna > actual.Columna)
                {
                    lista[j + 1] = lista[j];
                    j = j - 1;
                }
                lista[j + 1] = actual;
            }
        }

        private void HacerAparecerEnemigos()
        {
            // Se recorre una copia porque de la lista original se van sacando los que ya aparecieron.
            List<SpawnEnemigo> pendientes = new List<SpawnEnemigo>(_porAparecer);
            for (int i = 0; i < pendientes.Count; i++)
            {
                SpawnEnemigo spawn = pendientes[i];

                // Si la celda de entrada está ocupada, lo intenta de nuevo en el próximo tick.
                if (spawn.Tick <= _tickOleada && Grilla.EstaLibre(spawn.Carril, Grilla.Columnas - 1))
                {
                    Enemigo enemigo = Enemigo.Crear(spawn.Tipo, OleadaActual.Estrategia);
                    Grilla.Colocar(enemigo, spawn.Carril, Grilla.Columnas - 1);
                    _porAparecer.Remove(spawn);
                }
            }
        }

        private void TerminarOleada()
        {
            int premio = OleadaActual.Recompensa + BonusRecursosPorOleada;
            Estacion.Ganar(premio);

            List<Unidad> defensores = Grilla.Defensores();
            for (int i = 0; i < defensores.Count; i++)
            {
                defensores[i].Vida = defensores[i].VidaMaxima;
            }

            Avisar("Oleada " + NumeroOleada + " superada: +" + premio + " créditos y las unidades se reparan.");

            // STATE: el héroe vuelve a Normal. Si cayó, ya no está en el tablero y regresa a la reserva.
            bool heroeCaido = !_heroe.EstaViva;
            _heroe.Restaurar();
            if (heroeCaido)
            {
                Estacion.AgregarAReserva(_heroe);
                Avisar(_heroe.NombreCompleto + " se recuperó y volvió a la reserva.");
            }

            if (_indiceOleada == _oleadas.Count - 1)
            {
                Fase = FasePartida.Victoria;
                Avisar("¡VICTORIA! Kepler-7 está a salvo.");
            }
            else
            {
                EntregarPremio();
                _indiceOleada = _indiceOleada + 1;
                Fase = FasePartida.Preparacion;
            }
        }

        // --- Eventos del combate (los llama el Contexto) ---

        internal void Matar(Unidad unidad)
        {
            Grilla.Quitar(unidad);
            if (unidad is Enemigo)
            {
                Enemigo enemigo = (Enemigo)unidad;
                Estacion.Ganar(enemigo.Recompensa);
                EnemigosDerrotados = EnemigosDerrotados + 1;
                Avisar(enemigo.NombreCompleto + " derrotado (+" + enemigo.Recompensa + " créditos)");
            }
            else
            {
                DefensoresPerdidos = DefensoresPerdidos + 1;
                Avisar(unidad.NombreCompleto + " fue destruido");
            }
        }

        internal void GolpearNucleo(Enemigo enemigo)
        {
            int carril = enemigo.Carril;

            // Mientras queden tripulantes en el subsistema, el golpe los mata a ellos y no llega al núcleo.
            if (Tripulacion.TripulantesEn(carril) > 0)
            {
                int muertos = Tripulacion.Eliminar(carril, enemigo.Danio * 2 * TripulantesPorPuntoDeDanio);
                Grilla.Quitar(enemigo);
                Avisar("¡" + enemigo.NombreCompleto + " atacó " + Estacion.Subsistemas[carril] + "! Murieron " + muertos + " tripulantes "
                    + "(quedan " + Tripulacion.TripulantesEn(carril) + ").");

                if (Tripulacion.TripulantesEn(carril) == 0)
                {
                    Avisar("¡" + Estacion.Subsistemas[carril] + " se quedó sin tripulación: el núcleo está expuesto!");
                }
                return;
            }

            int antes = _integridad[carril];
            _integridad[carril] = Math.Max(0, antes - enemigo.Danio * 2);
            Grilla.Quitar(enemigo);

            Avisar("¡" + enemigo.NombreCompleto + " golpeó el núcleo de " + Estacion.Subsistemas[carril] + "! "
                + "Integridad " + _integridad[carril] + "/" + IntegridadMaxima);

            if (antes > 0 && _integridad[carril] == 0)
            {
                Avisar("¡El subsistema " + Estacion.Subsistemas[carril] + " cayó!");
            }

            if (CarrilesCaidos >= CarrilesCaidosParaPerder)
            {
                Fase = FasePartida.Derrota;
                Avisar("DERROTA: cayeron " + CarrilesCaidos + " subsistemas. Kepler-7 se perdió.");
            }
        }

        // --- Reparación del núcleo (la usan CurarComando y la emergencia del ControlCentral) ---

        // Repara el núcleo de un carril y devuelve cuánto se reparó. Un carril caído no se puede reparar.
        public int RepararNucleo(int carril, int cantidad)
        {
            int antes = _integridad[carril];
            if (antes == 0)
            {
                return 0;
            }

            _integridad[carril] = Math.Min(IntegridadMaxima, antes + cantidad);
            return _integridad[carril] - antes;
        }

        // Vuelve el núcleo a un valor anterior (para deshacer una reparación).
        internal void RestaurarIntegridad(int carril, int valor)
        {
            _integridad[carril] = valor;
        }

        // FLYWEIGHT: manda tripulantes nuevos a un subsistema (se puede durante la oleada).
        // Un subsistema caído ya no se puede reforzar.
        public bool EnviarTripulantes(int carril)
        {
            if (Fase == FasePartida.Victoria || Fase == FasePartida.Derrota)
            {
                return false;
            }

            if (_integridad[carril] == 0 || !Estacion.Gastar(CostoEnvioTripulantes))
            {
                return false;
            }

            Tripulacion.Agregar(carril, TripulantesPorEnvio);
            Avisar(TripulantesPorEnvio + " tripulantes llegan a " + Estacion.Subsistemas[carril] + " "
                + "(ahora hay " + Tripulacion.TripulantesEn(carril) + ", -" + CostoEnvioTripulantes + " cr).");
            return true;
        }

        // Termina la partida porque el operador apagó la estación (lo usa el ControlCentral).
        public void Apagar()
        {
            if (Fase == FasePartida.Victoria || Fase == FasePartida.Derrota)
            {
                return;
            }

            Fase = FasePartida.Derrota;
            FueApagada = true;
            Avisar("La estación fue apagada por el operador. Fin de la partida.");
        }

        // Arma el resumen para la pantalla final. Tiene sentido cuando la partida ya terminó.
        public ResumenPartida CrearResumen()
        {
            // En la victoria el índice queda en la última oleada, por eso ahí se cuentan todas.
            int oleadasSuperadas;
            if (Fase == FasePartida.Victoria)
            {
                oleadasSuperadas = TotalOleadas;
            }
            else
            {
                oleadasSuperadas = NumeroOleada - 1;
            }

            List<string> caidos = new List<string>();
            for (int carril = 0; carril < _integridad.Length; carril++)
            {
                if (_integridad[carril] == 0)
                {
                    caidos.Add(Estacion.Subsistemas[carril]);
                }
            }

            // Generados incluye los 10.000 del principio y todos los refuerzos que se mandaron.
            int tripulantesPerdidos = Tripulacion.TotalGenerados - Tripulacion.TotalVivos;

            return new ResumenPartida(Fase == FasePartida.Victoria, FueApagada, oleadasSuperadas, TotalOleadas,
                                      EnemigosDerrotados, DefensoresPerdidos, Tick, Estacion.Creditos,
                                      tripulantesPerdidos, caidos);
        }

        // --- Strategy: cambiar la IA durante la partida ---

        public void CambiarEstrategia(Enemigo enemigo, IEstrategiaIA nueva)
        {
            Avisar(enemigo.NombreCompleto + ": IA " + enemigo.Estrategia.Nombre + " → " + nueva.Nombre);
            enemigo.Estrategia = nueva;
        }

        // Cambia la IA de todos los enemigos que están en el tablero y de los que todavía no aparecieron.
        public void CambiarEstrategiaATodos(IEstrategiaIA nueva)
        {
            Avisar("Todos los enemigos pasan a IA " + nueva.Nombre);
            OleadaActual.Estrategia = nueva;

            List<Enemigo> enemigos = Grilla.Enemigos();
            for (int i = 0; i < enemigos.Count; i++)
            {
                enemigos[i].Estrategia = nueva;
            }
        }

        // --- Guardar y retomar (solo entre oleadas) ---

        private const string TipoHeroe = "Heroe";
        private const string TipoNave = "Nave";

        // Arma la foto de la partida para guardarla. Tiene sentido en la preparación: ahí no hay enemigos,
        // los defensores tienen la vida llena y los escudos están recargados, así que no hace falta guardar eso.
        public PartidaGuardada CrearGuardado()
        {
            PartidaGuardada guardada = new PartidaGuardada();
            guardada.NumeroOleada = NumeroOleada;
            guardada.Creditos = Estacion.Creditos;
            guardada.Tick = Tick;
            guardada.EnemigosDerrotados = EnemigosDerrotados;
            guardada.DefensoresPerdidos = DefensoresPerdidos;
            guardada.EstrategiaOleada = OleadaActual.Estrategia.Nombre;
            guardada.TripulantesGenerados = Tripulacion.TotalGenerados;

            for (int carril = 0; carril < Grilla.Carriles; carril++)
            {
                guardada.Integridad.Add(_integridad[carril]);
                guardada.TripulantesPorCarril.Add(Tripulacion.TripulantesEn(carril));
            }

            List<Unidad> defensores = Grilla.Defensores();
            for (int i = 0; i < defensores.Count; i++)
            {
                guardada.Unidades.Add(GuardarUnidad(defensores[i]));
            }
            for (int i = 0; i < Estacion.Reserva.Count; i++)
            {
                guardada.Unidades.Add(GuardarUnidad(Estacion.Reserva[i]));
            }

            ItemArma? arma = _heroe.ObtenerArma();
            if (arma != null)
            {
                guardada.ArmaHeroe = arma.ObtenerNombre();
                guardada.BonusArmaHeroe = arma.ObtenerBonusDanio();
            }
            ItemArmadura? armadura = _heroe.ObtenerArmadura();
            if (armadura != null)
            {
                guardada.ArmaduraHeroe = armadura.ObtenerNombre();
                guardada.BonusArmaduraHeroe = armadura.ObtenerBonusVida();
            }

            List<Defensa> defensas = _heroe.ObtenerCadena().ObtenerDefensas();
            for (int i = 0; i < defensas.Count; i++)
            {
                guardada.DefensasHeroe.Add(VisitanteGuardado.NombreDefensa(defensas[i]));
            }

            // VISITOR: cada ítem se pasa a datos con un visitante, sin preguntar de qué tipo es.
            List<ItemInventario> items = _inventario.ObtenerItems();
            for (int i = 0; i < items.Count; i++)
            {
                VisitanteGuardado visitante = new VisitanteGuardado();
                items[i].Aceptar(visitante);
                guardada.Inventario.Add(visitante.ObtenerResultado());
            }

            return guardada;
        }

        // Arma una partida nueva y la deja como estaba en el guardado.
        public static Partida DesdeGuardado(PartidaGuardada guardada)
        {
            Partida partida = new Partida(guardada.Creditos);
            partida.CargarGuardado(guardada);
            return partida;
        }

        private void CargarGuardado(PartidaGuardada guardada)
        {
            // Si el número viniera roto, se acomoda a una oleada que exista.
            _indiceOleada = guardada.NumeroOleada - 1;
            if (_indiceOleada < 0)
            {
                _indiceOleada = 0;
            }
            if (_indiceOleada > _oleadas.Count - 1)
            {
                _indiceOleada = _oleadas.Count - 1;
            }

            Tick = guardada.Tick;
            EnemigosDerrotados = guardada.EnemigosDerrotados;
            DefensoresPerdidos = guardada.DefensoresPerdidos;

            for (int carril = 0; carril < Grilla.Carriles; carril++)
            {
                if (carril < guardada.Integridad.Count)
                {
                    _integridad[carril] = guardada.Integridad[carril];
                }

                // La tripulación arranca con los 10.000 de siempre: se suman o se sacan hasta llegar a lo guardado.
                if (carril < guardada.TripulantesPorCarril.Count)
                {
                    int actuales = Tripulacion.TripulantesEn(carril);
                    int guardados = guardada.TripulantesPorCarril[carril];
                    if (guardados > actuales)
                    {
                        Tripulacion.Agregar(carril, guardados - actuales);
                    }
                    else if (guardados < actuales)
                    {
                        Tripulacion.Eliminar(carril, actuales - guardados);
                    }
                }
            }
            if (guardada.TripulantesGenerados > 0)
            {
                Tripulacion.TotalGenerados = guardada.TripulantesGenerados;
            }

            for (int i = 0; i < EstrategiasIA.Todas.Length; i++)
            {
                if (EstrategiasIA.Todas[i].Nombre == guardada.EstrategiaOleada)
                {
                    OleadaActual.Estrategia = EstrategiasIA.Todas[i];
                }
            }

            // El héroe: primero el equipo (la armadura cambia la vida máxima) y después la cadena en su orden.
            if (guardada.ArmaHeroe != "")
            {
                _heroe.EquiparArma(new ItemArma(guardada.ArmaHeroe, guardada.BonusArmaHeroe));
            }
            if (guardada.ArmaduraHeroe != "")
            {
                _heroe.EquiparArmadura(new ItemArmadura(guardada.ArmaduraHeroe, guardada.BonusArmaduraHeroe));
            }
            if (guardada.DefensasHeroe.Count > 0)
            {
                CadenaDefensas cadena = _heroe.ObtenerCadena();
                cadena.Vaciar();
                for (int i = 0; i < guardada.DefensasHeroe.Count; i++)
                {
                    cadena.Agregar(CrearDefensa(guardada.DefensasHeroe[i]));
                }
            }

            _inventario = new Inventario();
            for (int i = 0; i < guardada.Inventario.Count; i++)
            {
                ItemInventario? item = CrearItem(guardada.Inventario[i]);
                if (item != null)
                {
                    _inventario.Agregar(item);
                }
            }

            // El constructor dejó al héroe en la reserva: se lo saca para ponerlo donde estaba.
            Estacion.QuitarDeReserva(_heroe);
            bool heroeUbicado = false;
            for (int i = 0; i < guardada.Unidades.Count; i++)
            {
                UnidadGuardada datos = guardada.Unidades[i];
                Unidad? unidad = null;
                if (datos.Tipo != TipoHeroe || !heroeUbicado)
                {
                    unidad = CrearUnidad(datos);
                }

                if (unidad != null)
                {
                    if (unidad == _heroe)
                    {
                        heroeUbicado = true;
                    }
                    if (datos.Vida > 0 && datos.Vida <= unidad.VidaMaxima)
                    {
                        unidad.Vida = datos.Vida;
                    }

                    // Si la celda no estuviera libre (un guardado roto), la unidad va a la reserva en vez de perderse.
                    bool colocada = false;
                    if (datos.Carril >= 0)
                    {
                        colocada = Grilla.Colocar(unidad, datos.Carril, datos.Columna);
                    }
                    if (!colocada)
                    {
                        Estacion.AgregarAReserva(unidad);
                    }
                }
            }

            if (!heroeUbicado)
            {
                Estacion.AgregarAReserva(_heroe);
            }
        }

        private static UnidadGuardada GuardarUnidad(Unidad unidad)
        {
            UnidadGuardada datos = new UnidadGuardada();
            datos.Carril = unidad.Carril;
            datos.Columna = unidad.Columna;
            datos.Vida = unidad.Vida;

            if (unidad is Heroe)
            {
                datos.Tipo = TipoHeroe;
            }
            else if (unidad is NaveApoyo)
            {
                datos.Tipo = TipoNave;
                datos.Receta = ((NaveApoyo)unidad).Tipo;
            }
            else if (unidad is Tripulante)
            {
                Tripulante tripulante = (Tripulante)unidad;
                datos.Tipo = tripulante.Rol;
                for (int i = 0; i < tripulante.Equipamiento.Count; i++)
                {
                    Equipo equipo = tripulante.Equipamiento[i];
                    EquipoGuardado guardado = new EquipoGuardado();
                    guardado.Nombre = equipo.Nombre;
                    guardado.BonusDanio = equipo.BonusDanio;
                    guardado.BonusVida = equipo.BonusVida;
                    guardado.Costo = equipo.Costo;
                    datos.Equipamiento.Add(guardado);
                }
            }

            return datos;
        }

        // Vuelve a armar cada unidad con el mismo patrón que la creó la primera vez:
        // las naves con el Astillero (Builder) y los tripulantes desde su clase, con su equipo de antes.
        private Unidad? CrearUnidad(UnidadGuardada datos)
        {
            if (datos.Tipo == TipoHeroe)
            {
                return _heroe;
            }

            if (datos.Tipo == TipoNave)
            {
                INaveApoyoBuilder? receta = RecetaPorNombre(datos.Receta);
                if (receta == null)
                {
                    return null;
                }
                Astillero astillero = new Astillero();
                return astillero.Ensamblar(receta);
            }

            Tripulante? tripulante = TripulantePorRol(datos.Tipo);
            if (tripulante == null)
            {
                return null;
            }

            List<Equipo> equipo = new List<Equipo>();
            for (int i = 0; i < datos.Equipamiento.Count; i++)
            {
                EquipoGuardado guardado = datos.Equipamiento[i];
                equipo.Add(new Equipo(guardado.Nombre, guardado.BonusDanio, guardado.BonusVida, guardado.Costo));
            }
            tripulante.ReemplazarEquipamiento(equipo);
            return tripulante;
        }

        private static INaveApoyoBuilder? RecetaPorNombre(string receta)
        {
            switch (receta)
            {
                case "Exploradora":
                    return new NaveExploradoraBuilder();
                case "Combate":
                    return new NaveCombateBuilder();
                case "Carga":
                    return new NaveCargaBuilder();
                default:
                    return null;
            }
        }

        private static Tripulante? TripulantePorRol(string rol)
        {
            switch (rol)
            {
                case "Soldado":
                    return new Soldado();
                case "Médico":
                    return new Medico();
                case "Francotirador":
                    return new Francotirador();
                case "Comandante":
                    return new Comandante();
                default:
                    return null;
            }
        }

        private Defensa CrearDefensa(string nombre)
        {
            switch (nombre)
            {
                case "Escudo":
                    return new DefensaEscudo();
                case "Evasion":
                    return new DefensaEvasion(Azar);
                default:
                    return new DefensaBlindaje();
            }
        }

        private ItemInventario? CrearItem(ItemGuardado datos)
        {
            switch (datos.Tipo)
            {
                case "Arma":
                    return new ItemArma(datos.Nombre, datos.Bonus);
                case "Armadura":
                    return new ItemArmadura(datos.Nombre, datos.Bonus);
                case "Modulo":
                    return new ItemModuloDefensa(datos.Nombre, CrearDefensa(datos.Defensa));
                default:
                    return null;
            }
        }

        private void Avisar(string texto)
        {
            if (Mensaje != null)
            {
                Mensaje(texto);
            }
        }
    }
}
