// Importación del espacio de nombres System para excepciones y tipos base
using System;

// Espacio de nombres correspondiente a la carpeta Models
namespace ComunidadAlertaApp.Models
{
    /// <summary>
    /// Representa reportes de infraestructura pública (baches, luminarias, banquetas, puentes).
    /// Aplica el principio de HERENCIA al derivar de la clase Reporte.
    /// </summary>
    public class ReporteInfraestructura : Reporte
    {
        // Indica si el fallo se encuentra en una avenida principal o vía de alto tráfico
        public bool EsViaPrincipal { get; set; }

        // Indica si la falla representa un peligro físico inminente (ejemplo: un bache muy profundo o un poste a punto de caer)
        public bool RepresentaPeligroInminente { get; set; }

        /// <summary>
        /// Constructor para inicializar un reporte de infraestructura.
        /// Llama al constructor base de la clase Reporte mediante la palabra clave 'base'.
        /// </summary>
        /// <param name="folio">Folio único asignado.</param>
        /// <param name="titulo">Título corto del problema.</param>
        /// <param name="descripcion">Explicación detallada.</param>
        /// <param name="ubicacion">Dirección física del evento.</param>
        /// <param name="esViaPrincipal">Verdadero si está en vía principal.</param>
        /// <param name="representaPeligroInminente">Verdadero si genera peligro inmediato.</param>
        /// <param name="rutaFotografia">Fotografía de evidencia opcional.</param>
        public ReporteInfraestructura(
            string folio,
            string titulo,
            string descripcion,
            string ubicacion,
            bool esViaPrincipal,
            bool representaPeligroInminente,
            string rutaFotografia = "")
            : base(folio, titulo, descripcion, ubicacion, rutaFotografia) // Llama al constructor protegido de Reporte
        {
            // Asignación de propiedades específicas de esta clase hija
            this.EsViaPrincipal = esViaPrincipal;
            this.RepresentaPeligroInminente = representaPeligroInminente;

            // Al crear el objeto, calculamos de forma automática su prioridad inicial
            CalcularPrioridad();
        }

        /// <summary>
        /// Implementación polimórfica del método abstracto CalcularPrioridad.
        /// Aplica POLIMORFISMO mediante la palabra clave 'override'.
        /// </summary>
        public override void CalcularPrioridad()
        {
            // Regla de negocio para Infraestructura:
            // 1. Si representa peligro inminente y además es en vía principal -> Prioridad CRÍTICA
            if (RepresentaPeligroInminente && EsViaPrincipal)
            {
                this.Prioridad = NivelPrioridad.Critica;
            }
            // 2. Si representa peligro inminente O es en vía principal -> Prioridad ALTA
            else if (RepresentaPeligroInminente || EsViaPrincipal)
            {
                this.Prioridad = NivelPrioridad.Alta;
            }
            // 3. Caso estándar -> Prioridad MEDIA
            else
            {
                this.Prioridad = NivelPrioridad.Media;
            }
        }
    }
}