// Importación del espacio de nombres System para funcionalidades básicas del entorno .NET
using System;
// Importación de la librería principal de Windows Forms para manejar ventanas y controles
using System.Windows.Forms;
// Importación del espacio de nombres donde se encuentra alojado nuestro FormPrincipal
using ComunidadAlertaApp.Forms;

namespace ComunidadAlertaApp
{
    /// <summary>
    /// Clase principal que contiene el punto de entrada de la aplicación.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación (método Main).
        /// El atributo [STAThread] establece que el modelo de subprocesos para la aplicación es Single-Threaded Apartment,
        /// requisito indispensable para la correcta ejecución de controles visuales en Windows Forms.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Habilita los estilos visuales modernos del sistema operativo para los controles (botones, tablas, combos)
            Application.EnableVisualStyles();

            // Establece el estándar compatible para el renderizado de texto en controles
            Application.SetCompatibleTextRenderingDefault(false);

            // Inicia y ejecuta el bucle de eventos visuales con nuestro formulario principal
            Application.Run(new FormPrincipal());
        }
    }
}