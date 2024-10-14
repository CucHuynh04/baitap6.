using dal.Model;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace bus
{
    public class StudentService
    {
        private readonly StudentModel _context;

        public StudentService()
        {
            _context = new StudentModel(); // Khởi tạo context
        }

        // Lấy danh sách sinh viên
        public List<Student> GetAll()
        {
            return _context.Student
                        .Include(s => s.Faculty) // Nhúng thông tin khoa
                        .Include(s => s.Major)   // Nhúng thông tin chuyên ngành
                        .ToList();
        }

        // Thêm sinh viên
        public void Add(Student student)
        {
            _context.Student.Add(student); // Thêm sinh viên vào DbSet
            _context.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
        }

        // Cập nhật thông tin sinh viên
        public void Update(Student student)
        {
            var existingStudent = _context.Student.Find(student.StudentID);
            if (existingStudent != null)
            {
                // Cập nhật các thuộc tính của sinh viên
                existingStudent.FullName = student.FullName;
                existingStudent.FacultyID = student.FacultyID;
                existingStudent.AverageScore = student.AverageScore;
                existingStudent.MajorID = student.MajorID;

                _context.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
            }
        }

        // Xóa sinh viên theo ID
        public void Delete(int studentId)
        {
            var studentToDelete = _context.Student.Find(studentId);
            if (studentToDelete != null)
            {
                _context.Student.Remove(studentToDelete); // Xóa sinh viên
                _context.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
            }
        }
    }

    public class MajorService
    {
        private readonly StudentModel _context;

        public MajorService()
        {
            _context = new StudentModel();
        }

        public List<Major> GetAll()
        {
            return _context.Major.ToList();
        }
    }

    public class FacultyService
    {
        private readonly StudentModel _context;

        public FacultyService()
        {
            _context = new StudentModel();
        }

        public List<Faculty> GetAll()
        {
            return _context.Faculty.ToList();
        }
    }
}
