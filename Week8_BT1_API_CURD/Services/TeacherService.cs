using Microsoft.EntityFrameworkCore;
using Week8_BT1_API_CURD.Models;

namespace Week8_BT1_API_CURD.Services
{
    public class TeacherService
    {
        private readonly ApplicationDbContext _context;

        public TeacherService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Teacher>> GetTeachersAsync()
        {
            return await _context.Teachers.ToListAsync();
        }

        public async Task<Teacher> GetTeacherByIdAsync(int id)
        {
            return await _context.Teachers.FindAsync(id);
        }

        public async Task<Teacher> CreateTeacherAsync(Teacher teacher)
        {
            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync();
            return teacher;
        }

        public async Task UpdateTeacherAsync(int id, Teacher teacher)
        {
            if (id != teacher.TeacherID)
                throw new ArgumentException("ID mismatch");

            _context.Entry(teacher).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTeacherAsync(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null)
                throw new KeyNotFoundException("Teacher not found");

            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> TeacherExistsAsync(int id)
        {
            return await _context.Teachers.AnyAsync(e => e.TeacherID == id);
        }

        public async Task<object> GetTeacherDetailsAsync(int teacherId)
        {
            // Lấy danh sách tên lớp Teacher đang dạy
            var classNames = await _context.Classes
                .Where(c => c.TeacherID == teacherId)
                .Select(c => c.ClassName) // Thay đổi thành thuộc tính tương ứng
                .ToListAsync();

            // Lấy danh sách tên học sinh được dạy bởi Teacher
            var studentNames = await _context.Students
                .Where(s => s.ClassID != 0 && _context.Classes.Any(c => c.ClassID == s.ClassID && c.TeacherID == teacherId))
                .Select(s => s.Name)
                .ToListAsync();

            if (!classNames.Any() && !studentNames.Any())
            {
                return null;
            }

            return new
            {
                Classes = classNames,
                Students = studentNames
            };
        }


    }
}
