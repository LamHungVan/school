using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Week8_BT1_API_CURD.Models
{
    public class Teacher
    {
        [Key]
        public int TeacherID { get; set; } // Thay đổi kiểu dữ liệu nếu cần

        [Required]
        [MaxLength(100)] // Giới hạn độ dài tên giáo viên
        public string Name { get; set; }

        [Required]
        public int Age { get; set; }
    }
}
