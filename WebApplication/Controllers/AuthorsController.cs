using System.Collections.Generic;
using AutoMapper;
using Library.BLL.DTO;
using Library.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly ILogger<AuthorsController> _logger;
        private readonly IAuthorService _authorService;

        public AuthorsController(ILogger<AuthorsController> logger, IAuthorService authorService)
        {
            _logger = logger;
            _authorService = authorService;
        }

        public IActionResult Index()
        {
            var authorsList = _authorService.GetAll();
            
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BookDto, BookViewModel>();
                cfg.CreateMap<AuthorDto, AuthorViewModel>();
                cfg.CreateMap<GenreDto, GenreViewModel>();
            });
            config.AssertConfigurationIsValid();
            var mapper = config.CreateMapper();

            var books = mapper.Map<List<AuthorDto>, List<AuthorViewModel>>(authorsList);
            
            return View(books);
        }
        
        public IActionResult Author(int id)
        {
            var book = _authorService.Get(id);

            if (book == null)
            {
                return StatusCode(404);
            }
            
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BookDto, BookViewModel>();
                cfg.CreateMap<AuthorDto, AuthorViewModel>();
                cfg.CreateMap<GenreDto, GenreViewModel>();
            });
            config.AssertConfigurationIsValid();
            var mapper = config.CreateMapper();

            var bookViewModel = mapper.Map<AuthorDto, AuthorViewModel>(book);

            return View(bookViewModel);
        }
        
        public IActionResult Add()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Add(AuthorViewModel bookViewModel)
        {
            
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BookViewModel, BookDto>();
                cfg.CreateMap<AuthorViewModel, AuthorDto>();
                cfg.CreateMap<GenreViewModel, GenreDto>();
            });
            config.AssertConfigurationIsValid();
            var mapper = config.CreateMapper();
            
            return View();
        }
    }
}