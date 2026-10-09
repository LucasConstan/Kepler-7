namespace Kepler_7.Patrones.Builder
{
    /// <summary>
    /// BUILDER — Director. Conoce el orden de ensamblaje, pero no los detalles de cada receta:
    /// el mismo proceso produce naves distintas según el builder que recibe.
    /// </summary>
    public class Astillero
    {
        public NaveApoyo Ensamblar(INaveApoyoBuilder builder)
        {
            builder.Reiniciar();
            builder.ConstruirCasco();
            builder.InstalarMotor();
            builder.MontarArmamento();
            builder.InstalarModuloEspecial();
            return builder.ObtenerResultado();
        }
    }
}
