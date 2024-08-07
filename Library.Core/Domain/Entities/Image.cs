using System.ComponentModel.DataAnnotations;

namespace Library.Core.Domain.Entities
{
    public class Image
    {
        public Guid ImageID {  get; set; }

        [Required]
        public string? ImageName { get; set; }
        [Required]
        public string? ImagePath { get; set; }
    }
}
