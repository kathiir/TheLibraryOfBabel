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
    public class ReaderService : IReaderService
    {
        private readonly IRepository<Reader> _readerRepository;
        private readonly IBookLoanRecordService _bookLoanRecordService;

        public ReaderService(IRepository<Reader> readerRepository, IBookLoanRecordService bookLoanRecordService)
        {
            _readerRepository = readerRepository;
            _bookLoanRecordService = bookLoanRecordService;
        }

        public void AddOrUpdate(ReaderDto dto)
        {
            var reader = _readerRepository.Get(dto.Id);

            if (reader == null)
            {
                reader = new Reader();
            }

            reader.Name = dto.Name;

            foreach (var deleted in reader.BookLoanRecords.Where(record =>
                dto.BookLoanRecords.All(recordDto => recordDto.Id != record.Id)))
            {
                reader.BookLoanRecords.Remove(deleted);
            }

            _readerRepository.CreateOrUpdate(reader);

            dto.BookLoanRecords.ForEach(recordDto => { _bookLoanRecordService.AddOrUpdate(recordDto); });
        }

        public ReaderDto Get(int? id)
        {
            if (id == null)
                throw new ValidationException("Id not assigned");
            var entity = _readerRepository.Get(id.Value);
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

            var dto = mapper.Map<Reader, ReaderDto>(entity);

            return dto;
        }

        public List<ReaderDto> GetAll()
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
            return mapper.Map<IEnumerable<Reader>, List<ReaderDto>>(_readerRepository.GetAll());
        }

        public void Delete(int? id)
        {
            if (id != null)
            {
                _readerRepository.Delete(id.Value);
            }
        }
    }
}