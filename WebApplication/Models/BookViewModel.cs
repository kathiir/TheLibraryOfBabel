using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication.Models
{
    public class BookViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<AuthorViewModel> Authors { get; set; }
        public int NumberOfCopies { get; set; }
        // public int NumberOfCopiesCurrent { get; set; }
        public List<BookLoanRecordViewModel> BookLoanRecords { get; set; }
        public GenreViewModel Genre { get; set; }

        public BookViewModel()
        {
            Authors = new List<AuthorViewModel>();
            BookLoanRecords = new List<BookLoanRecordViewModel>();
        }

        public String GetAuthors()
        {
            String str = "";
            for (int i = 0; i < Authors.Count; i++)
            {
                str += Authors[i].Name;
                if (i != Authors.Count - 1)
                {
                    str += ", ";
                }
            }

            return str;
        }
    }
    
}