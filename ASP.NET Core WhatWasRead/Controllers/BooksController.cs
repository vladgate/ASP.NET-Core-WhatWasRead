using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using ASP.NET_Core_WhatWasRead.App_Data;
using ASP.NET_Core_WhatWasRead.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using ASP.NET_Core_WhatWasRead.Models;
using ASP.NET_Core_WhatWasRead.App_Data.DBModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ASP.NET_Core_WhatWasRead.Controllers
{
   public interface IBooksRequestManager
   {
      NameValueCollection GetQueryString(Controller controller);
      NameValueCollection GetQueryStringWhenAppend(Controller controller);
      bool IsAjaxRequest(Controller controller);
   }
   internal sealed class BooksRequestManager : IBooksRequestManager
   {
      public NameValueCollection GetQueryString(Controller controller)
      {
         var dict = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(controller.Request.QueryString.Value);
         NameValueCollection queryString = new NameValueCollection(dict.Count);
         foreach (var item in dict)
         {
            queryString.Add(item.Key, item.Value);
         }
         return queryString;
      }

      public NameValueCollection GetQueryStringWhenAppend(Controller controller)
      {
         string fullQuery = controller.Request.Headers["Referer"].ToString();
         string querystring = fullQuery.Substring(fullQuery.IndexOf('?'));
         NameValueCollection qscoll = HttpUtility.ParseQueryString(querystring);
         return qscoll;
      }

      public bool IsAjaxRequest(Controller controller)
      {
         return controller.Request.IsAjaxRequest();
      }
   }


   public class BooksController : Controller
   {
      private readonly int _booksPerPage = Globals.ITEMS_PER_PAGE;
      private readonly IRepository _repository;
      private IBooksRequestManager _booksRequestManager;

      public BooksController(IRepository repo)
      {
         _repository = repo;
      }
      public IBooksRequestManager BooksRequestManager
      {
         get
         {
            if (_booksRequestManager == null)
            {
               _booksRequestManager = new BooksRequestManager();
            }
            return _booksRequestManager;
         }
         set
         {
            _booksRequestManager = value;
         }
      }

      // GET: Category
      [HttpGet]
      public ActionResult List(int page = 1, string category = null, string tag = null)
      {
         Category currentCategory = null;
         Tag currentTag = null;
         if (category != null)
         {
            currentCategory = _repository.Categories.Where(cat => cat.NameForLinks == category).FirstOrDefault();
            if (currentCategory == null) //category does not exist
            {
               return View("_NotFoundView");
            }
         }
         if (tag != null)
         {
            currentTag = _repository.Tags.Where(x => x.NameForLinks == tag).FirstOrDefault();
            if (currentTag == null) //tag does not exist
            {
               return View("_NotFoundView");
            }
         }

         //update repository in accordance to query string
         IBooksRequestManager requestManager = this.BooksRequestManager;
         NameValueCollection query = requestManager.GetQueryString(this);
         string[] queryKeys = query.AllKeys;

         if (queryKeys.Length > 0 && !(queryKeys.Length == 1 && (queryKeys[0] == "tag" || queryKeys[0] == "page") || queryKeys[0] == "category"))
         {
            _repository.UpdateBooksFromFilterUsingRawSql(query, "books", "list");
            var pages = query["pages"];
            if (pages != null)
            {
               string[] ar = pages.Split('-');
               if (Int32.TryParse(ar[0], out int min))
               {
                  ViewBag.MinPagesActual = min;
               }
               if (Int32.TryParse(ar[1], out int max))
               {
                  ViewBag.MaxPagesActual = max;
               }
            }
         }

         BooksListViewModel viewModel = new BooksListViewModel();
         if (currentCategory == null)
         {
            if (currentTag == null)
            {
               viewModel.Books = _repository.Books
                   .OrderByDescending(b => b.BookId)
                   .Skip((page - 1) * _booksPerPage)
                   .Take(_booksPerPage).ToList();
               viewModel.PagingInfo = new PagingInfo { CurrentPage = page, ItemsPerPage = _booksPerPage, TotalItems = _repository.Books.Count() };
               viewModel.CurrentCategory = null;
               viewModel.CurrentTag = null;
            }
            else //all categories & specific tag
            {
               viewModel.Books = _repository.Books.Where(b => b.BookTags.Select(x => x.TagId).Contains(currentTag.TagId))
                   .OrderByDescending(b => b.BookId)
                   .Skip((page - 1) * _booksPerPage)
                   .Take(_booksPerPage).ToList();
               viewModel.PagingInfo = new PagingInfo { CurrentPage = page, ItemsPerPage = _booksPerPage, TotalItems = _repository.Books.Where(b => b.BookTags.Select(x => x.TagId).Contains(currentTag.TagId)).Count() };
               viewModel.CurrentCategory = null;
               viewModel.CurrentTag = currentTag.NameForLinks;
            }
         }
         else //specific category & all tags
         {
            viewModel.Books = _repository.Books
                .Where(b => b.CategoryId == currentCategory.CategoryId)
                .OrderByDescending(b => b.BookId)
                .Skip((page - 1) * _booksPerPage)
                .Take(_booksPerPage).ToList();
            viewModel.PagingInfo = new PagingInfo
            {
               CurrentPage = page,
               ItemsPerPage = _booksPerPage,
               TotalItems = _repository.Books.Where(b => b.CategoryId == currentCategory.CategoryId).Count()
            };
            viewModel.CurrentCategory = currentCategory.NameForLinks;
            viewModel.CurrentTag = null;
         }
         if (requestManager.IsAjaxRequest(this))
         {
            return ViewComponent("ListContent", viewModel);
         }
         return View(viewModel);
      }

      [HttpGet]
      public ActionResult ListToAppend(int page, string category = null, string tag = null)
      {
         Category currentCategory = null;
         Tag currentTag = null;
         if (category != null)
         {
            currentCategory = _repository.Categories.Where(cat => cat.NameForLinks == category).FirstOrDefault();
            if (currentCategory == null) //category does not exist
            {
               return new EmptyResult();
            }
         }
         if (tag != null)
         {
            currentTag = _repository.Tags.Where(x => x.NameForLinks == tag).FirstOrDefault();
            if (currentTag == null) //tag does not exist
            {
               return new EmptyResult();
            }
         }

         IBooksRequestManager requestManager = this.BooksRequestManager;
         NameValueCollection query = requestManager.GetQueryStringWhenAppend(this);
         string[] queryKeys = query.AllKeys;
         if (queryKeys.Length > 0 && !(queryKeys.Length == 1 && (queryKeys[0] == "tag" || queryKeys[0] == "page") || queryKeys[0] == "category"))
         {
            _repository.UpdateBooksFromFilterUsingRawSql(query, "books", "list");
         }
         IEnumerable<Book> books = null;
         if (currentCategory == null) //all categories
         {
            if (currentTag == null) //all categories & all tags
            {
               books = _repository.Books
                   .OrderByDescending(b => b.BookId)
                   .Skip((page - 1) * _booksPerPage)
                   .Take(_booksPerPage).ToList();
            }
            else //all categories & specific tag
            {
               books = _repository.Books.Where(b => b.BookTags.Select(x => x.TagId).Contains(currentTag.TagId))
                   .OrderByDescending(b => b.BookId)
                   .Skip((page - 1) * _booksPerPage)
                   .Take(_booksPerPage).ToList();
            }
         }
         else //specific category
         {
            books = _repository.Books
                .Where(b => b.CategoryId == currentCategory.CategoryId)
                .OrderByDescending(b => b.BookId)
                .Skip((page - 1) * _booksPerPage)
                .Take(_booksPerPage);
         }

         if (books == null || books.Count() == 0)
         {
            return new EmptyResult();
         }
         return ViewComponent("BookRows", books);
      }

      public FileContentResult GetImage(int id)
      {
         Book book = _repository.Books.FirstOrDefault(p => p.BookId == id);
         if (book != null)
        {
            return File(book.ImageData, book.ImageMimeType);
         }
         else
         {
            return null;
         }
      }

      [HttpGet]
      public ActionResult Details(int id)
      {
         Book book = _repository.FindBook(id);
         if (book == null)
         {
            return NotFound();
         }
         return View(book);
      }
      // GET: Books/Create
      public ActionResult Create()
      {
         CreateEditBookViewModel model = new Models.CreateEditBookViewModel();
         ViewBag.Categories = new SelectList(_repository.Categories, "CategoryId", "NameForLabels");
         ViewBag.Languages = new SelectList(_repository.Languages, "LanguageId", "NameForLabels");
         model.Authors = _repository.Authors.OrderBy(x => x.LastName).Select(x => new SelectListItem { Value = x.AuthorId.ToString(), Text = x.LastName + " " + x.FirstName }).ToList();
         model.Tags = _repository.Tags.OrderBy(x => x.NameForLabels).Select(x => new SelectListItem { Value = x.TagId.ToString(), Text = x.NameForLabels }).ToList();
         return View(model);
      }

      //POST: Books/Create
      //To protect from overposting attacks, please enable the specific properties you want to bind to, for 
      // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Create([Bind("BookId", "Name", "LanguageId", "Pages", "Description", "CategoryId", "Year", "SelectedAuthors", "ImageData", "ImageMimeType", "SelectedTags")] CreateEditBookViewModel model, IFormFile image)
      {
         if (image?.Length > 0)
         {
            model.ImageMimeType = image.ContentType;
            model.ImageData = new byte[image.Length];
            using (Stream stream = image.OpenReadStream())
            {
               stream.Read(model.ImageData, 0, (int)image.Length);
            }
            ModelState.Remove("ImageData");
            ModelState.Remove("ImageMimeType");
         }

         if (ModelState.IsValid)
         {
            Book book = new Book();
            ICollection<Author> authors = _repository.Authors.Where(x => model.SelectedAuthors.Contains(x.AuthorId)).ToList();
            foreach (Author author in authors)
            {
               book.AuthorsOfBooks.Add(new AuthorsOfBooks {Book = book, Author = author });
            }
            ICollection<Tag> tags = _repository.Tags.Where(x => model.SelectedTags.Contains(x.TagId)).ToList();
            foreach (Tag tag in tags)
            {
               book.BookTags.Add(new BookTags { Tag = tag, Book = book });
            }
            book.CategoryId = model.CategoryId;
            book.Description = model.Description;
            book.LanguageId = model.LanguageId;
            book.Name = model.Name;
            book.Pages = model.Pages;
            book.Year = model.Year;
            book.ImageMimeType = model.ImageMimeType;
            book.ImageData = model.ImageData;
            _repository.AddBook(book);
            _repository.SaveChanges();
            return RedirectToAction("Details", new { id = book.BookId });
         }
         else
         {
            ViewBag.CategoryId = new SelectList(_repository.Categories, "CategoryId", "NameForLabels", model.CategoryId);
            ViewBag.LanguageId = new SelectList(_repository.Languages, "LanguageId", "NameForLabels", model.LanguageId);
            model.Authors = _repository.Authors.OrderBy(x => x.LastName).Select(x => new SelectListItem { Value = x.AuthorId.ToString(), Text = x.LastName + " " + x.FirstName }).ToList();
            model.Tags = _repository.Tags.OrderBy(x => x.NameForLabels).Select(x => new SelectListItem { Value = x.TagId.ToString(), Text = x.NameForLabels }).ToList();
            return View(model);
         }
      }

      //GET: Books/Edit/5
      [HttpGet]
      public ActionResult Edit(int id)
      {
         Book book = _repository.FindBook(id);
         if (book == null)
         {
            return NotFound();
         }
         CreateEditBookViewModel model = new CreateEditBookViewModel();
         model.BookId = book.BookId;
         model.Name = book.Name;
         model.Pages = book.Pages;
         model.Description = book.Description;
         model.Year = book.Year;
         model.ImageData = book.ImageData;
         model.ImageMimeType = book.ImageMimeType;
         model.LanguageId = book.LanguageId;
         model.CategoryId = book.CategoryId;
         model.SelectedAuthors = book.AuthorsOfBooks.Select(ab=>ab.Author).Select(a => a.AuthorId).ToList();
         model.Authors = _repository.Authors.OrderBy(x => x.LastName).Select(x => new SelectListItem { Value = x.AuthorId.ToString(), Text = x.LastName + " " + x.FirstName }).ToList();

         model.SelectedTags = book.BookTags.Select(a => a.TagId).ToList();
         model.Tags = _repository.Tags.OrderBy(x => x.NameForLabels).Select(x => new SelectListItem { Value = x.TagId.ToString(), Text = x.NameForLabels }).ToList();

         ViewBag.Categories = new SelectList(_repository.Categories, "CategoryId", "NameForLabels", book.CategoryId);
         ViewBag.Languages = new SelectList(_repository.Languages, "LanguageId", "NameForLabels", book.LanguageId);
         return View(model);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Edit([Bind("BookId", "Name", "LanguageId", "Pages", "Description", "CategoryId", "Year", "SelectedAuthors", "ImageData", "ImageMimeType", "SelectedTags")] CreateEditBookViewModel model, IFormFile image = null)
      {
         if (image?.Length > 0)
         {
            model.ImageMimeType = image.ContentType;
            model.ImageData = new byte[image.Length];
            using (Stream stream = image.OpenReadStream())
            {
               stream.Read(model.ImageData, 0, (int)image.Length);
            }
         }

         if (ModelState.IsValid)
         {
            Book book = _repository.Books.FirstOrDefault(x => x.BookId == model.BookId);
            if (book != null)
            {
               ICollection<Author> authors = _repository.Authors.Where(x => model.SelectedAuthors.Contains(x.AuthorId)).ToList();
               if (authors.Count > 0)
               {
                  book.AuthorsOfBooks.Clear();
                  foreach (Author author in authors)
                  {
                     book.AuthorsOfBooks.Add(new AuthorsOfBooks { Author = author, Book = book });
                  }
               }

               book.BookTags.Clear();
               if (model.SelectedTags != null)
               {
                  ICollection<Tag> tags = _repository.Tags.Where(x => model.SelectedTags.Contains(x.TagId)).ToList();
                  if (tags.Count > 0)
                  {
                     foreach (Tag tag in tags)
                     {
                        book.BookTags.Add(new BookTags {Tag = tag, Book = book });
                     }
                  }
               }

               book.CategoryId = model.CategoryId;
               book.Description = model.Description;
               book.LanguageId = model.LanguageId;
               book.Name = model.Name;
               book.Pages = model.Pages;
               book.Year = model.Year;
               book.ImageMimeType = model.ImageMimeType;
               book.ImageData = model.ImageData;
               _repository.SaveChanges();
               return RedirectToAction("Details", new { id = book.BookId });
            }
            else
            {
               return new StatusCodeResult((int)HttpStatusCode.BadRequest);
            }
         }
         else
         {
            ViewBag.CategoryId = new SelectList(_repository.Categories, "CategoryId", "NameForLabels", model.CategoryId);
            ViewBag.LanguageId = new SelectList(_repository.Languages, "LanguageId", "NameForLabels", model.LanguageId);
            model.Authors = _repository.Authors.OrderBy(x => x.LastName).Select(x => new SelectListItem { Value = x.AuthorId.ToString(), Text = x.LastName + " " + x.FirstName }).ToList();
            model.Tags = _repository.Tags.OrderBy(x => x.NameForLabels).Select(x => new SelectListItem { Value = x.TagId.ToString(), Text = x.NameForLabels }).ToList();
            return View(model);
         }
      }
      public ActionResult Delete(int? id)
      {
         if (id == null)
         {
            return new StatusCodeResult((int)HttpStatusCode.BadRequest);
         }
         Book book = _repository.FindBook(id.Value);
         if (book == null)
         {
            return NotFound();
         }
         return View(book);
      }

      // POST: Books/Delete/5
      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public ActionResult DeleteConfirmed(int id)
      {
         Book book = _repository.FindBook(id);
         if (book == null)
         {
            return NotFound();
         }
         _repository.RemoveBook(book);
         _repository.SaveChanges();
         return RedirectToAction("List");
      }

      protected override void Dispose(bool disposing)
      {
         if (disposing)
         {
            _repository.Dispose();
         }
         base.Dispose(disposing);
      }


   }
}
