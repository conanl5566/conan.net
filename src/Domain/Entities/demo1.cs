
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotNET.Domain.Entities
{
    /// <summary>
    /// demo1
    /// </summary>
    [Table("demo1")]
    public class demo1
    {

     /// <summary>
     /// 
     /// </summary>
        public  int Id { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

    }
}