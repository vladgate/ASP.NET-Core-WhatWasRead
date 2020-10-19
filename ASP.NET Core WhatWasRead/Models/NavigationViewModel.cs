using ASP.NET_Core_WhatWasRead.App_Data.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ASP.NET_Core_WhatWasRead.Models
{
    public class NavigationViewModel
    {
        public string AuthorQueryWord { get { return "author"; } }
        public string LanguageQueryWord { get { return "lang"; } }
        public string PagesQueryWord { get { return "pages"; } }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
        public IEnumerable<Author> Authors { get; set; }
        public IEnumerable<Language> Languages { get; set; }
        public int MinPagesExpected { get; set; } // min amount of pages in book
        public int MaxPagesExpected { get; set; } // max amount of pages in book
        public int? MinPagesActual { get; set; } //save filter value
        public int? MaxPagesActual { get; set; } //save filter value
        public string CurrentCategory { get; set; }
        public string CurrentTag { get; set; }
    }
}