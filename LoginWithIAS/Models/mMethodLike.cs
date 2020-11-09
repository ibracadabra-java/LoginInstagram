using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginWithIAS.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class mMethodLike : mLikeManyPost
    {
        /// <summary>
        /// 
        /// </summary>
        public List<string> ListUser {get;set;}
        /// <summary>
        /// 
        /// </summary>
        public int vel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vel"></param>
        /// <returns></returns>
        public int tiempo(int vel) 
        {
            Random objram = new Random();
            switch (vel)
            {
                case 0:return objram.Next(15,21);
                case 1:return objram.Next(9,15);
                case 2:return objram.Next(4, 9);
                default: return 25;
                    
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vel"></param>
        /// <returns></returns>
        public int cant(int vel)
        {
            Random objram = new Random();
            switch (vel)
            {
                case 0: return objram.Next(15, 21);
                case 1: return objram.Next(9, 15);
                case 2: return objram.Next(4, 9);
                default: return 25;

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vel"></param>
        /// <returns></returns>
        public int cantInter(int vel)
        {
            Random objram = new Random();
            switch (vel)
            {
                case 0: return objram.Next(10, 13);
                case 1: return objram.Next(5, 10);
                case 2: return objram.Next(1, 5);
                default: return 15;

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vel"></param>
        /// <returns></returns>
        public int tiempoInter(int vel)
        {
            Random objram = new Random();
            switch (vel)
            {
                case 0: return objram.Next(10, 13);
                case 1: return objram.Next(5, 10);
                case 2: return objram.Next(1, 5);
                default: return 15;

            }
        }
    }
}