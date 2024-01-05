using System.ComponentModel.DataAnnotations;

namespace User.Core.Models
{
    public class UserActivityLogModel
    {
        public int? UserId { get; set; }

        [Required(ErrorMessage = "Activity Type is required")]
        [MaxLength(50, ErrorMessage = "Activity Type cannot exceed 50 characters")]
        //[Display(Name = "Activity Type")]
        public string ActivityType { get; set; } = null!;

        [MaxLength(255, ErrorMessage = "Activity Description cannot exceed 255 characters")]
        //[Display(Name = "Activity Description")]
        public string? ActivityDescription { get; set; }

        //[Display(Name = "Log Date")]
        [DataType(DataType.DateTime)]
        public DateTime LogDate { get; set; }
    }
}
