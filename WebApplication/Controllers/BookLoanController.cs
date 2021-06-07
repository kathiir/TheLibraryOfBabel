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
using Library.DAL.Entities;

namespace WebApplication.Controllers
{
    public class BookLoanController : Controller
    {
        private readonly ILogger<BookLoanController> _logger;
        private readonly IStaffService _staffService;
        private readonly IReaderService _readerService;
        private readonly IBookService _bookService;
        private readonly IBookLoanRecordService _bookLoanRecordService;

        public BookLoanController(ILogger<BookLoanController> logger, IStaffService staffService, 
            IReaderService readerService, IBookService bookService,
            IBookLoanRecordService bookLoanRecordService)
        {
            _logger = logger;
            _staffService = staffService;
            _readerService = readerService;
            _bookService = bookService;
            _bookLoanRecordService = bookLoanRecordService;
        }
        
        public IActionResult Index(
            string sortBy,
            int filter,
            string[] readerFilter,
            string[] bookFilter,
            string[] staffFilter,
            int? pageNumber) 
        {
            var bookDtos = _bookService.GetAll();
            var readerDtos = _readerService.GetAll();
            var staffDtos = _staffService.GetAll();

            var records = _bookLoanRecordService.GetAll();

            if (sortBy.IsNullOrEmpty())
            {
                sortBy = "book_asc";
            }

            if (readerFilter.Length == 1)
            {
                readerFilter = readerFilter[0].Split(",");
            }
            if (bookFilter.Length == 1)
            {
                bookFilter = bookFilter[0].Split(",");
            }
            if (staffFilter.Length == 1)
            {
                staffFilter = staffFilter[0].Split(",");
            }
            
            var bookFilt = Array.ConvertAll(bookFilter, s => int.Parse(s));
            var readerFilt = Array.ConvertAll(readerFilter, s => int.Parse(s));
            var staffFilt = Array.ConvertAll(staffFilter, s => int.Parse(s));


            ViewData["bookFilt"] = bookFilt;
            ViewData["readerFilt"] = readerFilt;
            ViewData["staffFilt"] = staffFilt;
            ViewData["bookFiltString"] = String.Join(",", bookFilt);
            ViewData["readerFiltString"] = String.Join(",", readerFilt);
            ViewData["staffFiltString"] = String.Join(",", staffFilt);

            ViewData["page"] = pageNumber;


            if (!bookFilt.IsNullOrEmpty())
            {
                records = records.Where(item => bookFilt.Contains(item.Id)).ToList();
            }
            
            if (!readerFilt.IsNullOrEmpty())
            {
                records = records.Where(item => readerFilt.Contains(item.Id)).ToList();
            }
            
            if (!staffFilt.IsNullOrEmpty())
            {
                records = records.Where(item => staffFilt.Contains(item.Id)).ToList();
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
            // config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();

            var books = mapper.Map<List<BookDto>, List<BookViewModel>>(bookDtos);
            var readers = mapper.Map<List<ReaderDto>, List<ReaderViewModel>>(readerDtos);
            var staff = mapper.Map<List<StaffDto>, List<StaffViewModel>>(staffDtos);
            

            var sort = sortBy.Split("_");

            bool desc = sort[1].Equals("desc");

            ViewData["bookOrder"] = "asc";
            ViewData["readerOrder"] = "asc";
            ViewData["staffOrder"] = "asc";
            ViewData["borrowOrder"] = "asc";
            ViewData["dueOrder"] = "asc";
            ViewData["returnOrder"] = "asc";

            ViewData["currentOrder"] = "asc";
            
            ViewData["orderBy"] = sort[0];
    
            switch (sort[0])
            {
                case "readers":
                    records = records.OrderBy(item => item.Reader == null ? "" : item.Reader.Name).ToList();
                    ViewData["readerOrder"] = desc ? "asc" : "desc";
                    ViewData["currentOrder"] = desc ? "asc" : "desc";
                    break;
                case "staff":
                    records = records.OrderBy(item => item.Staff == null ? "" : item.Staff.Name).ToList();
                    ViewData["staffOrder"] = desc ? "asc" : "desc";
                    ViewData["currentOrder"] = desc ? "asc" : "desc";
                    break;
                case "borrow":
                    records = records.OrderBy(item => item.BorrowDate).ToList();
                    ViewData["borrowOrder"] = desc ? "asc" : "desc";
                    ViewData["currentOrder"] = desc ? "asc" : "desc";
                    break;
                case "due":
                    records = records.OrderBy(item => item.DueDate).ToList();
                    ViewData["dueOrder"] = desc ? "asc" : "desc";
                    ViewData["currentOrder"] = desc ? "asc" : "desc";
                    break;
                case "return":
                    records = records.OrderBy(item => item.ReturnDate ?? DateTime.MaxValue).ToList();
                    ViewData["returnOrder"] = desc ? "asc" : "desc";
                    ViewData["currentOrder"] = desc ? "asc" : "desc";
                    break;
                default:
                    records = records.OrderBy(item => item.Book == null ? "" : item.Book.Name).ToList();
                    ViewData["bookOrder"] = desc ? "asc" : "desc";
                    ViewData["currentOrder"] = desc ? "asc" : "desc";
                    break;
            }

            if (desc)
            {
                records.Reverse();
            }

            ViewData["Readers"] = readers;
            ViewData["Books"] = books;
            ViewData["Staff"] = staff;
            
            var recordList = mapper.Map<List<BookLoanRecordDto>, List<BookLoanRecordViewModel>>(records);

            return View(PaginatedList<BookLoanRecordViewModel>.CreatePage(recordList.AsQueryable(), pageNumber ?? 1, 10));
        }
        
        public IActionResult RemoveRecord(int id)
        {
            _logger.LogInformation($"Removing record with id={id}");
            _bookService.Delete(id);
            return RedirectPermanent("~/BookLoan/");
        }
        
        public IActionResult ReturnRecord(int id)
        {
            _logger.LogInformation($"Returning record with id={id}");
            _bookService.Delete(id);
            return RedirectPermanent("~/BookLoan/");
        }
        
        public IActionResult ProlongRecord(int id)
        {
            _logger.LogInformation($"Prolonging record with id={id}");
            _bookService.Delete(id);
            return RedirectPermanent("~/BookLoan/");
        }
    }
}