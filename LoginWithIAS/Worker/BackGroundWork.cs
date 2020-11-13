using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace LoginWithIAS.Worker
{
    /// <summary>
    /// Traajos En segundo Plano
    /// </summary>
    public class BackGroundWork
    {
        private BackgroundWorker backGroundWorker;

        /// <summary>
        /// Constructor
        /// </summary>
        public BackGroundWork()
        {
            this.backGroundWorker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
        }

        /// <summary>
        /// Trabajo en segundo Plano
        /// </summary>
        public void RealizarTrabajo()
        {
            if (!backGroundWorker.IsBusy)
            {
                backGroundWorker.DoWork += RealizarTrabajo;                // Método que hará el trabajo
                backGroundWorker.ProgressChanged += NotificarProgreso;     // Método donde se notificará el progreso
                backGroundWorker.RunWorkerCompleted += TrabajoConcluido; // Método que se ejecutará al finalizar el trabajo


                backGroundWorker.RunWorkerAsync();
            }
        }

        private void TrabajoConcluido(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                //"Tarea Cancelada";
            }
            else if (e.Error != null)
            {
                //"Error";
            }
            else
            {
                //Todo finalizo bien
            }
        }

        private void NotificarProgreso(object sender, ProgressChangedEventArgs e)
        {
            //Notificacion de progreso 
        }

        private void RealizarTrabajo(object sender, DoWorkEventArgs e)
        {
            //Toda la Logica
        }

        /// <summary>
        /// Cancelar Trabajo
        /// </summary>
        public void CancelarTrabajo()
        {
            backGroundWorker.CancelAsync();
        }
    }
}