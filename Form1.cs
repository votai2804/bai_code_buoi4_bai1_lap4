using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BAI1_LAP4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                StudentContextDB contextDB = new StudentContextDB();
                List<FACULTY> dsfa = contextDB.FACULTY.ToList();
                List<STUDENT> dsst = contextDB.STUDENT.ToList();
                fillfa(dsfa);
                bindst(dsst);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }


            
        }
        private void fillfa(List<FACULTY> dsfa)
        {
            this.cbkhoa.DataSource = dsfa;
            this.cbkhoa.DisplayMember = "FacultyName";
            this.cbkhoa.ValueMember = "FacultyID";
        }
        private void bindst(List<STUDENT> dsst)
        {
            dataGridView1.Rows.Clear();
            foreach (var ds in dsst)
            {
                int index = dataGridView1.Rows.Add();
                dataGridView1.Rows[index].Cells[0].Value = ds.STUDENTID;
                dataGridView1.Rows[index].Cells[1].Value = ds.FULLNAME;
                dataGridView1.Rows[index].Cells[2].Value = ds.FACULTY.FACULTYNAME;
                dataGridView1.Rows[index].Cells[3].Value = ds.AVERAGESCORE;
            }
        }

        private void btnxoa_Click(object sender, EventArgs e)
        {
            try
            {
                StudentContextDB contextDB = new StudentContextDB();
                string studentID = txtmssv.Text;
                STUDENT studentToDelete = contextDB.STUDENT.SingleOrDefault(s => s.STUDENTID == studentID);

                if (studentToDelete != null)
                {
                    contextDB.STUDENT.Remove(studentToDelete);
                    contextDB.SaveChanges();
                    MessageBox.Show("Xóa sinh viên thành công!");

                    // Cập nhật lại danh sách sinh viên
                    bindst(contextDB.STUDENT.ToList());
                }
                else
                {
                    MessageBox.Show("Không tìm thấy sinh viên để xóa.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnsua_Click(object sender, EventArgs e)
        {
            try
            {
                StudentContextDB contextDB = new StudentContextDB();
                string studentID = txtmssv.Text;
                STUDENT studentToUpdate = contextDB.STUDENT.SingleOrDefault(s => s.STUDENTID == studentID);

                if (studentToUpdate != null)
                {
                    studentToUpdate.FULLNAME = txtten.Text;
                    studentToUpdate.AVERAGESCORE = Convert.ToDouble(txtdtb.Text);
                    studentToUpdate.FACULTYID = (string)cbkhoa.SelectedValue;

                    contextDB.STUDENT.AddOrUpdate(studentToUpdate);
                    contextDB.SaveChanges();
                    MessageBox.Show("Cập nhật sinh viên thành công!");

                    // Cập nhật lại danh sách sinh viên
                    bindst(contextDB.STUDENT.ToList());
                }
                else
                {
                    MessageBox.Show("Không tìm thấy sinh viên để cập nhật.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnthem_Click(object sender, EventArgs e)
        {
            try
            {
                StudentContextDB contextDB = new StudentContextDB();
                STUDENT newStudent = new STUDENT
                {
                    STUDENTID = txtmssv.Text,
                    FULLNAME = txtten.Text,
                    AVERAGESCORE = Convert.ToDouble(txtdtb.Text),
                    FACULTYID = (string)cbkhoa.SelectedValue
                };

                contextDB.STUDENT.Add(newStudent);
                contextDB.SaveChanges();
                MessageBox.Show("Thêm sinh viên thành công!");

                // Cập nhật lại danh sách sinh viên
                bindst(contextDB.STUDENT.ToList());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnthoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                txtmssv.Text = row.Cells[0].Value.ToString();
                txtten.Text = row.Cells[1].Value.ToString();
                cbkhoa.Text = row.Cells[2].Value.ToString();
                txtdtb.Text = row.Cells[3].Value.ToString();
            }
        }
    }
}
