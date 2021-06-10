using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Castle.Core.Internal;
using ClosedXML.Excel;
using Library.BLL.DTO;
using Library.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication.Models;
using WebApplication.Utils;

namespace WebApplication.Controllers
{
    public class BooksController : Controller
    {
        private readonly ILogger<BooksController> _logger;
        private readonly IBookService _bookService;
        private readonly IGenreService _genreService;
        private readonly IAuthorService _authorService;

        public BooksController(ILogger<BooksController> logger, IBookService bookService, IGenreService genreService,
            IAuthorService authorService)
        {
            _logger = logger;
            _bookService = bookService;
            _genreService = genreService;
            _authorService = authorService;
        }

        public IActionResult Index(
            string sortBy,
            string[] authorsFilter,
            string[] genreFilter,
            string search,
            int? pageNumber) 
        {
            _logger.LogInformation($"Retrieving Books, page={pageNumber}");

            var bookList = _bookService.GetAll();
            var genreDto = _genreService.GetAll();
            var authorDtos = _authorService.GetAll();

            if (sortBy.IsNullOrEmpty())
            {
                sortBy = "name_asc";
            }


            if (genreFilter.Length == 1)
            {
                genreFilter = genreFilter[0].Split(",");
            }
            if (authorsFilter.Length == 1)
            {
                authorsFilter = authorsFilter[0].Split(",");
            }
            
            var autFilt = Array.ConvertAll(authorsFilter, s => int.Parse(s));
            var genFilt = Array.ConvertAll(genreFilter, s => int.Parse(s));

            ViewData["authorFilt"] = autFilt;
            ViewData["genreFilt"] = genFilt;
            ViewData["authorFiltString"] = String.Join(",", authorsFilter);
            ViewData["genreFiltString"] = String.Join(",", genreFilter);

            ViewData["searchFilt"] = search;
            ViewData["page"] = pageNumber;


            if (search != null)
            {
                bookList = bookList.Where(item => item.Name.ToLower().Contains(search)).ToList();
            }

            if (!authorsFilter.IsNullOrEmpty())
            {
                bookList = bookList.Where(item => item.Authors.Any(it => autFilt.Contains(it.Id))).ToList();
            }
            
            if (!genreFilter.IsNullOrEmpty())
            {
                bookList = bookList.Where(item => genFilt.Contains(item.Genre.Id)).ToList();
            }

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BookLoanRecordDto, BookLoanRecordViewModel>();
                cfg.CreateMap<BookDto, BookViewModel>();
                cfg.CreateMap<AuthorDto, AuthorViewModel>();
                cfg.CreateMap<GenreDto, GenreViewModel>();
                cfg.CreateMap<ReaderDto, ReaderViewModel>();
                cfg.CreateMap<StaffDto, StaffViewModel>();
            });
            config.AssertConfigurationIsValid();
            var mapper = config.CreateMapper();

            var books = mapper.Map<List<BookDto>, List<BookViewModel>>(bookList);
            var genres = mapper.Map<List<GenreDto>, List<GenreViewModel>>(genreDto);
            var authors = mapper.Map<List<AuthorDto>, List<AuthorViewModel>>(authorDtos);

            var sort = sortBy.Split("_");

            bool desc = sort[1].Equals("desc");

            ViewData["authorsOrder"] = "asc";
            ViewData["genreOrder"] = "asc";
            ViewData["remainOrder"] = "asc";
            ViewData["numberOrder"] = "asc";
            ViewData["nameOrder"] = "asc";

            ViewData["currentOrder"] = sort[1];

            
            ViewData["orderBy"] = sort[0];
    
            switch (sort[0])
            {
                case "authors":
                    books = books.OrderBy(item => item.GetAuthors()).ToList();
                    ViewData["authorsOrder"] = desc ? "asc" : "desc";
                    break;
                case "genre":
                    books = books.OrderBy(item => item.Genre == null ? "" : item.Genre.Name).ToList();
                    ViewData["genreOrder"] = desc ? "asc" : "desc";
                    break;
                case "remain":
                    books = books.OrderByDescending(item => item.BookLoanRecords.Count(it => !it.ReturnDate.HasValue)).ToList();
                    ViewData["remainOrder"] = desc ? "asc" : "desc";
                    break;
                case "number":
                    books = books.OrderBy(item => item.NumberOfCopies).ToList();
                    ViewData["numberOrder"] = desc ? "asc" : "desc";
                    break;
                default:
                    books = books.OrderBy(item => item.Name).ToList();
                    ViewData["nameOrder"] = desc ? "asc" : "desc";
                    break;
            }

