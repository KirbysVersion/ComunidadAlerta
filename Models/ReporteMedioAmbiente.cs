// Importación del espacio de nombres System
using System;

namespace ComunidadAlertaApp.Models
{
    /// <summary>
    /// Enumeración específica para clasificar el tipo de incidente ambiental.
    /// </summary>
    public enum TipoIncidenteAmbiental
    {
        FugaAguaPotable,
        FugaDrenaje,
        BasuraAcumulada,
        ArbolCaidoORiesgo,
        ContaminacionAuditiva
    }

    /// <summary>
    /// Representa reportes ambientales (fugas de agua, focos de infección, arbolado).
    /// Aplica el principio de HERENCIA.
    /// </summary>
    public class ReporteMedioAmbiente : Reporte
    {
        // Clasificación del incidente ambiental
        public TipoIncidenteAmbiental TipoIncidente { get; set; }

        // Estimación de días que lleva el problema activo sin atender
        public int DiasAcumulados { get; set; }

        /// <summary>
        /// Constructor de la clase ReporteMedioAmbiente.
        /// Llama al constructor base mediante 'base'.
        /// </summary>
        public ReporteMedioAmbiente(
            string folio,
            string titulo,
            string descripcion,
            string ubicacion,
            TipoIncidenteAmbiental tipoIncidente,
            int diasAcumulados,
            string rutaFotografia = "")
            : base(folio, titulo, descripcion, ubicacion, rutaFotografia) // Invocación del constructor de Reporte
        {
            // Asignación de las propiedades específicas de la problemática ambiental
            this.TipoIncidente = tipoIncidente;

            // Si pasan un número negativo de días, lo corregimos a 0 para evitar incongruencias
            this.DiasAcumulados = diasAcumulados < 0 ? 0 : diasAcumulados;

            // Invocamos el cálculo automático de prioridad
            CalcularPrioridad();
        }

        /// <summary>
        /// Implementación polimórfica del cálculo de prioridad para problemas ambientales.
        /// Aplica POLIMORFISMO mediante 'override'.
        /// </summary>
        public override void CalcularPrioridad()
        {
            // Regla de negocio para Medio Ambiente:
            // 1. Las fugas de agua potable o drenaje son emergencias prioritarias -> CRÍTICA
            if (TipoIncidente == TipoIncidenteAmbiental.FugaAguaPotable || TipoIncidente == TipoIncidenteAmbiental.FugaDrenaje)
            {
                this.Prioridad = NivelPrioridad.Critica;
            }
            // 2. Si lleva más de 5 días sin solución -> Prioridad ALTA
            else if (DiasAcumulados >= 5)
            {
                this.Prioridad = NivelPrioridad.Alta;
            }
            // 3. Si lleva entre 2 y 4 días -> Prioridad MEDIA
            else if (DiasAcumulados >= 2)
            {
                this.Prioridad = NivelPrioridad.Media;
            }
            // 4. Caso de baja urgencia inicial -> Prioridad BAJA
            else
            {
                this.Prioridad = NivelPrioridad.Baja;
            }
        }
    }
}