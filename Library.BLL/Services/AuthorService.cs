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
    public class AuthorService : IAuthorService
    {
        private readonly IRepository<Author> _authorRepository;
        private readonly IRepository<Book> _bookRepository;

        public AuthorService(IRepository<Author> authorRepository, IRepository<Book> bookRepository)
        {
            _authorRepository = authorRepository;
            _bookRepository = bookRepository;
        }

        public void AddOrUpdate(AuthorDto authorDto)
        {
            var author = _authorRepository.Get(authorDto.Id);

            if (author == null)
            {
                author = new Author();
            }

            author.Name = authorDto.Name;

            foreach (var deleted in author.Books.Where(book => authorDto.Books.All(dto => dto.Id != book.Id)).ToList())
            {
                author.Books.Remove(deleted);
            }

            authorDto.Books.ForEach(dto =>
            {
                var book = _bookRepository.Get(dto.Id);
                if (book == null)
                {
                    return;
                }

                if (author.Books.All(b => b.Id != book.Id))
                {
                    author.Books.Add(book);
                }
            });

            _authorRepository.CreateOrUpdate(author);
        }

        public AuthorDto Get(int id)
        {
            var author = _authorRepository.Get(id);
            if (author == null)
                return null;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Author, AuthorDto>();
                cfg.CreateMap<Book, BookDto>();
                cfg.CreateMap<Genre, GenreDto>();
            });
            config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();

            var authorDto = mapper.Map<Author, AuthorDto>(author);

            return authorDto;
        }

        public List<AuthorDto> GetAll()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Author, AuthorDto>();
                cfg.CreateMap<Book, BookDto>();
                cfg.CreateMap<Genre, GenreDto>();
            });
            config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<Author>, List<AuthorDto>>(_authorRepository.GetAll());
        }

        public void Delete(int id)
        {
            _authorRepository.Delete(id);
        }


        public AuthorDto GetByNameOrCreate(string name)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Author, AuthorDto>();
                cfg.CreateMap<Book, BookDto>();
                cfg.CreateMap<Genre, GenreDto>();
            });
            config.AssertConfigurationIsValid();
            var mapper = config.CreateMapper();

            if (!_authorRepository.Find(item => item.Name.Equals(name)).Any())
                _authorRepository.Create(new Author {Name = name});

            var authorDto =
                mapper.Map<Author, AuthorDto>(_authorRepository.Find(item => item.Name.Equals(name)).First());

            return authorDto;
        }

        public List<AuthorDto> GetByNamesOrCreate(IEnumerable<string> names)
        {
            var authorDtos = new List<AuthorDto>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Author, AuthorDto>();
                cfg.CreateMap<Book, BookDto>();
                cfg.CreateMap<Genre, GenreDto>();
            });
            config.AssertConfigurationIsValid();
            var mapper = config.CreateMapper();

            foreach (var name in names)
            {
                if (!_authorRepository.Find(item => item.Name.Equals(name)).Any())
                    _authorRepository.Create(new Author {Name = name});

                var d = mapper.Map<Author, AuthorDto>(_authorRepository.Find(item => item.Name.Equals(name)).First());

                authorDtos.Add(d);
            }

            return authorDtos;
        }
    }
}