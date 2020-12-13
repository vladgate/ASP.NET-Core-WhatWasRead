using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ASP.NET_Core_WhatWasRead.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    AuthorId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(maxLength: 30, nullable: false),
                    LastName = table.Column<string>(maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.AuthorId);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameForLinks = table.Column<string>(maxLength: 50, nullable: false),
                    NameForLabels = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "FilterTargets",
                columns: table => new
                {
                    FilterTargetId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FilterTargetName = table.Column<string>(maxLength: 50, nullable: false),
                    FilterTargetSchema = table.Column<string>(maxLength: 50, nullable: false),
                    ControllerName = table.Column<string>(maxLength: 50, nullable: false),
                    ActionName = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilterTargets", x => x.FilterTargetId);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    LanguageId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameForLabels = table.Column<string>(maxLength: 50, nullable: false),
                    NameForLinks = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.LanguageId);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    TagId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameForLabels = table.Column<string>(maxLength: 50, nullable: false),
                    NameForLinks = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.TagId);
                });

            migrationBuilder.CreateTable(
                name: "Filters",
                columns: table => new
                {
                    FilterId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FilterTargetId = table.Column<int>(nullable: false),
                    FilterColumnName = table.Column<string>(maxLength: 50, nullable: false),
                    QueryWord = table.Column<string>(maxLength: 10, nullable: false),
                    Comparator = table.Column<string>(maxLength: 10, nullable: false),
                    FilterName = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filters", x => x.FilterId);
                    table.ForeignKey(
                        name: "FK_Filters_FilterTargets_FilterTargetId",
                        column: x => x.FilterTargetId,
                        principalTable: "FilterTargets",
                        principalColumn: "FilterTargetId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    BookId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 300, nullable: false),
                    LanguageId = table.Column<int>(nullable: false),
                    Pages = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    CategoryId = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    ImageData = table.Column<byte[]>(nullable: true),
                    ImageMimeType = table.Column<string>(maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.BookId);
                    table.ForeignKey(
                        name: "FK_Books_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Books_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "LanguageId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AuthorsOfBooks",
                columns: table => new
                {
                    AuthorId = table.Column<int>(nullable: false),
                    BookId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorsOfBooks", x => new { x.BookId, x.AuthorId });
                    table.ForeignKey(
                        name: "FK_AuthorsOfBooks_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "AuthorId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuthorsOfBooks_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookTags",
                columns: table => new
                {
                    BookId = table.Column<int>(nullable: false),
                    TagId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookTags", x => new { x.BookId, x.TagId });
                    table.ForeignKey(
                        name: "FK_BookTags_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "NameForLabels", "NameForLinks" },
                values: new object[,]
                {
                    { 1, "Программирование", "programmirovanie" },
                    { 2, "Художественная литература", "hudozhestvennaja-literatura" },
                    { 3, "Тестирование", "testirovanie" }
                });

            migrationBuilder.InsertData(
                table: "FilterTargets",
                columns: new[] { "FilterTargetId", "ActionName", "ControllerName", "FilterTargetName", "FilterTargetSchema" },
                values: new object[] { 1, "List", "Books", "[BooksWithAuthors]", "[dbo]" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "LanguageId", "NameForLabels", "NameForLinks" },
                values: new object[,]
                {
                    { 1, "русский", "ru" },
                    { 2, "английский", "en" },
                    { 3, "украинский", "ua" }
                });

            migrationBuilder.InsertData(
                table: "Filters",
                columns: new[] { "FilterId", "Comparator", "FilterColumnName", "FilterName", "FilterTargetId", "QueryWord" },
                values: new object[] { 1, "equal", "NameForLinks", "Язык", 1, "lang" });

            migrationBuilder.InsertData(
                table: "Filters",
                columns: new[] { "FilterId", "Comparator", "FilterColumnName", "FilterName", "FilterTargetId", "QueryWord" },
                values: new object[] { 2, "equal", "AuthorId", "Автор", 1, "author" });

            migrationBuilder.InsertData(
                table: "Filters",
                columns: new[] { "FilterId", "Comparator", "FilterColumnName", "FilterName", "FilterTargetId", "QueryWord" },
                values: new object[] { 3, "between", "Pages", "Количество страниц", 1, "pages" });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorsOfBooks_AuthorId",
                table: "AuthorsOfBooks",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_CategoryId",
                table: "Books",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_LanguageId",
                table: "Books",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_BookTags_TagId",
                table: "BookTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Filters_FilterTargetId",
                table: "Filters",
                column: "FilterTargetId");
            //create view
            string createViewQuery = @"GO
                                     CREATE VIEW [dbo].[BooksWithAuthors]
                                     AS 
                                     Select b.*, a.*, l.NameForLinks, t.TagId, t.NameForLabels as TagNameForLabels, t.NameForLinks as TagNameForLinks from [dbo].[Books] as b
                                     left join [dbo].[AuthorsOfBooks] as ab
                                     on b.BookId = ab.BookId
                                     inner join [dbo].[Authors] as a
                                     on ab.AuthorId = a.AuthorId
                                     left join [dbo].[Languages] as l
                                     on b.LanguageId = l.LanguageId
                                     left join [dbo].BookTags as bt
                                     on b.BookId = bt.BookId
                                     left join [dbo].Tags as t
                                     on bt.TagId = t.TagId;
                                     GO ";
            migrationBuilder.Sql(createViewQuery);

      }

      protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorsOfBooks");

            migrationBuilder.DropTable(
                name: "BookTags");

            migrationBuilder.DropTable(
                name: "Filters");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "FilterTargets");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.Sql(@" GO
                                  DROP VIEW [dbo].[BooksWithAuthors];
                                  GO
                                  ");

      }
   }
}
