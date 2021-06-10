using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using Library.BLL.DTO;
using Library.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication.Models;
using WebApplication.Utils;
using Castle.Core.Internal;
using ClosedXML.Excel;
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
            _logger.LogInformation($"Retrieving Authors, page={ pageNumber }, search={search}, sort={sortBy}");

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
                // cfg.CreateMap<BookLoanRecordDto, BookLoanRecordViewModel>();
                cfg.CreateMap<BookDto, BookViewModel>().ForMember(s => s.BookLoanRecords, m => m.Ignore());;
                cfg.CreateMap<AuthorDto, AuthorViewModel>();
                cfg.CreateMap<GenreDto, GenreViewModel>();
                // cfg.CreateMap<ReaderDto, ReaderViewModel>();
                // cfg.CreateMap<StaffDto, StaffViewModel>();
            });
            config.AssertConfigurationIsValid();
            var mapper = config.CreateMapper();

            var authors = mapper.Map<List<AuthorDto>, List<AuthorViewModel>>(authorList);

            var sort = sortBy.Split("_");

            bool desc = sort[1].Equals("desc");
            
            ViewData["numberOrder"] = "asc";
            ViewData["nameOrder"] = "asc";
            
            ViewData["currentOrder"] = sort[1];

            ViewData["orderBy"] = sort[0];
            
    
            switch (sort[0])
            {
                case "number":
                    authors = authors.OrderBy(item => item.Books.Count).ToList();
                    ViewData["numberOrder"] = desc ? "asc" : "desc";
                    break;
                default:
                    authors = authors.OrderBy(item => item.Name).ToList();
                    ViewData["nameOrder"] = desc ? "asc" : "desc";
                    break;
            }

            if (desc)
            {
                authors.Reverse();
            }

            // ViewData["Genres"] = genres;
            // ViewData["Authors"] = authors;
            
            return View(PaginatedList<AuthorViewModel>.CreatePage(authors.AsQueryable(), pageNumber ?? 1, 20));
        }
        
        public IActionResult Author(int id)
        {
            _logger.LogInformation($"Adding or editing Author with id ={id}");

            var bookDtos = _bookService.GetAll();

            var config = new MapperConfiguration(cfg =>
            {
                // cfg.CreateMap<BookLoanRecordDto, BookLoanRecordViewModel>();
                cfg.CreateMap<BookDto, BookViewModel>().ForMember(s => s.BookLoanRecords, m => m.Ignore());;
                cfg.CreateMap<AuthorDto, AuthorViewModel>();
                cfg.CreateMap<GenreDto, GenreViewModel>();
                // cfg.CreateMap<ReaderDto, ReaderViewModel>();
                // cfg.CreateMap<StaffDto, StaffViewModel>();
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
                cfg.CreateMap<BookViewModel, BookDto>().ForMember(s => s.BookLoanRecords, m => m.Ignore());;
                cfg.CreateMap<AuthorViewModel, AuthorDto>();
                cfg.CreateMap<GenreViewModel, GenreDto>();
                // cfg.CreateMap<BookLoanRecordViewModel, BookLoanRecordDto>();
                // cfg.CreateMap<ReaderViewModel, ReaderDto>();
                // cfg.CreateMap<StaffViewModel, StaffDto>();
            });
            var mapper = config.CreateMapper();

            var authorDto = mapper.Map<AuthorViewModel, AuthorDto>(authorViewModel);


            authorDto.Books = _bookService.GetAll().Where(item => books.Contains(item.Id)).ToList();
            
            _authorService.AddOrUpdate(authorDto);

            return RedirectPermanent("~/Authors/");
        }
        
        public IActionResult RemoveAuthor(int id)
        {
            _logger.LogInformation($"Removing author with id={id}");
            _authorService.Delete(id);
            return RedirectPermanent("~/Authors/");
        }
        
        public ActionResult Download()
        {
            using var workbook = new XLWorkbook();

            var authors = _authorService.GetAll();

            _logger.LogInformation($"Saving Excel file for Authors");

            var worksheet = workbook.Worksheets.Add("Items");
            worksheet.Cell("A1").Value = "Id";
            worksheet.Cell("B1").Value = "Author Name";
            worksheet.Cell("C1").Value = "Books";

            int row = 1;
            foreach (var item in authors)
            {
                var rowObj = worksheet.Row(++row);
                rowObj.Cell(1).Value = item.Id;
                rowObj.Cell(2).Value = item.Name;
                rowObj.Cell(3).Value = String.Join(",", item.Books.Select(cat => cat.Name).ToArray());
            }

            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = "Authors.xlsx",
                Inline = false, 
            };
            Response.Headers.Add("Content-Disposition", cd.ToString());
            using (MemoryStream stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Authors.xlsx");
            }
        }
        
    }
}