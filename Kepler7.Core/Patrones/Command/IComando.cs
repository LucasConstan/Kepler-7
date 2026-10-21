namespace Kepler7.Core.Patrones.Command
{
    /// <summary>
    /// COMMAND - Interfaz del comando. Cada acción del jugador es un objeto que sabe
    /// ejecutarse y deshacerse. Los dos métodos devuelven false si la acción no se pudo hacer
    /// (por ejemplo, si faltan créditos), así el gestor no la guarda en el historial.
    /// </summary>
    public interface IComando
    {
        string Descripcion { get; }

        bool Ejecutar();
        bool Deshacer();
    }
}
