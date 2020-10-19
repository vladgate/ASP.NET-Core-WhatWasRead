using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP.NET_Core_WhatWasRead.App_Data.DBModels
{
    public class Tag
    {
      public Tag()
      {
         this.BookTags = new HashSet<BookTags>();
      }
      public int TagId { get; set; }

      [Required]
      [MaxLength(50)]
      public string NameForLabels { get; set; }

      [Required]
      [MaxLength(50)]
      public string NameForLinks { get; set; }
      public virtual ICollection<BookTags> BookTags { get; set; }

   }
}