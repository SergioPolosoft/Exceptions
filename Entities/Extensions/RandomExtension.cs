using System;

namespace Entities
{
    public static class RandomExtension
    {
        public static int NextPair(this Random random,int min, int max)
        {
            int index = random.Next(min, max);
            if (index.IsEven())
            {
                index--;
            }
            return index;
        }
    }
}