            if (desc)
            {
                books.Reverse();
            }

            ViewData["Genres"] = genres;
            ViewData["Authors"] = authors;
            
            return View(PaginatedList<BookViewModel>.CreatePage(books.AsQueryable(), pageNumber ?? 1, 20));
        }

        public IActionResult Book(int id)
        {
            var authorDtos = _authorService.GetAll();
            var genreDto = _genreService.GetAll();


            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BookLoanRecordDto, BookLoanRecordViewModel>();
                cfg.CreateMap<BookDto, BookViewModel>();
                cfg.CreateMap<AuthorDto, AuthorViewModel>();
                cfg.CreateMap<GenreDto, GenreViewModel>();
                cfg.CreateMap<ReaderDto, ReaderViewModel>();
                cfg.CreateMap<StaffDto, StaffViewModel>();
            });
            config.AssertConfigurationIsValid();
            var mapper = config.CreateMapper();

            var authorViewModels = mapper.Map<List<AuthorDto>, List<AuthorViewModel>>(authorDtos);
            var genres = mapper.Map<List<GenreDto>, List<GenreViewModel>>(genreDto);

            ViewData["Authors"] = authorViewModels;
            ViewData["Genres"] = genres;

            var book = _bookService.Get(id);
            if (book == null)
            {
                ViewData["MinCount"] = 0;
                return View(new BookViewModel());
            }
            var bookViewModel = mapper.Map<BookDto, BookViewModel>(book);
            
            ViewData["MinCount"] = book.BookLoanRecords.Count(item => !item.ReturnDate.HasValue);

            return View(bookViewModel);
        }

        [HttpPost]
        public IActionResult Book(BookViewModel bookViewModel, string[] authors, int genre)
        {
            _logger.LogInformation(bookViewModel.Id == 0
                ? $"Adding new book"
                : $"Updating book with id={bookViewModel.Id}");

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BookViewModel, BookDto>();
                cfg.CreateMap<AuthorViewModel, AuthorDto>();
                cfg.CreateMap<GenreViewModel, GenreDto>();
                cfg.CreateMap<BookLoanRecordViewModel, BookLoanRecordDto>();
                cfg.CreateMap<ReaderViewModel, ReaderDto>();
                cfg.CreateMap<StaffViewModel, StaffDto>();
            });
            var mapper = config.CreateMapper();

            var bookDto = mapper.Map<BookViewModel, BookDto>(bookViewModel);

            bookDto.Genre = _genreService.Get(genre);

            bookDto.Authors = _authorService.GetByNamesOrCreate(authors);
            
            _bookService.AddOrUpdate(bookDto);

            return RedirectPermanent("~/Books/");
        }
        
        public IActionResult RemoveBook(int id)
        {
            _logger.LogInformation($"Removing book with id={id}");
            _bookService.Delete(id);
            return RedirectPermanent("~/Books/");
        }
        
        public ActionResult Download()
        {
            using var workbook = new XLWorkbook();

            var items = _bookService.GetAll();

            _logger.LogInformation($"Saving Excel file for Books");

            var worksheet = workbook.Worksheets.Add("Items");
            worksheet.Cell("A1").Value = "Id";
            worksheet.Cell("B1").Value = "Name";
            worksheet.Cell("C1").Value = "Authors";
            worksheet.Cell("D1").Value = "Genre";

            int row = 1;
            foreach (var item in items)
            {
                var rowObj = worksheet.Row(++row);
                rowObj.Cell(1).Value = item.Id;
                rowObj.Cell(2).Value = item.Name;
                rowObj.Cell(3).Value = String.Join(",", item.Authors.Select(cat => cat.Name).ToArray());
                rowObj.Cell(4).Value = item.Genre?.Name;
            }

            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = "Books.xlsx",
                Inline = false, 
            };
            Response.Headers.Add("Content-Disposition", cd.ToString());
            using (MemoryStream stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Books.xlsx");
            }
        }
    }
}