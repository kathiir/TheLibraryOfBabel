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
            _logger.LogInformation($"Retrieving Records, page={pageNumber}");

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

            ViewData["filter"] = filter;


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

            if (filter == 1)
            {
                records = records.Where(item => item.ReturnDate != null).ToList();
            }
            else if (filter == 2)
            {
                records = records.Where(item => item.ReturnDate == null).ToList();
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
        
        public IActionResult AddRecord()
        {
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

            var books = _bookService.GetAll().Where(item => item.NumberOfCopiesCurrent > 0).ToList();
            var readers = _readerService.GetAll();
            var staff = _staffService.GetAll();
            
            ViewData["Readers"] = mapper.Map<List<ReaderDto>, List<ReaderViewModel>>(readers);
            ViewData["Books"] = mapper.Map<List<BookDto>, List<BookViewModel>>(books);
            ViewData["Staff"] = mapper.Map<List<StaffDto>, List<StaffViewModel>>(staff);
            
            return View();
        }
        
        [HttpPost]
        public IActionResult AddRecord(int reader, int staff, int book)
        {

            var bookDto = _bookService.Get(book);
            if (bookDto != null && bookDto.NumberOfCopiesCurrent > 0 && reader != 0 && book != 0)
            {
                var record = new BookLoanRecordDto();
                record.Book = bookDto;
                record.Reader = _readerService.Get(reader);
                if (staff != 0)
                {
                    record.Staff = _staffService.Get(staff);
                }
                record.BorrowDate = DateTime.Now;
                record.DueDate = record.BorrowDate.AddDays(7);
                
                _bookLoanRecordService.AddOrUpdate(record);
                bookDto.NumberOfCopiesCurrent--;
                _bookService.AddOrUpdate(bookDto);
                _logger.LogInformation($"Adding record for reader={reader}, staff={staff}, book={book}");

            }
            
            return RedirectPermanent("~/BookLoan/");
        }

        
        public IActionResult RemoveRecord(int id)
        {
            _logger.LogInformation($"Removing record with id={id}");
            _bookLoanRecordService.Delete(id);
            return RedirectPermanent("~/BookLoan/");
        }
        
        public IActionResult ReturnRecord(int id)
        {
            _logger.LogInformation($"Returning record with id={id}");
            _bookLoanRecordService.Return(id);
            return RedirectPermanent("~/BookLoan/");
        }
        
        public IActionResult ProlongRecord(int id)
        {
            _logger.LogInformation($"Prolonging record with id={id}");
            _bookLoanRecordService.AddTime(id);
            return RedirectPermanent("~/BookLoan/");
        }
        
        public ActionResult Download()
        {
            using var workbook = new XLWorkbook();

            var items = _bookLoanRecordService.GetAll();

            _logger.LogInformation($"Saving Excel file for Records");

            var worksheet = workbook.Worksheets.Add("Items");
            worksheet.Cell("A1").Value = "Id";
            worksheet.Cell("B1").Value = "Book";
            worksheet.Cell("C1").Value = "Reader";
            worksheet.Cell("D1").Value = "Staff";
            worksheet.Cell("E1").Value = "Borrow Date";
            worksheet.Cell("F1").Value = "Due Date";
            worksheet.Cell("G1").Value = "Return Date";

            int row = 1;
            foreach (var item in items)
            {
                var rowObj = worksheet.Row(++row);
                rowObj.Cell(1).Value = item.Id;
                rowObj.Cell(2).Value = item.Book.Name;
                rowObj.Cell(3).Value = item.Reader.Name;
                rowObj.Cell(4).Value = item.Staff?.Name;
                rowObj.Cell(5).Value = item.BorrowDate;
                rowObj.Cell(6).Value = item.DueDate;
                rowObj.Cell(7).Value = item.ReturnDate;
            }

            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = "Records.xlsx",
                Inline = false, 
            };
            Response.Headers.Add("Content-Disposition", cd.ToString());
            using (MemoryStream stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Records.xlsx");
            }
        }
    }
}