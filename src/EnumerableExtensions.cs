using System.Collections;

namespace XamarinSnippets
{
    /// <summary>
    /// Copyright by Jan Morfeld
    /// https://github.com/Pantheas/XamarinSnippets
    /// 
    /// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
    /// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
    /// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
    /// </summary>
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