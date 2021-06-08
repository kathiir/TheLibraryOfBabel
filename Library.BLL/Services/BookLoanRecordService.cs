using System;
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
        private readonly IBookService _bookService;

        public BookLoanRecordService(IRepository<BookLoanRecord> bookLoanRecordRepository,
            IRepository<Book> bookRepository, IRepository<Reader> readerRepository,
            IRepository<Staff> staffRepository, IBookService bookService)
        {
            _bookLoanRecordRepository = bookLoanRecordRepository;
            _bookRepository = bookRepository;
            _readerRepository = readerRepository;
            _staffRepository = staffRepository;
            _bookService = bookService;
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

        public BookLoanRecordDto Get(int id)
        {
            var entity = _bookLoanRecordRepository.Get(id);
            if (entity == null)
                return null;


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

        public void Delete(int id)
        {
            var record = Get(id);
            if (record != null)
            {
                _bookLoanRecordRepository.Delete(id);
                if (!record.ReturnDate.HasValue)
                {
                    _bookService.UpCount(record.Book.Id);
                }
            }
        }

        public void Return(int id)
        {
            var record = _bookLoanRecordRepository.Get(id);
            if (record != null && !record.ReturnDate.HasValue)
            {
                record.ReturnDate = DateTime.Now;
                _bookLoanRecordRepository.CreateOrUpdate(record);
                _bookService.UpCount(record.Book.Id);

            }
        }

        public void AddTime(int id)
        {
            var record = _bookLoanRecordRepository.Get(id);
            if (record != null && !record.ReturnDate.HasValue)
            {
                record.DueDate = record.DueDate.AddDays(7);
                _bookLoanRecordRepository.CreateOrUpdate(record);
            }
        }
    }
}