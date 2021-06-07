using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Castle.Core.Internal;
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
                cfg.CreateMap<BookDto, BookViewModel>();
                cfg.CreateMap<AuthorDto, AuthorViewModel>();
                cfg.CreateMap<GenreDto, GenreViewModel>();
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

            ViewData["currentOrder"] = "asc";

            
            ViewData["orderBy"] = sort[0];
    
            switch (sort[0])
            {
                case "authors":
                    books = books.OrderBy(item => item.GetAuthors()).ToList();
                    ViewData["authorsOrder"] = desc ? "asc" : "desc";
                    ViewData["currentOrder"] = desc ? "asc" : "desc";
                    break;
                case "genre":
                    books = books.OrderBy(item => item.Genre == null ? "" : item.Genre.Name).ToList();
                    ViewData["genreOrder"] = desc ? "asc" : "desc";
                    ViewData["currentOrder"] = desc ? "asc" : "desc";
                    break;
                case "remain":
                    books = books.OrderBy(item => item.NumberOfCopiesCurrent).ToList();
                    ViewData["remainOrder"] = desc ? "asc" : "desc";
                    ViewData["currentOrder"] = desc ? "asc" : "desc";
                    break;
                case "number":
                    books = books.OrderBy(item => item.NumberOfCopies).ToList();
                    ViewData["numberOrder"] = desc ? "asc" : "desc";
                    ViewData["currentOrder"] = desc ? "asc" : "desc";
                    break;
                default:
                    books = books.OrderBy(item => item.Name).ToList();
                    ViewData["nameOrder"] = desc ? "asc" : "desc";
                    ViewData["currentOrder"] = desc ? "asc" : "desc";
                    break;
            }

            if (desc)
            {
                books.Reverse();
            }

            ViewData["Genres"] = genres;
            ViewData["Authors"] = authors;
            
            return View(PaginatedList<BookViewModel>.CreatePage(books.AsQueryable(), pageNumber ?? 1, 10));
        }

        public IActionResult Book(int id)
        {
            var authorDtos = _authorService.GetAll();
            var genreDto = _genreService.GetAll();


            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BookDto, BookViewModel>();
                cfg.CreateMap<AuthorDto, AuthorViewModel>();
                cfg.CreateMap<GenreDto, GenreViewModel>();
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
                return View(new BookViewModel());
            }
            var bookViewModel = mapper.Map<BookDto, BookViewModel>(book);

            return View(bookViewModel);
        }

        [HttpPost]
        public IActionResult Book(BookViewModel bookViewModel, string[] authors, int genre)
        {
            if (bookViewModel.Id == 0)
            {
                _logger.LogInformation($"Adding new book");
                bookViewModel.NumberOfCopiesCurrent = bookViewModel.NumberOfCopies;
            }
            else
                _logger.LogInformation($"Updating item with id={bookViewModel.Id}");

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BookViewModel, BookDto>();
                cfg.CreateMap<AuthorViewModel, AuthorDto>();
                cfg.CreateMap<GenreViewModel, GenreDto>();
            });
            // config.AssertConfigurationIsValid();
            var mapper = config.CreateMapper();

            var bookDto = mapper.Map<BookViewModel, BookDto>(bookViewModel);

            bookDto.Genre = _genreService.Get(genre);

            bookDto.Authors = _authorService.GetByNamesOrCreate(authors);

            int count = _bookService.GetLoanedCopiesCount(bookDto.Id);
            bookDto.NumberOfCopies = bookDto.NumberOfCopies >= count
                ? bookDto.NumberOfCopies
                : count;

            bookDto.NumberOfCopiesCurrent = bookDto.NumberOfCopies - count;

            _bookService.AddOrUpdate(bookDto);

            return RedirectPermanent("~/Books/");
        }
        
        public IActionResult RemoveBook(int id)
        {
            _logger.LogInformation($"Removing book with id={id}");
            _bookService.Delete(id);
            return RedirectPermanent("~/Books/");
        }
    }
}