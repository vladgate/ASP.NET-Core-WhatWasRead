using ASP.NET_Core_WhatWasRead.App_Data.DBModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_WhatWasRead.App_Data
{
   public interface IRepository
   {
      DbContext Context { get; }

      IEnumerable<Book> Books { get; }
      IEnumerable<Author> Authors { get; }
      IEnumerable<Category> Categories { get; }
      IEnumerable<Language> Languages { get; }
      IEnumerable<Tag> Tags { get; }

      IEnumerable<Book> UpdateBooksFromFilterUsingRawSql(NameValueCollection queryString, string controller, string action);
      void Dispose();
      void SaveChanges();
      void SetEntryStateToModified(object entity);

      void AddBook(Book book);
      void RemoveBook(Book book);
      Book FindBook(int id);
      
      void AddAuthor(Author newAuthor);
      void RemoveAuthor(Author author);

      void AddTag(Tag newTag);
      void RemoveTag(Tag tag);
   }
}
