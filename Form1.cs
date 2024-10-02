using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EntitySchool
{
    public partial class Form1 : Form
    {
        private Model1 context;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            context = new Model1();
            List<Student> students = context.Students.ToList();
            List<Faculty> faculties = context.Faculties.ToList();
            cmbKhoa.DataSource = faculties;
            cmbKhoa.DisplayMember = "FacultyName";
            cmbKhoa.ValueMember = "FacultyID";
            dataGridView1.Rows.Clear();
            foreach (var student in students)
            {
                int index = dataGridView1.Rows.Add();
                dataGridView1.Rows[index].Cells["colMa"].Value = student.StudentID;
                dataGridView1.Rows[index].Cells["colName"].Value = student.FullName;
                dataGridView1.Rows[index].Cells["colKhoa"].Value = student.Faculty.FacultyName;
                dataGridView1.Rows[index].Cells["colDiem"].Value = student.AverageScore;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            try
            {
                int studentID = int.Parse(txtMa.Text);
                string fullName = txtName.Text;
                int facultyID = (int)cmbKhoa.SelectedValue;
                decimal averageScore = decimal.Parse(txtDiem.Text);
                var existingStudent = context.Students.FirstOrDefault(student => student.StudentID == studentID);
                if (existingStudent != null)
                {
                    MessageBox.Show("Mã sinh viên đã tồn tại. Vui lòng nhập mã khác.");
                    return;
                }
                Student s = new Student()
                {
                    StudentID = studentID,
                    FullName = fullName,
                    FacultyID = facultyID,
                    AverageScore = averageScore,
                };
                context.Students.Add(s);
                context.SaveChanges();
                LoadDataGridView();
                MessageBox.Show("Thêm sinh viên thành công!");
                txtMa.Clear();
                txtName.Clear();
                txtDiem.Clear();
                cmbKhoa.SelectedIndex = 0;
            }
            catch (FormatException)
            {
                MessageBox.Show("Vui lòng nhập đúng định dạng cho Mã, Điểm.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm sinh viên: " + ex.Message);
            }

        }

        private void LoadDataGridView()
        {
            List<Student> students = context.Students.ToList();
            dataGridView1.Rows.Clear();
            foreach (var student in students)
            {
                int index = dataGridView1.Rows.Add();
                dataGridView1.Rows[index].Cells["colMa"].Value = student.StudentID;
                dataGridView1.Rows[index].Cells["colName"].Value = student.FullName;
                dataGridView1.Rows[index].Cells["colKhoa"].Value = student.Faculty.FacultyName;
                dataGridView1.Rows[index].Cells["colDiem"].Value = student.AverageScore;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0) 
            {
                int studentID = (int)dataGridView1.SelectedRows[0].Cells["colMa"].Value; 
                Student studentToDelete = context.Students.FirstOrDefault(s => s.StudentID == studentID);

                if (studentToDelete != null)
                {
                    context.Students.Remove(studentToDelete);
                    context.SaveChanges();
                    LoadDataGridView();

                    MessageBox.Show("Xóa sinh viên thành công!");
                }
                else
                {
                    MessageBox.Show("Sinh viên không tồn tại!");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sinh viên để xóa.");
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                try
                {
                    int studentID = (int)dataGridView1.SelectedRows[0].Cells["colMa"].Value;
                    Student studentEdit = context.Students.FirstOrDefault(s => s.StudentID == studentID);

                    if (studentEdit != null)
                    {
                        int newStudentID = int.Parse(txtMa.Text);
                        if (newStudentID != studentEdit.StudentID)
                        {
                            var existingStudent = context.Students.FirstOrDefault(s => s.StudentID == newStudentID);
                            if (existingStudent != null)
                            {
                                MessageBox.Show("Mã sinh viên mới đã tồn tại. Vui lòng nhập mã khác.");
                                return;
                            }
                            studentEdit.StudentID = newStudentID;
                        }
                        studentEdit.FullName = txtName.Text;
                        studentEdit.FacultyID = (int)cmbKhoa.SelectedValue;
                        studentEdit.AverageScore = decimal.Parse(txtDiem.Text);
                        context.SaveChanges();
                        LoadDataGridView();

                        MessageBox.Show("Cập nhật sinh viên thành công!");
                        txtMa.Clear();
                        txtName.Clear();
                        txtDiem.Clear();
                        cmbKhoa.SelectedIndex = 0;
                    }
                    else
                    {
                        MessageBox.Show("Sinh viên không tồn tại!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật sinh viên: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sinh viên để chỉnh sửa.");
            }
        }
    }
}
