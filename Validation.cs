using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Employee_Management_System_CRUD_Operation
{
    public static class Validation
    {
        public static bool nameValidation(string name)
        {
            string EmpNameRegex = @"^[A-Za-z]+( [A-Za-z]+)?$";

            if (string.IsNullOrEmpty(name))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please Enter the Employee Name!!");
                Console.ResetColor();
                return false;
            }

            if (name.Length < 3)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Name must be at least 3 character!!");
                Console.ResetColor();
                return false;
            }

            if (name.Contains("__"))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Username cannot contain consecutive underscores.!");
                Console.ResetColor();
                return false;
            }

            if (!Regex.IsMatch(name, EmpNameRegex))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Employee Name.Please Enter the valid Employee name!!");
                Console.ResetColor();
                return false;
            }

            return true;
        }

        public static bool emailValidation(string email)
        {
            string emailRegex = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z]+\.[a-zA-Z]{2,}$";
            if (string.IsNullOrEmpty(email))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please Enter the Employee Name!!");
                Console.ResetColor();
                return false;
            }

            if(!Regex.IsMatch(email, emailRegex)){
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Email Formate!!!");
                Console.ResetColor();
                return false;
            }

            return true;
        }

        public static bool phoneValidation(string phone)
        {
            string phoneRegex = @"^[6-9]\d{9}$";

            if (string.IsNullOrWhiteSpace(phone))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please ! Enter the Phone Number!!");
                Console.ResetColor();
                return false;
            }

            if (!Regex.IsMatch(phone, phoneRegex))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please ! Enter the Phone Number!!");
                Console.ResetColor();
                return false;
            }

            return true;
        }
    }
}
