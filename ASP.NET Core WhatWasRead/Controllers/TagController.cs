using ASP.NET_Core_WhatWasRead.App_Data;
using ASP.NET_Core_WhatWasRead.App_Data.DBModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ASP.NET_Core_WhatWasRead.Controllers
{
   public class TagController : Controller
   {
      private IRepository _repository;

      public TagController(IRepository repo)
      {
         _repository = repo;
      }
      // GET: Tag
      public ActionResult Index()
      {
         return View(_repository.Tags.OrderBy(a => a.NameForLabels));
      }

      // GET: Tag/Create
      public ActionResult Create()
      {
         return View();
      }

      // POST: Tag/Create
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Create([Bind("TagId", "NameForLabels", "NameForLinks")] Tag tag)
      {
         if (string.IsNullOrWhiteSpace(tag.NameForLabels))
         {
            ModelState.AddModelError("NameForLabels", "обязательное поле");
         }

         if (string.IsNullOrWhiteSpace(tag.NameForLinks))
         {
            ModelState.AddModelError("NameForLinks", "обязательное поле");
         }

         if (ModelState.IsValid)
         {
            try
            {
               _repository.AddTag(tag);
               _repository.SaveChanges();
               return RedirectToAction("Index");
            }
            catch (Exception)
            {
               return View();
            }
         }

         return View(tag);
      }

      // GET: Tag/Edit/5
      public ActionResult Edit(int? id)
      {
         Tag tag = _repository.Tags.FirstOrDefault(x => x.TagId == id);
         if (tag == null)
         {
            return NotFound();
         }
         return View(tag);
      }

      // POST: Tag/Edit/5
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Edit([Bind("TagId", "NameForLabels", "NameForLinks")] Tag model)
      {
         if (ModelState.IsValid)
         {
            Tag tag = _repository.Tags.FirstOrDefault(x => x.TagId == model.TagId);
            if (tag != null)
            {
               tag.NameForLabels = model.NameForLabels;
               tag.NameForLinks = model.NameForLinks;
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

      // GET: Tag/Delete/5
      public ActionResult Delete(int? id)
      {
         Tag tag = _repository.Tags.Where(a => a.TagId == id).FirstOrDefault();
         if (tag == null)
         {
            return NotFound();
         }
         return View((tag,""));
      }

      // POST: Tag/Delete/5
      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public ActionResult DeleteConfirmed(int id)
      {
         Tag tag = _repository.Tags.Where(t => t.TagId == id).FirstOrDefault();
         if (tag == null)
         {
            return NotFound();
         }
         try
         {
            _repository.RemoveTag(tag);
            _repository.SaveChanges();
         }
         catch (Exception ex) when (ex.InnerException != null && ex.InnerException.Message.Contains("DELETE statement conflicted with the REFERENCE constraint"))
         {
            return View((tag, "С данным тегом имеются книги, поэтому сейчас удалить его нельзя."));
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
