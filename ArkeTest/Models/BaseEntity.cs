using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArkeTest.Models
{
    public class BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }

        public BaseEntity()
        {
            CreatedAt = DateTime.UtcNow;
            ModifiedAt = DateTime.UtcNow;
        }

        public void UpdateModifiedAt()
        {
            ModifiedAt = DateTime.UtcNow;
        }
    }
}
