using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Library.BLL.DTO;
using Library.BLL.Interfaces;
using Library.DAL.Entities;
using Library.DAL.Interfaces;

namespace Library.BLL.Services
{
    public class BookLoanRecordService : IBookLoanRecordService
    {
        private readonly IRepository<BookLoanRecord> _bookLoanRecordRepository;
        private readonly IRepository<Book> _bookRepository;
        private readonly IRepository<Reader> _readerRepository;
        private readonly IRepository<Staff> _staffRepository;

        public BookLoanRecordService(IRepository<BookLoanRecord> bookLoanRecordRepository, 
            IRepository<Book> bookRepository, IRepository<Reader> readerRepository,
            IRepository<Staff> staffRepository)
        {
            _bookLoanRecordRepository = bookLoanRecordRepository;
            _bookRepository = bookRepository;
            _readerRepository = readerRepository;
            _staffRepository = staffRepository;
        }
        
        public void AddOrUpdate(BookLoanRecordDto dto)
        {
            if (dto.Book == null || dto.Reader == null)
            {
                return;
            }
            
            var bookLoanRecord = _bookLoanRecordRepository.Get(dto.Id);

            if (bookLoanRecord == null)
            {
                bookLoanRecord = new BookLoanRecord();
            }

            bookLoanRecord.BorrowDate = dto.BorrowDate;
            bookLoanRecord.DueDate = dto.DueDate;
            bookLoanRecord.ReturnDate = dto.ReturnDate;

            bookLoanRecord.Book = _bookRepository.Get(dto.Book.Id);
            bookLoanRecord.Reader = _readerRepository.Get(dto.Reader.Id);
            if (dto.Staff != null)
            {
                bookLoanRecord.Staff = _staffRepository.Get(dto.Staff.Id);
            }
            
            _bookLoanRecordRepository.CreateOrUpdate(bookLoanRecord);
        }

        public BookLoanRecordDto Get(int? id)
        {
            if (id == null)
                throw new ValidationException("Id not assigned");
            var entity = _bookLoanRecordRepository.Get(id.Value);
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

            var dto = mapper.Map<BookLoanRecord, BookLoanRecordDto>(entity);

            return dto;
        }

        public List<BookLoanRecordDto> GetAll()
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
            return mapper.Map<IEnumerable<BookLoanRecord>, List<BookLoanRecordDto>>(_bookLoanRecordRepository.GetAll());
        }

        public void Delete(int? id)
        {
            if (id != null)
            {
                _bookLoanRecordRepository.Delete(id.Value);
            }
        }
    }
}