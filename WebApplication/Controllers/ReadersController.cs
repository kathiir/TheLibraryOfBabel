using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using Library.BLL.DTO;
using Library.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication.Models;
using Castle.Core.Internal;
using ClosedXML.Excel;
using WebApplication.Utils;

namespace WebApplication.Controllers
{
    public class ReadersController : Controller
    {
        private readonly ILogger<ReadersController> _logger;
        private readonly IReaderService _readerService;

        public ReadersController(ILogger<ReadersController> logger, IReaderService readerService)
        {
            _logger = logger;
            _readerService = readerService;
        }
        
        public IActionResult Index(
            string sortBy,
            string search,
            int? pageNumber) 
        {
            _logger.LogInformation($"Retrieving Readers, page={pageNumber}");

            
            var readerList = _readerService.GetAll();

            if (sortBy.IsNullOrEmpty())
            {
                sortBy = "name_asc";
            }

            ViewData["searchFilt"] = search;
            ViewData["page"] = pageNumber;
            
            if (search != null)
            {
                readerList = readerList.Where(item => item.Name.ToLower().Contains(search)).ToList();
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
            
            var readers = mapper.Map<List<ReaderDto>, List<ReaderViewModel>>(readerList);

            var sort = sortBy.Split("_");

            bool desc = sort[1].Equals("desc");
            
            ViewData["numberOrder"] = "asc";
            ViewData["nameOrder"] = "asc";
            
            ViewData["currentOrder"] = sort[1];

            ViewData["orderBy"] = sort[0];
    
            switch (sort[0])
            {
                case "number":
                    readers = readers.OrderBy(item => item.BookLoanRecords.Count).ToList();
                    ViewData["numberOrder"] = desc ? "asc" : "desc";
                    break;
                default:
                    readers = readers.OrderBy(item => item.Name).ToList();
                    ViewData["nameOrder"] = desc ? "asc" : "desc";
                    break;
            }

            if (desc)
            {
                readers.Reverse();
            }
            
            return View(PaginatedList<ReaderViewModel>.CreatePage(readers.AsQueryable(), pageNumber ?? 1, 20));
        }
        
        public IActionResult Reader(int id)
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

            var readerDto = _readerService.Get(id);
            if (readerDto == null)
            {
                return View(new ReaderViewModel());
            }
            var readerViewModel = mapper.Map<ReaderDto, ReaderViewModel>(readerDto);

            return View(readerViewModel);
        }
        
        [HttpPost]
        public IActionResult Reader(ReaderViewModel readerViewModel)
        {
            if (readerViewModel.Id == 0)
            {
                _logger.LogInformation($"Adding new reader");
            }
            else
                _logger.LogInformation($"Updating reader with id={readerViewModel.Id}");

            var readerDto = _readerService.Get(readerViewModel.Id);
            if (readerDto == null)
            {
                readerDto = new ReaderDto();
            }

            readerDto.Name = readerViewModel.Name;

            _readerService.AddOrUpdate(readerDto);

            return RedirectPermanent("~/Readers/");
        }
        
        public IActionResult RemoveReader(int id)
        {
            _logger.LogInformation($"Removing reader with id={id}");
            _readerService.Delete(id);
            return RedirectPermanent("~/Readers/");
        }
        
        public ActionResult Download()
        {
            using var workbook = new XLWorkbook();

            var items = _readerService.GetAll();

            _logger.LogInformation($"Saving Excel file for Readers");

            var worksheet = workbook.Worksheets.Add("Items");
            worksheet.Cell("A1").Value = "Id";
            worksheet.Cell("B1").Value = "Name";

            int row = 1;
            foreach (var item in items)
            {
                var rowObj = worksheet.Row(++row);
                rowObj.Cell(1).Value = item.Id;
                rowObj.Cell(2).Value = item.Name;
            }

            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = "Readers.xlsx",
                Inline = false, 
            };
            Response.Headers.Add("Content-Disposition", cd.ToString());
            using (MemoryStream stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Readers.xlsx");
            }
        }
    }
}