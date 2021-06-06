using System.ComponentModel.DataAnnotations.Schema;

namespace Library.DAL.Entities
{
    public class BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
    }
}