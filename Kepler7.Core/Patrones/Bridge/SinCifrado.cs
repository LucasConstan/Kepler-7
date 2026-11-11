namespace Kepler7.Core.Patrones.Bridge
{
    // El texto viaja tal cual: cualquiera que escuche la frecuencia lo puede leer.
    public class SinCifrado : ICifrado
    {
        public string Nombre
        {
            get { return "Sin cifrado"; }
        }

        public string Descripcion
        {
            get { return "El texto viaja en claro. Rápido, pero cualquiera lo puede leer."; }
        }

        public string Cifrar(string texto)
        {
            return texto;
        }

        public string Descifrar(string cifrado)
        {
            return cifrado;
        }
    }
}
