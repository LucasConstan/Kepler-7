namespace Kepler7.Core.Patrones.Bridge
{
    // Patrón Bridge: esta es la "implementación", es decir, DÓNDE se guardan los datos.
    // Solo sabe guardar y leer texto con un nombre (clave). No sabe qué es un progreso ni un log:
    // eso lo decide la otra mitad del puente (Persistencia).
    public interface IAlmacenamiento
    {
        string Nombre { get; }
        string Descripcion { get; }

        void Guardar(string clave, string contenido);

        // Devuelve null si con esa clave todavía no se guardó nada.
        string? Cargar(string clave);

        // Si con esa clave no hay nada, no hace nada.
        void Borrar(string clave);
    }
}
