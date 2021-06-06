using System;
using System.Collections.Generic;
using System.Linq;
using Library.BLL.DTO;
using Library.BLL.Interfaces;
using Library.BLL.Services;
using Library.DAL.EF;
using Library.DAL.Entities;
using Library.DAL.Interfaces;
using Library.DAL.Repositories;

namespace ConsoleLibrary
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ApplicationContext db = new ApplicationContext()) //NOT WORKING ALARM/REMEMBER TO ADD DB
            {
                //
                // Staff staff = new Staff {Name = "AAAAAAAAAAAA"};
                // Staff staff1 = new Staff {Name = "AAAAAAAAAAAA1"};
                //
                // Reader reader = new Reader {Name = "Reader1"};
                //
                //
                // IRepository<Staff> repository = new Repository<Staff>(db);
                //
                // IRepository<Reader> repositoryr = new Repository<Reader>(db);
                //
                // IRepository<Author> arep = new Repository<Author>(db);
                //
                // Author author = new Author {Name = "AAAAAAAAAAAA"};
                // author.Books.Add(new Book {Name = "ad", NumberOfCopies = 2});
                //
                // arep.Create(author);
                //
                // repository.Create(staff);
                // repository.Create(staff1);
                //
                // repositoryr.Create(reader);
                // db.SaveChanges();
                //
                // IBaseService<AuthorDTO> service = new AuthorService(arep);
                //
                // List<AuthorDTO> list = service.GetAll();
                //
                // Console.WriteLine(list[0].Books[0].Name);
                //
                // // db.ChangeTracker.Clear();
                // db.SaveChanges();
                //
                //
                // service.AddOrUpdate(list[0]);
                //
                // Reader reader2 = new Reader {Name = "Reader2"};
                //
                //
                // repositoryr.Create(reader2);


                // Genre genre = new Genre{Name = "Fantasy"};
                // repository.Create(genre);
                // genre = new Genre{Name = "Adventure"};
                // repository.Create(genre);
                // genre = new Genre{Name = "Horror"};
                // repository.Create(genre);
                // genre = new Genre{Name = "Science Fiction"};
                // repository.Create(genre);
                // genre = new Genre{Name = "History"};
                // repository.Create(genre);
                // genre = new Genre{Name = "Education"};
                // repository.Create(genre);

                // var e = repository.Get(1);

                // e.Name = "Fantasy";

                // repository.Update(e);


                // // db.SaveChanges();
                // IRepository<Book> repository = new Repository<Book>(db);
                // IRepository<Author> repositoryA = new Repository<Author>(db);
                // IRepository<Genre> repositoryg = new Repository<Genre>(db);
                //
                // BookDto book = new BookDto {Name = "asrs", NumberOfCopies = 5, NumberOfCopiesCurrent = 5};
                //
                // AuthorService serviceA = new AuthorService(repositoryA);
                // GenreService serviceg = new GenreService(repositoryg);
                //
                //
                // // AuthorDTO author = serviceA.Get(11);
                // var gen = serviceg.Get(5);
                // book.Authors = new List<AuthorDto>();
                // // book.Authors.Add(author);
                //
                // BookService service = new BookService(repository);
                // service.AddOrUpdate(book);                
                // book.Genre = gen;
                // service.AddOrUpdate(book);                
                //
                // // repository.Create(book);
                //
                // foreach (var item in repository.GetAll())
                // {
                //     Console.WriteLine(item.Authors);
                // }

                IRepository<Book> repository = new Repository<Book>(db);
                IRepository<Author> repositoryA = new Repository<Author>(db);
                IRepository<Genre> repositoryg = new Repository<Genre>(db);

                // repository.Delete();

                Author author = new Author {Name = "AAAA132123A"};
                
                Book book = new Book {Name = "asd"};
                
                book.Authors.Add(author);

                repository.CreateOrUpdate(book);

                Console.WriteLine("-------Books----------");
                
                foreach (var item in repository.GetAll())
                {
                    Console.WriteLine(item.Name);
                    foreach (var itemAuthor in item.Authors)
                    {
                        Console.WriteLine(itemAuthor.Name);
                    }
                }

                Console.WriteLine("-------New----------");

                Author author2 = new Author {Name = "bbb"};

                
                book.Name = "dfsdf";
                
                book.Authors.Add(author2);

                repository.CreateOrUpdate(book);
                
                foreach (var item in repository.GetAll())
                {
                    Console.WriteLine(item.Name);
                    foreach (var itemAuthor in item.Authors)
                    {
                        Console.WriteLine(itemAuthor.Name);
                    }
                }
            }
        }
    }
}