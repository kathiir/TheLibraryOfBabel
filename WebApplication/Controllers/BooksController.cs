using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using AutoMapper;
using Library.BLL.DTO;
using Library.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication.Models;

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

        public IActionResult Index()
        {
            var bookList = _bookService.GetAll();
            var genreDto = _genreService.GetAll();

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

            ViewData["Genres"] = genres;

            return View(books);
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
            _logger.LogInformation($"Removing user with id={id}");
            _bookService.Delete(id);
            return RedirectPermanent("~/Books/");
        }
    }
}