using System.Collections;

namespace XamarinSnippets
{
    public static class EnumerableExtensions
    {
        public static bool Any(
            this IEnumerable enumerable)
        {
            if (enumerable == null)
            {
                return false;
            }


            var enumerator = enumerable.GetEnumerator();

            return enumerator.MoveNext();
        }


        public static int Count(
            this IEnumerable enumerable)
        {
            if (enumerable == null)
            {
                return 0;
            }


            int count = 0;
            var enumerator = enumerable.GetEnumerator();

            while (enumerator.MoveNext())
            {
                count++;
            }

            return count;
        }


        public static int IndexOf(
            this IEnumerable enumerable,
            object @object)
        {
            if (enumerable == null)
            {
                return -1;
            }


            int index = 0;
            var enumerator = enumerable.GetEnumerator();

            while (enumerator.MoveNext())
            {
                if (enumerator.Current == @object)
                {
                    return index;
                }

                index++;
            }

            return -1;
        }


        public static object ElementAt(
            this IEnumerable enumerable,
            int index)
        {
            if (enumerable == null)
            {
                return null;
            }


            var enumerator = enumerable.GetEnumerator();

            int count = 0;
            while(enumerator.MoveNext())
            {
                if (index == count)
                {
                    return enumerator.Current;
                }

                count++;
            }


            return null;
        }
    }
}