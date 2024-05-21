
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using SchoolManagement.Common.Exceptions;
using System.Text;
using System.Web;

namespace SchoolManagement.Common.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string? s) => string.IsNullOrEmpty(s);

        public static string EncodeXmlUTF(this string? value)
        {
            var builder = new StringBuilder();
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            foreach (char c in value)
            {
                if (c < 32)
                {
                    builder.Append($"_x{(int)c:X4}_");
                }
                else
                {
                    builder.Append(c);
                }
            }
            return builder.ToString();
        }

        public static string EncodeHtml(this string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            return HttpUtility.HtmlEncode(value);
        }

        public static string DecodeHtml(this string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            return HttpUtility.HtmlDecode(value);
        }
        public static Guid ToGuid(this string? s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                return new Guid(s);
            }
            return Guid.Empty;
        }
        public static string ToEmailString(this string text)
        {
            var result = string.Empty;
            if (string.IsNullOrEmpty(text))
            {
                return result;
            }
            if (text.IndexOf("<") > -1)
            {
                result = text.Replace("<", "");
            }
            if (text.IndexOf(">") > -1)
            {
                result = result.Replace(">", "");
            }
            return result;
        }
        public static DateTime? ConvertToDateTime(this string? dateString, string formatDate = "yyyyMMdd")
        {
            if (DateTime.TryParseExact(dateString, formatDate, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime dateValue))
            {
                return dateValue;
            }
            else
            {
                return null;
            }
        }
        public static string? ConvertDateTimeToString(this DateTime? date, string formatDate = "dd-MMM-yy")
        {
            return date == null ? null : DateTime.Parse(date.ToString()!, System.Globalization.DateTimeFormatInfo.CurrentInfo).ToString(formatDate);
        }
        public static DateTime? ConvertToDateTimeZone(this DateTime? dateTime)
        {
            if (dateTime == null)
            {
                return null;
            }
            else
            {
                return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dateTime.Value, "Greenwich Standard Time", "Singapore Standard Time");
            }
        }

        public static DateTime? CovertToSGTimeZone(this DateTime? dateTime)
        {
            if (dateTime == null)
            {
                return null;
            }
            else
            {
                return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dateTime.Value, "Singapore Standard Time");
            }
        }

        public static string ToFormatString(this DateTime dateTime, string formatDate = "yyyyMMdd")
        {
            return dateTime.ToString(formatDate);
        }

        public static string ToUpperStr(this string? orgValue)
        {
            if (orgValue == null)
            {
                return String.Empty;
            }
            else
            {
                return orgValue.ToUpper()!;
            }
        }

        public static string? ConvertToFullStatus(this string? value)
        {
            return value switch
            {
                "Y" => "Yes",
                "N" => "No",
                "A" => "Active",
                "D" => "Deactive",
                "I" => "Inactive",
                _ => null
            };
        }
        public static decimal? ConvertStringToDecimal(this string? value)
        {
            if (value == null || value == string.Empty)
            {
                return null;
            }
            if (decimal.TryParse(value, out decimal result))
            {
                return result;
            }
            else
            {
                throw new LogicException("Can not convert string type to decimal type");
            }
        }
        public static Task<T> ToTask<T>(this T result)
        {
            return Task.FromResult<T>(result);
        }
        public static ValueTask<T> ToValueTask(this T result)
        {
            return ValueTask.FromResult<T>(result);
        }
        public static T? ToObject<T>(this string jsonstr, JsonSerializerSettings? jsonSerializerSettings = null)
        {
            return JsonConvert.DeserializeObject<T>(jsonstr, jsonSerializerSettings);
        }
        public static string ToJson<T>(this T obj, JsonSerializerSettings? jsonSerializerSettings = null)
        {
            return JsonConvert.SerializeObject(obj, jsonSerializerSettings);
        }
        public static T[] WrapWithArray<T>(this T obj, params T[] objs)
        {
            if (objs.Any())
            {
                var arr = Array.CreateInstance(typeof(T), objs.Length + 1) as T[];
                arr![0] = obj;
                for (int i = 1; i <= objs.Length; i++)
                {
                    arr![i] = objs[i - 1];
                }
                return arr!;
            }
            return new T[] { obj };
        }
        public static List<T> WrapWithList<T>(this T obj)
        {
            return new List<T>() { obj };
        }
        public static IEnumerable<string> SplitByComma(this string str, StringSplitOptions stringSplitOptions = StringSplitOptions.RemoveEmptyEntries)
        {
            return str.SplitBy(',', stringSplitOptions);
        }
        public static IEnumerable<string> SplitBy(this string str, char separator, StringSplitOptions stringSplitOptions = StringSplitOptions.RemoveEmptyEntries)
        {
            return str.Split(separator, stringSplitOptions);
        }
        public static IEnumerable<string> SplitBy(this string str, string separator, StringSplitOptions stringSplitOptions = StringSplitOptions.RemoveEmptyEntries)
        {
            return str.Split(separator, stringSplitOptions);
        }
        public static string GenerateStudentEmail(string fullName, string id)
        {
            // Tách họ, tên đệm và tên
            string[] nameParts = fullName.Split(' ');
            string lastName = nameParts[0];
            string middleName = string.Join(" ", nameParts.Skip(1).Take(nameParts.Length - 2));
            string firstName = nameParts[nameParts.Length - 1];

            // Lấy ký tự đầu tiên từ họ, tên đệm và tên
            string initials = lastName[0].ToString().ToLower();
            initials += string.Join("", middleName.Split(' ').Select(n => n[0].ToString().ToLower()));
            initials += firstName[0].ToString().ToLower();

            // Lấy 6 chữ số cuối của ID
            string idSuffix = id.Substring(id.Length - 6);

            // Kết hợp các kí tự vừa lấy được với id
            return $"{initials}{idSuffix}@thptquangtrung.edu";
        }

        public static string RemoveAccents(string text)
        {
            // Bảng chữ cái có dấu và không dấu
            string withAccentChars = "àáạảãâầấậẩẫăằắặẳẵèéẹẻẽêềếệểễìíịỉĩòóọỏõôồốộổỗơờớợởỡùúụủũưừứựửữỳýỵỷỹđ";
            string withoutAccentChars = "aaaaaaaaaaaaaaaaaeeeeeeeeeeeiiiiiooooooooooooooooouuuuuuuuuuuyyyyyd";

            // Loại bỏ dấu từ text
            for (int i = 0; i < withAccentChars.Length; i++)
            {
                text = text.Replace(withAccentChars[i], withoutAccentChars[i]);
            }

            return text;
        }
    }
}
