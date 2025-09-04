using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Data;
using Microsoft.IdentityModel.Protocols;
using System.Configuration;

namespace Employee_Management_System_CRUD_Operation
{
    public class Employee
    {
        public int ID { get; set; }
        public string Emp_name { get; set; }
        public string Emp_email { get; set; }
        public int Emp_deptID { get; set; }
        public int Emp_salary { get; set; }
        public string Emp_phone { get; set; }

    }
    internal class Program
    {
        public static void RegisterEmployee(string Emp_name, string Emp_email, int Emp_DeptID, int Emp_Salary, string Emp_Phone, string DBLink)
        {
            string Emp_ID_Query = "select Top 1 ID from Employees order by ID desc;";
            string registerQuery = "insert into Employees (ID, Emp_Name, Emp_Email, DepartmentID, Emp_Salary, Emp_Phone) values (@Emp_ID, @Emp_name, @Emp_email, @Emp_DeptID, @Emp_Salary, @Emp_Phone)";
            string selectQuery = "select * from Employees;";
            List<Employee> empList = new List<Employee>();    

            
            using (SqlConnection con = new SqlConnection(DBLink))
            {
                con.Open();
                int Emp_ID = 0;
                using (SqlCommand IDcmd = new SqlCommand(Emp_ID_Query, con))
                {
                    Emp_ID = (int)IDcmd.ExecuteScalar()+1;
                }

                using (SqlCommand selectCmd = new SqlCommand(selectQuery, con))
                {
                    SqlDataReader reader = selectCmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Employee e = new Employee()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            Emp_name = reader["Emp_Name"].ToString(),
                            Emp_email = reader["Emp_Email"].ToString(),
                            Emp_deptID = Convert.ToInt32(reader["DepartmentID"]),
                            Emp_salary = Convert.ToInt32(reader["Emp_Salary"]),
                            Emp_phone = reader["Emp_Phone"].ToString()
                        };
                        empList.Add(e);
                    }
                    reader.Close();
                }

                if(empList.Any(e=> e.Emp_email == Emp_email))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Employee Already Exist!!!");
                    Console.ResetColor();
                    return;
                }

                using (SqlCommand registerCmd = new SqlCommand(registerQuery, con))
                {

                    registerCmd.Parameters.AddWithValue("@Emp_ID", Emp_ID);
                    registerCmd.Parameters.AddWithValue("@Emp_name", Emp_name);
                    registerCmd.Parameters.AddWithValue("@Emp_email", Emp_email);
                    registerCmd.Parameters.AddWithValue("@Emp_DeptID", Emp_DeptID);
                    registerCmd.Parameters.AddWithValue("@Emp_Salary", Emp_Salary);
                    registerCmd.Parameters.AddWithValue("@Emp_Phone", Emp_Phone);

                    int rowAffected = registerCmd.ExecuteNonQuery();    

                    if(rowAffected == 1)
                    {
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Successfully Registered!!");
                        Console.ResetColor();
                        Console.WriteLine();
                        Console.WriteLine();
                    }
                }
                Console.WriteLine() ;
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(new string('-', 105));
                Console.WriteLine("|  ID  |   Employee Name   |        Employee Email        | DeptID | Employee Salary |  Employee Phone  |");
                Console.WriteLine(new string('-', 105));
                Console.ResetColor();

