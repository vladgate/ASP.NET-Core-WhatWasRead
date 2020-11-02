using ASP.NET_Core_WhatWasRead.App_Data;
using ASP.NET_Core_WhatWasRead.App_Data.DBModels;
using ASP.NET_Core_WhatWasRead.Controllers;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Progress_UnitTests
{
   [TestFixture]
   public class AuthorControllerTest
   {
      private Author[] _authors = new Author[5]
{
         new Author {AuthorId = 1, FirstName = "F1", LastName="L1"},
         new Author {AuthorId = 2, FirstName = "F2", LastName="L2"},
         new Author {AuthorId = 3, FirstName = "F3", LastName="L3"},
         new Author {AuthorId = 4, FirstName = "F4", LastName="L4"},
         new Author {AuthorId = 5, FirstName = "F5", LastName="L5"},
};
      [Test]
      public void Index_ReturnsCorrectViewWithAllAuthors()
      {
         //Arrange
         IRepository mockRepo = Substitute.For<IRepository>();
         mockRepo.Authors.Returns(_authors);

         //Act
         AuthorController target = new AuthorController(mockRepo);
         ActionResult result = target.Index();

         //Assert
         Assert.IsInstanceOf<ViewResult>(result);
         IEnumerable<Author> model = (result as ViewResult).Model as IEnumerable<Author>;
         Assert.AreEqual(_authors.Count(), model.Count());
         Assert.AreEqual("L1", model.First().LastName);
      }

      [Test]
      public void Create_GET_ReturnsCorrectView()
      {
         //Arrange
         IRepository mockRepo = Substitute.For<IRepository>();

         //Act
         AuthorController target = new AuthorController(mockRepo);
         ActionResult result = target.Create();

         //Assert
         Assert.IsInstanceOf<ViewResult>(result);
      }

      [Test]
      public void Create_POST_InvalidModel_ReturnsCorrectView()
      {
         //Arrange
         IRepository mockRepo = Substitute.For<IRepository>();
         mockRepo.Authors.Returns(_authors);

         //Act
         Author invalidModel = new Author();
         AuthorController target = new AuthorController(mockRepo);
         ActionResult result = target.Create(invalidModel);

         //Assert
         Assert.IsInstanceOf<ViewResult>(result);
         Author model = (result as ViewResult).Model as Author;
         Assert.AreEqual(invalidModel, model);
      }

      [Test]
      public void Create_POST_ValidModel_ReturnsCorrectView()
      {
         //Arrange
         IRepository mockRepo = Substitute.For<IRepository>();
         Author validModel = new Author { FirstName = "FirstName", LastName = "LastName" };
         mockRepo.Authors.Returns(_authors);

         //Act
         AuthorController target = new AuthorController(mockRepo);
         ActionResult result = target.Create(validModel);

         //Assert
         mockRepo.Received(1).AddAuthor(Arg.Any<Author>());
         mockRepo.Received(1).SaveChanges();
         Assert.IsInstanceOf<RedirectToActionResult>(result);
         Assert.AreEqual("Index", (result as RedirectToActionResult).ActionName);
      }

      [Test]
      public void Edit_GET_InvalidId_ReturnsHttpNotFound()
      {
         //Arrange
         IRepository mockRepo = Substitute.For<IRepository>();
         int invalidid = 999;
         mockRepo.Authors.Returns(_authors);

         //Act
         AuthorController target = new AuthorController(mockRepo);
         ActionResult result = target.Edit(invalidid);

         //Assert
         Assert.IsInstanceOf<NotFoundResult>(result);
      }

      [Test]
      public void Edit_GET_ValidId_ReturnsCorrectView()
      {
         //Arrange
         IRepository mockRepo = Substitute.For<IRepository>();
         int validid = 1;
         mockRepo.Authors.Returns(_authors);
         Author expected = _authors.Where(a => a.AuthorId == validid).First();
         //Act
         AuthorController target = new AuthorController(mockRepo);
         ActionResult result = target.Edit(validid);

         //Assert
         Assert.IsInstanceOf<ViewResult>(result);
         Author model = (result as ViewResult).Model as Author;
         Assert.AreEqual(expected, model);
      }

      [Test]
      public void Edit_POST_InvalidModel_ReturnsCorrectView()
      {
         //Arrange
         IRepository mockRepo = Substitute.For<IRepository>();
         mockRepo.Authors.Returns(_authors);

         //Act
         Author invalidModel = new Author();
         AuthorController target = new AuthorController(mockRepo);
         target.ModelState.AddModelError("error", "some error");
         ActionResult result = target.Edit(invalidModel);

         //Assert
         Assert.IsInstanceOf<ViewResult>(result);
         Author model = (result as ViewResult).Model as Author;
         Assert.AreEqual(invalidModel, model);
      }

      [Test]
      public void Edit_POST_ValidModelWithValidId_ReturnsCorrectView()
      {
         //Arrange
         IRepository mockRepo = Substitute.For<IRepository>();
         mockRepo.Authors.Returns(_authors);
         int valiId = 1;
         Author validModel = new Author { AuthorId = valiId, FirstName = "FirstName", LastName = "LastName" };
         Author repoModel = _authors.Where(a => a.AuthorId == valiId).First();

         //Act
         AuthorController target = new AuthorController(mockRepo);
         ActionResult result = target.Edit(validModel);

         //Assert
         mockRepo.Received(1).SaveChanges();
         Assert.AreEqual(validModel.FirstName, repoModel.FirstName);
         Assert.AreEqual(validModel.LastName, repoModel.LastName);
         Assert.IsInstanceOf<RedirectToActionResult>(result);
         Assert.AreEqual("Index", (result as RedirectToActionResult).ActionName);
      }

      [Test]
      public void Edit_POST_ValidModelButInValidId_ReturnsHttpNotFound()
      {
         //Arrange
         IRepository mockRepo = Substitute.For<IRepository>();
         mockRepo.Authors.Returns(_authors);
         int invaliId = 100;
         Author validModel = new Author { AuthorId = invaliId, FirstName = "FirstName", LastName = "LastName" };

         //Act
         AuthorController target = new AuthorController(mockRepo);
         ActionResult result = target.Edit(validModel);

         //Assert
         Assert.IsInstanceOf<NotFoundResult>(result);
      }

      [Test]
      public void Delete_GET_InvalidId_ReturnsHttpNotFound()
      {
         //Arrange
         IRepository mockRepo = Substitute.For<IRepository>();
         int invalidid = 999;
         mockRepo.Authors.Returns(_authors);

         //Act
         AuthorController target = new AuthorController(mockRepo);
         ActionResult result = target.Delete(invalidid);

         //Assert
         Assert.IsInstanceOf<NotFoundResult>(result);
      }

      [Test]
      public void Delete_GET_ValidId_ReturnsCorrectView()
      {
         //Arrange
         IRepository mockRepo = Substitute.For<IRepository>();
         int validid = 1;
         mockRepo.Authors.Returns(_authors);
         Author expected = _authors.Where(a => a.AuthorId == validid).First();
         //Act
         AuthorController target = new AuthorController(mockRepo);
         ActionResult result = target.Delete(validid);

         //Assert
         Assert.IsInstanceOf<ViewResult>(result);
         Author model = (result as ViewResult).Model as Author;
         Assert.AreEqual(expected, model);
      }

      [Test]
      public void DeleteConfirmed_InvalidId_ReturnsHttpNotFound()
      {
         //Arrange
         IRepository mockRepo = Substitute.For<IRepository>();
         mockRepo.Authors.Returns(_authors);

         //Act
         int invalidId = 999;
         AuthorController target = new AuthorController(mockRepo);
         ActionResult result = target.DeleteConfirmed(invalidId);

         //Assert
         Assert.IsInstanceOf<NotFoundResult>(result);
      }

      [Test]
      public void DeleteConfirmed_ValidId_ReturnsCorrectView()
      {
         //Arrange
         IRepository mockRepo = Substitute.For<IRepository>();
         mockRepo.Authors.Returns(_authors);

         //Act
         int validId = 1;
         AuthorController target = new AuthorController(mockRepo);
         ActionResult result = target.DeleteConfirmed(validId);

         //Assert
         mockRepo.Received(1).RemoveAuthor(Arg.Any<Author>());
         mockRepo.Received(1).SaveChanges();
         Assert.IsInstanceOf<RedirectToActionResult>(result);
         Assert.AreEqual("Index", (result as RedirectToActionResult).ActionName);
      }

   }
}
