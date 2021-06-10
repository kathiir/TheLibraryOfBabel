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
        private readonly IAuthorService _authorService;

        public BookLoanController(ILogger<BookLoanController> logger, IStaffService staffService, 
            IReaderService readerService, IBookService bookService,
            IBookLoanRecordService bookLoanRecordService, IAuthorService authorService)
        {
            _logger = logger;
            _staffService = staffService;
            _readerService = readerService;
            _bookService = bookService;
            _bookLoanRecordService = bookLoanRecordService;
            _authorService = authorService;
        }
        
        public IActionResult Index(FilterViewModel filterViewModel, string command) 
        {
            _logger.LogInformation($"Retrieving Records, page={filterViewModel.PageNumber}");

            if (command == "reset")
            {
                filterViewModel = new FilterViewModel();
            }
            else if (command == "download")
            {
                return Download(filterViewModel);
            }
            
            var authorDtos = _authorService.GetAll();
            var bookDtos = _bookService.GetAll();
            var readerDtos = _readerService.GetAll();
            var staffDtos = _staffService.GetAll();

            var records = _bookLoanRecordService.GetAll();

            var authorFilt = ConvertFilter(filterViewModel.AuthorFilter);
            var bookFilt = ConvertFilter(filterViewModel.BookFilter);
            var readerFilt = ConvertFilter(filterViewModel.ReaderFilter);
            var staffFilt = ConvertFilter(filterViewModel.StaffFilter);
            
            records = ApplyFilter(filterViewModel, records);
            
            ViewData["Total"] = records.Count;
            ViewData["Yeti"] = records.Count(dto => !dto.ReturnDate.HasValue);
            ViewData["Returned"] = records.Count(dto => dto.ReturnDate.HasValue);
            ViewData["Overdue"] = records.Count(dto => !dto.ReturnDate.HasValue && dto.DueDate < DateTime.Now);


            ViewData["filter"] = filterViewModel.Filter;
            
            ViewData["authorFilt"] = authorFilt;
            ViewData["bookFilt"] = bookFilt;
            ViewData["readerFilt"] = readerFilt;
            ViewData["staffFilt"] = staffFilt;
            ViewData["authorFiltString"] = String.Join(",", authorFilt);
            ViewData["bookFiltString"] = String.Join(",", bookFilt);
            ViewData["readerFiltString"] = String.Join(",", readerFilt);
            ViewData["staffFiltString"] = String.Join(",", staffFilt);
            
            ViewData["page"] = filterViewModel.PageNumber;
            
            ViewData["bookOrder"] = "asc";
            ViewData["readerOrder"] = "asc";
            ViewData["staffOrder"] = "asc";
            ViewData["borrowOrder"] = "asc";
            ViewData["dueOrder"] = "asc";
            ViewData["returnOrder"] = "asc";

            ViewData["BorrowDateMinFilter"] = filterViewModel.BorrowDateMinFilter?.ToString("yyyy-MM-dd");
            ViewData["BorrowDateMaxFilter"] = filterViewModel.BorrowDateMaxFilter?.ToString("yyyy-MM-dd");
            ViewData["DueDateMinFilter"] = filterViewModel.DueDateMinFilter?.ToString("yyyy-MM-dd");
            ViewData["DueDateManFilter"] = filterViewModel.DueDateManFilter?.ToString("yyyy-MM-dd");
            ViewData["ReturnDateMinFilter"] = filterViewModel.ReturnDateMinFilter?.ToString("yyyy-MM-dd");
            ViewData["ReturnDateManFilter"] = filterViewModel.ReturnDateManFilter?.ToString("yyyy-MM-dd");
            
            var sort = filterViewModel.SortBy.Split("_");
            var desc = sort[1].Equals("desc");

            ViewData["orderBy"] = sort[0];
            ViewData["currentOrder"] = sort[1];
            
            switch (sort[0])
            {
                case "readers":
                    ViewData["readerOrder"] = desc ? "asc" : "desc";
                    break;
                case "staff":
                    ViewData["staffOrder"] = desc ? "asc" : "desc";
                    break;
                case "borrow":
                    ViewData["borrowOrder"] = desc ? "asc" : "desc";
                    break;
                case "due":
                    ViewData["dueOrder"] = desc ? "asc" : "desc";
                    break;
                case "return":
                    ViewData["returnOrder"] = desc ? "asc" : "desc";
                    break;
                default:
                    ViewData["bookOrder"] = desc ? "asc" : "desc";
                    break;
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

            var mapper = config.CreateMapper();

            var authors = mapper.Map<List<AuthorDto>, List<AuthorViewModel>>(authorDtos);
            var books = mapper.Map<List<BookDto>, List<BookViewModel>>(bookDtos);
            var readers = mapper.Map<List<ReaderDto>, List<ReaderViewModel>>(readerDtos);
            var staff = mapper.Map<List<StaffDto>, List<StaffViewModel>>(staffDtos);
            
            ViewData["Authors"] = authors;
            ViewData["Readers"] = readers;
            ViewData["Books"] = books;
            ViewData["Staff"] = staff;
            
            var recordList = mapper.Map<List<BookLoanRecordDto>, List<BookLoanRecordViewModel>>(records);

            return View(PaginatedList<BookLoanRecordViewModel>.CreatePage(recordList.AsQueryable(), filterViewModel.PageNumber ?? 1, 20));
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

            var books = _bookService.GetAll().Where(item => item.NumberOfCopies > item.BookLoanRecords.Count(i => !i.ReturnDate.HasValue)).ToList();
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
            if (bookDto != null && bookDto.NumberOfCopies > bookDto.BookLoanRecords.Count(i => !i.ReturnDate.HasValue) && reader != 0 && book != 0)
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
                // bookDto.NumberOfCopiesCurrent--;
                // _bookService.AddOrUpdate(bookDto);
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
        
        public ActionResult Download(FilterViewModel filterViewModel)
        {
            
            using var workbook = new XLWorkbook();

            var items = _bookLoanRecordService.GetAll();

            if (filterViewModel != null)
            {
                items = ApplyFilter(filterViewModel, items, true);
            }

            _logger.LogInformation($"Saving Excel file for Records");

            var worksheet = workbook.Worksheets.Add("Items");
            worksheet.Cell("A1").Value = "Id";
            worksheet.Cell("B1").Value = "Book";
            worksheet.Cell("C1").Value = "Reader";
            worksheet.Cell("D1").Value = "Staff";
            worksheet.Cell("E1").Value = "Borrow Date";
            worksheet.Cell("F1").Value = "Due Date";
            worksheet.Cell("G1").Value = "Return Date";
            
            worksheet.Cell("I1").Value = "Total records";
            worksheet.Cell("J1").Value = items.Count;
            worksheet.Cell("I2").Value = "Books yet to return";
            worksheet.Cell("J2").Value = items.Count(dto => !dto.ReturnDate.HasValue);
            worksheet.Cell("I3").Value = "Books returned";
            worksheet.Cell("J3").Value = items.Count(dto => dto.ReturnDate.HasValue);
            worksheet.Cell("I4").Value = "Books overdue";
            worksheet.Cell("J4").Value = items.Count(dto => !dto.ReturnDate.HasValue && dto.DueDate < DateTime.Now);


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

        [NonAction]
        private int[] ConvertFilter(string[] filter)
        {
            if (filter.Length == 1)
            {
                filter = filter[0].Split(",");
            }
            return Array.ConvertAll(filter, int.Parse);
        }
        
        [NonAction]
        private List<BookLoanRecordDto> ApplyFilter(FilterViewModel filterViewModel, List<BookLoanRecordDto> records, bool isDownload = false)
        {
            if (filterViewModel.SortBy.IsNullOrEmpty())
            {
                filterViewModel.SortBy = "book_asc";
            }
            if (isDownload)
            {
                filterViewModel.SortBy = "id_asc";
            }
            var authorFilt = ConvertFilter(filterViewModel.AuthorFilter);
            var bookFilt = ConvertFilter(filterViewModel.BookFilter);
            var readerFilt = ConvertFilter(filterViewModel.ReaderFilter);
            var staffFilt = ConvertFilter(filterViewModel.StaffFilter);
            
            if (!authorFilt.IsNullOrEmpty())
            {
                records = records.Where(item => item.Book.Authors.Any(it => authorFilt.Contains(it.Id))).ToList();
            }
            if (!bookFilt.IsNullOrEmpty())
            {
                records = records.Where(item => bookFilt.Contains(item.Book.Id)).ToList();
            }
            if (!readerFilt.IsNullOrEmpty())
            {
                records = records.Where(item => readerFilt.Contains(item.Reader.Id)).ToList();
            }
            if (!staffFilt.IsNullOrEmpty())
            {
                records = records.Where(item => staffFilt.Contains(item.Staff.Id)).ToList();
            }
            
            if (filterViewModel.BorrowDateMinFilter.HasValue)
            {
                records = records.Where(item => item.BorrowDate >= filterViewModel.BorrowDateMinFilter).ToList();
            }
            if (filterViewModel.BorrowDateMaxFilter.HasValue)
            {
                records = records.Where(item => item.BorrowDate < filterViewModel.BorrowDateMaxFilter.Value.AddDays(1)).ToList();
            }
            if (filterViewModel.DueDateMinFilter.HasValue)
            {
                records = records.Where(item => item.DueDate >= filterViewModel.DueDateMinFilter).ToList();
            }
            if (filterViewModel.DueDateManFilter.HasValue)
            {
                records = records.Where(item => item.DueDate < filterViewModel.DueDateManFilter.Value.AddDays(1)).ToList();
            }
            if (filterViewModel.ReturnDateMinFilter.HasValue)
            {
                records = records.Where(item => item.ReturnDate >= filterViewModel.ReturnDateMinFilter).ToList();
            }
            if (filterViewModel.ReturnDateManFilter.HasValue)
            {
                records = records.Where(item => item.ReturnDate < filterViewModel.ReturnDateManFilter.Value.AddDays(1)).ToList();
            }

            records = filterViewModel.Filter switch
            {
                1 => records.Where(item => item.ReturnDate != null).ToList(),
                2 => records.Where(item => item.ReturnDate == null).ToList(),
                _ => records
            };
            
            var sort = filterViewModel.SortBy.Split("_");
            var desc = sort[1].Equals("desc");

            records = sort[0] switch
            {
                "readers" => records.OrderBy(item => item.Reader == null ? "" : item.Reader.Name).ToList(),
                "staff" => records.OrderBy(item => item.Staff == null ? "" : item.Staff.Name).ToList(),
                "borrow" => records.OrderBy(item => item.BorrowDate).ToList(),
                "due" => records.OrderBy(item => item.DueDate).ToList(),
                "id" => records.OrderBy(item => item.Id).ToList(),
                "return" => records.OrderBy(item => item.ReturnDate ?? DateTime.MaxValue).ToList(),
                _ => records.OrderBy(item => item.Book == null ? "" : item.Book.Name).ToList()
            };
            
            if (desc)
            {
                records.Reverse();
            }

            return records;
        }
    }
}