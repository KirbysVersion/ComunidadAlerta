// Importación de espacios de nombres base
using System;

// Definición del espacio de nombres para la capa de servicios del proyecto
namespace ComunidadAlertaApp.Services
{
    /// <summary>
    /// Interfaz genérica que define el contrato de almacenamiento para el sistema.
    /// Aplica el principio de INTERFACES y Abstracción.
    /// T representa el tipo de datos que se va a guardar o cargar (ej. List<Reporte>).
    /// </summary>
    /// <typeparam name="T">Tipo de dato o colección a persistir.</typeparam>
    public interface IGestorPersistencia<T>
    {
        /// <summary>
        /// Método para guardar datos en un archivo.
        /// </summary>
        /// <param name="datos">Objeto o lista de objetos a guardar.</param>
        /// <param name="rutaArchivo">Ruta completa del archivo de destino.</param>
        void Guardar(T datos, string rutaArchivo);

        /// <summary>
        /// Método para recuperar/cargar datos desde un archivo.
        /// </summary>
        /// <param name="rutaArchivo">Ruta completa del archivo a leer.</param>
        /// <returns>Regresa el objeto deserializado del tipo T.</returns>
        T Cargar(string rutaArchivo);
    }
}