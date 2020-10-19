using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP.NET_Core_WhatWasRead.App_Data.DBModels
{
    public class Author
   {
      public Author()
      {
         AuthorsOfBooks = new HashSet<AuthorsOfBooks>();
      }
      public int AuthorId { get; set; }

      [Required]
      [StringLength(30)]
      public string FirstName { get; set; }

      [Required]
      [StringLength(30)]
      public string LastName { get; set; }
      public virtual ICollection<AuthorsOfBooks> AuthorsOfBooks { get; set; }

      public string DisplayText
        {
            get
            {
                return this.LastName + " " + this.FirstName;
            }
        }
    }
}
