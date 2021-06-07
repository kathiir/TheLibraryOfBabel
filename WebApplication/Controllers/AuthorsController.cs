using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Library.BLL.DTO;
using Library.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication.Models;
using WebApplication.Utils;
using Castle.Core.Internal;
using Library.BLL.Services;

namespace WebApplication.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly ILogger<AuthorsController> _logger;
        private readonly IAuthorService _authorService;
        private readonly IBookService _bookService;

        public AuthorsController(ILogger<AuthorsController> logger, IAuthorService authorService, IBookService bookService)
        {
            _logger = logger;
            _authorService = authorService;
            _bookService = bookService;
        }

        public IActionResult Index(
            string sortBy,
            string search,
            int? pageNumber) 
        {
            var authorList = _authorService.GetAll();

            if (sortBy.IsNullOrEmpty())
            {
                sortBy = "name_asc";
            }

            ViewData["searchFilt"] = search;
            ViewData["page"] = pageNumber;
            
            if (search != null)
            {
                authorList = authorList.Where(item => item.Name.ToLower().Contains(search)).ToList();
            }

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BookDto, BookViewModel>();
                cfg.CreateMap<AuthorDto, AuthorViewModel>();
                cfg.CreateMap<GenreDto, GenreViewModel>();
            });
            config.AssertConfigurationIsValid();
            var mapper = config.CreateMapper();

            var authors = mapper.Map<List<AuthorDto>, List<AuthorViewModel>>(authorList);

            var sort = sortBy.Split("_");

            bool desc = sort[1].Equals("desc");
            
            ViewData["numberOrder"] = "asc";
            ViewData["nameOrder"] = "asc";
            
            ViewData["currentOrder"] = "asc";

            ViewData["orderBy"] = sort[0];
    
            switch (sort[0])
            {
                case "number":
                    authors = authors.OrderBy(item => item.Books.Count).ToList();
                    ViewData["numberOrder"] = desc ? "asc" : "desc";
                    ViewData["currentOrder"] = desc ? "asc" : "desc";
                    break;
                default:
                    authors = authors.OrderBy(item => item.Name).ToList();
                    ViewData["nameOrder"] = desc ? "asc" : "desc";
                    ViewData["currentOrder"] = desc ? "asc" : "desc";
                    break;
            }

            if (desc)
            {
                authors.Reverse();
            }

            // ViewData["Genres"] = genres;
            // ViewData["Authors"] = authors;
            
            return View(PaginatedList<AuthorViewModel>.CreatePage(authors.AsQueryable(), pageNumber ?? 1, 10));
        }
        
        public IActionResult Author(int id)
        {
            var bookDtos = _bookService.GetAll();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BookDto, BookViewModel>();
                cfg.CreateMap<AuthorDto, AuthorViewModel>();
                cfg.CreateMap<GenreDto, GenreViewModel>();
            });
            config.AssertConfigurationIsValid();
            var mapper = config.CreateMapper();

            var bookViewModels = mapper.Map<List<BookDto>, List<BookViewModel>>(bookDtos);

            ViewData["Books"] = bookViewModels;

            var authorDto = _authorService.Get(id);
            if (authorDto == null)
            {
                return View(new AuthorViewModel());
            }
            var authorViewModel = mapper.Map<AuthorDto, AuthorViewModel>(authorDto);

            return View(authorViewModel);
        }
        
        [HttpPost]
        public IActionResult Author(AuthorViewModel authorViewModel, int[] books)
        {
            if (authorViewModel.Id == 0)
            {
                _logger.LogInformation($"Adding new author");
            }
            else
                _logger.LogInformation($"Updating author with id={authorViewModel.Id}");

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BookViewModel, BookDto>();
                cfg.CreateMap<AuthorViewModel, AuthorDto>();
                cfg.CreateMap<GenreViewModel, GenreDto>();
            });
            var mapper = config.CreateMapper();

            var authorDto = mapper.Map<AuthorViewModel, AuthorDto>(authorViewModel);


            authorDto.Books = _bookService.GetAll().Where(item => books.Contains(item.Id)).ToList();
            
            _authorService.AddOrUpdate(authorDto);

            return RedirectPermanent("~/Authors/");
        }
        
        public IActionResult RemoveAuthor(int id)
        {
            _logger.LogInformation($"Removing user with id={id}");
            _authorService.Delete(id);
            return RedirectPermanent("~/Authors/");
        }
        
    }
}