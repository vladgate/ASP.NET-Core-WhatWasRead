using System;
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.Specialized;
using ASP.NET_Core_WhatWasRead.App_Data.DBModels;
using NSubstitute;
using ASP.NET_Core_WhatWasRead.App_Data;
using ASP.NET_Core_WhatWasRead.Controllers;
using Microsoft.AspNetCore.Mvc;
using ASP.NET_Core_WhatWasRead.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace My_Progress_UnitTests
{
   [TestFixture]
   public class BooksControllerTest
   {

      #region InitVariables
      private Language[] _languages = new Language[3]
      {
         new Language {LanguageId = 1, NameForLabels="English", NameForLinks = "en"},
         new Language {LanguageId = 2, NameForLabels="Russian", NameForLinks = "ru"},
         new Language {LanguageId = 3, NameForLabels="Ukrainian", NameForLinks = "ua"},
      };

      private Author[] _authors = new Author[5]
{
         new Author {AuthorId = 1, FirstName = "F1", LastName="L1"},
         new Author {AuthorId = 2, FirstName = "F2", LastName="L2"},
         new Author {AuthorId = 3, FirstName = "F3", LastName="L3"},
         new Author {AuthorId = 4, FirstName = "F4", LastName="L4"},
         new Author {AuthorId = 5, FirstName = "F5", LastName="L5"},
};

      private Tag[] _tags = new Tag[3]
      {
         new Tag {TagId = 1, NameForLabels="Tag1", NameForLinks = "tag1"},
         new Tag {TagId = 2, NameForLabels="Tag2", NameForLinks = "tag2"},
         new Tag {TagId = 3, NameForLabels="Tag3", NameForLinks = "tag3"},
      };

      private Category[] _categories = new Category[3]
      {
        new Category() { CategoryId = 1, NameForLinks = "cat1", NameForLabels = "Category 1" },
        new Category() { CategoryId = 2, NameForLinks = "cat2", NameForLabels = "Category 2" },
        new Category() { CategoryId = 3, NameForLinks = "cat3", NameForLabels = "Category 3" }
      };

      private string _imageMimeType = "image/jpeg";

      private Book[] CreateBooks()
      {
         Book book1 = new Book
         {
            BookId = 1,
            Category = _categories[0],
            CategoryId = _categories[0].CategoryId,
            Pages = 10,
            Description = "Desc1",
            Name = "Book1",
            Year = 2001,
            Language = _languages[0],
            LanguageId = _languages[0].LanguageId,
            ImageMimeType = _imageMimeType,
            ImageData = new byte[] { 1 },
         };
         book1.AuthorsOfBooks = new List<AuthorsOfBooks>() { new AuthorsOfBooks { AuthorId = _authors[0].AuthorId, Author = _authors[0], BookId = book1.BookId, Book = book1 } };
         book1.BookTags = new List<BookTags>() { new BookTags { TagId = _tags[0].TagId, Tag = _tags[0], BookId = book1.BookId, Book = book1 } };

         Book book2 = new Book
         {
            BookId = 2,
            Category = _categories[0],
            CategoryId = _categories[0].CategoryId,
            Pages = 20,
            Description = "Desc2",
            Name = "Book2",
            Year = 2002,
            Language = _languages[0],
            LanguageId = _languages[0].LanguageId,
            ImageMimeType = _imageMimeType,
            ImageData = new byte[] { 2 },
         };
         book2.AuthorsOfBooks = new List<AuthorsOfBooks>() { new AuthorsOfBooks {AuthorId = _authors[0].AuthorId, Author = _authors[0], Book = book2, BookId = book2.BookId }, new AuthorsOfBooks { AuthorId = _authors[1].AuthorId,  Author = _authors[1], Book = book2, BookId = book2.BookId } };
         book2.BookTags = new List<BookTags>() { new BookTags { TagId = _tags[0].TagId, Tag = _tags[0], BookId = book2.BookId, Book = book2 }, new BookTags { TagId = _tags[1].TagId, Tag = _tags[1], BookId = book2.BookId, Book = book2 } };


         Book book3 = new Book
         {
            BookId = 3,
            Category = _categories[0],
            CategoryId = _categories[0].CategoryId,
            Pages = 30,
            Description = "Desc3",
            Name = "Book3",
            Year = 2003,
            Language = _languages[1],
            LanguageId = _languages[1].LanguageId,
            ImageMimeType = _imageMimeType,
            ImageData = new byte[] { 3 },
         };
         book3.AuthorsOfBooks = new List<AuthorsOfBooks>() { new AuthorsOfBooks {AuthorId = _authors[1].AuthorId, Author = _authors[1], BookId = book3.BookId, Book = book3 }, new AuthorsOfBooks { AuthorId = _authors[2].AuthorId, Author = _authors[2], Book = book3, BookId = book3.BookId }, new AuthorsOfBooks { AuthorId = _authors[3].AuthorId, Author = _authors[3], Book = book3, BookId = book3.BookId } };
         book3.BookTags = new List<BookTags>() { new BookTags { TagId = _tags[1].TagId, Tag = _tags[1], BookId = book3.BookId, Book = book3 }, new BookTags { TagId = _tags[2].TagId, Tag = _tags[2], Book = book3,  BookId = book3.BookId, } };

         Book book4 = new Book
         {
            BookId = 4,
            Category = _categories[0],
            CategoryId = _categories[0].CategoryId,
            Pages = 40,
            Description = "Desc4",
            Name = "Book4",
            Year = 2004,
            Language = _languages[1],
            LanguageId = _languages[1].LanguageId,
            ImageMimeType = _imageMimeType,
            ImageData = new byte[] { 4 }
         };
         book4.AuthorsOfBooks = new List<AuthorsOfBooks>() { new AuthorsOfBooks { AuthorId = _authors[3].AuthorId, Author = _authors[3], BookId = book3.BookId, Book = book3 } };
         book4.BookTags = new List<BookTags>() { new BookTags { TagId = _tags[0].TagId, Tag = _tags[0], BookId = book3.BookId, Book = book3 }, new BookTags { TagId = _tags[1].TagId, Tag = _tags[1], Book = book3, BookId = book3.BookId } };

         Book book5 = new Book
         {
            BookId = 5,
            Category = _categories[1],
            CategoryId = _categories[1].CategoryId,
            Pages = 50,
            Description = "Desc5",
            Name = "Book5",
            Year = 2005,
            Language = _languages[2],
            LanguageId = _languages[2].LanguageId,
            ImageMimeType = _imageMimeType,
            ImageData = new byte[] { 5 },
         };
         book5.AuthorsOfBooks = new List<AuthorsOfBooks>() { new AuthorsOfBooks { AuthorId = _authors[3].AuthorId, Author = _authors[3], BookId = book3.BookId, Book = book3 }, new AuthorsOfBooks { AuthorId = _authors[4].AuthorId, Author = _authors[4], BookId = book3.BookId, Book = book3 } };
         book5.BookTags = new List<BookTags>() { new BookTags { TagId = _tags[2].TagId, Tag = _tags[2], BookId = book3.BookId, Book = book3 } };

         return new Book[] { book1, book2, book3, book4, book5 };
      }
      #endregion

      #region GetImage
      [Test]
      public void GetImage_ValidId_ReturnsCorrectByteArray()
      {
         //Arrange
         IRepository mockRepo = Substitute.For<IRepository>();
         Book[] books = CreateBooks();
         mockRepo.Books.Returns(books);

         //Act
         BooksController controller = new BooksController(mockRepo);
         FileContentResult actualResult = controller.GetImage(5);
         byte[] actual = actualResult.FileContents;
         string mimeType = actualResult.ContentType;
         //Assert
         Assert.AreEqual(new byte[] { 5 }, actual);
         Assert.AreEqual(_imageMimeType, mimeType);
      }

      [Test]
      public void GetImage_InvalidId_ReturnsNull()
      {
         //Arrange
         IRepository mockRepo = Substitute.For<IRepository>();
         Book[] books = CreateBooks();
         mockRepo.Books.Returns(books);

         //Act
         BooksController controller = new BooksController(mockRepo);
         int nonExistId = 10;
         FileResult actualResult = controller.GetImage(nonExistId);
         //Assert
         Assert.IsNull(actualResult);
      }

      #endregion
      #region Details
      [Test]
      public void Details_InvalidId_ReturnsHttpNotFoundView()
      {
         //Arrange
         IRepository mockRepo = Substitute.For<IRepository>();
         Book[] books = CreateBooks();
         mockRepo.Books.Returns(books);
         int invalidId = 999;
         mockRepo.FindBook(invalidId).Returns(null as Book);

         //Act
         BooksController controller = new BooksController(mockRepo);
         ActionResult result = controller.Details(invalidId);

         //Assert
         Assert.IsInstanceOf<NotFoundResult>(result);
      }

      [Test]
      public void Details_ValidId_ReturnsCorrectView()
      {
         //Arrange
         IRepository mockRepo = Substitute.For<IRepository>();
         Book[] books = CreateBooks();
         mockRepo.Books.Returns(books);
         int validId = 1;
         Book expected = books.Where(b => b.BookId == validId).FirstOrDefault();
         mockRepo.FindBook(validId).Returns(expected);

         //Act
         BooksController controller = new BooksController(mockRepo);
         ActionResult result = controller.Details(validId);

         //Assert
         Assert.IsInstanceOf<ViewResult>(result);
         Assert.IsTrue((result as ViewResult).Model == expected);

      }
      #endregion
      #region Delete
      [Test]
      public void Delete_NullId_ReturnsBadRequestStatusCode()
      {
         //arrange
         IRepository mockRepo = Substitute.For<IRepository>();
         Book[] books = CreateBooks();
         mockRepo.Books.Returns(books);
         int? nullId = null;

         //act
         BooksController controller = new BooksController(mockRepo);
         ActionResult result = controller.Delete(nullId);

         //assert
         Assert.IsInstanceOf<StatusCodeResult>(result);
         Assert.IsTrue((result as StatusCodeResult).StatusCode == 400);
      }

      [Test]
      public void Delete_InvalidId_ReturnsHttpNotFound()
      {
         //arrange
         IRepository mockRepo = Substitute.For<IRepository>();
         Book[] books = CreateBooks();
         mockRepo.Books.Returns(books);
         int invalidId = 999;

         //act
         BooksController controller = new BooksController(mockRepo);
         ActionResult result = controller.Delete(invalidId);

         //assert
         Assert.IsInstanceOf<NotFoundResult>(result);
      }

      [Test]
      public void Delete_ValidId_ReturnsCorrectView()
      {
         //arrange
         IRepository mockRepo = Substitute.For<IRepository>();
         Book[] books = CreateBooks();
         mockRepo.Books.Returns(books);
         int validId = 1;
         Book expectedBook = books.Where(b => b.BookId == validId).FirstOrDefault();
         mockRepo.FindBook(validId).Returns(expectedBook);

         //act
         BooksController controller = new BooksController(mockRepo);
         ActionResult result = controller.Delete(validId);

         //assert
         Assert.IsInstanceOf<ViewResult>(result);
         Assert.AreSame(expectedBook, (result as ViewResult).Model);
      }

      [Test]
      public void DeleteConfirmed_InvalidId_ReturnsHttpNotFound()
      {
         //arrange
         IRepository mockRepo = Substitute.For<IRepository>();
         Book[] books = CreateBooks();
         mockRepo.Books.Returns(books);
         int invalidId = 999;

         //act
         BooksController controller = new BooksController(mockRepo);
         ActionResult result = controller.DeleteConfirmed(invalidId);

         //assert
         Assert.IsInstanceOf<NotFoundResult>(result);
      }

      [Test]
      public void DeleteConfirmed_ValidId_DeletedCorrectly()
      {
         //arrange
         IRepository mockRepo = Substitute.For<IRepository>();
         Book[] books = CreateBooks();
         mockRepo.Books.Returns(books);
         int validId = 1;
         Book expectedBook = books.Where(b => b.BookId == validId).FirstOrDefault();
         mockRepo.FindBook(validId).Returns(expectedBook);
         mockRepo.RemoveBook(Arg.Any<Book>());

         //act
         BooksController controller = new BooksController(mockRepo);
         ActionResult result = controller.DeleteConfirmed(validId);
         RedirectToActionResult result2 = result as RedirectToActionResult;

         //assert
         mockRepo.Received(1).RemoveBook(expectedBook);
         Assert.IsInstanceOf<RedirectToActionResult>(result);
         Assert.IsTrue(result2.ActionName == "List");
      }

      #endregion
      #region Edit
      [Test]
      public void Edit_InvalidId_ReturnsHttpNotFound()
      {
         //arrange
         IRepository mockRepo = Substitute.For<IRepository>();
         Book[] books = CreateBooks();
         mockRepo.Books.Returns(books);
         int invalidId = 999;

         //act
         BooksController controller = new BooksController(mockRepo);
         ActionResult result = controller.Edit(invalidId);

         //assert
         Assert.IsInstanceOf<NotFoundResult>(result);
      }

      [Test]
      public void Edit_GET_ValidId_ReturnsCorrectModel()
      {
         //arrange
         IRepository mockRepo = Substitute.For<IRepository>();
         Book[] books = CreateBooks();
         mockRepo.Books.Returns(books);
         int validId = 1;
         Book editedBook = books.Where(b => b.BookId == validId).FirstOrDefault();
         mockRepo.FindBook(validId).Returns(editedBook);
         mockRepo.Authors.Returns(_authors);
         mockRepo.Tags.Returns(_tags);
         mockRepo.Categories.Returns(_categories);
         mockRepo.Languages.Returns(_languages);

         //act
         BooksController controller = new BooksController(mockRepo);
         ActionResult result = controller.Edit(validId);
         ViewResult viewResult = result as ViewResult;
         SelectList categories = viewResult.ViewData["Categories"] as SelectList;
         SelectList languages = viewResult.ViewData["Languages"] as SelectList;
         CreateEditBookViewModel model = viewResult.Model as CreateEditBookViewModel;

         //assert
         Assert.IsInstanceOf<ViewResult>(result);
         Assert.AreEqual(editedBook.AuthorsOfBooks.Select(a => a.AuthorId).AsEnumerable(), model.SelectedAuthors);
         Assert.AreEqual(_authors.Count(), model.Authors.Count());
         Assert.AreEqual(editedBook.BookTags.Select(a => a.TagId).AsEnumerable(), model.SelectedTags);
         Assert.AreEqual(_tags.Count(), model.Tags.Count());
         Assert.IsInstanceOf<SelectList>(viewResult.ViewData["Categories"]);
         Assert.IsInstanceOf<SelectList>(viewResult.ViewData["Languages"]);
         Assert.IsTrue(categories.Count() == mockRepo.Categories.Count());
         Assert.IsTrue(languages.Count() == mockRepo.Languages.Count());
      }

      [Test]
      public void Edit_POST_InvalidBookId_ReturnsHttpBadRequest()
      {
         //arrange
         IRepository mockRepo = Substitute.For<IRepository>();
         Book[] books = CreateBooks();
         mockRepo.Books.Returns(books);
         int invalidId = 999;
         Book editedBook = books.Where(b => b.BookId == invalidId).FirstOrDefault();

         //act
         BooksController controller = new BooksController(mockRepo);
         CreateEditBookViewModel model = new CreateEditBookViewModel();
         ActionResult result = controller.Edit(model, null);

         //assert
         Assert.IsInstanceOf<StatusCodeResult>(result);
         Assert.IsTrue((result as StatusCodeResult).StatusCode == 400);
      }

      [Test]
      public void Edit_POST_ValidModel_ReturnsCorrectModel()
      {
         //arrange
         IRepository mockRepo = Substitute.For<IRepository>();
         Book[] books = CreateBooks();
         mockRepo.Books.Returns(books);
         int validId = 1;
         Book editedBook = books.Where(b => b.BookId == validId).FirstOrDefault();

         //act
         BooksController controller = new BooksController(mockRepo);
         CreateEditBookViewModel model = CreateCreateEditBookViewModel(editedBook, mockRepo);
         ActionResult result = controller.Edit(model, null);
         RedirectToActionResult result2 = result as RedirectToActionResult;

         //assert
         Assert.IsInstanceOf<RedirectToActionResult>(result);
         Assert.IsTrue(result2.ActionName == "Details");
         Assert.IsTrue(result2.RouteValues["id"].Equals(validId));
      }

      private CreateEditBookViewModel CreateCreateEditBookViewModel(Book book, IRepository repository)
      {
         if (book == null)
         {
            throw new ArgumentNullException(nameof(book));
         }
         if (repository == null)
         {
            throw new ArgumentNullException(nameof(repository));
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
         model.SelectedAuthors = book.AuthorsOfBooks.Select(a => a.AuthorId).ToList();
         model.Authors = repository.Authors.OrderBy(x => x.LastName).Select(x => new SelectListItem { Value = x.AuthorId.ToString(), Text = x.LastName + " " + x.FirstName }).ToList();

         model.SelectedTags = book.BookTags.Select(a => a.TagId).ToList();
         model.Tags = repository.Tags.OrderBy(x => x.NameForLabels).Select(x => new SelectListItem { Value = x.TagId.ToString(), Text = x.NameForLabels }).ToList();
         return model;
      }

      #endregion
      #region Create
      [Test]
      public void Create_GET_ReturnsCorrectView()
      {
         //arrange
         IRepository mockRepo = Substitute.For<IRepository>();
         mockRepo.Authors.Returns(_authors);
         mockRepo.Tags.Returns(_tags);
         mockRepo.Categories.Returns(_categories);
         mockRepo.Languages.Returns(_languages);

         //act
         BooksController controller = new BooksController(mockRepo);
         ActionResult result = controller.Create();
         ViewResult viewResult = result as ViewResult;
         SelectList categories = viewResult.ViewData["Categories"] as SelectList;
         SelectList languages = viewResult.ViewData["Languages"] as SelectList;
         CreateEditBookViewModel model = (result as ViewResult).Model as CreateEditBookViewModel;

         //assert
         Assert.IsInstanceOf<ViewResult>(result);
         Assert.IsInstanceOf<SelectList>(viewResult.ViewData["Categories"]);
         Assert.IsInstanceOf<SelectList>(viewResult.ViewData["Languages"]);
         Assert.IsTrue(categories.Count() == mockRepo.Categories.Count());
         Assert.IsTrue(languages.Count() == mockRepo.Languages.Count());
         Assert.AreEqual(_authors.Count(), model.Authors.Count());
         Assert.AreEqual(_tags.Count(), model.Tags.Count());
      }

      [Test]
      public void Create_POST_ValidModel_AddAndRedirectCorrectly()
      {
         //arrange
         IRepository mockRepo = Substitute.For<IRepository>();
         List<Book> books = CreateBooks().ToList();
         Book newBook = new Book { Name = "NewBook" };
         mockRepo.Books.Returns(books);
         int counter = 0;
         mockRepo.When(b => b.AddBook(Arg.Any<Book>())).Do(Callback.First(b => books.Add(newBook)).AndAlways(x => counter++));
         //mockRepo.Setup(m => m.AddBook(It.IsAny<Book>())).Callback(() => books.Add(newBook));
         mockRepo.Authors.Returns(_authors);
         mockRepo.Tags.Returns(_tags);

         //act
         BooksController controller = new BooksController(mockRepo);
         CreateEditBookViewModel model = CreateCreateEditBookViewModel(newBook, mockRepo);
         ActionResult result = controller.Create(model, null);
         RedirectToActionResult result2 = result as RedirectToActionResult;

         //assert
         Assert.IsInstanceOf<RedirectToActionResult>(result);
         Assert.AreEqual(newBook.Name, books.Last().Name);
         Assert.IsTrue(result2.ActionName == "Details");
         Assert.IsTrue(counter == 1);
      }
      [Test]
      public void Create_POST_InvalidModel_ReturnCorrectView()
      {
         //arrange
         IRepository mockRepo = Substitute.For<IRepository>();
         Book newBook = new Book { Name = "NewBook" };
         mockRepo.Authors.Returns(_authors);
         mockRepo.Tags.Returns(_tags);
         mockRepo.Categories.Returns(_categories);
         mockRepo.Languages.Returns(_languages);

         //act
         BooksController controller = new BooksController(mockRepo);
         controller.ModelState.AddModelError("error", "model error");
         CreateEditBookViewModel model = CreateCreateEditBookViewModel(newBook, mockRepo);
         ActionResult result = controller.Create(model, null);
         ViewResult viewResult = result as ViewResult;
         SelectList categories = viewResult.ViewData["CategoryId"] as SelectList;
         SelectList languages = viewResult.ViewData["LanguageId"] as SelectList;

         //assert
         Assert.IsInstanceOf<ViewResult>(result);
         Assert.AreEqual(model, viewResult.Model);
         Assert.IsTrue(categories.Count() == mockRepo.Categories.Count());
         Assert.IsTrue(languages.Count() == mockRepo.Languages.Count());
         Assert.AreEqual(_authors.Count(), model.Authors.Count());
         Assert.AreEqual(_tags.Count(), model.Tags.Count());
      }

      #endregion
      #region List

      [Test]
      public void List_DefaultParameters_ReturnsCorrectCountOfBooks()
      {
         //Arrange
         IRepository mockRepo = Substitute.For<IRepository>();
         Book[] books = CreateBooks();
         mockRepo.Books.Returns(books);
         BooksController target = new BooksController(mockRepo);
         IBooksRequestManager mockRequestManager = Substitute.For<IBooksRequestManager>();
         NameValueCollection emptyCollection = new System.Collections.Specialized.NameValueCollection();
         mockRequestManager.GetQueryString(Arg.Any<BooksController>()).Returns(emptyCollection);
         target.BooksRequestManager = mockRequestManager;
         int booksPerPage = 4;
         //Act
         ActionResult result = target.List();
         BooksListViewModel model = ((ViewResult)result).Model as BooksListViewModel;

         //Assert
         Assert.IsInstanceOf<ViewResult>(result);
         Assert.AreEqual(booksPerPage, model.Books.Count());
      }

      [Test]
      public void List_DefaultParametersWhenNotAjaxRequest_ReturnsCorrectOrderOfBooks()
      {
         //Arrange
         IRepository mockRepo = Substitute.For<IRepository>();
         Book[] books = CreateBooks();
         mockRepo.Books.Returns(books);
         BooksController target = new BooksController(mockRepo);
         IBooksRequestManager mockRequestManager = Substitute.For<IBooksRequestManager>();
         NameValueCollection emptyCollection = new System.Collections.Specialized.NameValueCollection();
         mockRequestManager.GetQueryString(Arg.Any<BooksController>()).Returns(emptyCollection);
         target.BooksRequestManager = mockRequestManager;
         //Act
         ActionResult result = target.List();
         BooksListViewModel model = ((ViewResult)result).Model as BooksListViewModel;
         Book[] booksResult = model.Books.ToArray();
         //Assert
         Assert.IsInstanceOf<ViewResult>(result);
         Assert.AreSame(books[4], booksResult[0]);
         Assert.AreSame(books[3], booksResult[1]);
         Assert.AreSame(books[2], booksResult[2]);
         Assert.AreSame(books[1], booksResult[3]);
      }
      [Test]
      public void List_DefaultParametersWhenAjaxRequest_ReturnsCorrectOrderOfBooks()
      {
         //Arrange
         IRepository mockRepo = Substitute.For<IRepository>();
         Book[] books = CreateBooks();
         mockRepo.Books.Returns(books);
         BooksController target = new BooksController(mockRepo);
         IBooksRequestManager mockRequestManager = Substitute.For<IBooksRequestManager>();
         NameValueCollection emptyCollection = new System.Collections.Specialized.NameValueCollection();
         mockRequestManager.GetQueryString(Arg.Any<BooksController>()).Returns(emptyCollection);
         mockRequestManager.IsAjaxRequest(Arg.Any<BooksController>()).Returns(true);
         target.BooksRequestManager = mockRequestManager;
         //Act
         ActionResult result = target.List();
         BooksListViewModel model = ((ViewComponentResult)result).Arguments as BooksListViewModel;
         Book[] booksResult = model.Books.ToArray();
         //Assert
         Assert.IsInstanceOf<ViewComponentResult>(result);
         Assert.AreSame(books[4], booksResult[0]);
         Assert.AreSame(books[3], booksResult[1]);
         Assert.AreSame(books[2], booksResult[2]);
         Assert.AreSame(books[1], booksResult[3]);
      }


      [Test]
      public void List_InvalidCategory_ReturnsNotFoundView()
      {
         //Arrange
         IRepository mockRepo = Substitute.For<IRepository>();
         mockRepo.Categories.Returns(_categories);
         BooksController target = new BooksController(mockRepo);
         IBooksRequestManager mockRequestManager = Substitute.For<IBooksRequestManager>();
         string invalidCategory = "invalidCategory";
         int page = 1;

         //Act
         ActionResult result = target.List(page, invalidCategory);

         //Assert
         Assert.IsInstanceOf<ViewResult>(result);
         Assert.AreEqual("_NotFoundView", (result as ViewResult).ViewName);
      }

      [TestCase("cat1", 1, 4)]
      [TestCase("cat1", 2, 0)]
      public void List_ValidCategoryNoFilter_ReturnsCorrectCountOfBooksOnParticularPage(string category, int page, int expectedCount)
      {
         //Arrange
         IRepository mockRepo = Substitute.For<IRepository>();
         Book[] books = CreateBooks();
         mockRepo.Categories.Returns(_categories);
         mockRepo.Books.Returns(books);
         BooksController target = new BooksController(mockRepo);
         IBooksRequestManager mockRequestManager = Substitute.For<IBooksRequestManager>();
         NameValueCollection emptyCollection = new System.Collections.Specialized.NameValueCollection();
         mockRequestManager.GetQueryString(Arg.Any<BooksController>()).Returns(emptyCollection);
         target.BooksRequestManager = mockRequestManager;

         //Act
         ActionResult result = target.List(page, category);
         BooksListViewModel model = ((ViewResult)result).Model as BooksListViewModel;
         Book[] booksResult = model.Books.ToArray();

         //Assert
         Assert.IsInstanceOf<ViewResult>(result);
         Assert.AreEqual(expectedCount, booksResult.Count());
      }

      [TestCase("cat1", "lang", "en", 1, 2)]
      [TestCase("cat1", "lang", "en", 2, 0)]
      public void List_ValidCategoryWithFilter_ReturnsCorrectCountOfBooksOnParticularPage(string category, string queryKey, string queryValue, int page, int expectedCount)
      {
         //Arrange
         IRepository mockRepo = Substitute.For<IRepository>();
         Book[] books = CreateBooks();
         mockRepo.Categories.Returns(_categories);
         mockRepo.Books.Returns(books.Where(b => b.Language.NameForLinks == queryValue)); //apply filter
         NameValueCollection queryCollection = new System.Collections.Specialized.NameValueCollection();
         queryCollection.Add(queryKey, queryValue);
         mockRepo.UpdateBooksFromFilterUsingRawSql(queryCollection, Arg.Any<string>(), Arg.Any<string>()).Returns(mockRepo.Books);
         BooksController target = new BooksController(mockRepo);

         IBooksRequestManager mockRequestManager = Substitute.For<IBooksRequestManager>();
         mockRequestManager.GetQueryString(Arg.Any<BooksController>()).Returns(queryCollection);
         target.BooksRequestManager = mockRequestManager;

         //Act
         ActionResult result = target.List(page, category);
         BooksListViewModel model = ((ViewResult)result).Model as BooksListViewModel;
         Book[] booksResult = model.Books.ToArray();

         //Assert
         Assert.IsInstanceOf<ViewResult>(result);
         Assert.AreEqual(expectedCount, booksResult.Count());
      }

      [TestCase("cat1", 1, 4, 0)]
      [TestCase("cat1", 1, 3, 1)]
      public void List_ValidCategoryNoFilter_ReturnsCorrectOrderOfBooks(string category, int page, int bookId, int bookIndex)
      {
         //Arrange
         IRepository mockRepo = Substitute.For<IRepository>();
         Book[] books = CreateBooks();
         mockRepo.Categories.Returns(_categories);
         mockRepo.Books.Returns(books);

         BooksController target = new BooksController(mockRepo);
         IBooksRequestManager mockRequestManager = Substitute.For<IBooksRequestManager>();
         NameValueCollection emptyCollection = new System.Collections.Specialized.NameValueCollection();
         mockRequestManager.GetQueryString(Arg.Any<BooksController>()).Returns(emptyCollection);
         target.BooksRequestManager = mockRequestManager;

         //Act
         BooksListViewModel result = ((ViewResult)target.List(page, category)).Model as BooksListViewModel;
         Book[] booksResult = result.Books.ToArray();

         //Assert
         Assert.AreEqual(bookId, booksResult[bookIndex].BookId);
      }

      [TestCase("cat1", "lang", "en", 1, 1, 1)]
      [TestCase("cat1", "lang", "en", 1, 2, 0)]
      public void List_ValidCategoryWithFilter_ReturnsCorrectOrderOfBooks(string category, string queryKey, string queryValue, int page, int bookId, int bookIndex)
      {
         //Arrange
         IRepository mockRepo = Substitute.For<IRepository>();
         Book[] books = CreateBooks();
         mockRepo.Categories.Returns(_categories);
         mockRepo.Books.Returns(books.Where(b => b.Language.NameForLinks == queryValue)); //apply filter
         NameValueCollection queryCollection = new System.Collections.Specialized.NameValueCollection();
         queryCollection.Add(queryKey, queryValue);
         mockRepo.UpdateBooksFromFilterUsingRawSql(queryCollection, Arg.Any<string>(), Arg.Any<string>()).Returns(mockRepo.Books);
         BooksController target = new BooksController(mockRepo);

         IBooksRequestManager mockRequestManager = Substitute.For<IBooksRequestManager>();
         NameValueCollection emptyCollection = new System.Collections.Specialized.NameValueCollection();
         mockRequestManager.GetQueryString(Arg.Any<BooksController>()).Returns(emptyCollection);
         target.BooksRequestManager = mockRequestManager;

         //Act
         BooksListViewModel result = ((ViewResult)target.List(page, category)).Model as BooksListViewModel;
         Book[] booksResult = result.Books.ToArray();

         //Assert
         Assert.AreEqual(bookId, booksResult[bookIndex].BookId);
      }

      [Test]
      public void List_InvalidTag_ReturnsNotFoundView()
      {
         //Arrange
         IRepository mockRepo = Substitute.For<IRepository>();
         mockRepo.Categories.Returns(_categories);
         BooksController target = new BooksController(mockRepo);

         IBooksRequestManager mockRequestManager = Substitute.For<IBooksRequestManager>();
         string invalidTag = "invalidTag";
         int page = 1;

         //Act
         ActionResult result = target.List(page, invalidTag);

         //Assert
         Assert.IsInstanceOf<ViewResult>(result);
         Assert.AreEqual("_NotFoundView", (result as ViewResult).ViewName);
      }

      [TestCase("tag1", 1, 3)]
      [TestCase("tag1", 2, 0)]
      public void List_ValidTagNoFilter_ReturnsCorrectCountOfBooksOnParticularPage(string tag, int page, int expectedCount)
      {
         //Arrange
         IRepository mockRepo = Substitute.For<IRepository>();
         Book[] books = CreateBooks();
         mockRepo.Tags.Returns(_tags);
         mockRepo.Books.Returns(books);
         BooksController target = new BooksController(mockRepo);

         IBooksRequestManager mockRequestManager = Substitute.For<IBooksRequestManager>();
         NameValueCollection emptyCollection = new System.Collections.Specialized.NameValueCollection();
         mockRequestManager.GetQueryString(Arg.Any<BooksController>()).Returns(emptyCollection);
         target.BooksRequestManager = mockRequestManager;

         //Act
         ActionResult result = target.List(page, null, tag);
         BooksListViewModel model = ((ViewResult)result).Model as BooksListViewModel;
         Book[] booksResult = model.Books.ToArray();

         //Assert
         Assert.IsInstanceOf<ViewResult>(result);
         Assert.AreEqual(expectedCount, booksResult.Count());
      }

      [TestCase("tag1", "lang", "en", 1, 2)]
      [TestCase("tag1", "lang", "en", 2, 0)]
      public void List_ValidTagWithFilter_ReturnsCorrectCountOfBooksOnParticularPage(string tag, string queryKey, string queryValue, int page, int expectedCount)
      {
         //Arrange
         IRepository mockRepo = Substitute.For<IRepository>();
         Book[] books = CreateBooks();
         mockRepo.Tags.Returns(_tags);
         mockRepo.Books.Returns(books.Where(b => b.Language.NameForLinks == queryValue)); //apply filter
         NameValueCollection queryCollection = new System.Collections.Specialized.NameValueCollection();
         queryCollection.Add(queryKey, queryValue);
         mockRepo.UpdateBooksFromFilterUsingRawSql(queryCollection, Arg.Any<string>(), Arg.Any<string>()).Returns(mockRepo.Books);
         BooksController target = new BooksController(mockRepo);

         IBooksRequestManager mockRequestManager = Substitute.For<IBooksRequestManager>();
         mockRequestManager.GetQueryString(Arg.Any<BooksController>()).Returns(queryCollection);
         target.BooksRequestManager = mockRequestManager;

         //Act
         ActionResult result = target.List(page, null, tag);
         BooksListViewModel model = ((ViewResult)result).Model as BooksListViewModel;
         Book[] booksResult = model.Books.ToArray();

         //Assert
         Assert.IsInstanceOf<ViewResult>(result);
         Assert.AreEqual(expectedCount, booksResult.Count());
      }

      [TestCase("tag1", 1, 4, 0)]
      [TestCase("tag1", 1, 2, 1)]
      public void List_ValidTagNoFilter_ReturnsCorrectOrderOfBooks(string tag, int page, int bookId, int bookIndex)
      {
         //Arrange
         IRepository mockRepo = Substitute.For<IRepository>();
         Book[] books = CreateBooks();
         mockRepo.Tags.Returns(_tags);
         mockRepo.Books.Returns(books);

         BooksController target = new BooksController(mockRepo);
         IBooksRequestManager mockRequestManager = Substitute.For<IBooksRequestManager>();
         NameValueCollection emptyCollection = new System.Collections.Specialized.NameValueCollection();
         mockRequestManager.GetQueryString(Arg.Any<BooksController>()).Returns(emptyCollection);
         target.BooksRequestManager = mockRequestManager;

         //Act
         BooksListViewModel result = ((ViewResult)target.List(page, null, tag)).Model as BooksListViewModel;
         Book[] booksResult = result.Books.ToArray();

         //Assert
         Assert.AreEqual(bookId, booksResult[bookIndex].BookId);
      }

      [TestCase("tag1", "lang", "en", 1, 1, 1)]
      [TestCase("tag1", "lang", "en", 1, 2, 0)]
      public void List_ValidTagWithFilter_ReturnsCorrectOrderOfBooks(string tag, string queryKey, string queryValue, int page, int bookId, int bookIndex)
      {
         //Arrange
         IRepository mockRepo = Substitute.For<IRepository>();
         Book[] books = CreateBooks();
         mockRepo.Tags.Returns(_tags);
         mockRepo.Books.Returns(books.Where(b => b.Language.NameForLinks == queryValue)); //apply filter
         NameValueCollection queryCollection = new System.Collections.Specialized.NameValueCollection();
         queryCollection.Add(queryKey, queryValue);
         mockRepo.UpdateBooksFromFilterUsingRawSql(queryCollection, Arg.Any<string>(), Arg.Any<string>()).Returns(mockRepo.Books);
         BooksController target = new BooksController(mockRepo);

         IBooksRequestManager mockRequestManager = Substitute.For<IBooksRequestManager>();
         NameValueCollection emptyCollection = new System.Collections.Specialized.NameValueCollection();
         mockRequestManager.GetQueryString(Arg.Any<BooksController>()).Returns(emptyCollection);
         target.BooksRequestManager = mockRequestManager;

         //Act
         BooksListViewModel result = ((ViewResult)target.List(page, null, tag)).Model as BooksListViewModel;
         Book[] booksResult = result.Books.ToArray();

         //Assert
         Assert.AreEqual(bookId, booksResult[bookIndex].BookId);
      }


      #endregion
      #region ListToAppend

      [Test]
      public void ListToAppend_DefaultParameters_ReturnsCorrectCountOfBooks()
      {
         //Arrange
         IRepository mockRepo = Substitute.For<IRepository>();
         Book[] books = CreateBooks();
         mockRepo.Books.Returns(books);
         BooksController target = new BooksController(mockRepo);
         IBooksRequestManager mockRequestManager = Substitute.For<IBooksRequestManager>();
         NameValueCollection emptyCollection = new System.Collections.Specialized.NameValueCollection();
         mockRequestManager.GetQueryStringWhenAppend(Arg.Any<BooksController>()).Returns(emptyCollection);
         target.BooksRequestManager = mockRequestManager;
         int page = 2;
         //Act
         ActionResult result = target.ListToAppend(page);
         IEnumerable<Book> model = ((ViewComponentResult)result).Arguments as IEnumerable<Book>;

         //Assert
         Assert.IsInstanceOf<ViewComponentResult>(result);
         Assert.AreEqual(1, model.Count());
      }

      [Test]
      public void ListToAppend_DefaultParameters_ReturnsCorrectOrderOfBooks()
      {
         //Arrange
         IRepository mockRepo = Substitute.For<IRepository>();
         Book[] books = CreateBooks();
         mockRepo.Books.Returns(books);
         BooksController target = new BooksController(mockRepo);
         IBooksRequestManager mockRequestManager = Substitute.For<IBooksRequestManager>();
         NameValueCollection emptyCollection = new System.Collections.Specialized.NameValueCollection();
         mockRequestManager.GetQueryStringWhenAppend(Arg.Any<BooksController>()).Returns(emptyCollection);
         target.BooksRequestManager = mockRequestManager;
         int page = 1;
         //Act
         ActionResult result = target.ListToAppend(page);
         IEnumerable<Book> model = ((ViewComponentResult)result).Arguments as IEnumerable<Book>;
         Book[] booksResult = model.ToArray();
         //Assert
         Assert.IsInstanceOf<ViewComponentResult>(result);
         Assert.AreSame(books[4], booksResult[0]);
         Assert.AreSame(books[3], booksResult[1]);
         Assert.AreSame(books[2], booksResult[2]);
         Assert.AreSame(books[1], booksResult[3]);
      }

      [Test]
      public void ListToAppend_InValidCategory_ReturnsEmptyResult()
      {
         //Arrange
         IRepository mockRepo = Substitute.For<IRepository>();
         mockRepo.Categories.Returns(_categories);
         BooksController target = new BooksController(mockRepo);

         IBooksRequestManager mockRequestManager = Substitute.For<IBooksRequestManager>();
         string invalidCategory = "invalidCategory";
         int page = 1;

         //Act
         ActionResult result = target.ListToAppend(page, invalidCategory);

         //Assert
         Assert.IsInstanceOf<EmptyResult>(result);
      }

      [Test]
      public void ListToAppend_WhenNoBooksInResult_ReturnsEmptyResult()
      {
         //Arrange
         IRepository mockRepo = Substitute.For<IRepository>();
         Book[] books = CreateBooks();
         mockRepo.Books.Returns(books);
         BooksController target = new BooksController(mockRepo);
         IBooksRequestManager mockRequestManager = Substitute.For<IBooksRequestManager>();
         NameValueCollection emptyCollection = new System.Collections.Specialized.NameValueCollection();
         mockRequestManager.GetQueryStringWhenAppend(Arg.Any<BooksController>()).Returns(emptyCollection);
         target.BooksRequestManager = mockRequestManager;
         int page = 3;
         //Act
         ActionResult result = target.ListToAppend(page);

         //Assert
         Assert.IsInstanceOf<EmptyResult>(result);
      }

      [TestCase("cat1", 1, 4)]
      public void ListToAppend_ValidCategoryNoFilter_ReturnsCorrectCountOfBooksOnParticularPage(string category, int page, int expectedCount)
      {
         //Arrange
         IRepository mockRepo = Substitute.For<IRepository>();
         Book[] books = CreateBooks();
         mockRepo.Categories.Returns(_categories);
         mockRepo.Books.Returns(books);
         BooksController target = new BooksController(mockRepo);

         IBooksRequestManager mockRequestManager = Substitute.For<IBooksRequestManager>();
         NameValueCollection emptyCollection = new System.Collections.Specialized.NameValueCollection();
         mockRequestManager.GetQueryStringWhenAppend(Arg.Any<BooksController>()).Returns(emptyCollection);
         target.BooksRequestManager = mockRequestManager;

         //Act
         ActionResult result = target.ListToAppend(page, category);
         IEnumerable<Book> model = ((ViewComponentResult)result).Arguments as IEnumerable<Book>;
         Book[] booksResult = model.ToArray();

         //Assert
         Assert.IsInstanceOf<ViewComponentResult>(result);
         Assert.AreEqual(expectedCount, booksResult.Count());
      }

      [TestCase("cat1", "lang", "en", 1, 2)]
      public void ListToAppend_ValidCategoryWithFilter_ReturnsCorrectCountOfBooksOnParticularPage(string category, string queryKey, string queryValue, int page, int expectedCount)
      {
         //Arrange
         IRepository mockRepo = Substitute.For<IRepository>();
         Book[] books = CreateBooks();
         mockRepo.Categories.Returns(_categories);
         mockRepo.Books.Returns(books.Where(b => b.Language.NameForLinks == queryValue)); //apply filter
         NameValueCollection queryCollection = new System.Collections.Specialized.NameValueCollection();
         queryCollection.Add(queryKey, queryValue);
         mockRepo.UpdateBooksFromFilterUsingRawSql(queryCollection, Arg.Any<string>(), Arg.Any<string>()).Returns(mockRepo.Books);
         BooksController target = new BooksController(mockRepo);

         IBooksRequestManager mockRequestManager = Substitute.For<IBooksRequestManager>();
         mockRequestManager.GetQueryStringWhenAppend(Arg.Any<BooksController>()).Returns(queryCollection);
         target.BooksRequestManager = mockRequestManager;

         //Act
         ActionResult result = target.ListToAppend(page, category);
         IEnumerable<Book> model = ((ViewComponentResult)result).Arguments as IEnumerable<Book>;
         Book[] booksResult = model.ToArray();

         //Assert
         Assert.IsInstanceOf<ViewComponentResult>(result);
         Assert.AreEqual(expectedCount, booksResult.Count());
      }

      [TestCase("cat1", 1, 4, 0)]
      [TestCase("cat1", 1, 3, 1)]
      public void ListToAppend_ValidCategoryNoFilter_ReturnsCorrectOrderOfBooks(string category, int page, int bookId, int bookIndex)
      {
         //Arrange
         IRepository mockRepo = Substitute.For<IRepository>();
         Book[] books = CreateBooks();
         mockRepo.Categories.Returns(_categories);
         mockRepo.Books.Returns(books);

         BooksController target = new BooksController(mockRepo);
         IBooksRequestManager mockRequestManager = Substitute.For<IBooksRequestManager>();
         NameValueCollection emptyCollection = new System.Collections.Specialized.NameValueCollection();
         mockRequestManager.GetQueryStringWhenAppend(Arg.Any<BooksController>()).Returns(emptyCollection);
         target.BooksRequestManager = mockRequestManager;

         //Act
         ActionResult result = target.ListToAppend(page, category);
         IEnumerable<Book> model = ((ViewComponentResult)result).Arguments as IEnumerable<Book>;
         Book[] booksResult = model.ToArray();

         //Assert
         Assert.AreEqual(bookId, booksResult[bookIndex].BookId);
      }

      [TestCase("cat1", "lang", "en", 1, 1, 1)]
      [TestCase("cat1", "lang", "en", 1, 2, 0)]
      public void ListToAppend_ValidCategoryWithFilter_ReturnsCorrectOrderOfBooks(string category, string queryKey, string queryValue, int page, int bookId, int bookIndex)
      {
         //Arrange
         IRepository mockRepo = Substitute.For<IRepository>();
         Book[] books = CreateBooks();
         mockRepo.Categories.Returns(_categories);
         mockRepo.Books.Returns(books.Where(b => b.Language.NameForLinks == queryValue)); //apply filter
         NameValueCollection queryCollection = new System.Collections.Specialized.NameValueCollection();
         queryCollection.Add(queryKey, queryValue);
         mockRepo.UpdateBooksFromFilterUsingRawSql(queryCollection, Arg.Any<string>(), Arg.Any<string>()).Returns(mockRepo.Books);
         BooksController target = new BooksController(mockRepo);

         IBooksRequestManager mockRequestManager = Substitute.For<IBooksRequestManager>();
         NameValueCollection emptyCollection = new System.Collections.Specialized.NameValueCollection();
         mockRequestManager.GetQueryStringWhenAppend(Arg.Any<BooksController>()).Returns(emptyCollection);
         target.BooksRequestManager = mockRequestManager;

         //Act
         ActionResult result = target.ListToAppend(page, category);
         IEnumerable<Book> model = ((ViewComponentResult)result).Arguments as IEnumerable<Book>;
         Book[] booksResult = model.ToArray();

         //Assert
         Assert.AreEqual(bookId, booksResult[bookIndex].BookId);
      }

      [Test]
      public void ListToAppend_InValidTag_ReturnsEmptyResult()
      {
         //Arrange
         IRepository mockRepo = Substitute.For<IRepository>();
         mockRepo.Categories.Returns(_categories);
         BooksController target = new BooksController(mockRepo);

         IBooksRequestManager mockRequestManager = Substitute.For<IBooksRequestManager>();
         string invalidTag = "invalidTag";
         int page = 1;

         //Act
         ActionResult result = target.ListToAppend(page, invalidTag);

         //Assert
         Assert.IsInstanceOf<EmptyResult>(result);
      }

      [TestCase("tag1", 1, 3)]
      public void ListToAppend_ValidTagNoFilter_ReturnsCorrectCountOfBooksOnParticularPage(string tag, int page, int expectedCount)
      {
         //Arrange
         IRepository mockRepo = Substitute.For<IRepository>();
         Book[] books = CreateBooks();
         mockRepo.Tags.Returns(_tags);
         mockRepo.Books.Returns(books);
         BooksController target = new BooksController(mockRepo);

         IBooksRequestManager mockRequestManager = Substitute.For<IBooksRequestManager>();
         NameValueCollection emptyCollection = new System.Collections.Specialized.NameValueCollection();
         mockRequestManager.GetQueryStringWhenAppend(Arg.Any<BooksController>()).Returns(emptyCollection);
         target.BooksRequestManager = mockRequestManager;

         //Act
         ActionResult result = target.ListToAppend(page, null, tag);
         IEnumerable<Book> model = ((ViewComponentResult)result).Arguments as IEnumerable<Book>;
         Book[] booksResult = model.ToArray();

         //Assert
         Assert.IsInstanceOf<ViewComponentResult>(result);
         Assert.AreEqual(expectedCount, booksResult.Count());
      }

      [TestCase("tag1", "lang", "en", 1, 2)]
      public void ListToAppend_ValidTagWithFilter_ReturnsCorrectCountOfBooksOnParticularPage(string tag, string queryKey, string queryValue, int page, int expectedCount)
      {
         //Arrange
         IRepository mockRepo = Substitute.For<IRepository>();
         Book[] books = CreateBooks();
         mockRepo.Tags.Returns(_tags);
         mockRepo.Books.Returns(books.Where(b => b.Language.NameForLinks == queryValue)); //apply filter
         NameValueCollection queryCollection = new System.Collections.Specialized.NameValueCollection();
         queryCollection.Add(queryKey, queryValue);
         mockRepo.UpdateBooksFromFilterUsingRawSql(queryCollection, Arg.Any<string>(), Arg.Any<string>()).Returns(mockRepo.Books);
         BooksController target = new BooksController(mockRepo);

         IBooksRequestManager mockRequestManager = Substitute.For<IBooksRequestManager>();
         mockRequestManager.GetQueryStringWhenAppend(Arg.Any<BooksController>()).Returns(queryCollection);
         target.BooksRequestManager = mockRequestManager;

         //Act
         ActionResult result = target.ListToAppend(page, null, tag);
         IEnumerable<Book> model = ((ViewComponentResult)result).Arguments as IEnumerable<Book>;
         Book[] booksResult = model.ToArray();

         //Assert
         Assert.IsInstanceOf<ViewComponentResult>(result);
         Assert.AreEqual(expectedCount, booksResult.Count());
      }

      [TestCase("tag1", 1, 4, 0)]
      [TestCase("tag1", 1, 2, 1)]
      public void ListToAppend_ValidTagNoFilter_ReturnsCorrectOrderOfBooks(string tag, int page, int bookId, int bookIndex)
      {
         //Arrange
         IRepository mockRepo = Substitute.For<IRepository>();
         Book[] books = CreateBooks();
         mockRepo.Tags.Returns(_tags);
         mockRepo.Books.Returns(books);

         BooksController target = new BooksController(mockRepo);
         IBooksRequestManager mockRequestManager = Substitute.For<IBooksRequestManager>();
         NameValueCollection emptyCollection = new System.Collections.Specialized.NameValueCollection();
         mockRequestManager.GetQueryStringWhenAppend(Arg.Any<BooksController>()).Returns(emptyCollection);
         target.BooksRequestManager = mockRequestManager;

         //Act
         ActionResult result = target.ListToAppend(page, null, tag);
         IEnumerable<Book> model = ((ViewComponentResult)result).Arguments as IEnumerable<Book>;
         Book[] booksResult = model.ToArray();

         //Assert
         Assert.AreEqual(bookId, booksResult[bookIndex].BookId);
      }

      [TestCase("tag1", "lang", "en", 1, 1, 1)]
      [TestCase("tag1", "lang", "en", 1, 2, 0)]
      public void ListToAppend_ValidTagWithFilter_ReturnsCorrectOrderOfBooks(string tag, string queryKey, string queryValue, int page, int bookId, int bookIndex)
      {
         //Arrange
         IRepository mockRepo = Substitute.For<IRepository>();
         Book[] books = CreateBooks();
         mockRepo.Tags.Returns(_tags);
         mockRepo.Books.Returns(books.Where(b => b.Language.NameForLinks == queryValue)); //apply filter
         NameValueCollection queryCollection = new System.Collections.Specialized.NameValueCollection();
         queryCollection.Add(queryKey, queryValue);
         mockRepo.UpdateBooksFromFilterUsingRawSql(queryCollection, Arg.Any<string>(), Arg.Any<string>()).Returns(mockRepo.Books);
         BooksController target = new BooksController(mockRepo);

         IBooksRequestManager mockRequestManager = Substitute.For<IBooksRequestManager>();
         NameValueCollection emptyCollection = new System.Collections.Specialized.NameValueCollection();
         mockRequestManager.GetQueryStringWhenAppend(Arg.Any<BooksController>()).Returns(emptyCollection);
         target.BooksRequestManager = mockRequestManager;

         //Act
         ActionResult result = target.ListToAppend(page, null, tag);
         IEnumerable<Book> model = ((ViewComponentResult)result).Arguments as IEnumerable<Book>;
         Book[] booksResult = model.ToArray();

         //Assert
         Assert.AreEqual(bookId, booksResult[bookIndex].BookId);
      }


      #endregion

   }
}
