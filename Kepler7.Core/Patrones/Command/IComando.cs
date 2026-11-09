namespace Kepler7.Core.Patrones.Command
{
    // Patrón Command: cada acción del jugador es un objeto que sabe ejecutarse y deshacerse.
    // Los dos métodos devuelven false si no se pudo (por ejemplo, si faltan créditos),
    // y en ese caso el gestor no la guarda como hecha.
    public interface IComando
    {
        string Descripcion { get; }

        bool Ejecutar();
        bool Deshacer();
    }
}
