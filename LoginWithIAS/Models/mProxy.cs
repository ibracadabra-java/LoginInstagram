using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginWithIAS.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class mProxy
    {
        private string addressProxy;
        private string usernameProxy;
        private string passProxy;
        private string pais;
        private int estado;
        private int currentTask;
        private int tareaPrioridad;
        private bool errorResult;
        private int id_proxy;
        /// <summary>
        /// 
        /// </summary>
        public string AddressProxy { get => addressProxy; set => addressProxy = value; }
        /// <summary>
        /// Usuario del Proxy
        /// </summary>
        public string UsernameProxy { get => usernameProxy; set => usernameProxy = value; }
        /// <summary>
        /// Password del proxy
        /// </summary>
        public string PassProxy { get => passProxy; set => passProxy = value; }
        /// <summary>
        /// Pais del prpxy
        /// </summary>
        public string Pais { get => pais; set => pais = value; }
        /// <summary>
        /// Estado del proxy
        /// </summary>
        public int Estado { get => estado; set => estado = value; }
        /// <summary>
        /// Cantidad de Tareas ejecutandose
        /// </summary>
        public int CurrentTask { get => currentTask; set => currentTask = value; }
        /// <summary>
        /// Tarea para la que es prioridad
        /// </summary>
        public int TareaPrioridad { get => tareaPrioridad; set => tareaPrioridad = value; }
        /// <summary>
        /// Error en la asignacion de proxy
        /// </summary>
        public bool ErrorResult { get => errorResult; set => errorResult = value; }
        /// <summary>
        /// identificador del proxy
        /// </summary>
        public int Id_proxy { get => id_proxy; set => id_proxy = value; }
    }
}