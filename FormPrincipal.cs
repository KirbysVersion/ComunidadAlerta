using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ComunidadAlertaApp.Models;
using ComunidadAlertaApp.Services;

namespace ComunidadAlertaApp.Forms
{
    /// <summary>
    /// Formulario Principal del sistema Comunidad Alerta.
    /// Administra la interfaz gráfica y la interacción con el usuario.
    /// </summary>
    public class FormPrincipal : Form
    {
        // =========================================================
        // SERVICIOS
        // =========================================================

        private readonly ServicioPdf _servicioPdf =
            new ServicioPdf();

        // =========================================================
        // PERSISTENCIA
        // =========================================================

        private readonly IGestorPersistencia<List<Reporte>>
            _gestorPersistencia;

        private readonly string _rutaArchivoJson =
            Path.Combine(
                Application.StartupPath,
                "reportes_comunitarios.json");

        // =========================================================
        // DATOS
        // =========================================================

        private List<Reporte> _listaReportes;

        // =========================================================
        // CONTROLES
        // =========================================================

        private DataGridView gridReportes;

        private ComboBox comboTipoReporte;
        private ComboBox cmbPrioridad;

        private TextBox txtTitulo;
        private TextBox txtDescripcion;
        private TextBox txtUbicacion;

        private CheckBox chkViaPrincipal;
        private CheckBox chkPeligroInminente;

        private ComboBox comboTipoAmbiental;
        private NumericUpDown numDiasAcumulados;

        private Button btnRegistrar;
        private Button btnCargarFoto;
        private Button btnExportarPdf;

        private Label lblRutaFoto;

        private ComboBox comboFiltroEstado;
        private TextBox txtBuscarFolio;

        private Button btnBuscar;
        private Button btnLimpiarFiltros;

        private Button btnCambiarEstado;
        private ComboBox comboNuevoEstado;

        private Button btnAgregarSeguimiento;
        private TextBox txtComentarioSeguimiento;

        private ListBox lstHistorialSeguimiento;

        private Panel panelControlesEspecificos;

        // =========================================================
        // FOTO
        // =========================================================

        private string _rutaFotoSeleccionada = "";

        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public FormPrincipal()
        {
            _gestorPersistencia =
                new GestorJson();

            _listaReportes =
                new List<Reporte>();

            Text =
                "Comunidad Alerta - Sistema Ciudadano de Reportes";

            Size =
                new Size(1100, 750);

            StartPosition =
                FormStartPosition.CenterScreen;

            ConstruirInterfazGrafica();

            CargarReportesGuardados();
        }

        // =========================================================
        // CONSTRUIR INTERFAZ
        // =========================================================

