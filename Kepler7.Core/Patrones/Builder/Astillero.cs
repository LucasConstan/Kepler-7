namespace Kepler7.Core.Patrones.Builder
{
    // El director del Builder: sabe en qué orden se arma una nave, pero no los detalles de cada receta.
    // Con el mismo proceso salen naves distintas según el builder que recibe.
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
