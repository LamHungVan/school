using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Week8_BT1_API_CURD.Models;

namespace Week8_BT1_API_CURD.Services
{
    public class ClassService
    {
        private readonly ApplicationDbContext _context;

        public ClassService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Class>> GetClassesAsync()
        {
            return await _context.Classes.ToListAsync();
        }

        public async Task<Class> GetClassByIdAsync(int id)
        {
            return await _context.Classes.FindAsync(id);
        }

        public async Task<Class> AddClassAsync(Class classObj)
        {
            // Ensure ClassID is not set by the client; let the database auto-generate it.
            classObj.ClassID = 0;

            _context.Classes.Add(classObj);
            await _context.SaveChangesAsync();
            return classObj;
        }

        public async Task UpdateClassAsync(Class classObj)
        {
            _context.Entry(classObj).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ClassExistsAsync(classObj.ClassID))
                {
                    throw new ArgumentException("Class not found.");
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task DeleteClassAsync(int id)
        {
            // Tìm lớp học
            var classObj = await _context.Classes.FindAsync(id);
            if (classObj == null)
            {
                throw new ArgumentException("Class not found.");
            }

            // Tìm và xóa tất cả học sinh liên quan đến lớp
            var students = _context.Students.Where(s => s.ClassID == id);
            _context.Students.RemoveRange(students);

            // Tìm và xóa tất cả giáo viên liên quan đến lớp
            var teachers = _context.Teachers.Where(t => t.TeacherID == classObj.TeacherID);
            _context.Teachers.RemoveRange(teachers);

            // Xóa lớp học
            _context.Classes.Remove(classObj);

            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();
        }


        private async Task<bool> ClassExistsAsync(int id)
        {
            return await _context.Classes.AnyAsync(e => e.ClassID == id);
        }
    }
}