        private void ConstruirInterfazGrafica()
        {
            // =====================================================
            // GRUPO DE REGISTRO
            // =====================================================

            GroupBox grpRegistro =
                new GroupBox
                {
                    Text = " Registrar Nuevo Reporte ",
                    Location = new Point(15, 15),
                    Size = new Size(520, 380),
                    Font = new Font(
                        "Segoe UI",
                        9.5f,
                        FontStyle.Bold)
                };

            // -----------------------------------------------------
            // TIPO DE REPORTE
            // -----------------------------------------------------

            Label lblTipo =
                new Label
                {
                    Text = "Tipo de Reporte:",
                    Location = new Point(15, 30),
                    AutoSize = true,
                    Font = new Font(
                        "Segoe UI",
                        9f)
                };

            comboTipoReporte =
                new ComboBox
                {
                    Location = new Point(140, 27),
                    Width = 350,
                    DropDownStyle =
                        ComboBoxStyle.DropDownList,
                    Font = new Font(
                        "Segoe UI",
                        9f)
                };

            comboTipoReporte.Items.AddRange(
                new string[]
                {
                    "Infraestructura (Baches, Luminarias, etc.)",
                    "Medio Ambiente (Fugas, Basura, etc.)"
                });

            comboTipoReporte.SelectedIndex = 0;

            comboTipoReporte.SelectedIndexChanged +=
                ComboTipoReporte_SelectedIndexChanged;

            // -----------------------------------------------------
            // TÍTULO
            // -----------------------------------------------------

            Label lblTitulo =
                new Label
                {
                    Text = "Título:",
                    Location = new Point(15, 65),
                    AutoSize = true,
                    Font = new Font(
                        "Segoe UI",
                        9f)
                };

            txtTitulo =
                new TextBox
                {
                    Location = new Point(140, 62),
                    Width = 350,
                    Font = new Font(
                        "Segoe UI",
                        9f)
                };

            // -----------------------------------------------------
            // UBICACIÓN
            // -----------------------------------------------------

            Label lblUbicacion =
                new Label
                {
                    Text = "Ubicación:",
                    Location = new Point(15, 100),
                    AutoSize = true,
                    Font = new Font(
                        "Segoe UI",
                        9f)
                };

            txtUbicacion =
                new TextBox
                {
                    Location = new Point(140, 97),
                    Width = 350,
                    Font = new Font(
                        "Segoe UI",
                        9f)
                };

            // -----------------------------------------------------
            // DESCRIPCIÓN
            // -----------------------------------------------------

            Label lblDesc =
                new Label
                {
                    Text = "Descripción:",
                    Location = new Point(15, 135),
                    AutoSize = true,
                    Font = new Font(
                        "Segoe UI",
                        9f)
                };

            txtDescripcion =
                new TextBox
                {
                    Location = new Point(140, 132),
                    Width = 350,
                    Height = 50,
                    Multiline = true,
                    Font = new Font(
                        "Segoe UI",
                        9f)
                };

            // =====================================================
            // PRIORIDAD
            // =====================================================

            Label lblPrioridad =
                new Label
                {
                    Text = "Prioridad:",
                    Location = new Point(15, 215),
                    AutoSize = true,
                    Font = new Font(
                        "Segoe UI",
                        8.5f)
                };

            cmbPrioridad =
                new ComboBox
                {
                    Location = new Point(80, 212),
                    Size = new Size(140, 25),
                    DropDownStyle =
                        ComboBoxStyle.DropDownList,
                    Font = new Font(
                        "Segoe UI",
                        8.5f)
                };

            cmbPrioridad.DataSource =
                Enum.GetValues(
                    typeof(NivelPrioridad));

            // =====================================================
            // PANEL DE OPCIONES ESPECÍFICAS
            // =====================================================

            panelControlesEspecificos =
                new Panel
                {
                    Location = new Point(15, 250),
                    Size = new Size(475, 70)
                };

            // -----------------------------------------------------
            // INFRAESTRUCTURA
            // -----------------------------------------------------

            chkViaPrincipal =
                new CheckBox
                {
                    Text =
                        "¿Es en vía principal / avenida?",
                    Location = new Point(5, 5),
                    AutoSize = true,
                    Font = new Font(
                        "Segoe UI",
                        8.5f)
                };

            chkPeligroInminente =
                new CheckBox
                {
                    Text =
                        "¿Representa peligro inminente?",
                    Location = new Point(5, 35),
                    AutoSize = true,
                    Font = new Font(
                        "Segoe UI",
                        8.5f)
                };

            // -----------------------------------------------------
            // MEDIO AMBIENTE
            // -----------------------------------------------------

            Label lblTipoAmb =
                new Label
                {
                    Text = "Incidente:",
                    Location = new Point(5, 8),
                    AutoSize = true,
                    Visible = false,
                    Font = new Font(
                        "Segoe UI",
                        8.5f)
                };

            comboTipoAmbiental =
                new ComboBox
                {
                    Location = new Point(80, 5),
                    Width = 200,
                    DropDownStyle =
                        ComboBoxStyle.DropDownList,
                    Visible = false,
                    Font = new Font(
                        "Segoe UI",
                        8.5f)
                };

            comboTipoAmbiental.DataSource =
                Enum.GetValues(
                    typeof(TipoIncidenteAmbiental));

            Label lblDias =
                new Label
                {
                    Text = "Días activo:",
                    Location = new Point(290, 8),
                    AutoSize = true,
                    Visible = false,
                    Font = new Font(
                        "Segoe UI",
                        8.5f)
                };

            numDiasAcumulados =
                new NumericUpDown
                {
                    Location = new Point(360, 5),
                    Width = 60,
                    Minimum = 0,
                    Maximum = 365,
                    Visible = false,
                    Font = new Font(
                        "Segoe UI",
                        8.5f)
                };

            panelControlesEspecificos.Controls.Add(
                chkViaPrincipal);

            panelControlesEspecificos.Controls.Add(
                chkPeligroInminente);

            panelControlesEspecificos.Controls.Add(
                lblTipoAmb);

            panelControlesEspecificos.Controls.Add(
                comboTipoAmbiental);

            panelControlesEspecificos.Controls.Add(
                lblDias);

            panelControlesEspecificos.Controls.Add(
                numDiasAcumulados);

            // =====================================================
            // FOTO
            // =====================================================

            btnCargarFoto =
                new Button
                {
                    Text = "Adjuntar Foto",
                    Location = new Point(15, 325),
                    Width = 110,
                    Height = 30,
                    Font = new Font(
                        "Segoe UI",
                        8.5f)
                };

            btnCargarFoto.Click +=
                BtnCargarFoto_Click;

            lblRutaFoto =
                new Label
                {
                    Text = "Sin foto seleccionada",
                    Location = new Point(135, 333),
                    AutoSize = true,
                    Font = new Font(
                        "Segoe UI",
                        8f,
                        FontStyle.Italic)
                };

            // =====================================================
            // REGISTRAR
            // =====================================================

            btnRegistrar =
                new Button
                {
                    Text =
                        "Guardar y Registrar Reporte",
                    Location = new Point(15, 360),
                    Width = 475,
                    Height = 40,
                    BackColor = Color.LightSkyBlue,
                    Font = new Font(
                        "Segoe UI",
                        9.5f,
                        FontStyle.Bold)
                };

            btnRegistrar.Click +=
                BtnRegistrar_Click;

            grpRegistro.Controls.AddRange(
                new Control[]
                {
                    lblTipo,
                    comboTipoReporte,
                    lblTitulo,
                    txtTitulo,
                    lblUbicacion,
                    txtUbicacion,
                    lblDesc,
                    txtDescripcion,
                    lblPrioridad,
                    cmbPrioridad,
                    panelControlesEspecificos,
                    btnCargarFoto,
                    lblRutaFoto,
                    btnRegistrar
                });

            // =====================================================
            // LISTADO DE REPORTES
            // =====================================================

            GroupBox grpListado =
                new GroupBox
                {
                    Text = " Reportes Registrados ",
                    Location = new Point(550, 15),
                    Size = new Size(515, 380),
                    Font = new Font(
                        "Segoe UI",
                        9.5f,
                        FontStyle.Bold)
                };

            // -----------------------------------------------------
            // ESTADÍSTICAS
            // -----------------------------------------------------

            Button btnVerEstadisticas =
                new Button
                {
                    Text = "📊 Estadísticas",
                    Location = new Point(412, 325),
                    Width = 85,
                    Height = 30,
                    BackColor = Color.Khaki,
                    Font = new Font(
                        "Segoe UI",
                        8.5f,
                        FontStyle.Bold)
                };

            btnVerEstadisticas.Click +=
                (s, e) =>
                {
                    using (
                        FormEstadisticas frmEst =
                            new FormEstadisticas(
                                _listaReportes))
                    {
                        frmEst.ShowDialog();
                    }
                };

            // -----------------------------------------------------
            // EXPORTAR PDF
            // -----------------------------------------------------

            btnExportarPdf =
                new Button
                {
                    Text = "📄 Exportar PDF",
                    Location = new Point(315, 325),
                    Width = 90,
                    Height = 30,
                    BackColor = Color.LightGreen,
                    Font = new Font(
                        "Segoe UI",
                        8.5f,
                        FontStyle.Bold)
                };

            btnExportarPdf.Click +=
                btnExportarPdf_Click;

            // -----------------------------------------------------
            // BÚSQUEDA
            // -----------------------------------------------------

            Label lblBuscar =
                new Label
                {
                    Text = "Folio:",
                    Location = new Point(15, 30),
                    AutoSize = true,
                    Font = new Font(
                        "Segoe UI",
                        8.5f)
                };

            txtBuscarFolio =
                new TextBox
                {
                    Location = new Point(55, 27),
                    Width = 100,
                    Font = new Font(
                        "Segoe UI",
                        8.5f)
                };

            btnBuscar =
                new Button
                {
                    Text = "Buscar",
                    Location = new Point(160, 26),
                    Width = 65,
                    Height = 25,
                    Font = new Font(
                        "Segoe UI",
                        8.5f)
                };

            btnBuscar.Click +=
                BtnBuscar_Click;

            // -----------------------------------------------------
            // FILTRO
            // -----------------------------------------------------

            Label lblFiltro =
                new Label
                {
                    Text = "Estado:",
                    Location = new Point(235, 30),
                    AutoSize = true,
                    Font = new Font(
                        "Segoe UI",
                        8.5f)
                };

            comboFiltroEstado =
                new ComboBox
                {
                    Location = new Point(285, 27),
                    Width = 120,
                    DropDownStyle =
                        ComboBoxStyle.DropDownList,
                    Font = new Font(
                        "Segoe UI",
                        8.5f)
                };

            comboFiltroEstado.Items.Add("Todos");

            foreach (
                var est in
                Enum.GetValues(
                    typeof(EstadoReporte)))
            {
                comboFiltroEstado.Items.Add(est);
            }

            comboFiltroEstado.SelectedIndex = 0;

            comboFiltroEstado.SelectedIndexChanged +=
                ComboFiltroEstado_SelectedIndexChanged;

            btnLimpiarFiltros =
                new Button
                {
                    Text = "Ver Todos",
                    Location = new Point(412, 26),
                    Width = 85,
                    Height = 25,
                    Font = new Font(
                        "Segoe UI",
                        8.5f)
                };

            btnLimpiarFiltros.Click +=
                (s, e) =>
                    MostrarReportesEnGrid(
                        _listaReportes);

            // -----------------------------------------------------
            // GRID
            // -----------------------------------------------------

            gridReportes =
                new DataGridView
                {
                    Location = new Point(15, 60),
                    Size = new Size(485, 260),
                    ReadOnly = true,
                    SelectionMode =
                        DataGridViewSelectionMode.FullRowSelect,
                    MultiSelect = false,
                    AllowUserToAddRows = false,
                    AutoSizeColumnsMode =
                        DataGridViewAutoSizeColumnsMode.Fill,
                    Font = new Font(
                        "Segoe UI",
                        8.5f)
                };

            gridReportes.SelectionChanged +=
                GridReportes_SelectionChanged;

            grpListado.Controls.AddRange(
                new Control[]
                {
                    lblBuscar,
                    txtBuscarFolio,
                    btnBuscar,
                    lblFiltro,
                    comboFiltroEstado,
                    btnLimpiarFiltros,
                    gridReportes,
                    btnExportarPdf,
                    btnVerEstadisticas
                });

            // =====================================================
            // GESTIÓN Y SEGUIMIENTO
            // =====================================================

            GroupBox grpDetalle =
                new GroupBox
                {
                    Text =
                        " Gestión y Seguimiento del Reporte Seleccionado ",
                    Location = new Point(15, 405),
                    Size = new Size(1050, 280),
                    Font = new Font(
                        "Segoe UI",
                        9.5f,
                        FontStyle.Bold)
                };

            // -----------------------------------------------------
            // ESTADO
            // -----------------------------------------------------

            Label lblEstado =
                new Label
                {
                    Text = "Cambiar Estado:",
                    Location = new Point(15, 30),
                    AutoSize = true,
                    Font = new Font(
                        "Segoe UI",
                        8.5f)
                };

            comboNuevoEstado =
                new ComboBox
                {
                    Location = new Point(120, 27),
                    Width = 140,
                    DropDownStyle =
                        ComboBoxStyle.DropDownList,
                    Font = new Font(
                        "Segoe UI",
                        8.5f)
                };

            comboNuevoEstado.DataSource =
                Enum.GetValues(
                    typeof(EstadoReporte));

            btnCambiarEstado =
                new Button
                {
                    Text = "Actualizar Estado",
                    Location = new Point(270, 26),
                    Width = 130,
                    Height = 26,
                    Font = new Font(
                        "Segoe UI",
                        8.5f)
                };

            btnCambiarEstado.Click +=
                BtnCambiarEstado_Click;

            // -----------------------------------------------------
            // HISTORIAL
            // -----------------------------------------------------

            Label lblHistorial =
                new Label
                {
                    Text =
                        "Historial de Seguimientos:",
                    Location = new Point(15, 70),
                    AutoSize = true,
                    Font = new Font(
                        "Segoe UI",
                        8.5f)
                };

            lstHistorialSeguimiento =
                new ListBox
                {
                    Location = new Point(15, 95),
                    Size = new Size(580, 160),
                    Font = new Font(
                        "Segoe UI",
                        8.5f)
                };

            // -----------------------------------------------------
            // COMENTARIO
            // -----------------------------------------------------

            Label lblNuevoComentario =
                new Label
                {
                    Text =
                        "Nuevo Avance / Comentario:",
                    Location = new Point(610, 70),
                    AutoSize = true,
                    Font = new Font(
                        "Segoe UI",
                        8.5f)
                };

            txtComentarioSeguimiento =
                new TextBox
                {
                    Location = new Point(610, 95),
                    Size = new Size(420, 110),
                    Multiline = true,
                    Font = new Font(
                        "Segoe UI",
                        8.5f)
                };

            btnAgregarSeguimiento =
                new Button
                {
                    Text =
                        "Agregar Seguimiento",
                    Location = new Point(610, 215),
                    Size = new Size(420, 35),
                    BackColor = Color.LightGreen,
                    Font = new Font(
                        "Segoe UI",
                        9f,
                        FontStyle.Bold)
                };

            btnAgregarSeguimiento.Click +=
                BtnAgregarSeguimiento_Click;

            grpDetalle.Controls.AddRange(
                new Control[]
                {
                    lblEstado,
                    comboNuevoEstado,
                    btnCambiarEstado,
                    lblHistorial,
                    lstHistorialSeguimiento,
                    lblNuevoComentario,
                    txtComentarioSeguimiento,
                    btnAgregarSeguimiento
                });

            // =====================================================
            // AGREGAR GRUPOS AL FORMULARIO
            // =====================================================

            Controls.AddRange(
                new Control[]
                {
                    grpRegistro,
                    grpListado,
                    grpDetalle
                });
        }

