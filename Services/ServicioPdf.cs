using System;
using System.IO;
// Librerías base de iText 7 para maquetación y estructura
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Colors;
using iText.IO.Image;
using ComunidadAlertaApp.Models;

namespace ComunidadAlertaApp.Services
{
    /// <summary>
    /// Servicio encargado de la exportación de reportes comunitarios a formato PDF.
    /// </summary>
    public class ServicioPdf
    {
        /// <summary>
        /// Genera un archivo PDF individual con la ficha técnica completa de un reporte.
        /// </summary>
        /// <param name="reporte">Objeto del reporte a exportar.</param>
        /// <param name="rutaDestino">Ruta del sistema donde se guardará el PDF.</param>
        public void ExportarReporteAPdf(Reporte reporte, string rutaDestino)
        {
            // Validamos que el reporte recibido no sea nulo antes de procesar
            if (reporte == null)
            {
                throw new ArgumentNullException(nameof(reporte), "El reporte no puede ser nulo.");
            }

            // Inicializamos el escritor del archivo PDF en la ruta de destino
            using (PdfWriter writer = new PdfWriter(rutaDestino))
            {
                // Creamos el documento PDF de bajo nivel
                using (PdfDocument pdf = new PdfDocument(writer))
                {
                    // Creamos el documento de alto nivel para agregar los elementos visuales
                    Document document = new Document(pdf);

                    // --- ENCABEZADO Y TÍTULO ---
                    // Creamos el título usando tamaño de fuente y color estándar
                    Paragraph tituloHeader = new Paragraph("COMUNIDAD ALERTA - FICHA TÉCNICA DE REPORTE")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontSize(16)
                        .SetFontColor(ColorConstants.BLUE);

                    document.Add(tituloHeader);
                    document.Add(new Paragraph("\n"));

                    // --- TABLA DE DATOS TÉCNICOS ---
                    // Creamos una tabla de 2 columnas proporcionales
                    Table tablaDatos = new Table(new float[] { 1f, 2f });
                    tablaDatos.SetWidth(UnitValue.CreatePercentValue(100));

                    // Inserción de filas clave / valor utilizando texto plano
                    AgregarFilaTabla(tablaDatos, "Folio:", reporte.Folio);
                    AgregarFilaTabla(tablaDatos, "Título:", reporte.Titulo);
                    AgregarFilaTabla(tablaDatos, "Ubicación:", reporte.Ubicacion);
                    AgregarFilaTabla(tablaDatos, "Fecha de Registro:", reporte.FechaRegistro.ToString("dd/MM/yyyy HH:mm"));
                    AgregarFilaTabla(tablaDatos, "Estado Actual:", reporte.Estado.ToString());
                    AgregarFilaTabla(tablaDatos, "Prioridad:", reporte.Prioridad.ToString());

                    // Evaluamos el tipo de reporte concreto
                    if (reporte is ReporteInfraestructura infra)
                    {
                        AgregarFilaTabla(tablaDatos, "Tipo de Reporte:", "Infraestructura Urbana");
                        AgregarFilaTabla(tablaDatos, "Via Principal:", infra.EsViaPrincipal ? "Si" : "No");

                        // IMPORTANTE: Si en tu modelo la propiedad se llama de otra forma (ej. PeligroInminente), 
                        // cambia 'EsPeligroInminente' por el nombre exacto de la propiedad en tu clase ReporteInfraestructura.cs
                        AgregarFilaTabla(tablaDatos, "Peligro Inminente:", infra.RepresentaPeligroInminente ? "Si" : "No");
                    }
                    else if (reporte is ReporteMedioAmbiente amb)
                    {
                        AgregarFilaTabla(tablaDatos, "Tipo de Reporte:", "Medio Ambiente");
                        AgregarFilaTabla(tablaDatos, "Tipo de Incidente:", amb.TipoIncidente.ToString());
                        AgregarFilaTabla(tablaDatos, "Dias Acumulados:", amb.DiasAcumulados.ToString());
                    }

                    document.Add(tablaDatos);

                    // --- DESCRIPCIÓN DEL REPORTE ---
                    document.Add(new Paragraph("\nDescripcion del Incidente:"));
                    Paragraph parrafoDescripcion = new Paragraph(string.IsNullOrWhiteSpace(reporte.Descripcion) ? "Sin descripcion especificada." : reporte.Descripcion);
                    document.Add(parrafoDescripcion);

                    // --- EVIDENCIA FOTOGRÁFICA ---
                    if (!string.IsNullOrEmpty(reporte.RutaFotografia) && File.Exists(reporte.RutaFotografia))
                    {
                        document.Add(new Paragraph("\nEvidencia Fotografica:"));

                        ImageData dataImagen = ImageDataFactory.Create(reporte.RutaFotografia);
                        iText.Layout.Element.Image imgEvidencia = new iText.Layout.Element.Image(dataImagen);

                        // Ajustamos la altura máxima y centramos la imagen
                        imgEvidencia.SetMaxHeight(250);
                        imgEvidencia.SetHorizontalAlignment(HorizontalAlignment.CENTER);

                        document.Add(imgEvidencia);
                    }

                    // --- PIE DE PÁGINA ---
                    Paragraph piePagina = new Paragraph($"\nDocumento generado automaticamente el {DateTime.Now:dd/MM/yyyy HH:mm:ss}")
                        .SetFontSize(8)
                        .SetTextAlignment(TextAlignment.RIGHT);

                    document.Add(piePagina);

                    // Cerramos el documento para liberar recursos
                    document.Close();
                }
            }
        }

        /// <summary>
        /// Método auxiliar para insertar pares de etiqueta y valor en la tabla sin usar estilos avanzados.
        /// </summary>
        private void AgregarFilaTabla(Table tabla, string etiqueta, string valor)
        {
            Cell celdaEtiqueta = new Cell().Add(new Paragraph(etiqueta));
            Cell celdaValor = new Cell().Add(new Paragraph(valor ?? "N/A"));

            tabla.AddCell(celdaEtiqueta);
            tabla.AddCell(celdaValor);
        }
    }
}