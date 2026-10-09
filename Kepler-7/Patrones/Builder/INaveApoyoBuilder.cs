namespace Kepler_7.Patrones.Builder
{
    /// <summary>
    /// BUILDER — Interfaz del constructor. Define los pasos para ensamblar una nave de apoyo;
    /// cada receta (Exploradora, Combate, Carga) los implementa a su manera.
    /// </summary>
    public interface INaveApoyoBuilder
    {
        string NombreReceta { get; }
        string DescripcionReceta { get; }
        int Costo { get; }

        void Reiniciar();
        void ConstruirCasco();
        void InstalarMotor();
        void MontarArmamento();
        void InstalarModuloEspecial();
        NaveApoyo ObtenerResultado();
    }
}
