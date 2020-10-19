using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP.NET_Core_WhatWasRead.App_Data.DBModels
{
    public class Category
   {
      public Category()
      {
         this.Books = new HashSet<Book>();
      }

      public int CategoryId { get; set; }

      [Required]
      [MaxLength(50)]
      public string NameForLinks { get; set; }

      [Required]
      [MaxLength(50)]
      public string NameForLabels { get; set; }
      public virtual ICollection<Book> Books { get; set; }


   }
}