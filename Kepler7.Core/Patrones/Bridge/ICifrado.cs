namespace Kepler7.Core.Patrones.Bridge
{
    // Patrón Bridge (comunicaciones con la Tierra): esta es la "implementación", es decir, CÓMO viaja
    // protegido el texto. No sabe qué tipo de mensaje está cifrando: eso lo decide la otra mitad del
    // puente (Mensaje). Así 3 mensajes x 3 cifrados son 6 clases y no 9.
    public interface ICifrado
    {
        string Nombre { get; }
        string Descripcion { get; }

        string Cifrar(string texto);

        // Lo que hace la estación receptora en la Tierra para leer el mensaje.
        string Descifrar(string cifrado);
    }
}
