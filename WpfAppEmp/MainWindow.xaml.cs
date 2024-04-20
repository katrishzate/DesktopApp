using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfAppEmp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadGrid();
        }

        SqlConnection con = new SqlConnection(@"Data Source=localhost\SQLEXPRESS;Initial Catalog=db_employee;Integrated Security=True");

        public void clearData()
        {
            emp_name.Clear();
            emp_age.Clear();
            emp_salary.Clear();
            join_date.Clear();
            phone.Clear();
            id_search.Clear();
        }
        internal void LoadGrid()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Employee", con);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();
            datagrid.ItemsSource = dt.DefaultView;
        }
        public void ButtonCreate_Click(object sender, RoutedEventArgs e) 
        { 
            try
            {
                    SqlCommand cmd = new SqlCommand("INSERT INTO Employee VALUES(@Name, @Age, @Salary, @JoinDate, @Contact)", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Name", emp_name.Text);
                    cmd.Parameters.AddWithValue("@Age", emp_age.Text);
                    cmd.Parameters.AddWithValue("@Salary", emp_salary.Text);
                    cmd.Parameters.AddWithValue("@JoinDate", join_date.Text);
                    cmd.Parameters.AddWithValue("@Contact", phone.Text);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    LoadGrid();
                    MessageBoxWrapper.Show("Register Completed", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                    clearData();
            }
            catch(SqlException ex)
            {
                MessageBoxWrapper.Show(ex.Message);
            }
        }
        public void ButtonRead_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(id_search.Text, out int employeeId))
            {
                MessageBox.Show("Please enter a valid ID.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string query = "SELECT * FROM Employee WHERE ID = @ID";
            using (SqlConnection connection = new SqlConnection(@"Data Source=localhost\SQLEXPRESS;Initial Catalog=db_employee;Integrated Security=True"))
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@ID", employeeId);

                try
                {
                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();

                        emp_name.Text = reader["emp_name"].ToString();
                        emp_age.Text = reader["emp_age"].ToString();
                        emp_salary.Text = reader["emp_salary"].ToString();
                        join_date.Text = reader["join_date"].ToString();
                        phone.Text = reader["phone"].ToString();
                    }
                    else
                    {
                        MessageBox.Show("No record found with the given ID.", "Read Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("An error occurred while reading the record: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        public void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = (@"Data Source=localhost\SQLEXPRESS;Initial Catalog=db_employee;Integrated Security=True");
            string updateQuery = "UPDATE Employee SET emp_name = @Name, emp_age = @Age, emp_salary = @Salary, join_date = @JoinDate, phone = @Contact WHERE ID = @ID";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(updateQuery, con);
                cmd.Parameters.AddWithValue("@Name", emp_name.Text);
                cmd.Parameters.AddWithValue("@Age", emp_age.Text);
                cmd.Parameters.AddWithValue("@Salary", emp_salary.Text);
                cmd.Parameters.AddWithValue("@JoinDate", join_date.Text);
                cmd.Parameters.AddWithValue("@Contact", phone.Text);
                cmd.Parameters.AddWithValue("@ID", id_search.Text);

                try
                {
                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Record Updated", "Updated", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("No record found with the given ID.", "Update Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("An error occurred while updating the record: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            clearData();
            LoadGrid();
        }
        public void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM Employee WHERE ID = " + id_search.Text + " ", con);
            try 
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Record Deleted", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                con.Close();
                clearData();
                LoadGrid();

            }
            catch(SqlException ex)
            {
                MessageBox.Show("Not Deleted" + ex.Message);
            }
            finally 
            { 
                con.Close(); 
            }    
        }
    }
}