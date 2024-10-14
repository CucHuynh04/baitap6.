using bus;
using dal.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace gui
{
    public partial class Form1 : Form
    {
        private readonly StudentService studentService;
        private readonly MajorService majorService;
        private readonly FacultyService facultyService;

        public Form1()
        {
            InitializeComponent();
            studentService = new StudentService();
            majorService = new MajorService();
            facultyService = new FacultyService();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadStudents();
            LoadFacultyList();
        }

        private void LoadStudents()
        {
            try
            {
                var students = studentService.GetAll();
                dgvStudent.DataSource = students;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách sinh viên: " + ex.Message);
            }
        }

        private void LoadFacultyList()
        {
            try
            {
                var faculties = facultyService.GetAll();
                cmbKhoa.DataSource = faculties;
                cmbKhoa.DisplayMember = "FacultyName";
                cmbKhoa.ValueMember = "FacultyID";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách khoa: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Chức năng xóa sinh viên
            if (dgvStudent.SelectedRows.Count > 0)
            {
                var selectedRow = dgvStudent.SelectedRows[0];
                int studentId = (int)selectedRow.Cells[0].Value;

                var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn xóa sinh viên này không?",
                                                     "Xác nhận xóa",
                                                     MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    studentService.Delete(studentId);
                    MessageBox.Show("Xóa sinh viên thành công.");
                    LoadStudents();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sinh viên để xóa.");
            }
        }

        private void btnAvatar_Click(object sender, EventArgs e)
        {
            // Logic chọn và hiển thị ảnh đại diện
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "C:\\";
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                openFileDialog.Title = "Chọn ảnh đại diện";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    picAvatar.Image = Image.FromFile(openFileDialog.FileName);
                }
            }
        }

        private void btnSua(object sender, EventArgs e)
        {
            if (dgvStudent.SelectedRows.Count > 0)
            {
                var selectedRow = dgvStudent.SelectedRows[0];
                int studentId = (int)selectedRow.Cells[0].Value;

                // Kiểm tra dữ liệu đầu vào
                if (!string.IsNullOrWhiteSpace(txtHoTen.Text) &&
                    !string.IsNullOrWhiteSpace(txtDTB.Text))
                {
                    // Chức năng sửa
                    var student = new Student
                    {
                        StudentID = studentId,
                        FullName = txtHoTen.Text,
                        FacultyID = (int)cmbKhoa.SelectedValue,
                        AverageScore = double.TryParse(txtDTB.Text, out double score) ? score : 0,
                        MajorID = checkBox1.Checked ? null : (int?)cmbKhoa.SelectedValue
                    };

                    try
                    {
                        studentService.Update(student);
                        MessageBox.Show("Sửa sinh viên thành công.");

                        // Tải lại danh sách sinh viên
                        LoadStudents();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi sửa sinh viên: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng điền đầy đủ thông tin để sửa sinh viên.");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sinh viên để sửa.");
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
          
                // Kiểm tra xem người dùng đã nhập đầy đủ thông tin chưa
                if (string.IsNullOrWhiteSpace(txtMSSV.Text) ||
                    string.IsNullOrWhiteSpace(txtHoTen.Text) ||
                    string.IsNullOrWhiteSpace(txtDTB.Text))
                {
                    MessageBox.Show("Vui lòng điền đầy đủ thông tin sinh viên.");
                    return;
                }

                // Tạo đối tượng sinh viên mới
                var student = new Student
                {
                    StudentID = int.TryParse(txtMSSV.Text, out int studentId) ? studentId : 0, // Gán giá trị MSSV từ txtMSSV
                    FullName = txtHoTen.Text,
                    FacultyID = (int)cmbKhoa.SelectedValue,
                    AverageScore = double.TryParse(txtDTB.Text, out double score) ? score : 0,
                    MajorID = checkBox1.Checked ? null : (int?)cmbKhoa.SelectedValue
                };

                try
                {
                    // Gọi phương thức Add từ StudentService
                    studentService.Add(student);
                    MessageBox.Show("Thêm sinh viên thành công.");

                    // Tải lại danh sách sinh viên
                    LoadStudents();

                    // Xóa dữ liệu trong các trường nhập
                    txtMSSV.Clear();
                    txtHoTen.Clear();
                    txtDTB.Clear();
                    cmbKhoa.SelectedIndex = -1; // Reset ComboBox
                    checkBox1.Checked = false; // Reset Checkbox
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi thêm sinh viên: " + ex.Message);
                }
            }

        }
    }


