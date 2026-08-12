// Importación de librerías base de System y manipulación de gráficos/UI
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
// Importación del modelo de datos de nuestro proyecto
using ComunidadAlertaApp.Models;

namespace ComunidadAlertaApp.Forms
{
    /// <summary>
    /// Formulario secundario para visualizar métricas, conteos y estadísticas globales del sistema.
    /// </summary>
    public class FormEstadisticas : Form
    {
        // Referencia local a la lista de reportes recibida desde el FormPrincipal
        private readonly List<Reporte> _reportes;

        // Controles de la interfaz gráfica
        private Label lblTotalReportes;
        private ListBox lstEstadisticasEstado;
        private ListBox lstEstadisticasPrioridad;
        private Label lblDistribucionTipo;
        private Button btnCerrar;

        /// <summary>
        /// Constructor que recibe la colección actual de reportes para procesar sus estadísticas.
        /// </summary>
        /// <param name="reportes">Lista de objetos tipo Reporte.</param>
        public FormEstadisticas(List<Reporte> reportes)
        {
            // Asignación de la lista (o lista vacía si es nula)
            this._reportes = reportes ?? new List<Reporte>();

            // Configuración de la ventana emergente
            this.Text = "Estadísticas y Métricas del Sistema - Comunidad Alerta";
            this.Size = new Size(600, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Construcción programática de los controles
            ConstruirInterfazGrafica();

            // Ejecución del cálculo de métricas
            CalcularYMostrarEstadisticas();
        }

        /// <summary>
        /// Diseña los paneles y listas donde se presentarán los datos estadísticos.
        /// </summary>
        private void ConstruirInterfazGrafica()
        {
            // Etiqueta destacada para el Total de Reportes
            lblTotalReportes = new Label
            {
                Text = "Total de Reportes Registrados: 0",
                Location = new Point(20, 20),
                AutoSize = true,
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.DarkBlue
            };

            // Grupo 1: Estado de los reportes
            GroupBox grpEstado = new GroupBox
            {
                Text = " Reportes por Estado ",
                Location = new Point(20, 60),
                Size = new Size(260, 200),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
            };

            lstEstadisticasEstado = new ListBox
            {
                Location = new Point(15, 25),
                Size = new Size(230, 150),
                Font = new Font("Segoe UI", 9f, FontStyle.Regular)
            };
            grpEstado.Controls.Add(lstEstadisticasEstado);

            // Grupo 2: Prioridad de los reportes
            GroupBox grpPrioridad = new GroupBox
            {
                Text = " Reportes por Nivel de Prioridad ",
                Location = new Point(300, 60),
                Size = new Size(260, 200),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
            };

            lstEstadisticasPrioridad = new ListBox
            {
                Location = new Point(15, 25),
                Size = new Size(230, 150),
                Font = new Font("Segoe UI", 9f, FontStyle.Regular)
            };
            grpPrioridad.Controls.Add(lstEstadisticasPrioridad);

            // Resumen de categorías
            lblDistribucionTipo = new Label
            {
                Text = "Categorías: 0 Infraestructura | 0 Medio Ambiente",
                Location = new Point(20, 280),
                AutoSize = true,
                Font = new Font("Segoe UI", 10f, FontStyle.Italic)
            };

            // Botón para cerrar el diálogo
            btnCerrar = new Button
            {
                Text = "Cerrar Ventana",
                Location = new Point(220, 390),
                Size = new Size(150, 40),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
            };
            btnCerrar.Click += (s, e) => this.Close();

            // Agregar controles a la ventana
            this.Controls.AddRange(new Control[] { lblTotalReportes, grpEstado, grpPrioridad, lblDistribucionTipo, btnCerrar });
        }

        /// <summary>
        /// Procesa la lista de reportes utilizando LINQ para obtener conteos y porcentajes.
        /// </summary>
        private void CalcularYMostrarEstadisticas()
        {
            int total = _reportes.Count;
            lblTotalReportes.Text = $"Total de Reportes Registrados: {total}";

            if (total == 0)
            {
                lstEstadisticasEstado.Items.Add("No hay datos para procesar.");
                lstEstadisticasPrioridad.Items.Add("No hay datos para procesar.");
                return;
            }

            // 1. Estadísticas por Estado
            lstEstadisticasEstado.Items.Clear();
            foreach (EstadoReporte estado in Enum.GetValues(typeof(EstadoReporte)))
            {
                int cantidad = _reportes.Count(r => r.Estado == estado);
                double porcentaje = (double)cantidad / total * 100;
                lstEstadisticasEstado.Items.Add($"{estado}: {cantidad} ({porcentaje:F1}%)");
            }

            // 2. Estadísticas por Prioridad
            lstEstadisticasPrioridad.Items.Clear();
            foreach (NivelPrioridad prioridad in Enum.GetValues(typeof(NivelPrioridad)))
            {
                int cantidad = _reportes.Count(r => r.Prioridad == prioridad);
                double porcentaje = (double)cantidad / total * 100;
                lstEstadisticasPrioridad.Items.Add($"{prioridad}: {cantidad} ({porcentaje:F1}%)");
            }

            // 3. Conteo por Tipo Concreto (Polimorfismo)
            int totalInfraestructura = _reportes.Count(r => r is ReporteInfraestructura);
            int totalMedioAmbiente = _reportes.Count(r => r is ReporteMedioAmbiente);

            lblDistribucionTipo.Text = $"Distribución: {totalInfraestructura} Infraestructura | {totalMedioAmbiente} Medio Ambiente";
        }
    }
}