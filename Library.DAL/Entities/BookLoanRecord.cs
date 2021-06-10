using System;
using System.ComponentModel.DataAnnotations;

namespace Library.DAL.Entities
{
    public class BookLoanRecord : BaseEntity
    {
        [Required]
        public virtual Book Book { get; set; }
        [Required]
        public virtual Reader Reader { get; set; }
        [Required]
        public virtual Staff Staff { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}