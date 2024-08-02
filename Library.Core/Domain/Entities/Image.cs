using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Domain.Entities
{
    public class Image
    {
        [Key]
        public Guid ImageID { get; set; }
        public string ImagePath { get; set; }
    }
}
