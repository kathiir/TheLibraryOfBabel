using System.Linq;
using AutoMapper;
using Library.BLL.DTO;
using Library.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStaffService _staffService;
        private readonly IReaderService _readerService;
        private readonly IBookService _bookService;
        private readonly IBookLoanRecordService _bookLoanRecordService;
        private readonly IGenreService _genreService;
        private readonly IAuthorService _authorService;

        public HomeController(ILogger<HomeController> logger, IStaffService staffService, 
            IReaderService readerService, IBookService bookService,
            IBookLoanRecordService bookLoanRecordService, IGenreService genreService,
            IAuthorService authorService)
        {
            _logger = logger;
            _staffService = staffService;
            _readerService = readerService;
            _bookService = bookService;
            _bookLoanRecordService = bookLoanRecordService;
            _genreService = genreService;
            _authorService = authorService;
        }

        public IActionResult Index()
        {
            _logger.LogInformation($"Retrieving Statistics");
            
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BookLoanRecordDto, BookLoanRecordViewModel>();
                cfg.CreateMap<BookDto, BookViewModel>();
                cfg.CreateMap<AuthorDto, AuthorViewModel>();
                cfg.CreateMap<GenreDto, GenreViewModel>();
                cfg.CreateMap<ReaderDto, ReaderViewModel>();
                cfg.CreateMap<StaffDto, StaffViewModel>();
            });
            var mapper = config.CreateMapper();

            var books = _bookService.GetAll();
            var records = _bookLoanRecordService.GetAll();
            var authors = _authorService.GetAll();
            var genres = _genreService.GetAll();
            var readers = _readerService.GetAll();
            var staff = _staffService.GetAll();

            ViewData["ToReturn"] = records.Count(item => !item.ReturnDate.HasValue);
            var id = records.GroupBy(i => i.Book.Id)
                .OrderByDescending(g => g.Count())
                .Take(1)
                .Select(g => g.Key);
            ViewData["PopularBook"] = _bookService.Get(id.FirstOrDefault())?.Name;
            ViewData["Reading"] = readers
                .OrderByDescending(item => item.BookLoanRecords.Count)
                .FirstOrDefault()?.Name;
            ViewData["Working"] = staff
                .OrderByDescending(item => item.BookLoanRecords.Count)
                .FirstOrDefault()?.Name;
            ViewData["Writing"] = authors
                .OrderByDescending(item => item.Books.Count)
                .FirstOrDefault()?.Name;
            id = books
                .Where(it => it.Genre != null)
                .GroupBy(i => i.Genre.Id)
                .OrderByDescending(g => g.Count())
                .Take(1)
                .Select(g => g.Key);
            ViewData["Genre"] = _genreService.Get(id.FirstOrDefault())?.Name;
            
            return View();
        }

    }
}