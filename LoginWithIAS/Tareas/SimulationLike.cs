using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;    

namespace LoginWithIAS.Tareas
{
    /// <summary>
    /// 
    /// </summary>
    public class SimulationLike
    {
        /// <summary>
        /// 
        /// </summary>
       public BackgroundWorker worker = new BackgroundWorker() ;
       
        /// <summary>
        /// 
        /// </summary>
        private void SimulationLikeHuman()
        {
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            if (worker.IsBusy != true)
            {
                // Start the asynchronous operation.
                worker.RunWorkerAsync();
            }
        }
    }
}