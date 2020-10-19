using ASP.NET_Core_WhatWasRead.App_Data;
using ASP.NET_Core_WhatWasRead.App_Data.DBModels;
using ASP.NET_Core_WhatWasRead.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ASP.NET_Core_WhatWasRead.ViewComponents
{
   public class ListOfCategoriesViewComponent : ViewComponent
   {
      private readonly IRepository _repository;

      public ListOfCategoriesViewComponent(IRepository repository)
      {
         _repository = repository;
      }

      public IViewComponentResult Invoke(string currentCategory, string currentTag, int? minPages, int? maxPages)
      {
         NavigationViewModel model = new NavigationViewModel();
         IEnumerable<Category> categories = _repository.Categories.OrderBy(c => c.NameForLabels).ToList();
         IEnumerable<Tag> tags = _repository.Tags.OrderBy(t => t.NameForLabels).ToList();
         IEnumerable<Author> authors = _repository.Authors.OrderBy(f => f.LastName).ToList();
         IEnumerable<Language> languages = _repository.Languages.OrderBy(f => f.NameForLabels).ToList();
         int minPagesDB = _repository.Books.Count() > 0 ? _repository.Books.Select(b => b.Pages).Min() : 0;
         int maxPagesDB = _repository.Books.Count() > 0 ? _repository.Books.Select(b => b.Pages).Max() : 0;

         model.Categories = categories;
         model.Tags = tags;
         model.Authors = authors;
         model.Languages = languages;
         model.MinPagesExpected = minPagesDB;
         model.MaxPagesExpected = maxPagesDB;
         model.CurrentCategory = currentCategory;
         model.CurrentTag = currentTag;
         model.MinPagesActual = minPages.HasValue ? Math.Max(minPages.Value, minPagesDB) : minPagesDB;
         model.MaxPagesActual = maxPages.HasValue ? Math.Min(maxPages.Value, maxPagesDB) : maxPagesDB;
         return View(model);
      }
   }
}