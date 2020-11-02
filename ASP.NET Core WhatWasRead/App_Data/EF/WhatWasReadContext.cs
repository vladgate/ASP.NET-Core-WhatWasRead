﻿using ASP.NET_Core_WhatWasRead.App_Data.DBModels;
using ASP.NET_Core_WhatWasRead.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_WhatWasRead.App_Data.EF
{
   public class WhatWasReadContext : DbContext
   {
      private readonly IConfigurationRoot _config;

      public WhatWasReadContext(IConfigurationRoot config) : base()
      {
         _config = config;
         var pm = Database.GetPendingMigrations();
         Database.Migrate();
      }

      public IConfigurationRoot Config => _config;

      public virtual DbSet<Author> Authors { get; set; }
      public virtual DbSet<Book> Books { get; set; }
      public virtual DbSet<Category> Categories { get; set; }
      public virtual DbSet<Language> Languages { get; set; }
      public virtual DbSet<Tag> Tags { get; set; }

      public virtual DbSet<Filter> Filters { get; set; }
      public virtual DbSet<FilterTarget> FilterTargets { get; set; }

      public virtual DbSet<BooksWithAuthor> BooksWithAuthors { get; set; } //view


      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
         modelBuilder.Entity<AuthorsOfBooks>()
            .HasKey(ab => new { ab.AuthorId, ab.BookId });
         modelBuilder.Entity<BookTags>()
            .HasKey(bt => new { bt.BookId, bt.TagId });

         //init data
         modelBuilder.Entity<DBModels.Category>().HasData(new Category[]
         {
            new Category { CategoryId = 1, NameForLabels = "Программирование", NameForLinks = "programmirovanie" },
            new Category { CategoryId = 2, NameForLabels = "Художественная литература", NameForLinks = "hudozhestvennaja-literatura" },
            new Category { CategoryId = 3, NameForLabels = "Тестирование", NameForLinks = "testirovanie" }
         });
         modelBuilder.Entity<DBModels.Language>().HasData(new Language[]
         {
            new Language { LanguageId = 1, NameForLabels = "русский", NameForLinks = "ru" },
            new Language { LanguageId = 2, NameForLabels = "английский", NameForLinks = "en" },
            new Language { LanguageId = 3, NameForLabels = "украинский", NameForLinks = "ua" }
         });
         modelBuilder.Entity<DBModels.FilterTarget>().HasData(new FilterTarget { FilterTargetId = 1, ActionName = "List", ControllerName = "Books", FilterTargetName = "[BooksWithAuthors]", FilterTargetSchema = "[dbo]" });
         modelBuilder.Entity<DBModels.Filter>().HasData(new Filter[]
         {
            new Filter {FilterId = 1, FilterTargetId = 1, FilterColumnName = "NameForLinks", QueryWord = "lang", Comparator = "equal", FilterName = "Язык" },
            new Filter {FilterId = 2, FilterTargetId = 1, FilterColumnName = "AuthorId", QueryWord = "author", Comparator = "equal", FilterName = "Автор" },
            new Filter {FilterId = 3, FilterTargetId = 1, FilterColumnName = "Pages", QueryWord = "pages", Comparator = "between", FilterName = "Количество страниц" }
         });

         //define view
         modelBuilder.Entity<BooksWithAuthor>(entity =>
         {
            entity.HasNoKey();
            entity.ToView<BooksWithAuthor>("BooksWithAuthors");
         });

      }

      protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
      {
         base.OnConfiguring(optionsBuilder);
         string connectionString = _config[Globals.CONNECTION_NAME_CONFIG];
         optionsBuilder.UseSqlServer(connectionString);
      }

   }
}
