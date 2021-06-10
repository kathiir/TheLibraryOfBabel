using System;
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
    public class BookService : IBookService
    {
        private readonly IRepository<Book> _bookRepository;
        private readonly IRepository<Author> _authorRepository;
        private readonly IRepository<Genre> _genreRepository;
        private readonly IRepository<BookLoanRecord> _bookLoanRecordRepository;

        public BookService(IRepository<Book> bookRepository, IRepository<Author> authorRepository,
            IRepository<Genre> genreRepository, IRepository<BookLoanRecord> bookLoanRecordRepository)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _genreRepository = genreRepository;
            _bookLoanRecordRepository = bookLoanRecordRepository;
        }

        public void AddOrUpdate(BookDto dto)
        {
            var book = _bookRepository.Get(dto.Id);

            if (book == null)
            {
                book = new Book();
            }

            book.Name = dto.Name;

            var count = book.BookLoanRecords.Count(item => item.ReturnDate.HasValue);
            book.NumberOfCopies = dto.NumberOfCopies >= count ? dto.NumberOfCopies : count;
            
            
            if (dto.Genre != null)
            {
                book.Genre = _genreRepository.Get(dto.Genre.Id);
            }

            foreach (var deleted in book.Authors.Where(
                author => dto.Authors.All(authorDto => authorDto.Id != author.Id)).ToList())
            {
                book.Authors.Remove(deleted);
            }

            dto.Authors.ForEach(authorDto =>
            {
                var author = _authorRepository.Get(authorDto.Id);
                if (author == null)
                {
                    author = new Author();
                    author.Name = authorDto.Name;
                    _authorRepository.CreateOrUpdate(author);
                }

                if (book.Authors.All(a => a.Id != author.Id))
                {
                    book.Authors.Add(author);
                }
            });
            
            _bookRepository.CreateOrUpdate(book);
        }

        public BookDto Get(int id)
        {
            var book = _bookRepository.Get(id);
            if (book == null)
                return null;


            // var a = book.Genre;
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

            var dto = mapper.Map<Book, BookDto>(book);

            return dto;
        }

        public List<BookDto> GetAll()
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
            return mapper.Map<IEnumerable<Book>, List<BookDto>>(_bookRepository.GetAll());
        }

        public void Delete(int id)
        {
            // var records = _bookLoanRecordRepository.GetAll().ToList();
            //
            // records = records.Where(item => item.Book.Id == id).ToList();
            //
            // foreach (var bookLoanRecord in records)
            // {
            //     _bookLoanRecordRepository.Delete(bookLoanRecord.Id);
            // }
            
            _bookRepository.Delete(id);
        }

    }
}