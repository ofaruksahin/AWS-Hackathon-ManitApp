using Pgvector;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManitApp.API.Domain.Entities
{
    public class OrderVector
    {
        public int Id { get; set; }
        public int OrderId { get; set; }

        [Column(TypeName = "vector(3)")]
        public Vector Vector { get; set; }

        public virtual Order Order { get; set; }
    }
}
