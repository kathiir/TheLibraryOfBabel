using System.Collections.Generic;
using AutoMapper;
using Library.BLL.DTO;
using Library.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class GenreController : Controller
    {
        private readonly ILogger<GenreController> _logger;
        private readonly IGenreService _genreService;


        public GenreController(ILogger<GenreController> logger, IGenreService genreService)
        {
            _logger = logger;
            _genreService = genreService;
        }
        
        public IActionResult Index()
        {
            var genreDtos = _genreService.GetAll();
            
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<GenreDto, GenreViewModel>();
            });
            config.AssertConfigurationIsValid();
            var mapper = config.CreateMapper();

            var genreViewModels = mapper.Map<List<GenreDto>, List<GenreViewModel>>(genreDtos);
            
            return View(genreViewModels);
        }
        
    }
}