namespace Kepler7.Core.Patrones.Strategy
{
    // Las estrategias no guardan datos propios, así que alcanza con una sola instancia de cada una
    // y todos los enemigos pueden compartirla.
    public static class EstrategiasIA
    {
        public static readonly IEstrategiaIA Agresiva = new IAAgresiva();
        public static readonly IEstrategiaIA Defensiva = new IADefensiva();
        public static readonly IEstrategiaIA Aleatoria = new IAAleatoria();
        public static readonly IEstrategiaIA Calculadora = new IACalculadora();

        // Con esta lista se llena el combo de IA en la pantalla principal.
        public static readonly IEstrategiaIA[] Todas = { Agresiva, Defensiva, Aleatoria, Calculadora };
    }
}
