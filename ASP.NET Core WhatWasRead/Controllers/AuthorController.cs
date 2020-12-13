using ASP.NET_Core_WhatWasRead.App_Data;
using ASP.NET_Core_WhatWasRead.App_Data.DBModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ASP.NET_Core_WhatWasRead.Controllers
{
   public class AuthorController : Controller
   {
      private IRepository _repository;

      public AuthorController(IRepository repo)
      {
         _repository = repo;
      }

      // GET: Author
      public ActionResult Index()
      {
         return View(_repository.Authors.OrderBy(a => a.LastName));
      }

      // GET: Author/Create
      public ActionResult Create()
      {
         return View();
      }

      // POST: Author/Create
      [HttpPost]
      public ActionResult Create([Bind("FirstName", "LastName")] Author model)
      {
         if (string.IsNullOrWhiteSpace(model.FirstName))
         {
            ModelState.AddModelError("firstname", "обязательное поле");
         }
         if (string.IsNullOrWhiteSpace(model.LastName))
         {
            ModelState.AddModelError("lastname", "обязательное поле");
         }
         if (ModelState.IsValid)
         {
            try
            {
               Author newAuthor = new Author { FirstName = model.FirstName, LastName = model.LastName };
               _repository.AddAuthor(newAuthor);
               _repository.SaveChanges();
               return RedirectToAction("Index");
            }
            catch
            {
               return View();
            }
         }
         return View(model);
      }

      // GET: Author/Edit/5
      public ActionResult Edit(int id)
      {
         Author author = _repository.Authors.FirstOrDefault(x => x.AuthorId == id);
         if (author == null)
         {
            return NotFound();
         }
         return View(author);
      }

      // POST: Author/Edit/5
      [HttpPost]
      public ActionResult Edit([Bind("AuthorId", "FirstName", "LastName")] Author model)
      {
         if (ModelState.IsValid)
         {
            Author author = _repository.Authors.FirstOrDefault(x => x.AuthorId == model.AuthorId);
            if (author != null)
            {
               author.FirstName = model.FirstName;
               author.LastName = model.LastName;
               _repository.SaveChanges();
               return RedirectToAction("Index");
            }
            else
            {
               return NotFound();
            }
         }
         return View(model);
      }

      // GET: Author/Delete/5
      public ActionResult Delete(int id)
      {
         Author author = _repository.Authors.Where(a => a.AuthorId == id).FirstOrDefault();
         if (author == null)
         {
            return NotFound();
         }
         return View((author,""));
      }

      // POST: Author/Delete/5
      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public ActionResult DeleteConfirmed(int id)
      {
         Author author = _repository.Authors.Where(a => a.AuthorId == id).FirstOrDefault();
         if (author == null)
         {
            return NotFound();
         }
         try
         {
            _repository.RemoveAuthor(author);
            _repository.SaveChanges();
         }
         catch (Exception ex) when (ex.InnerException != null && ex.InnerException.Message.Contains("DELETE statement conflicted with the REFERENCE constraint"))
         {
            return View((author, "У даного автора имеются книги, поэтому сейчас удалить его нельзя."));
         }

         catch (Exception)
         {
            return BadRequest();
         }
         return RedirectToAction("Index");
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
