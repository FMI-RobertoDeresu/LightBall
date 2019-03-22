using System.Linq;

namespace Assets.Scripts.Utils
{
    public static class CommonUtils
    {
        public static int GetEndingNumber(string str)
        {
            var numberStr = string.Join("", str.Reverse().TakeWhile(char.IsDigit).Reverse());
            var number = int.Parse(numberStr);
            return number;
        }
    }
}