using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedList
{
    public class LinkedList<T> : IEnumerable<T>, ICollection<T>
    {
        private class Node
        {
            public T Item { get; set; }
            public Node Next { get; set; }
            public Node Previous { get; set; }
        }

        private Node first;
        private Node last;

        public int Count
        {
            get
            {
                if (ReferenceEquals(first, null)) return 0;

                Node iNode = first;
                int count = 1;
                while (!ReferenceEquals(iNode.Next, null))
                {
                    count++;
                    iNode = iNode.Next;
                }
                return count;
            }
        }

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            //if (ReferenceEquals(item, null)) throw new ArgumentNullException("item", "A null reference is passed to a method.");
            Node newNode = new Node();

            if (ReferenceEquals(first, null))
            {

                newNode.Item = item;
                newNode.Next = null;
                newNode.Previous = null;
                first = newNode;
                last = newNode;
                return;
            }

            newNode.Item = item;
            newNode.Next = null;
            newNode.Previous = last;
            last.Next = newNode;
            last = newNode;
            return;        
        }

        public void Clear()
        {
            if (ReferenceEquals(first, null)) return;
            if (ReferenceEquals(first.Next, null))
            {
                first = null;
                last = null;
                return;
            }

            Node deletedNode = first.Next;
            while (!ReferenceEquals(deletedNode.Next, last))
            {

                deletedNode.Previous = null;
                deletedNode.Previous.Next = null;
                deletedNode = deletedNode.Next;
            }
            last.Previous = null;
            last = null;
            return;
        }

        public bool Contains(T item)
        {
            //if (ReferenceEquals(item, null)) throw new ArgumentNullException("item", "A null reference is passed to a method.");
            if (ReferenceEquals(first, null)) return false;

            Node iNode = first;
            while (!ReferenceEquals(iNode.Next, null))
            {
                if (ReferenceEquals(iNode.Item, item)) return true;
                if (iNode.Item.GetType() != item.GetType())
                {
                    iNode = iNode.Next;
                    continue;
                }
                if (iNode.Equals(item)) return true;
                iNode = iNode.Next;
            }
            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (ReferenceEquals(array, null)) throw new ArgumentNullException("array", "A null reference is passed to a method.");
            if (arrayIndex < 0) throw new ArgumentOutOfRangeException();
            if (array.Rank > 1 || (this.Count > array.Length - arrayIndex)) throw new ArgumentException();

            Node iNode = first;
            while (!ReferenceEquals(iNode.Next, null))
            {
                array[arrayIndex] = iNode.Item;
                arrayIndex++;
                iNode = iNode.Next;
            }

            //ArrayTypeMismatchException
            //Тип исходного массива Array не может быть автоматически приведен к типу массива назначения array.
            //RankException
            //Исходный массив — многомерный.
            //InvalidCastException
            //По меньшей мере один элемент в исходном массиве Array нельзя привести к типу массива назначения array.
        }

        public bool Remove(T item)
        {
            if (ReferenceEquals(first, null)) return false;

            bool isContain = false;

            Node iNode = first;
            while (!ReferenceEquals(iNode.Next, null) || ReferenceEquals(iNode, first))
            {
                if (ReferenceEquals(iNode.Item, item))
                {
                    isContain = true;
                    break;
                }
                if (iNode.Item.GetType() != item.GetType())
                {
                    iNode = iNode.Next;
                    continue;
                }
                if (iNode.Equals(item))
                {
                    isContain = true;
                    break;
                }
                iNode = iNode.Next;
            }

            if (isContain)
            {
                if (ReferenceEquals(iNode, first) && ReferenceEquals(iNode, last))
                {
                    first = null;
                    last = null;
                    return true;
                }
                if (ReferenceEquals(iNode, first))
                {
                    first.Next.Previous = null;
                    first = first.Next;
                    return true;
                }
                if (ReferenceEquals(iNode, last))
                {
                    last.Previous.Next = null;
                    last  =last.Previous;
                    return true;
                }

                iNode.Previous.Next = iNode.Next;
                iNode.Next.Previous = iNode.Previous;
                iNode.Previous = null;
                iNode.Next = null;
                iNode = null;
                return true;
            }

            return false;
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (ReferenceEquals(first, null)) yield break;

            yield return first.Item;

            Node iNode = first;
            while (!ReferenceEquals(iNode.Next, null))
            {
                yield return iNode.Item;
                iNode = iNode.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Insert(T item, T insertedItem)
        {

        }

        public T First()
        {
            if (ReferenceEquals(first, null)) throw new InvalidOperationException("LinkedList is empty");

            if(ReferenceEquals(first,last))
            {
                Node returnedNode = first;
                first = null;
                last = null;
                return returnedNode.Item;
            }

            Node returnedFirst = first;
            first.Next.Previous = null;
            first = first.Next;

            return returnedFirst.Item;   
        }

        public T Last()
        {
            if (ReferenceEquals(last, null)) throw new InvalidOperationException("LinkedList is empty");
            if (ReferenceEquals(first, last))
            {
                Node returnedNode = first;
                first = null;
                last = null;
                return returnedNode.Item;
            }

            Node returnedLast = last;
            last.Previous.Next = null;
            last = last.Previous;

            return returnedLast.Item;
        }
    }
}
