namespace Kepler7.Core.Modelo
{
    // Una pieza de equipo de un tripulante dentro de una partida guardada.
    public class EquipoGuardado
    {
        public string Nombre { get; set; } = "";
        public int BonusDanio { get; set; }
        public int BonusVida { get; set; }
        public int Costo { get; set; }
    }
}
