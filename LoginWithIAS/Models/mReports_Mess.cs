using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginWithIAS.Models
{
    /// <summary>
    /// Clase para el reporte de los mensajes
    /// </summary>
    public class mReports_Mess
    {
        /// <summary>
        /// 
        /// </summary>
        public mReports_Mess()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public mReports_Mess(string thread_id, string item_id, long cliente_id, int cant_total, int cant_env, int cant_vistos, int cant_reacc, string list_ids )
        {
            this.Thread_Id = thread_id;
            this.Cliente_Id = cliente_id;
            this.Item_Id = item_id;
            this.Cant_Total = cant_total;
            this.Cant_Env = cant_env;
            this.Cant_Vistos = cant_vistos;
            this.Cant_Reacc = cant_reacc;
            this.List_Ids = list_ids;
        }
        /// <summary>
        /// Identificador del Hilo de conversacion
        /// </summary>
        public string Thread_Id { get; set; }

        /// <summary>
        /// Identificador
        /// </summary>
        public string Item_Id { get; set; }

        /// <summary>
        /// Identificador del Cliente
        /// </summary>
        public long Cliente_Id { get; set; }

        /// <summary>
        /// Cantidad Total de Mensajes a Enviar
        /// </summary>
        public int Cant_Total { get; set; }

        /// <summary>
        /// Cantidad de mensajes enviados
        /// </summary>
        public int Cant_Env { get; set; }

        /// <summary>
        /// Cantidad de Mensajes Vistos
        /// </summary>
        public int Cant_Vistos { get; set; }

        /// <summary>
        /// Cantidad de Reacciones a los mensajes
        /// </summary>
        public int Cant_Reacc { get; set; }

        /// <summary>
        /// Lista de Identificadores a los que se les envio el mensaje
        /// Los indentificadores estan separados por coma.
        /// </summary>
        public string List_Ids { get; set; }
    }
}