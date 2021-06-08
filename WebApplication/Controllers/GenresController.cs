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
using Castle.Core.Internal;
using ClosedXML.Excel;
using WebApplication.Utils;

namespace WebApplication.Controllers
{
    public class GenresController : Controller
    {
        private readonly ILogger<GenresController> _logger;
        private readonly IGenreService _genreService;


        public GenresController(ILogger<GenresController> logger, IGenreService genreService)
        {
            _logger = logger;
            _genreService = genreService;
        }

        public IActionResult Index(
            string sortBy,
            string search,
            int? pageNumber) 
        {
            _logger.LogInformation($"Retrieving Genres, page={pageNumber}");

            var genreList = _genreService.GetAll();
            
            if (sortBy.IsNullOrEmpty())
            {
                sortBy = "asc";
            }

            ViewData["searchFilt"] = search;
            ViewData["page"] = pageNumber;
            
            ViewData["nameOrder"] = sortBy.Equals("asc") ? "desc" : "asc";

            if (search != null)
            {
                genreList = genreList.Where(item => item.Name.ToLower().Contains(search)).ToList();
            }
            
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<GenreDto, GenreViewModel>();
            });
            config.AssertConfigurationIsValid();
            var mapper = config.CreateMapper();

            var genreViewModels = mapper.Map<List<GenreDto>, List<GenreViewModel>>(genreList);
            
            if (sortBy.Equals("desc"))
            {
                genreViewModels.Reverse();
            }

            return View(PaginatedList<GenreViewModel>.CreatePage(genreViewModels.AsQueryable(), pageNumber ?? 1, 10));
        }

        public IActionResult Genre(int id)
        {
            
            var genreDto = _genreService.Get(id);
            if (genreDto == null)
            {
                return View(new GenreViewModel());
            }
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<GenreDto, GenreViewModel>();
            });
            config.AssertConfigurationIsValid();
            var mapper = config.CreateMapper();
            
            var genreViewModel = mapper.Map<GenreDto, GenreViewModel>(genreDto);

            return View(genreViewModel);
        }

        [HttpPost]
        public IActionResult Genre(GenreViewModel genreViewModel)
        {
            if (genreViewModel.Id == 0)
            {
                _logger.LogInformation($"Adding new genre");
            }
            else
                _logger.LogInformation($"Updating genre with id={genreViewModel.Id}");

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<GenreViewModel, GenreDto>();
            });
            var mapper = config.CreateMapper();
            
            var genre =  mapper.Map<GenreViewModel, GenreDto>(genreViewModel);
            
            _genreService.AddOrUpdate(genre);
            
            return RedirectPermanent("~/Genres/");
        }


        public IActionResult RemoveGenre(int id)
        {
            _logger.LogInformation($"Removing genre with id={id}");
            _genreService.Delete(id);
            return RedirectPermanent("~/Authors/");
        }
        
        public ActionResult Download()
        {
            using var workbook = new XLWorkbook();

            var items = _genreService.GetAll();

            _logger.LogInformation($"Saving Excel file for Genres");

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
                FileName = "Genres.xlsx",
                Inline = false, 
            };
            Response.Headers.Add("Content-Disposition", cd.ToString());
            using (MemoryStream stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Genres.xlsx");
            }
        }
    }
}