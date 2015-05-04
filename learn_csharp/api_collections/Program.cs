using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api_collections
{
    public delegate int IntPredicate<T>(T obj);
    public class SHList<T> : List<T>
    {
        Comparison<T> mComparison = null;
        public SHList(Comparison<T> comparison)
        {
            mComparison = comparison;
        }

        new public void Add(T obj)
        {
            base.Add(obj);
            Sort();
        }
        new public void AddRange(IEnumerable<T> collection)
        {
            base.AddRange(collection);
            Sort();
        }
        new public void Remove(T obj)
        {
            base.Remove(obj);
            Sort();
        }
        new public void Sort()
        {
            base.Sort(mComparison);
        }
        public T Find(T temp)
        {
            // 2分查找
            int maxcnt = (this.Count + 1) / 2;
            int low = 0, high = this.Count - 1;
            int mid = 0;
            while (low <= high)
            {
                mid = low + (high - low) / 2;

                T right = this[mid];
                int va = mComparison(temp, right);

                if (va == 0)
                {
                    return right;
                }

                if (va < 0)
                {
                    high = mid - 1;
                }
                else
                {
                    low = mid + 1;
                }
            }

            return default(T);
        }
        public T Find(IntPredicate<T> match)
        {
            // 2分查找
            int maxcnt = (this.Count + 1) / 2;
            int low = 0, high = this.Count - 1;
            int mid = 0;
            while (low <= high)
            {
                mid = low + (high - low) / 2;

                T right = this[mid];
                int va = match(right);

                if (va == 0)
                {
                    return right;
                }

                if (va < 0)
                {
                    high = mid - 1;
                }
                else
                {
                    low = mid + 1;
                }
            }

            return default(T);
        }

        public static void Test()
        {
            SHList<int> a = new SHList<int>(delegate(int left, int right)
            {
                if (left < right)
                    return -1;
                else if (left > right)
                    return 1;
                else
                    return 0;
            });
            a.Add(1);
            a.Add(5);
            a.Add(3);
            a.Add(2);
            a.Add(4);
            int ret = a.Find(1);
            Console.WriteLine(ret);
            ret = a.Find(2);
            Console.WriteLine(ret);
            ret = a.Find(3);
            Console.WriteLine(ret);
            ret = a.Find(4);
            Console.WriteLine(ret);
            ret = a.Find(5);
            Console.WriteLine(ret);

            ret = a.Find(delegate(int right)
            {
                int left = 2;
                if (left < right)
                    return -1;
                else if (left > right)
                    return 1;
                else
                    return 0;
            });
            Console.WriteLine(ret);
            ret = a.Find(delegate(int right)
            {
                int left = 3;
                if (left < right)
                    return -1;
                else if (left > right)
                    return 1;
                else
                    return 0;
            });
            Console.WriteLine(ret);
            ret = a.Find(delegate(int right)
            {
                int left = 4;
                if (left < right)
                    return -1;
                else if (left > right)
                    return 1;
                else
                    return 0;
            });
            Console.WriteLine(ret);
            ret = a.Find(delegate(int right)
            {
                int left = 5;
                if (left < right)
                    return -1;
                else if (left > right)
                    return 1;
                else
                    return 0;
            });
            Console.WriteLine(ret);
            ret = a.Find(delegate(int right)
            {
                int left = 1;
                if (left < right)
                    return -1;
                else if (left > right)
                    return 1;
                else
                    return 0;
            });
            Console.WriteLine(ret);
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            SHList<int>.Test();            
        }
    }
}
