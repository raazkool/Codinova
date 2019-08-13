using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CodinovaTask.Model
{
    public abstract class BaseEntity
    {
        [Required]
        [DisplayName("Created  Date")]
        public DateTime CreatedDate { get; set; }
        [Required]
        [DisplayName("Created  By")]
        public int CreatedBy { get; set; }
        [DisplayName("Modified  Date")]
        public Nullable<DateTime> ModifiedDate { get; set; }
        [DisplayName("Modified  By")]
        public Nullable<int> ModifiedBy { get; set; }
        [DefaultValue(false)]
        public bool IsDelated { get; set; }
        [DefaultValue(true)]
        public bool IsActive { get; set; }
        public BaseEntity()
        {
            CreatedDate = DateTime.Now;
        }
    }
}
