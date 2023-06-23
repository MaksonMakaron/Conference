using Conference.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Conference.Classes
{
    public class Authorization
    {
        public static  User CurrentUser = null;

        public static User SignIn(int id, string password)
        {
            var user = App.DataBase.Users.Where(p => p.Id == id && p.Password == password).FirstOrDefault();

            if (user != null)
            {
                return user;
            }

            return null;
        }

        public static bool ValidatePassword(string password, string confirmPassword)
        {
            // Проверка длины пароля (не менее 6 символов).
            if (password.Length < 6)
            {
                return false;
            }

            // Проверка наличия заглавных и строчных букв.
            if (!Regex.IsMatch(password, @"[a-z]") || !Regex.IsMatch(password, @"[A-Z]"))
            {
                return false;
            }

            // Проверка наличия специальных символов.
            if (!Regex.IsMatch(password, @"[!@#$%^&*()]"))
            {
                return false;
            }

            // Проверка наличия цифр.
            if (!Regex.IsMatch(password, @"\d"))
            {
                return false;
            }

            // Проверка соответствия подтверждения пароля.
            if (password != confirmPassword)
            {
                return false;
            }

            return true;
        }
    }
}

