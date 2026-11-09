namespace Kepler7.Core.Patrones.Builder
{
    // Patrón Builder: estos son los pasos para armar una nave.
    // Cada receta (Exploradora, Combate, Carga) los hace a su manera.
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
