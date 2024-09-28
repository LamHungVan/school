using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Week8_BT1_API_CURD.Models
{
    public class Student
    {
        [Key] // Đánh dấu là khóa chính
        public Guid StudentUUID { get; set; }

        [Required] // Bắt buộc phải có
        [MaxLength(100)] // Giới hạn độ dài chuỗi
        public string Name { get; set; }

        [Required]
        [MaxLength(10)]
        public string Gender { get; set; }
        public string Description { get; set; }
        [Required]
        public int Age { get; set; }

        [Required]
        public double GPA { get; set; }

        [ForeignKey("Class")] // Khóa ngoại liên kết với bảng Classes
        public int ClassID { get; set; }
       
    }
}
