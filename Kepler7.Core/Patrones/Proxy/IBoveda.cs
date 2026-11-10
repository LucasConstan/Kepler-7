namespace Kepler7.Core.Patrones.Proxy
{
    // Patrón Proxy: la Bóveda real y su proxy tienen las mismas operaciones,
    // así quien la usa no sabe (ni le importa) si habla con una o con la otra.
    public interface IBoveda
    {
        int ConsultarSaldo();

        // Los dos devuelven false si la operación no se hizo.
        bool Depositar(int cantidad);
        bool Retirar(int cantidad);
    }
}
