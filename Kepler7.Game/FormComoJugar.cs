namespace Kepler7.Game
{
    // El tutorial: unas páginas que se recorren con Anterior y Siguiente. Se abre desde la pantalla
    // inicial y desde el menú del juego. Entre corchetes va el patrón que hay detrás de cada cosa.
    public partial class FormComoJugar : Form
    {
        private static readonly string[] Titulos =
        {
            "El objetivo",
            "El tablero",
            "Armar tus defensas",
            "La oleada",
            "El héroe",
            "El Hub y las recompensas",
            "Guardar y seguir"
        };

        private static readonly string[] Textos =
        {
            "Sos el operador de la estación Kepler-7. Cada uno de sus 6 subsistemas (Energía, Soporte Vital, " +
            "Navegación, Comunicaciones, Seguridad y Defensa) es un carril del tablero.\n\n" +
            "Los enemigos entran por la derecha y avanzan hacia la izquierda, donde está el núcleo de cada subsistema.\n\n" +
            "•  Ganás si resistís las 5 oleadas.\n" +
            "•  Perdés si caen 3 subsistemas (su núcleo llega a 0).",

            "•  Las celdas celestes son tu zona de defensa: ahí desplegás tus unidades.\n" +
            "•  Las rosadas son por donde se acercan los enemigos.\n" +
            "•  A la izquierda de cada carril ves la integridad de su núcleo y cuántos tripulantes tiene.\n\n" +
            "Cuando un enemigo llega al final del carril, primero mata tripulantes. Recién cuando un subsistema " +
            "se queda sin tripulación, los golpes dañan su núcleo.\n\n" +
            "Hacé clic en cualquier unidad del tablero para ver sus datos.",

            "Empezás con 500 créditos. Las unidades se compran desde el menú:\n\n" +
            "•  Astillero [Builder]: naves de Exploración, Combate o Carga (la de Carga da créditos extra en cada oleada).\n" +
            "•  Laboratorio [Prototype]: clonás Soldados, Médicos, Francotiradores y Comandantes, y les comprás equipo.\n\n" +
            "Lo que comprás va a la Reserva. Elegí una unidad de la reserva y hacé clic en una celda celeste para " +
            "desplegarla. Entre oleadas también podés mover a tus defensores.\n\n" +
            "Deshacer y Rehacer [Command] sirven para arrepentirte de una compra o de un movimiento.",

            "Cuando estés listo, apretá Iniciar oleada (o usá el Control central [Facade]). Podés cambiar la velocidad o pausar.\n\n" +
            "•  Cada unidad juega su turno siempre con los mismos pasos [Template Method].\n" +
            "•  Los enemigos deciden según su IA [Strategy], y se la podés cambiar a uno o a todos.\n" +
            "•  Disparo orbital: 30 de daño al enemigo que elijas (40 créditos).\n" +
            "•  Tripulación [Flyweight]: mandá 250 tripulantes a un subsistema (50 créditos).\n" +
            "•  Emergencia del Control central: repara y cura el subsistema más dañado, una vez por oleada.\n\n" +
            "Entre oleadas podés reparar núcleos dañados (60 créditos).",

            "Tu héroe es el Comandante dorado (Hé). Arranca en la reserva y se despliega como cualquier unidad.\n\n" +
            "•  Su estado cambia solo [State]: con poca vida se Enfurece y pega más, un golpe fuerte lo Aturde " +
            "(pierde 2 turnos) y sin vida Cae hasta el final de la oleada.\n" +
            "•  Los golpes pasan por su cadena de defensas [Chain of Responsibility]: Blindaje, Escudo y Evasión. " +
            "Cambiá el orden con Subir y Bajar.\n" +
            "•  En el Inventario [Visitor] le equipás armas, armaduras y módulos de defensa (solo entre oleadas). " +
            "Las primeras oleadas que superás te dan ítems nuevos.",

            "El Hub [Proxy, Bridge] guarda tu progreso de una partida a otra:\n\n" +
            "•  Cada oleada superada te da XP y monedas al instante.\n" +
            "•  Cada logro (hay uno por patrón) se puede ganar una vez por partida. Desde que superás la primera " +
            "oleada, cada logro es una recompensa que cobrás en el Hub: +60 XP y +40 monedas.\n" +
            "•  Con las monedas comprás mejoras en la tienda para tu próxima partida. Se pagan a través de la " +
            "Bóveda, que solo deja gastar desde el nivel 2.\n\n" +
            "El panel de logros dice qué hacer para conseguir cada uno: es una buena guía para recorrer todos los patrones.",

            "•  Guardar todo (en el Hub) guarda tu progreso y, si estás entre oleadas, también la partida.\n" +
            "•  Al abrir el juego, Continuar partida la retoma desde donde la dejaste.\n" +
            "•  La partida guardada se usa una sola vez, y se borra cuando la partida termina.\n\n" +
            "Consejo: repartí defensores en todos los carriles. Un carril vacío es una invitación para que los " +
            "enemigos lleguen al núcleo.\n\n" +
            "¡Suerte, operador!"
        };

        private int _pagina = 0;

        public FormComoJugar()
        {
            InitializeComponent();
            new TransicionAparecer(this);
            MostrarPagina();
        }

        private void btnAnterior_Click(object? sender, EventArgs e)
        {
            if (_pagina > 0)
            {
                _pagina = _pagina - 1;
                MostrarPagina();
            }
        }

        private void btnSiguiente_Click(object? sender, EventArgs e)
        {
            // En la última página el botón dice "Terminar" y cierra el tutorial.
            if (_pagina == Titulos.Length - 1)
            {
                Close();
                return;
            }

            _pagina = _pagina + 1;
            MostrarPagina();
        }

        private void MostrarPagina()
        {
            lblPaso.Text = "Paso " + (_pagina + 1) + " de " + Titulos.Length;
            lblTitulo.Text = Titulos[_pagina];
            lblTexto.Text = Textos[_pagina];
            btnAnterior.Enabled = _pagina > 0;

            if (_pagina == Titulos.Length - 1)
            {
                btnSiguiente.Text = "Terminar";
            }
            else
            {
                btnSiguiente.Text = "Siguiente";
            }
        }
    }
}