                using (SqlCommand selectCmd = new SqlCommand(selectQuery, con))
                {
                    SqlDataReader reader = selectCmd.ExecuteReader();
                    while (reader.Read())
                    {

                        if ((int)reader["ID"] == Emp_ID)
                        {
                            Console.ForegroundColor= ConsoleColor.Blue;
                            Console.WriteLine($"| {reader["ID"],-4} | {reader["Emp_Name"],-17} | {reader["Emp_Email"],-28} | {reader["DepartmentID"],-6} | {reader["Emp_Salary"],-15} | {reader["Emp_Phone"],-16} |");
                            Console.WriteLine();
                            Console.ResetColor() ;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"| {reader["ID"],-4} | {reader["Emp_Name"],-17} | {reader["Emp_Email"],-28} | {reader["DepartmentID"],-6} | {reader["Emp_Salary"],-15} | {reader["Emp_Phone"],-16} |");
                            Console.WriteLine();
                            Console.ResetColor();
                        }

                    }
                }

                Console.WriteLine();
                Console.WriteLine();

            }
        }

        public static void DisplayAllEmployee(String DBLink)
        {
            string selectQuery = "select * from Employees;";
            using (SqlConnection con = new SqlConnection(DBLink))
            {
                con.Open();
                using (SqlCommand selectCmd = new SqlCommand(selectQuery, con))
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("                                          ~** Employee Data **~                                                  ");
                    Console.ResetColor();
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine(new string('-', 105));
                    Console.WriteLine("|  ID  |   Employee Name   |        Employee Email        | DeptID | Employee Salary |  Employee Phone  |");
                    Console.WriteLine(new string('-', 105));
                    Console.ResetColor();


                    SqlDataReader reader = selectCmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"| {reader["ID"],-4} | {reader["Emp_Name"],-17} | {reader["Emp_Email"],-28} | {reader["DepartmentID"],-6} | {reader["Emp_Salary"],-15} | {reader["Emp_Phone"],-16} |");
                        Console.WriteLine();
                        Console.ResetColor();
                    }
                }
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(new string('-', 105));
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine();
                Console.ReadKey();
            }

        }
        static void Main(string[] args)
        {
            try
            {
                while (true)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("------------------------------------------------------------------------------------");
                    Console.ResetColor();
                    Console.WriteLine("                          ~** Employee Management System **~                        ");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("------------------------------------------------------------------------------------");
                    Console.ResetColor();

                    Console.WriteLine();
                    Console.WriteLine("                                   Choose the Option                                 ");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("                                 1. Regsiter New Employee                            ");
                    Console.WriteLine("                                 2. View All Employees                               ");
                    Console.WriteLine("                                 3. Update Employee Details                          ");
                    Console.WriteLine("                                 4. Delete an Employee                               ");
                    Console.WriteLine("                                 5. Filter an Employee (Stored Procedure)            ");
                    Console.WriteLine("                                 6. View Employees in Department View                ");
                    Console.ResetColor();
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("------------------------------------------------------------------------------------");
                    Console.ResetColor();

                    int Choice = 0;
                choice:
                    try
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("Enter the Choice: ");
                        int choice = int.Parse(Console.ReadLine());
                        Console.ResetColor();

                        if (choice >= 1 && choice <= 6)
                        {
                            Choice = choice;
                            Console.WriteLine();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Enter the Choice Within the given Range (1 to 6) :(");
                            goto choice;
                        }
                    }
                    catch (FormatException)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid Choice! Please Enter only the Digit from Range (1 - 6)! ");
                        Console.ResetColor();
                        goto choice;
                    }

                    //string DBLink = "Data Source=BSD-SABARIP01\\SQLEXPRESS;Initial Catalog=Employee_Management_System;Integrated Security=SSPI;TrustServerCertificate=True;";
                    string DBLink = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;



                    List<int> IDs = new List<int>();

                    //using (SqlConnection conn = new SqlConnection(DBLink))
                    //{
                    //    conn.Open();
                    //    Console.WriteLine(conn.State);
                    //    conn.Close();   
                    //}
                    switch (Choice)
                    {
                        case 1:
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.WriteLine("                  You Choose to Register New Employee to the DB                     ");
                            Console.ResetColor();
                            Console.WriteLine();
                            Console.WriteLine();

                            //------------------------------
                            string Emp_name = "";
                        EmpName:

                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write("Enter the Name of Employee: ");
                            Console.ResetColor();
                            string EmpName = Console.ReadLine();  //Employee Name

                            if (Validation.nameValidation(EmpName))
                            {
                                Emp_name = EmpName;
                            }
                            else
                            {
                                goto EmpName;
                            }

                            //------------------------------

                            String Emp_email = "";
                        Emp_email:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write("Enter the Email of Employee: ");
                            Console.ResetColor();
                            string EmpEmail = Console.ReadLine();  //Employee Email

                            if (Validation.emailValidation(EmpEmail))
                            {
                                Emp_email = EmpEmail;
                            }
                            else
                            {
                                goto Emp_email;
                            }

                        //------------------------------

                        DeptChoice:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Choose the Department      : ");
                            Console.ResetColor();
                            int DeptCount = 0;
                            Hashtable DepartmentHash = new Hashtable();

                            int Emp_DeptID = 0; //Department ID 

                            Console.ForegroundColor = ConsoleColor.Blue;
                            string DepartQuery = "select distinct d.DeptName from Employees e inner join Departments d on e.DepartmentID = d.DeptID;";
                            using (SqlConnection conn = new SqlConnection(DBLink))
                            {
                                conn.Open();
                                using (SqlCommand DepartCMD = new SqlCommand(DepartQuery, conn))
                                {
                                    SqlDataReader reader = DepartCMD.ExecuteReader();
                                    int SNcount = 1;
                                    while (reader.Read())
                                    {
                                        Console.WriteLine($"             {SNcount}. {reader["DeptName"].ToString()}");
                                        DepartmentHash.Add(SNcount, reader["DeptName"].ToString());
                                        SNcount++;
                                    }
                                    DeptCount = SNcount;
                                    reader.Close();
                                }


                                int DepartmentID = 0;

                                int deptChoice = int.Parse(Console.ReadLine());
                                if (deptChoice == 0 || deptChoice > DeptCount)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine($"Enter the Department Choice Correctly within the Range (1 to {DeptCount}");
                                    Console.ResetColor();
                                    goto DeptChoice;

                                }
                                else
                                {
                                    string deptName = (String)DepartmentHash[deptChoice];
                                    String DepartmentIDQuery = "select distinct d.DeptID from Employees e inner join Departments d on e.DepartmentID = d.DeptID where d.DeptName = @deptName;";
                                    using (SqlCommand departNameCMD = new SqlCommand(DepartmentIDQuery, conn))
                                    {
                                        departNameCMD.Parameters.AddWithValue("@deptName", deptName);
                                        DepartmentID = (int)departNameCMD.ExecuteScalar();
                                    }
                                }
                                Emp_DeptID = DepartmentID;

                                //Console.WriteLine("Department ID - "+ DepartmentID);
                            }
                        //------------------------------
                        Salary:
                            int Emp_Salary = 0;
                            //Salary
                            try
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.Write("Enter the Salary of Employee: ");
                                Console.ResetColor();
                                int Salary = int.Parse(Console.ReadLine());
                                if (Salary == 0 && Salary <= 1000)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Enter the Correct Salary ! it not be less then 1000");
                                    Console.ResetColor();
                                    goto Salary;
                                }
                                Emp_Salary = Salary;
                            }
                            catch (FormatException)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Invalid Salary! Please Enter only the Digit! ");
                                Console.ResetColor();
                                goto Salary;
                            }
                            //------------------------------
                            String Emp_Phone = "";
                        Emp_phone:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write("Enter Employee Phone Number : ");
                            Console.ResetColor();
                            String EmpPhone = Console.ReadLine();   //Phone Number

                            if (Validation.phoneValidation(EmpPhone))
                            {
                                Emp_Phone = EmpPhone;
                            }
                            else
                            {
                                goto Emp_phone;
                            }

                            RegisterEmployee(Emp_name, Emp_email, Emp_DeptID, Emp_Salary, Emp_Phone, DBLink);

                            break;

                        case 2:
                            Console.WriteLine();

                            DisplayAllEmployee(DBLink);

                            break;

                        case 3:
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.WriteLine("                           You Choose to Update the Employee                             ");
                            Console.ResetColor();
                            Console.WriteLine();
                            Console.WriteLine();



                        IDs:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Enter the Employee ID for Update: ");
                            int Emp_id = int.Parse(Console.ReadLine());
                            using (SqlConnection conn = new SqlConnection(DBLink))
                            {
                                conn.Open();
                                using (SqlCommand cmd = new SqlCommand("GetAllIDs", conn))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    SqlDataReader reader = cmd.ExecuteReader();

                                    while (reader.Read())
                                    {
                                        IDs.Add((int)reader["ID"]);
                                    }
                                }
                            }

                            if (!IDs.Any(e => e == Emp_id))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("ID not Found! Re-Enter the ID");
                                Console.ResetColor();
                                goto IDs;
                            }
                            else
                            {
                                Console.ResetColor();
                                Console.WriteLine();

                                string SelectByIDQuery = "select * from Employees where ID = @Emp_id;";

                                Console.WriteLine();
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                Console.WriteLine($"                                          ~**ID: {Emp_id} 's Data **~                                                  ");
                                Console.ResetColor();
                                Console.WriteLine();
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                                Console.WriteLine(new string('-', 105));
                                Console.WriteLine("|  ID  |   Employee Name   |        Employee Email        | DeptID | Employee Salary |  Employee Phone  |");
                                Console.WriteLine(new string('-', 105));
                                Console.ResetColor();

                                using (SqlConnection con = new SqlConnection(DBLink))
                                {
                                    con.Open();
                                    using (SqlCommand cmd = new SqlCommand(SelectByIDQuery, con))
                                    {
                                        cmd.Parameters.AddWithValue("@Emp_id", Emp_id);
                                        SqlDataReader reader = cmd.ExecuteReader();

                                        while (reader.Read())
                                        {
                                            Console.ForegroundColor = ConsoleColor.Green;
                                            Console.WriteLine($"| {reader["ID"],-4} | {reader["Emp_Name"],-17} | {reader["Emp_Email"],-28} | {reader["DepartmentID"],-6} | {reader["Emp_Salary"],-15} | {reader["Emp_Phone"],-16} |");
                                            Console.ResetColor();
                                        }
                                    }
                                }
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                                Console.WriteLine(new string('-', 105));
                                Console.ResetColor();
                                Console.WriteLine();

                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine("------------------------------------------------------------------------------------");
                                Console.ResetColor();
                                Console.WriteLine("                             Choose the Update Option                               ");
                                Console.ForegroundColor = ConsoleColor.Blue;
                                Console.WriteLine("                             1. Update Employee's Name                                ");
                                Console.WriteLine("                             2. Update Employee's Email                               ");
                                Console.WriteLine("                             3. Update Employee's Department (Pending)                          ");
                                Console.WriteLine("                             4. Update Employee's Salary                              ");
                                Console.WriteLine("                             5. Update Employee's Phone Number                        ");
                                Console.WriteLine();
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine("------------------------------------------------------------------------------------");
                                Console.ResetColor();


                                int updateChoice = 0;
                            updateChoice:
                                try
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("Enter the Choice for Updating: ");
                                    Console.ResetColor();
                                    int UpdateChoice = int.Parse(Console.ReadLine());

                                    if (UpdateChoice == 0 || UpdateChoice > 5)
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("Invalid Choice! Please Enter the Choice Between 1 to 5!!");
                                        Console.ResetColor();
                                        goto updateChoice;
                                    }

                                    updateChoice = UpdateChoice;
                                }
                                catch (Exception ex)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Invalid Choice!! Please Enter the Choice in number only!");
                                    Console.ResetColor();
                                    goto updateChoice;
                                }


                                switch (updateChoice)
                                {
                                    case 1:
                                        string UpdateQueryName = $"update Employees set Emp_Name = @updateData where ID = @ID;";
                                        Console.ForegroundColor = ConsoleColor.Magenta;
                                        Console.WriteLine("                            Updating the Employee Name                              ");
                                        Console.ResetColor();
                                        Console.WriteLine();
                                        Console.ForegroundColor = ConsoleColor.Yellow;
                                        Console.Write("Enter the Updated Name : ");
                                        Console.ResetColor();
                                        string updateName = Console.ReadLine();
                                        using (SqlConnection con = new SqlConnection(DBLink))
                                        {
                                            con.Open();
                                            using (SqlCommand cmd = new SqlCommand(UpdateQueryName, con))
                                            {

                                                //cmd.Parameters.AddWithValue("@updateDetail", "Emp_Name");
                                                cmd.Parameters.AddWithValue("@updateData", updateName);
                                                cmd.Parameters.AddWithValue("@ID", Emp_id);

                                                int rowAffected = cmd.ExecuteNonQuery();
                                                if (rowAffected > 0)
                                                {
                                                    Console.ForegroundColor = ConsoleColor.Green;
                                                    Console.WriteLine("Successfully Updated!");
                                                    Console.ResetColor();
                                                }
                                            }
                                        }
                                        DisplayAllEmployee(DBLink);

                                        break;

                                    case 2:
                                        string UpdateQueryEmail = $"update Employees set Emp_Email = @updateData where ID = @ID;";
                                        Console.ForegroundColor = ConsoleColor.Magenta;
                                        Console.WriteLine("                            Updating the Employee Email                              ");
                                        Console.ResetColor();
                                        Console.WriteLine();
                                        Console.ForegroundColor = ConsoleColor.Yellow;
                                        Console.Write("Enter the Updated Email : ");
                                        Console.ResetColor();
                                        string updateEmail = Console.ReadLine();
                                        using (SqlConnection con = new SqlConnection(DBLink))
                                        {
                                            con.Open();
                                            using (SqlCommand cmd = new SqlCommand(UpdateQueryEmail, con))
                                            {

                                                //cmd.Parameters.AddWithValue("@updateDetail", "Emp_Name");
                                                cmd.Parameters.AddWithValue("@updateData", updateEmail);
                                                cmd.Parameters.AddWithValue("@ID", Emp_id);

                                                int rowAffected = cmd.ExecuteNonQuery();
                                                if (rowAffected > 0)
                                                {
                                                    Console.ForegroundColor = ConsoleColor.Green;
                                                    Console.WriteLine("Successfully Updated!");
                                                    Console.ResetColor();
                                                }
                                            }
                                        }
                                        DisplayAllEmployee(DBLink);
                                        break;

                                    case 3:
                                        string UpdateQueryDept = $"update Employees set DepartmentID = @updateData where ID = @ID;";
                                        Console.ForegroundColor = ConsoleColor.Magenta;
                                        Console.WriteLine("                            Updating the Employee Department                              ");
                                        Console.ResetColor();
                                        Console.WriteLine();
                                        Console.ForegroundColor = ConsoleColor.Yellow;
                                        Console.Write("Enter the Updated DepartmentID : ");
                                        Console.ResetColor();
                                        int updateDeptID = int.Parse(Console.ReadLine());
                                        using (SqlConnection con = new SqlConnection(DBLink))
                                        {
                                            con.Open();
                                            using (SqlCommand cmd = new SqlCommand(UpdateQueryDept, con))
                                            {

                                                //cmd.Parameters.AddWithValue("@updateDetail", "Emp_Name");
                                                cmd.Parameters.AddWithValue("@updateData", updateDeptID);
                                                cmd.Parameters.AddWithValue("@ID", Emp_id);

                                                int rowAffected = cmd.ExecuteNonQuery();
                                                if (rowAffected > 0)
                                                {
                                                    Console.ForegroundColor = ConsoleColor.Green;
                                                    Console.WriteLine("Successfully Updated!");
                                                    Console.ResetColor();
                                                }
                                            }
                                        }
                                        DisplayAllEmployee(DBLink);

                                        break;

                                    case 4:
                                        string UpdateQuerySalary = $"update Employees set Emp_Salary = @updateData where ID = @ID;";
                                        Console.ForegroundColor = ConsoleColor.Magenta;
                                        Console.WriteLine("                            Updating the Employee Salary                              ");
                                        Console.ResetColor();
                                        Console.WriteLine();
                                        Console.ForegroundColor = ConsoleColor.Yellow;
                                        Console.Write("Enter the Updated Salary : ");
                                        Console.ResetColor();
                                        int updateSalary = int.Parse(Console.ReadLine());
                                        using (SqlConnection con = new SqlConnection(DBLink))
                                        {
                                            con.Open();
                                            using (SqlCommand cmd = new SqlCommand(UpdateQuerySalary, con))
                                            {

                                                //cmd.Parameters.AddWithValue("@updateDetail", "Emp_Name");
                                                cmd.Parameters.AddWithValue("@updateData", updateSalary);
                                                cmd.Parameters.AddWithValue("@ID", Emp_id);

                                                int rowAffected = cmd.ExecuteNonQuery();
                                                if (rowAffected > 0)
                                                {
                                                    Console.ForegroundColor = ConsoleColor.Green;
                                                    Console.WriteLine("Successfully Updated!");
                                                    Console.ResetColor();
                                                }
                                            }
                                        }
                                        DisplayAllEmployee(DBLink);

                                        break;

                                    case 5:
                                        string UpdateQuery = $"update Employees set Emp_Phone = @updateData where ID = @ID;";
                                        Console.ForegroundColor = ConsoleColor.Magenta;
                                        Console.WriteLine("                            Updating the Employee Phone Number                              ");
                                        Console.ResetColor();
                                        Console.WriteLine();
                                        Console.ForegroundColor = ConsoleColor.Yellow;
                                        Console.Write("Enter the Updated Phone Number : ");
                                        Console.ResetColor();
                                        string updatePhone = Console.ReadLine();
                                        using (SqlConnection con = new SqlConnection(DBLink))
                                        {
                                            con.Open();
                                            using (SqlCommand cmd = new SqlCommand(UpdateQuery, con))
                                            {

                                                //cmd.Parameters.AddWithValue("@updateDetail", "Emp_Name");
                                                cmd.Parameters.AddWithValue("@updateData", updatePhone);
                                                cmd.Parameters.AddWithValue("@ID", Emp_id);

                                                int rowAffected = cmd.ExecuteNonQuery();
                                                if (rowAffected > 0)
                                                {
                                                    Console.ForegroundColor = ConsoleColor.Green;
                                                    Console.WriteLine("Successfully Updated!");
                                                    Console.ResetColor();
                                                }
                                            }
                                        }
                                        DisplayAllEmployee(DBLink);

                                        break;
                                }
                            }


                            break;

                        case 4:
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.WriteLine("                  You Choose to Delete the Employee to the DB                     ");
                            Console.ResetColor();
                            Console.WriteLine();
                            Console.WriteLine();

                            List<int> IDS = new List<int>();

                        IDS:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Enter the Employee ID for Delete: ");
                            int EmpId = int.Parse(Console.ReadLine());
                            using (SqlConnection conn = new SqlConnection(DBLink))
                            {
                                conn.Open();
                                using (SqlCommand cmd = new SqlCommand("GetAllIDs", conn))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    SqlDataReader reader = cmd.ExecuteReader();

                                    while (reader.Read())
                                    {
                                        IDs.Add((int)reader["ID"]);
                                    }
                                }
                            }

                            if (!IDs.Any(e => e == EmpId))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("ID not Found! Re-Enter the ID");
                                Console.ResetColor();
                                goto IDs;
                            }
                            else
                            {
                                String deleteQuery = "delete from Employees where id = @deletID;";
                                using (SqlConnection conn = new SqlConnection(DBLink))
                                {
                                    conn.Open();
                                    using (SqlCommand cmd = new SqlCommand(deleteQuery, conn))
                                    {
                                        cmd.Parameters.AddWithValue("@deletID", EmpId);

                                        int rowAffected = cmd.ExecuteNonQuery();

                                        if (rowAffected > 0)
                                        {
                                            Console.ForegroundColor = ConsoleColor.Green;
                                            Console.WriteLine("Successfully Deleted!!");
                                            Console.ResetColor();
                                            DisplayAllEmployee(DBLink);
                                        }
                                        else
                                        {
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.WriteLine("Something Went Wrong!! Try Agian!");
                                            Console.ResetColor();
                                            goto IDS;
                                        }
                                    }
                                }
                            }
                            break;

                        case 5:
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.WriteLine("                  You Choose to Search the Employee by ID                    ");
                            Console.ResetColor();
                            Console.WriteLine();
                            Console.WriteLine();

                            List<int> ID_Search = new List<int>();
                        ID_Search:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Enter the Employee ID for Delete: ");
                            int EmpID_Search = int.Parse(Console.ReadLine());
                            using (SqlConnection conn = new SqlConnection(DBLink))
                            {
                                conn.Open();
                                using (SqlCommand cmd = new SqlCommand("GetAllIDs", conn))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    SqlDataReader reader = cmd.ExecuteReader();

                                    while (reader.Read())
                                    {
                                        ID_Search.Add((int)reader["ID"]);
                                    }
                                }
                            }

                            if (!ID_Search.Any(e => e == EmpID_Search))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("ID not Found! Re-Enter the ID");
                                Console.ResetColor();
                                goto ID_Search;
                            }
                            else
                            {
                                using (SqlConnection conn = new SqlConnection(DBLink))
                                {
                                    conn.Open();
                                    using (SqlCommand cmd = new SqlCommand("GetEmpByID", conn))
                                    {
                                        Console.WriteLine();
                                        Console.ForegroundColor = ConsoleColor.Magenta;
                                        Console.WriteLine($"                                          ~**ID: {EmpID_Search} 's Data **~                                                  ");
                                        Console.ResetColor();
                                        Console.WriteLine();
                                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                                        Console.WriteLine(new string('-', 105));
                                        Console.WriteLine("|  ID  |   Employee Name   |        Employee Email        | DeptID | Employee Salary |  Employee Phone  |");
                                        Console.WriteLine(new string('-', 105));
                                        Console.ResetColor();

                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.AddWithValue("@ID", EmpID_Search);

                                        SqlDataReader reader = cmd.ExecuteReader();
                                        while (reader.Read())
                                        {
                                            Console.ForegroundColor = ConsoleColor.Green;
                                            Console.WriteLine($"| {reader["ID"],-4} | {reader["Emp_Name"],-17} | {reader["Emp_Email"],-28} | {reader["DepartmentID"],-6} | {reader["Emp_Salary"],-15} | {reader["Emp_Phone"],-16} |");
                                            Console.ResetColor();
                                        }
                                    }
                                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                                    Console.WriteLine(new string('-', 105));
                                    Console.ResetColor();
                                    Console.WriteLine();
                                    Console.ReadKey();
                                }
                            }
                            break;

                        case 6:
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.WriteLine("                  You Choose to Display the Employee by Departments                    ");
                            Console.ResetColor();
                            Console.WriteLine();
                            Console.WriteLine();

                            using (SqlConnection conn = new SqlConnection(DBLink))
                            {
                                conn.Open();
                                using (SqlCommand cmd = new SqlCommand("GetEmpByDept", conn))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;

                                    SqlDataReader reader = cmd.ExecuteReader();
                                    Console.WriteLine();

                                    Console.WriteLine();
                                    Console.ForegroundColor = ConsoleColor.Magenta;
                                    Console.WriteLine($"                                          ~**IT Departement Employees**~                                                  ");
                                    Console.ResetColor();
                                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                                    Console.WriteLine(new string('-', 105));
                                    Console.WriteLine("|  ID  |   Employee Name   |        Employee Email        | DeptID | Employee Salary |  Employee Phone  |");
                                    Console.WriteLine(new string('-', 105));
                                    Console.ResetColor();

                                    while (reader.Read())
                                    {
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.WriteLine($"| {reader["ID"],-4} | {reader["Emp_Name"],-17} | {reader["Emp_Email"],-28} | {reader["DepartmentID"],-6} | {reader["Emp_Salary"],-15} | {reader["Emp_Phone"],-16} |");
                                        Console.ResetColor();
                                    }
                                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                                    Console.WriteLine(new string('-', 105));
                                    Console.ResetColor();
                                    Console.WriteLine();

                                    reader.NextResult();

                                    Console.WriteLine();
                                    Console.ForegroundColor = ConsoleColor.Magenta;
                                    Console.WriteLine($"                                          ~**HR Departement Employees**~                                                  ");
                                    Console.ResetColor();
                                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                                    Console.WriteLine(new string('-', 105));
                                    Console.WriteLine("|  ID  |   Employee Name   |        Employee Email        | DeptID | Employee Salary |  Employee Phone  |");
                                    Console.WriteLine(new string('-', 105));
                                    Console.ResetColor();

                                    while (reader.Read())
                                    {
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.WriteLine($"| {reader["ID"],-4} | {reader["Emp_Name"],-17} | {reader["Emp_Email"],-28} | {reader["DepartmentID"],-6} | {reader["Emp_Salary"],-15} | {reader["Emp_Phone"],-16} |");
                                        Console.ResetColor();
                                    }
                                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                                    Console.WriteLine(new string('-', 105));
                                    Console.ResetColor();
                                    Console.WriteLine();


                                    reader.NextResult();

                                    Console.WriteLine();
                                    Console.ForegroundColor = ConsoleColor.Magenta;
                                    Console.WriteLine($"                                          ~**Finance Departement Employees**~                                                  ");
                                    Console.ResetColor();
                                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                                    Console.WriteLine(new string('-', 105));
                                    Console.WriteLine("|  ID  |   Employee Name   |        Employee Email        | DeptID | Employee Salary |  Employee Phone  |");
                                    Console.WriteLine(new string('-', 105));
                                    Console.ResetColor();

                                    while (reader.Read())
                                    {
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.WriteLine($"| {reader["ID"],-4} | {reader["Emp_Name"],-17} | {reader["Emp_Email"],-28} | {reader["DepartmentID"],-6} | {reader["Emp_Salary"],-15} | {reader["Emp_Phone"],-16} |");
                                        Console.ResetColor();
                                    }
                                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                                    Console.WriteLine(new string('-', 105));
                                    Console.ResetColor();
                                    Console.WriteLine();


                                    reader.NextResult();

                                    Console.WriteLine();
                                    Console.ForegroundColor = ConsoleColor.Magenta;
                                    Console.WriteLine($"                                          ~**Marketing Departement Employees**~                                                  ");
                                    Console.ResetColor();
                                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                                    Console.WriteLine(new string('-', 105));
                                    Console.WriteLine("|  ID  |   Employee Name   |        Employee Email        | DeptID | Employee Salary |  Employee Phone  |");
                                    Console.WriteLine(new string('-', 105));
                                    Console.ResetColor();

                                    while (reader.Read())
                                    {
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.WriteLine($"| {reader["ID"],-4} | {reader["Emp_Name"],-17} | {reader["Emp_Email"],-28} | {reader["DepartmentID"],-6} | {reader["Emp_Salary"],-15} | {reader["Emp_Phone"],-16} |");
                                        Console.ResetColor();
                                    }
                                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                                    Console.WriteLine(new string('-', 105));
                                    Console.ResetColor();
                                    Console.WriteLine();


                                    reader.NextResult();

                                    Console.WriteLine();
                                    Console.ForegroundColor = ConsoleColor.Magenta;
                                    Console.WriteLine($"                                          ~**Operation Departement Employees**~                                                  ");
                                    Console.ResetColor();
                                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                                    Console.WriteLine(new string('-', 105));
                                    Console.WriteLine("|  ID  |   Employee Name   |        Employee Email        | DeptID | Employee Salary |  Employee Phone  |");
                                    Console.WriteLine(new string('-', 105));
                                    Console.ResetColor();

                                    while (reader.Read())
                                    {
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.WriteLine($"| {reader["ID"],-4} | {reader["Emp_Name"],-17} | {reader["Emp_Email"],-28} | {reader["DepartmentID"],-6} | {reader["Emp_Salary"],-15} | {reader["Emp_Phone"],-16} |");
                                        Console.ResetColor();
                                    }
                                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                                    Console.WriteLine(new string('-', 105));
                                    Console.ResetColor();
                                    Console.WriteLine();
                                }
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : {0}",ex.Message);
                Console.WriteLine(ex);
            }
        }
    }
}
