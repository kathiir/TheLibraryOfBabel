using System.Collections.Generic;
using AutoMapper;
using Library.BLL.DTO;
using Library.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IReaderService _readerService;
        private readonly IStaffService _staffService;

        public UsersController(ILogger<UsersController> logger, IReaderService readerService, IStaffService staffService)
        {
            _logger = logger;
            _readerService = readerService;
            _staffService = staffService;
        }
        
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult UserList()
        {
            var readerDtos = _readerService.GetAll();
            
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BookDto, BookViewModel>();
                cfg.CreateMap<AuthorDto, AuthorViewModel>();
                cfg.CreateMap<GenreDto, GenreViewModel>();
                cfg.CreateMap<BookLoanRecordDto, BookLoanRecordViewModel>();
                cfg.CreateMap<StaffDto, StaffViewModel>();
                cfg.CreateMap<ReaderDto, ReaderViewModel>();

            });
            config.AssertConfigurationIsValid();
            var mapper = config.CreateMapper();

            var readerViewModels = mapper.Map<List<ReaderDto>, List<ReaderViewModel>>(readerDtos);

            return View(readerViewModels);
        }

        public IActionResult User(int id)
        {
            var readerDtos = _readerService.Get(id);
            
            if (readerDtos == null)
            {
                return StatusCode(404);
            }
            
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BookDto, BookViewModel>();
                cfg.CreateMap<AuthorDto, AuthorViewModel>();
                cfg.CreateMap<GenreDto, GenreViewModel>();
                cfg.CreateMap<BookLoanRecordDto, BookLoanRecordViewModel>();
                cfg.CreateMap<StaffDto, StaffViewModel>();
                cfg.CreateMap<ReaderDto, ReaderViewModel>();

            });
            config.AssertConfigurationIsValid();
            var mapper = config.CreateMapper();

            var readerViewModels = mapper.Map<ReaderDto, ReaderViewModel>(readerDtos);

            return View(readerViewModels);
        }
        
        public IActionResult Staff(int id)
        {
            var staffDto = _staffService.Get(id);
            
            if (staffDto == null)
            {
                return StatusCode(404);
            }
            
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BookDto, BookViewModel>();
                cfg.CreateMap<AuthorDto, AuthorViewModel>();
                cfg.CreateMap<GenreDto, GenreViewModel>();
                cfg.CreateMap<BookLoanRecordDto, BookLoanRecordViewModel>();
                cfg.CreateMap<StaffDto, StaffViewModel>();
                cfg.CreateMap<ReaderDto, ReaderViewModel>();

            });
            config.AssertConfigurationIsValid();
            var mapper = config.CreateMapper();

            var staffViewModel = mapper.Map<StaffDto, StaffViewModel>(staffDto);

            return View(staffViewModel);

        }

    }
}