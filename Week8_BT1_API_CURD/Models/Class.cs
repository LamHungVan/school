using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Week8_BT1_API_CURD.Models
{
    public class Class
    {
        [Key] // Đánh dấu là khóa chính
        public int ClassID { get; set; }

        [Required]
        [MaxLength(100)] // Giới hạn độ dài tên lớp
        public string ClassName { get; set; }

        [ForeignKey("Teacher")] // Khóa ngoại liên kết với bảng Teachers
        public int TeacherID { get; set; }


    }
}
