namespace SchoolManagement.Common.Extensions
{
    public static class AccountExtensions
    {
        public static string GenerateUserName(string fullName, string studentId)
        {
            // Loại bỏ dấu cách và chuyển tên học sinh thành chữ thường
            string userName = fullName.Replace(" ", "").ToLower();

            // Lấy 4 ký tự cuối cùng của studentId và thêm vào tên người dùng
            userName += studentId.Substring(studentId.Length - 4);

            return userName;
        }

        public static string GenerateRandomPassword(int length = 8)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var passwordChars = new char[length];
            for (int i = 0; i < length; i++)
            {
                passwordChars[i] = chars[random.Next(chars.Length)];
            }
            return new string(passwordChars);
        }
    }
}
