using System;
using System.Collections.Generic;
using System.Linq;
using ASP.NET_Core_WhatWasRead.App_Data.DBModels;

namespace ASP.NET_Core_WhatWasRead.Models
{
    public class BooksListViewModel
    {
        public IEnumerable<Book> Books { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentCategory { get; set; }
        public string CurrentTag { get; set; }

    }
}