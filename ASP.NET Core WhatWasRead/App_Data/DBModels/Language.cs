using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP.NET_Core_WhatWasRead.App_Data.DBModels
{
    public class Language
   {
      public Language()
      {
         this.Books = new HashSet<Book>();
      }

      public int LanguageId { get; set; }

      [Required]
      [MaxLength(50)]
      public string NameForLabels { get; set; }

      [Required]
      [MaxLength(50)]
      public string NameForLinks { get; set; }
      public virtual ICollection<Book> Books { get; set; }

   }
}