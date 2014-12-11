namespace Entities
{
    public static class IntExtension
    {
        public static bool IsEven(this int intNumber)
        {
            return intNumber % 2 != 0;
        }
    }
}