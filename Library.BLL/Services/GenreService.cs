using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Library.BLL.DTO;
using Library.BLL.Interfaces;
using Library.DAL.Entities;
using Library.DAL.Interfaces;

namespace Library.BLL.Services
{
    public class GenreService : IGenreService
    {
        private readonly IRepository<Genre> _repository;

        public GenreService(IRepository<Genre> repository)
        {
            _repository = repository;
        }

        public void AddOrUpdate(GenreDto dto)
        {
            var genre = _repository.Get(dto.Id);

            if (genre == null)
            {
                genre = new Genre();
            }

            genre.Name = dto.Name;
            
            _repository.CreateOrUpdate(genre);
        }

        public GenreDto Get(int? id)
        {
            if (id == null)
                throw new ValidationException("Id not assigned");
            var entity = _repository.Get(id.Value);
            if (entity == null)
                throw new ValidationException("Author Not found");

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Genre, GenreDto>();
            });
            config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();

            var dto = mapper.Map<Genre, GenreDto>(entity);

            return dto;
        }

        public List<GenreDto> GetAll()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Genre, GenreDto>();
            });
            config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<Genre>, List<GenreDto>>(_repository.GetAll());
        }

        public void Delete(int? id)
        {
            if (id != null)
            {
                _repository.Delete(id.Value);
            }
        }
    }
}