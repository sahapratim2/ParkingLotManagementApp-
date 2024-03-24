using System.Text;
using System.Text.RegularExpressions;

namespace ParkingManagementApp.Common.Extensions
{
    public static class DataTimeExtension
    {
    }

    public static class StringExtension
    {

        public static bool IsNotNullOrEmpty(this string input)
        {
            return !String.IsNullOrEmpty(input);
        }
    }

    public static class DecimalExtension
    {
        public static bool IsZero(this decimal value)
        {
            if (value == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static string ToString(this decimal value, bool IgnoreZeroValue)
        {
            if (IgnoreZeroValue)
            {
                if (value.IsZero())
                {
                    return string.Empty;
                }
                else
                {
                    return value.ToString();
                }
            }
            else
            {
                return value.ToString();
            }
        }
    }

    public static class IntegerExtension
    {
        public static string ToString(this int value, bool isZeroToEmpty)
        {
            if (isZeroToEmpty)
            {
                if (value == 0)
                {
                    return string.Empty;
                }
                else
                {
                    return value.ToString();
                }
            }
            else
            {
                return value.ToString();
            }
        }
    }
}
