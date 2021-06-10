using System;

namespace WebApplication.Models
{
    public class FilterViewModel
    {
        public string SortBy { get; set; } = "";
        public int Filter { get; set; }
        public string[] AuthorFilter { get; set; } = Array.Empty<string>();
        public string[] ReaderFilter { get; set; } = Array.Empty<string>();
        public string[] BookFilter { get; set; } = Array.Empty<string>();
        public string[] StaffFilter { get; set; } = Array.Empty<string>();
        public int? PageNumber { get; set; }
        public DateTime? BorrowDateMinFilter { get; set; }
        public DateTime? BorrowDateMaxFilter { get; set; }
        public DateTime? DueDateMinFilter { get; set; }
        public DateTime? DueDateManFilter { get; set; }
        public DateTime? ReturnDateMinFilter { get; set; }
        public DateTime? ReturnDateManFilter { get; set; }
    }
}