// Importación del espacio de nombres System para tipos básicos como DateTime y ArgumentException
using System;
// Importación de Collections.Generic para trabajar con listas de objetos (List<T>, IReadOnlyCollection<T>)
using System.Collections.Generic;

// Definición del espacio de nombres que organiza la carpeta de modelos del proyecto
namespace ComunidadAlertaApp.Models
{
    /// <summary>
    /// Enumeración que representa los estados por los que atraviesa un reporte ciudadano.
    /// </summary>
    public enum EstadoReporte
    {
        Registrado,  // Estado inicial del reporte
        EnRevision,  // El reporte está siendo analizado por la autoridad
        EnProceso,   // Se están ejecutando acciones para resolver el problema
        Resuelto,    // La problemática fue atendida exitosamente
        Cerrado      // El ciudadano o sistema dio por concluido el caso
    }

    /// <summary>
    /// Enumeración para clasificar el nivel de prioridad del reporte.
    /// </summary>
    public enum NivelPrioridad
    {
        Baja,     // Atención no urgente
        Media,    // Atención normal
        Alta,     // Requiere atención prioritaria
        Critica   // Emergencia o riesgo inminente
    }

    /// <summary>
    /// Clase abstracta base que sirve de plantilla para todos los tipos de reportes.
    /// Aplica Abstracción y Encapsulamiento.
    /// </summary>
    public abstract class Reporte
    {
        // Propiedad que almacena la clave única del reporte (ej. "RPT-001")
        // 'private set' garantiza que el folio solo se establezca en el constructor
        public string Folio { get; private set; }

        // Propiedad para el título o nombre corto de la incidencia
        public string Titulo { get; set; }

        // Propiedad con la descripción detallada del problema reportado
        public string Descripcion { get; set; }

        // Propiedad que indica la ubicación física o dirección del incidente
        public string Ubicacion { get; set; }

        // Propiedad que guarda automáticamente la fecha y hora de creación
        public DateTime FechaRegistro { get; private set; }

        // Propiedad para almacenar la ruta local del archivo de imagen
        public string RutaFotografia { get; set; }

        // Propiedad que indica el estado actual del reporte en la máquina de estados
        public EstadoReporte Estado { get; private set; }

        // Propiedad para el nivel de prioridad. 
        // CORRECCIÓN: 'protected set' permite que las clases hijas asignen el valor al calcular la prioridad.
        public NivelPrioridad Prioridad { get; protected set; }

        // Campo privado que almacena la lista interna de seguimientos (Composición)
        private readonly List<Seguimiento> _historialSeguimiento;

        // Propiedad de solo lectura para exponer los seguimientos sin permitir modificación directa
        public IReadOnlyCollection<Seguimiento> HistorialSeguimiento => _historialSeguimiento.AsReadOnly();

        /// <summary>
        /// Constructor protegido para inicializar los datos obligatorios del reporte.
        /// </summary>
        /// <param name="folio">Identificador del reporte.</param>
        /// <param name="titulo">Título de la problemática.</param>
        /// <param name="descripcion">Detalle de la falla.</param>
        /// <param name="ubicacion">Dirección de la incidencia.</param>
        /// <param name="rutaFotografia">Ruta de la imagen de evidencia opcional.</param>
        protected Reporte(string folio, string titulo, string descripcion, string ubicacion, string rutaFotografia = "")
        {
            // Validación de datos obligatorios usando excepciones
            if (string.IsNullOrWhiteSpace(folio))
                throw new ArgumentException("El folio no puede ser nulo o estar vacío.", nameof(folio));

            if (string.IsNullOrWhiteSpace(titulo))
                throw new ArgumentException("El título no puede estar vacío.", nameof(titulo));

            // Asignación de propiedades encapsuladas
            this.Folio = folio.Trim();
            this.Titulo = titulo.Trim();
            this.Descripcion = descripcion;
            this.Ubicacion = ubicacion;
            this.RutaFotografia = rutaFotografia;
            this.FechaRegistro = DateTime.Now;

            // Inicialización del estado por defecto
            this.Estado = EstadoReporte.Registrado;

            // Inicialización de la lista contenedora de seguimientos
            this._historialSeguimiento = new List<Seguimiento>();
        }

        /// <summary>
        /// Método abstracto que cada tipo específico de reporte debe implementar para calcular su prioridad.
        /// Aplica el principio de Polimorfismo.
        /// </summary>
        public abstract void CalcularPrioridad();

        /// <summary>
        /// Permite actualizar el estado del reporte siguiendo el flujo de trabajo.
        /// </summary>
        /// <param name="nuevoEstado">El nuevo estado asignado.</param>
        public void CambiarEstado(EstadoReporte nuevoEstado)
        {
            this.Estado = nuevoEstado;
        }

        /// <summary>
        /// Agrega un nuevo seguimiento a la lista interna. Aplica Composición.
        /// </summary>
        /// <param name="nuevoSeguimiento">Objeto de clase Seguimiento.</param>
        public void AgregarSeguimiento(Seguimiento nuevoSeguimiento)
        {
            if (nuevoSeguimiento == null)
            {
                throw new ArgumentNullException(nameof(nuevoSeguimiento), "El seguimiento no puede ser nulo.");
            }

            // Se añade el seguimiento a la lista privada del reporte
            this._historialSeguimiento.Add(nuevoSeguimiento);
        }
    }
}