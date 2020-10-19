using ASP.NET_Core_WhatWasRead.App_Data.DBModels;
using ASP.NET_Core_WhatWasRead.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Web;

namespace ASP.NET_Core_WhatWasRead.Infrastructure
{
   public static class Helper
   {
      public static HtmlString PageLinks(this IHtmlHelper html, PagingInfo pagingInfo, string category = null)
      {
         StringBuilder result = new StringBuilder();
         for (int i = 1; i <= pagingInfo.TotalPages; i++)
         {
            result.Append(html.ActionLink(i.ToString(), "List", "Book", new { page = i, category = category }, new { @class = i == pagingInfo.CurrentPage ? "page-link selected" : "page-link" }));
         }
         return new HtmlString(result.ToString());
      }
      public static HtmlString CreateBookList(this IHtmlHelper html, IEnumerable<Book> books)
      {
         //UrlHelper urlHelper = new UrlHelper(html.ViewContext.RequestContext);
         UrlHelper urlHelper = new UrlHelper(html.ViewContext);
         StringBuilder result = new StringBuilder();
         int counter = 0;
         TagBuilder divRow = new TagBuilder("div");
         divRow.AddCssClass("row-books");

         foreach (Book book in books)
         {
            counter++;
            TagBuilder p = new TagBuilder("p");

            TagBuilder aboutAndImg = new TagBuilder("div");
            aboutAndImg.AddCssClass("img-and-about");

            TagBuilder bookElImg = new TagBuilder("div");
            bookElImg.AddCssClass("book-img");
            TagBuilder img = new TagBuilder("img");
            img.MergeAttribute("alt", book.Name);
            img.MergeAttribute("src", book.ImageData == null ? @"\Images\no_image.png" : urlHelper.Action(new UrlActionContext { Action = "GetImage", Controller = "Books", Values = new { id = book.BookId } }));
            bookElImg.InnerHtml.AppendHtml(img);

            TagBuilder bookElName = new TagBuilder("div");
            bookElName.AddCssClass("book-name");
            p.InnerHtml.Append(book.Name);
            bookElName.InnerHtml.AppendHtml(p.ToString());

            TagBuilder bookAuthors = new TagBuilder("div");
            bookAuthors.AddCssClass("book-authors");
            string auth = string.Empty;
            if (book.AuthorsOfBooks.Count == 1)
            {
               auth = "Автор: " + book.DisplayAuthors();
            }
            else
            {
               auth = "Авторы: " + book.DisplayAuthors();
            }
            p.InnerHtml.Append(auth);
            bookAuthors.InnerHtml.Append(p.ToString());

            TagBuilder bookLink = new TagBuilder("div");
            bookLink.AddCssClass("book-details");
            TagBuilder link = new TagBuilder("a");
            //string route = @"/books/" + book.BookId;
            string route = urlHelper.RouteUrl(new UrlRouteContext { RouteName = "BooksIdAction", Values = new { id = book.BookId, action = "Details" } });
            link.MergeAttribute("href", route);
            link.InnerHtml.Append("Подробнее");
            bookLink.InnerHtml.AppendHtml(link);

            aboutAndImg.InnerHtml.AppendHtml(bookElImg);
            aboutAndImg.InnerHtml.AppendHtml(bookElName);
            aboutAndImg.InnerHtml.AppendHtml(bookAuthors);
            aboutAndImg.InnerHtml.AppendHtml(bookLink);
            divRow.InnerHtml.AppendHtml(aboutAndImg);
            if (counter % Globals.ITEMS_PER_BOOKROW == 0 || counter == books.Count()) // конец горизонтального ряда или конец элементов
            {
               result.Append(divRow.ToString());
               if (counter != books.Count())
               {
                  divRow = new TagBuilder("div");
                  divRow.AddCssClass("row-books");
               }
            }
         }

         return new HtmlString(result.ToString());
      }
      public static HtmlString CreateTagLinks(this IHtmlHelper html, Tag[] tags)
      {
         string GetString(IHtmlContent htmlContent)
         {
            using (var writer = new System.IO.StringWriter())
            {
               htmlContent.WriteTo(writer, HtmlEncoder.Default);
               return writer.ToString();
            }
         }

         StringBuilder sb = new StringBuilder();
         for (int i = 0; i < tags.Length; i++)
         {
            IHtmlContent content = html.ActionLink(tags[i].NameForLabels, "List", "Books", new { tag = tags[i].NameForLinks }, new { @class = "tag-a" });
            sb.Append(GetString(content));
            if (i != tags.Length - 1) // not last element
            {
               sb.Append(", ");
            }
         }
         return new HtmlString(sb.ToString());
      }
      public static string CreateFilterPartOfLink(this IHtmlHelper html, string currentPath, NameValueCollection currentQueryString, string queryWord, string value, out bool check)
      {
         check = false;
         NameValueCollection newQueryString = new NameValueCollection(currentQueryString.Count + 1);
         if (currentQueryString.AllKeys.Contains(queryWord))
         {
            string res;
            if (currentQueryString[queryWord].Contains(value))
            {
               if (currentQueryString[queryWord].Contains("," + value + ","))
               {
                  res = currentQueryString[queryWord].Replace("," + value + ",", ",");
                  check = true;
               }
               else if (currentQueryString[queryWord].StartsWith(value + ",")) //remove leading ','
               {
                  res = currentQueryString[queryWord].Replace(value + ",", "");
                  check = true;
               }
               else if (currentQueryString[queryWord].EndsWith("," + value)) //remove trailing ','
               {
                  res = currentQueryString[queryWord].Replace("," + value, "");
                  check = true;
               }
               else if (currentQueryString[queryWord] != value)
               {
                  res = currentQueryString[queryWord] + "," + value;
               }
               else //
               {
                  res = ""; //default
                  check = true;
               }

               if (!String.IsNullOrWhiteSpace(res))
               {
                  newQueryString[queryWord] = res;
               }
            }
            else
            {
               newQueryString[queryWord] = currentQueryString[queryWord] + "," + value;
            }
         }
         else
         {
            newQueryString.Add(queryWord, value);
         }
         for (int i = 0; i < currentQueryString.Count; i++)
         {
            if (currentQueryString.AllKeys[i] == queryWord)
            {
               continue;
            }
            newQueryString.Add(currentQueryString.AllKeys[i], currentQueryString[i]);
         }

         string queryString = BuildQueryString(newQueryString);
         if (!String.IsNullOrWhiteSpace(queryString))
         {
            if (currentPath.EndsWith("/"))
            {
               currentPath = currentPath.Remove(currentPath.Length - 1, 1); //remove last '/'
            }
         }
         return currentPath + queryString;
      }
      public static string BuildQueryString(NameValueCollection currentQueryString)
      {
         if (currentQueryString.Count == 0)
         {
            return "";
         }
         StringBuilder result = new StringBuilder();
         result.Append("?");
         for (int i = 0; i < currentQueryString.AllKeys.Length; i++)
         {
            result.Append(currentQueryString.AllKeys[i]);
            result.Append("=");
            result.Append(currentQueryString[i]);
            result.Append("&");
         }
         result.Remove(result.Length - 1, 1);
         return result.ToString();
      }
      public static string BuildQueryString(QueryString currentQueryString)
      {
         NameValueCollection collection = HttpUtility.ParseQueryString(currentQueryString.ToString());
         return BuildQueryString(collection);
      }
      public static NameValueCollection GetQueryCollection(QueryString currentQueryString)
      {
         return HttpUtility.ParseQueryString(currentQueryString.ToString());
      }
      public static bool IsAjaxRequest(this HttpRequest request)
      {
         if (request == null)
            throw new ArgumentNullException(nameof(request));
         if (request.Headers != null)
            return request.Headers["X-Requested-With"] == "XMLHttpRequest";
         return false;
      }
   }
}

