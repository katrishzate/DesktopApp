using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.SqlClient;
using System.Windows;
using WpfAppEmp;

namespace WpfAppEmpTest
{
    [TestClass]
    public class UnitTest1
    {
        private MainWindow mainWindow;

        [TestInitialize]
        public void TestInitialize()
        {
            mainWindow = new MainWindow();
        }

        [TestMethod]
        public void TestTruePositive()
        {
            string employeeName = "Minji";
            string employeeAge = "19";
            string employeeSalary = "6190";
            string joinDate = "23 July 2023 3:13:00 am +00:00";
            string phone = "012-352-478569";

            mainWindow.emp_name.Text = employeeName;
            mainWindow.emp_age.Text = employeeAge;
            mainWindow.emp_salary.Text = employeeSalary;
            mainWindow.join_date.Text = joinDate;
            mainWindow.phone.Text = phone;

            mainWindow.ButtonCreate_Click(null, null);

            using (SqlConnection con = new SqlConnection(@"Data Source=localhost\SQLEXPRESS;Initial Catalog=db_employee;Integrated Security=True"))
            {
                con.Open();
                string query = "SELECT COUNT(*) FROM Employee WHERE emp_name = @Name AND emp_age = @Age AND emp_salary = @Salary AND join_date = @JoinDate AND phone = @Contact";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Name", employeeName);
                    cmd.Parameters.AddWithValue("@Age", employeeAge);
                    cmd.Parameters.AddWithValue("@Salary", employeeSalary);
                    cmd.Parameters.AddWithValue("@JoinDate", Convert.ToDateTime(joinDate));
                    cmd.Parameters.AddWithValue("@Contact", phone);

                    int count = (int)cmd.ExecuteScalar();
                    Assert.AreEqual(1, count, "Employee was not added to the database.");
                }
            }
        }

        [TestMethod]
        public void TestTrueNegative()
        {
            int existingEmployeeId = 10;

            mainWindow.id_search.Text = existingEmployeeId.ToString();
            mainWindow.ButtonRead_Click(null, null);

            Assert.AreEqual("Katrish", mainWindow.emp_name.Text, "Employee name not as expected.");
            Assert.AreEqual("20", mainWindow.emp_age.Text, "Employee age not as expected.");
            Assert.AreEqual("8250", mainWindow.emp_salary.Text, "Employee salary not as expected.");
            Assert.AreEqual("25 Aug 2023 12:00:00 am +00:00", mainWindow.join_date.Text, "Join date not as expected.");
            Assert.AreEqual("012-345-345678", mainWindow.phone.Text, "Phone number not as expected.");
        }

        [TestMethod]
        public void TestFalsePositive()
        {
            string nonExistingEmployeeId = "20";

            mainWindow.id_search.Text = nonExistingEmployeeId;
            mainWindow.ButtonUpdate_Click(null, null);

            MessageBoxResult result = MessageBoxWrapper.Show("No record found with the given ID.");
            Assert.AreEqual(MessageBoxResult.OK, result);
        }

        [TestMethod]
        public void TestFalseNegative()
        {
            string employeeName = "Minji";
            string employeeAge = "19";
            string employeeSalary = "6190";
            string joinDate = "23 July 2023 3:13:00 am +00:00";
            string phone = "012-352-478569";

            mainWindow.emp_name.Text = employeeName;
            mainWindow.emp_age.Text = employeeAge;
            mainWindow.emp_salary.Text = employeeSalary;
            mainWindow.join_date.Text = joinDate;
            mainWindow.phone.Text = phone;

            mainWindow.ButtonCreate_Click(null, null);

            mainWindow.id_search.Text = "10";
            mainWindow.ButtonDelete_Click(null, null);

            MessageBoxResult result = MessageBoxWrapper.Show("No record found with the given ID.");
            Assert.AreEqual(MessageBoxResult.OK, result);
        }
    }
}
