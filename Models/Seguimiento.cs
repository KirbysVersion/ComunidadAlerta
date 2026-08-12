using System;

namespace ComunidadAlertaApp.Models
{
    /// <summary>
    /// Representa un avance, actualización o evidencia dentro de un reporte ciudadano.
    /// Aplica el principio de Encapsulamiento.
    /// </summary>
    public class Seguimiento
    {
        // Propiedad que almacena la fecha y hora exacta en que se registró el avance.
        // { get; private set; } permite leer la fecha desde fuera, pero solo asignarla dentro de esta clase.
        public DateTime Fecha { get; private set; }

        // Propiedad para el comentario o descripción del avance.
        public string Comentario { get; private set; }

        // Propiedad que guarda la ruta local de la imagen de evidencia (si aplica).
        public string RutaFotografiaEvidencia { get; private set; }

        // Propiedad que indica quién realizó el seguimiento (ej. "Ciudadano", "Técnico de Servicios").
        public string Autor { get; private set; }

        /// <summary>
        /// Constructor de la clase Seguimiento.
        /// Inicializa y valida los datos obligatorios.
        /// </summary>
        /// <param name="comentario">Detalle del avance registrado.</param>
        /// <param name="autor">Persona o funcionario que registra el avance.</param>
        /// <param name="rutaFotografiaEvidencia">Ruta opcional de una foto de evidencia.</param>
        public Seguimiento(string comentario, string autor, string rutaFotografiaEvidencia = "")
        {
            // Validación de campo obligatorio para evitar datos vacíos (Manejo de defensiva de datos)
            if (string.IsNullOrWhiteSpace(comentario))
            {
                throw new ArgumentException("El comentario de seguimiento no puede estar vacío.", nameof(comentario));
            }

            // Asignación de la fecha actual al momento de instanciar el objeto.
            this.Fecha = DateTime.Now;

            // Trim() limpia espacios en blanco al inicio y al final.
            this.Comentario = comentario.Trim();
            this.Autor = string.IsNullOrWhiteSpace(autor) ? "Anónimo" : autor.Trim();
            this.RutaFotografiaEvidencia = rutaFotografiaEvidencia;
        }

        /// <summary>
        /// Sobrescribe el método ToString() para facilitar la presentación del seguimiento en controles UI como ListBox.
        /// </summary>
        public override string ToString()
        {
            // Formatea la fecha para mostrarla de manera legible junto con el autor y comentario.
            return $"[{Fecha:dd/MM/yyyy HH:mm}] ({Autor}): {Comentario}";
        }
    }
}