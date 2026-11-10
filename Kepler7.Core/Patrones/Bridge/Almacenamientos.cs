namespace Kepler7.Core.Patrones.Bridge
{
    // Una sola instancia de cada almacenamiento para toda la ejecución (igual que EstrategiasIA).
    // Así, lo que se guardó en Memoria sigue ahí aunque se cierre y se vuelva a abrir la ventana del Hub.
    public static class Almacenamientos
    {
        public static readonly IAlmacenamiento Archivo = new AlmacenamientoArchivo();
        public static readonly IAlmacenamiento Memoria = new AlmacenamientoMemoria();

        // Con esta lista se llena el combo de la ventana del Hub.
        public static readonly IAlmacenamiento[] Todos = { Archivo, Memoria };
    }
}