        // =========================================================
        // EXPORTAR REPORTE A PDF
        // =========================================================

        private void btnExportarPdf_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                // Verificar que exista una selección
                if (
                    gridReportes.SelectedRows.Count == 0)
                {
                    MessageBox.Show(
                        "Por favor, selecciona un reporte de la lista para poder exportarlo a PDF.",
                        "Atención",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                // Obtener el folio de la fila seleccionada
                string folioSeleccionado =
                    gridReportes
                    .SelectedRows[0]
                    .Cells["Folio"]
                    .Value
                    .ToString();

                // Buscar el reporte real en la lista
                Reporte reporteSeleccionado =
                    _listaReportes.FirstOrDefault(
                        r =>
                            r.Folio ==
                            folioSeleccionado);

                if (reporteSeleccionado == null)
                {
                    MessageBox.Show(
                        "No se pudo encontrar el reporte seleccionado.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }

                // Cuadro para elegir dónde guardar el PDF
                using (
                    SaveFileDialog saveFileDialog =
                        new SaveFileDialog())
                {
                    saveFileDialog.Filter =
                        "Archivos PDF (*.pdf)|*.pdf";

                    saveFileDialog.FileName =
                        $"Reporte_{reporteSeleccionado.Folio}_{DateTime.Now:yyyyMMdd}.pdf";

                    saveFileDialog.Title =
                        "Guardar reporte como PDF";

                    if (
                        saveFileDialog.ShowDialog() ==
                        DialogResult.OK)
                    {
                        _servicioPdf.ExportarReporteAPdf(
                            reporteSeleccionado,
                            saveFileDialog.FileName);

                        MessageBox.Show(
                            "¡El reporte se ha exportado correctamente a formato PDF!",
                            "Éxito",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ocurrió un error al generar el PDF: {ex.Message}",
                    "Error de Exportación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // =========================================================
        // CAMBIO DE TIPO DE REPORTE
        // =========================================================

        private void ComboTipoReporte_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            bool esInfraestructura =
                comboTipoReporte.SelectedIndex == 0;

            chkViaPrincipal.Visible =
                esInfraestructura;

            chkPeligroInminente.Visible =
                esInfraestructura;

            foreach (
                Control ctrl in
                panelControlesEspecificos.Controls)
            {
                if (
                    ctrl == comboTipoAmbiental ||
                    ctrl == numDiasAcumulados ||
                    ctrl.Text.Contains("Incidente") ||
                    ctrl.Text.Contains("Días"))
                {
                    ctrl.Visible =
                        !esInfraestructura;
                }
            }
        }

        // =========================================================
        // CARGAR FOTO
        // =========================================================

        private void BtnCargarFoto_Click(
            object sender,
            EventArgs e)
        {
            using (
                OpenFileDialog ofd =
                    new OpenFileDialog())
            {
                ofd.Filter =
                    "Archivos de Imagen|*.jpg;*.jpeg;*.png;*.bmp";

                ofd.Title =
                    "Seleccionar Fotografía de Evidencia";

                if (
                    ofd.ShowDialog() ==
                    DialogResult.OK)
                {
                    _rutaFotoSeleccionada =
                        ofd.FileName;

                    lblRutaFoto.Text =
                        Path.GetFileName(
                            _rutaFotoSeleccionada);
                }
            }
        }

        // =========================================================
        // REGISTRAR REPORTE
        // =========================================================

        private void BtnRegistrar_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                string folio =
                    $"RPT-{1000 + _listaReportes.Count + 1}";

                Reporte nuevoReporte;

                if (
                    comboTipoReporte.SelectedIndex ==
                    0)
                {
                    nuevoReporte =
                        new ReporteInfraestructura(
                            folio,
                            txtTitulo.Text,
                            txtDescripcion.Text,
                            txtUbicacion.Text,
                            chkViaPrincipal.Checked,
                            chkPeligroInminente.Checked,
                            _rutaFotoSeleccionada);
                }
                else
                {
                    TipoIncidenteAmbiental tipoIncidente =
                        (TipoIncidenteAmbiental)
                        comboTipoAmbiental.SelectedItem;

                    nuevoReporte =
                        new ReporteMedioAmbiente(
                            folio,
                            txtTitulo.Text,
                            txtDescripcion.Text,
                            txtUbicacion.Text,
                            tipoIncidente,
                            (int)
                            numDiasAcumulados.Value,
                            _rutaFotoSeleccionada);
                }

                _listaReportes.Add(
                    nuevoReporte);

                GuardarEnPersistencia();

                MostrarReportesEnGrid(
                    _listaReportes);

                LimpiarFormularioRegistro();

                MessageBox.Show(
                    $"Reporte {folio} registrado exitosamente con Prioridad {nuevoReporte.Prioridad}.",
                    "Registro Exitoso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al registrar el reporte: {ex.Message}",
                    "Error de Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        // =========================================================
        // MOSTRAR REPORTES EN GRID
        // =========================================================

        private void MostrarReportesEnGrid(
            List<Reporte> lista)
        {
            var reporteVista =
                lista.Select(
                    r => new
                    {
                        r.Folio,
                        r.Titulo,

                        Tipo =
                            r.GetType() ==
                            typeof(
                                ReporteInfraestructura)
                                ? "Infraestructura"
                                : "Medio Ambiente",

                        r.Ubicacion,
                        r.Prioridad,
                        r.Estado,

                        Fecha =
                            r.FechaRegistro.ToString(
                                "dd/MM/yyyy HH:mm")
                    })
                .ToList();

            gridReportes.DataSource = null;

            gridReportes.DataSource =
                reporteVista;
        }

        // =========================================================
        // SELECCIÓN DE REPORTE
        // =========================================================

        private void GridReportes_SelectionChanged(
            object sender,
            EventArgs e)
        {
            if (
                gridReportes.SelectedRows.Count >
                0)
            {
                string folioSeleccionado =
                    gridReportes
                    .SelectedRows[0]
                    .Cells["Folio"]
                    .Value
                    .ToString();

                Reporte reporte =
                    _listaReportes.FirstOrDefault(
                        r =>
                            r.Folio ==
                            folioSeleccionado);

                if (reporte != null)
                {
                    comboNuevoEstado.SelectedItem =
                        reporte.Estado;

                    cmbPrioridad.SelectedItem =
                        reporte.Prioridad;

                    ActualizarHistorialSeguimiento(
                        reporte);
                }
            }
        }

        // =========================================================
        // HISTORIAL
        // =========================================================

        private void ActualizarHistorialSeguimiento(
            Reporte reporte)
        {
            lstHistorialSeguimiento.Items.Clear();

            foreach (
                var seg in
                reporte.HistorialSeguimiento)
            {
                lstHistorialSeguimiento.Items.Add(
                    seg.ToString());
            }
        }

        // =========================================================
        // AGREGAR SEGUIMIENTO
        // =========================================================

        private void BtnAgregarSeguimiento_Click(
            object sender,
            EventArgs e)
        {
            if (
                gridReportes.SelectedRows.Count ==
                0)
            {
                MessageBox.Show(
                    "Por favor selecciona un reporte de la lista.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            string folioSeleccionado =
                gridReportes
                .SelectedRows[0]
                .Cells["Folio"]
                .Value
                .ToString();

            Reporte reporte =
                _listaReportes.FirstOrDefault(
                    r =>
                        r.Folio ==
                        folioSeleccionado);

            if (
                reporte != null &&
                !string.IsNullOrWhiteSpace(
                    txtComentarioSeguimiento.Text))
            {
                try
                {
                    Seguimiento nuevoSeguimiento =
                        new Seguimiento(
                            txtComentarioSeguimiento.Text,
                            "Técnico Ciudadano");

                    reporte.AgregarSeguimiento(
                        nuevoSeguimiento);

                    GuardarEnPersistencia();

                    ActualizarHistorialSeguimiento(
                        reporte);

                    txtComentarioSeguimiento.Clear();

                    MessageBox.Show(
                        "Seguimiento agregado correctamente.",
                        "Éxito",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        ex.Message,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        // =========================================================
        // CAMBIAR ESTADO
        // =========================================================

        private void BtnCambiarEstado_Click(
            object sender,
            EventArgs e)
        {
            if (
                gridReportes.SelectedRows.Count ==
                0)
            {
                return;
            }

            string folioSeleccionado =
                gridReportes
                .SelectedRows[0]
                .Cells["Folio"]
                .Value
                .ToString();

            Reporte reporte =
                _listaReportes.FirstOrDefault(
                    r =>
                        r.Folio ==
                        folioSeleccionado);

            if (reporte != null)
            {
                EstadoReporte nuevoEstado =
                    (EstadoReporte)
                    comboNuevoEstado.SelectedItem;

                reporte.CambiarEstado(
                    nuevoEstado);

                GuardarEnPersistencia();

                MostrarReportesEnGrid(
                    _listaReportes);

                MessageBox.Show(
                    $"Estado del reporte {reporte.Folio} actualizado a {nuevoEstado}.",
                    "Actualización",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        // =========================================================
        // BUSCAR
        // =========================================================

        private void BtnBuscar_Click(
            object sender,
            EventArgs e)
        {
            string busqueda =
                txtBuscarFolio.Text
                .Trim()
                .ToUpper();

            if (
                !string.IsNullOrEmpty(
                    busqueda))
            {
                var filtrados =
                    _listaReportes
                    .Where(
                        r =>
                            r.Folio
                            .ToUpper()
                            .Contains(busqueda))
                    .ToList();

                MostrarReportesEnGrid(
                    filtrados);
            }
        }

        // =========================================================
        // FILTRO DE ESTADO
        // =========================================================

        private void ComboFiltroEstado_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            if (
                comboFiltroEstado.SelectedIndex ==
                0)
            {
                MostrarReportesEnGrid(
                    _listaReportes);
            }
            else
            {
                EstadoReporte estadoSeleccionado =
                    (EstadoReporte)
                    comboFiltroEstado.SelectedItem;

                var filtrados =
                    _listaReportes
                    .Where(
                        r =>
                            r.Estado ==
                            estadoSeleccionado)
                    .ToList();

                MostrarReportesEnGrid(
                    filtrados);
            }
        }

        // =========================================================
        // GUARDAR
        // =========================================================

        private void GuardarEnPersistencia()
        {
            try
            {
                _gestorPersistencia.Guardar(
                    _listaReportes,
                    _rutaArchivoJson);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No se pudo guardar la información en JSON: {ex.Message}",
                    "Error de Persistencia",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // =========================================================
        // CARGAR
        // =========================================================

        private void CargarReportesGuardados()
        {
            try
            {
                _listaReportes =
                    _gestorPersistencia.Cargar(
                        _rutaArchivoJson);

                MostrarReportesEnGrid(
                    _listaReportes);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al cargar datos desde JSON: {ex.Message}",
                    "Error de Lectura",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        // =========================================================
        // LIMPIAR FORMULARIO
        // =========================================================

        private void LimpiarFormularioRegistro()
        {
            txtTitulo.Clear();

            txtDescripcion.Clear();

            txtUbicacion.Clear();

            chkViaPrincipal.Checked =
                false;

            chkPeligroInminente.Checked =
                false;

            _rutaFotoSeleccionada =
                "";

            lblRutaFoto.Text =
                "Sin foto seleccionada";

            if (
                cmbPrioridad.Items.Count >
                0)
            {
                cmbPrioridad.SelectedIndex =
                    0;
            }
        }
    }
}