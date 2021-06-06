using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using Library.BLL.DTO;
using Library.BLL.Interfaces;
using Library.DAL.Entities;
using Library.DAL.Interfaces;

namespace Library.BLL.Services
{
    public class StaffService : IStaffService
    {
        private readonly IRepository<Staff> _staffRepository;
        private readonly IBookLoanRecordService _bookLoanRecordService;

        public StaffService(IRepository<Staff> staffRepository, IBookLoanRecordService bookLoanRecordService)
        {
            _staffRepository = staffRepository;
            _bookLoanRecordService = bookLoanRecordService;
        }

        public void AddOrUpdate(StaffDto dto)
        {
            var staff = _staffRepository.Get(dto.Id);

            if (staff == null)
            {
                staff = new Staff();
            }

            staff.Name = dto.Name;
            
            foreach (var deleted in staff.BookLoanRecords.Where(record => dto.BookLoanRecords.All(recordDto => recordDto.Id != record.Id)))
            {
                staff.BookLoanRecords.Remove(deleted);
            }
            
            _staffRepository.CreateOrUpdate(staff);

            dto.BookLoanRecords.ForEach(recordDto =>
            {
                // var record = _bookLoanRecordRepository.Get(dto.Id);
                // if (record == null)
                // {
                    _bookLoanRecordService.AddOrUpdate(recordDto);
                    // return;
                // }
            });
            
        }

        public StaffDto Get(int? id)
        {
            if (id == null)
                throw new ValidationException("Id not assigned");
            var entity = _staffRepository.Get(id.Value);
            if (entity == null)
                throw new ValidationException("Author Not found");

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BookLoanRecord, BookLoanRecordDto>();
                cfg.CreateMap<Book, BookDto>();
                cfg.CreateMap<Author, AuthorDto>();
                cfg.CreateMap<Genre, GenreDto>();
                cfg.CreateMap<Reader, ReaderDto>();
                cfg.CreateMap<Staff, StaffDto>();
            });
            config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();

            var dto = mapper.Map<Staff, StaffDto>(entity);

            return dto;
        }

        public List<StaffDto> GetAll()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BookLoanRecord, BookLoanRecordDto>();
                cfg.CreateMap<Book, BookDto>();
                cfg.CreateMap<Author, AuthorDto>();
                cfg.CreateMap<Genre, GenreDto>();
                cfg.CreateMap<Reader, ReaderDto>();
                cfg.CreateMap<Staff, StaffDto>();
            });
            config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<Staff>, List<StaffDto>>(_staffRepository.GetAll());

        }

        public void Delete(int? id)
        {
            if (id != null)
            {
                _staffRepository.Delete(id.Value);
            }
        }
    }
}