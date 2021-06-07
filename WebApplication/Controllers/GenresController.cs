using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Library.BLL.DTO;
using Library.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication.Models;
using Castle.Core.Internal;
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
    }
}