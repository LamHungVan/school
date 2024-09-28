using Microsoft.EntityFrameworkCore;
using Week8_BT1_API_CURD.Models;

namespace Week8_BT1_API_CURD.Services
{
    public class StudentService
    {
        private readonly ApplicationDbContext _context;

        public StudentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Student>> GetStudentsAsync()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<double?> GetStudentGPAAsync(Guid studentUUID)
        {
            return await _context.Students
                .Where(s => s.StudentUUID == studentUUID)
                .Select(s => s.GPA)
                .FirstOrDefaultAsync();
        }


        public async Task<IEnumerable<Student>> GetTopGPAStudentsAsync()
        {
            var maxGPA = await _context.Students.MaxAsync(s => s.GPA);
            return await _context.Students
                .Where(s => s.GPA == maxGPA)
                .ToListAsync();
        }

        public async Task<Student> GetStudentByIdAsync(Guid studentUUID)
        {
            return await _context.Students.FindAsync(studentUUID);
        }

        public async Task<Student> AddStudentAsync(Student student)
        {
            if (student.StudentUUID == Guid.Empty)
            {
                student.StudentUUID = Guid.NewGuid();
            }

            var classExists = await _context.Classes.AnyAsync(c => c.ClassID == student.ClassID);
            if (!classExists)
            {
                throw new ArgumentException("ClassID does not exist.");
            }

            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task UpdateStudentAsync(Student student)
        {
            _context.Entry(student).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await StudentExistsAsync(student.StudentUUID))
                {
                    throw new ArgumentException("Student not found.");
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task DeleteStudentAsync(Guid studentUUID)
        {
            var student = await _context.Students.FindAsync(studentUUID);
            if (student == null)
            {
                throw new ArgumentException("Student not found.");
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
        }

        private async Task<bool> StudentExistsAsync(Guid studentUUID)
        {
            return await _context.Students.AnyAsync(e => e.StudentUUID == studentUUID);
        }
    }
}
