namespace Kepler7.Core.Modelo
{
    // Un ítem del inventario dentro de una partida guardada. Lo arma el VisitanteGuardado.
    public class ItemGuardado
    {
        // "Arma", "Armadura" o "Modulo".
        public string Tipo { get; set; } = "";
        public string Nombre { get; set; } = "";

        // Daño extra para un arma, vida extra para una armadura.
        public int Bonus { get; set; }

        // Solo para los módulos: qué defensa agrega ("Blindaje", "Escudo" o "Evasion").
        public string Defensa { get; set; } = "";
    }
}
