namespace Kepler7.Core.Modelo
{
    // Todos los logros del juego: uno por cada patrón y tres del progreso general.
    // Los de patrones se pueden ganar una vez por partida; los tres de progreso, una sola vez.
    // Los nombres están como constantes para que quien desbloquea un logro no tenga que escribirlo a mano.
    public static class CatalogoLogros
    {
        // Así figura el "patrón" de los tres logros de progreso general.
        public const string PatronProgreso = "Progreso";

        public const string Builder = "Ingeniero naval";
        public const string Prototype = "Clonador";
        public const string Command = "Viajero del tiempo";
        public const string Strategy = "Hacker de la IA";
        public const string TemplateMethod = "Primera batalla";
        public const string Facade = "Operador de emergencias";
        public const string Flyweight = "Reclutador";
        public const string State = "Furia";
        public const string ChainOfResponsibility = "Estratega defensivo";
        public const string Visitor = "Bien equipado";
        public const string Proxy = "Retiro autorizado";
        public const string Bridge = "Enlace con la Tierra";

        // Estos tres ya existían antes del catálogo. Si se les cambia el nombre,
        // los jugadores que ya los tenían guardados los perderían.
        public const string PrimeraMision = "Primera misión";
        public const string Veterano = "Veterano (nivel 3)";
        public const string Ahorrista = "Ahorrista (150 monedas)";

        // En este orden se muestran en el panel del Hub.
        public static readonly Logro[] Todos =
        {
            new Logro("Builder", Builder, "Ensamblá una nave en el Astillero."),
            new Logro("Prototype", Prototype, "Cloná un tripulante en el Laboratorio."),
            new Logro("Command", Command, "Deshacé una acción."),
            new Logro("Strategy", Strategy, "Cambiale la IA a uno o a todos los enemigos."),
            new Logro("Template Method", TemplateMethod, "Terminá una oleada."),
            new Logro("Facade", Facade, "Usá el modo emergencia del Control central."),
            new Logro("Flyweight", Flyweight, "Mandá tripulantes a un subsistema."),
            new Logro("State", State, "Hacé que el héroe quede Enfurecido."),
            new Logro("Chain of Responsibility", ChainOfResponsibility, "Cambiá el orden de la cadena de defensas."),
            new Logro("Visitor", Visitor, "Equipá un ítem del inventario."),
            new Logro("Proxy", Proxy, "Comprá una mejora en la tienda del Hub."),
            new Logro("Bridge", Bridge, "Mandá un mensaje cifrado (AES o Cuántico) a la Tierra."),
            new Logro(PatronProgreso, PrimeraMision, "Ganá XP por primera vez."),
            new Logro(PatronProgreso, Veterano, "Llegá al nivel 3."),
            new Logro(PatronProgreso, Ahorrista, "Juntá 150 monedas en la Bóveda.")
        };
    }
}
