﻿using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Library.BLL.DTO;
using Library.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication.Models;
using WebApplication.Utils;
using Castle.Core.Internal;

namespace WebApplication.Controllers
{
    public class StaffController : Controller
    {
        private readonly ILogger<StaffController> _logger;
        private readonly IStaffService _staffService;

        public StaffController(ILogger<StaffController> logger, IStaffService staffService)
        {
            _logger = logger;
            _staffService = staffService;
        }
        public IActionResult Index(
                    string sortBy,
                    string search,
                    int? pageNumber) 
                {
                    var staffList = _staffService.GetAll();
        
                    if (sortBy.IsNullOrEmpty())
                    {
                        sortBy = "name_asc";
                    }
        
                    ViewData["searchFilt"] = search;
                    ViewData["page"] = pageNumber;
                    
                    if (search != null)
                    {
                        staffList = staffList.Where(item => item.Name.ToLower().Contains(search)).ToList();
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
                    
                    var staff = mapper.Map<List<StaffDto>, List<StaffViewModel>>(staffList);
        
                    var sort = sortBy.Split("_");
        
                    bool desc = sort[1].Equals("desc");
                    
                    ViewData["numberOrder"] = "asc";
                    ViewData["nameOrder"] = "asc";
                    
                    ViewData["currentOrder"] = "asc";
        
                    ViewData["orderBy"] = sort[0];
            
                    switch (sort[0])
                    {
                        case "number":
                            staff = staff.OrderBy(item => item.BookLoanRecords.Count).ToList();
                            ViewData["numberOrder"] = desc ? "asc" : "desc";
                            ViewData["currentOrder"] = desc ? "asc" : "desc";
                            break;
                        default:
                            staff = staff.OrderBy(item => item.Name).ToList();
                            ViewData["nameOrder"] = desc ? "asc" : "desc";
                            ViewData["currentOrder"] = desc ? "asc" : "desc";
                            break;
                    }
        
                    if (desc)
                    {
                        staff.Reverse();
                    }
                    
                    return View(PaginatedList<StaffViewModel>.CreatePage(staff.AsQueryable(), pageNumber ?? 1, 10));
                }
        
        
        public IActionResult Staff(int id)
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

            var staffDto = _staffService.Get(id);
            if (staffDto == null)
            {
                return View(new StaffViewModel());
            }
            var readerViewModel = mapper.Map<StaffDto, StaffViewModel>(staffDto);

            return View(readerViewModel);
        }
        
        [HttpPost]
        public IActionResult Staff(StaffViewModel staffViewModel)
        {
            if (staffViewModel.Id == 0)
            {
                _logger.LogInformation($"Adding new staff");
            }
            else
                _logger.LogInformation($"Updating staff with id={staffViewModel.Id}");

            var staffDto = _staffService.Get(staffViewModel.Id);
            if (staffDto == null)
            {
                staffDto = new StaffDto();
            }

            staffDto.Name = staffViewModel.Name;

            _staffService.AddOrUpdate(staffDto);

            return RedirectPermanent("~/Staff/");
        }
        
        public IActionResult RemoveStaff(int id)
        {
            _logger.LogInformation($"Removing staff with id={id}");
            _staffService.Delete(id);
            return RedirectPermanent("~/Staff/");
        }
    }
}