using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArkeTest.Models
{
    public class BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required Guid Id { get; set; }

        public required DateTime CreatedAt { get; set; }

        public required DateTime ModifiedAt { get; set; }

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
