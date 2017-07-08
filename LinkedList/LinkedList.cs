using System;
using System.Collections;
using System.Collections.Generic;


namespace LinkedList
{
    public class LinkedListEventArgs : EventArgs
    {
        public bool IsExecuted { get; }
        public string Message { get; }

        public LinkedListEventArgs(bool isExecuted, string message)
        {
            IsExecuted = isExecuted;
            Message = message;
        }
    }


    public sealed class LinkedListNode<T>
    {
        public LinkedListNode(T value) : this(value, null, null, null) { }
        internal LinkedListNode(T value, LinkedListNode<T> next, LinkedListNode<T> previous, LinkedList<T> list)
        {
            this.Value = value;
            this.List = list;
            this.Next = next;
            this.Previous = previous;
        }
        public LinkedList<T> List { get; internal set; }
        public T Value { get; set; }
        public LinkedListNode<T> Next { get; internal set; }
        public LinkedListNode<T> Previous { get; internal set; }
    }



    public sealed class LinkedList<T> : IEnumerable<T>, ICollection<T>
    {
        public LinkedList()
        {
            this.Count = 0;
            this.First = null;
            this.Last = null;
        }

        public int Count { get; private set; }
        public LinkedListNode<T> First { get; private set; }
        public LinkedListNode<T> Last { get; private set; }
        public bool IsReadOnly => false;

        public event EventHandler<LinkedListEventArgs> Added;
        public event EventHandler<LinkedListEventArgs> Removed;
        public event EventHandler<LinkedListEventArgs> Cleared;

        /*
        //public int Count
        //{
        //    get
        //    {
        //        if (ReferenceEquals(null,first)) return 0;

        //        Node iNode = first;
        //        int count = 1;
        //        while (!ReferenceEquals(iNode, last))
        //        {
        //            count++;
        //            iNode = iNode.Next;
        //        }
        //        return count;
        //    }
        //}

        //public T First()
        //{
        //    if (ReferenceEquals(first, null)) throw new InvalidOperationException("LinkedList is empty");

        //    if(ReferenceEquals(first,last))
        //    {
        //        Node returnedNode = first;
        //        first = null;
        //        last = null;
        //        return returnedNode.Value;
        //    }

        //    Node returnedFirst = first;
        //    first.Next.Previous = null;
        //    first = first.Next;

        //    return returnedFirst.Value;   
        //}

        //public T Last()
        //{
        //    if (ReferenceEquals(last, null)) throw new InvalidOperationException("LinkedList is empty");
        //    if (ReferenceEquals(first, last))
        //    {
        //        Node returnedNode = first;
        //        first = null;
        //        last = null;
        //        return returnedNode.Value;
        //    }

        //    Node returnedLast = last;
        //    last.Previous.Next = null;
        //    last = last.Previous;

        //    return returnedLast.Value;
        //}
        */

        public void Add(T value)
        {
            LinkedListNode<T> newNode = new LinkedListNode<T>(value, null, Last, this);


            if (ReferenceEquals(null, First))
            {
                First = newNode;
                Last = newNode;
                Count = 1;
                Added?.Invoke(this, new LinkedListEventArgs(true, "The value was added into the LinkedList."));
                return;
            }
            Last.Next = newNode;
            Last = newNode;
            Count++;
            Added?.Invoke(this, new LinkedListEventArgs(true, "The value was added into the LinkedList."));
            return;
        }

        public void Clear()
        {
            if (ReferenceEquals(null, First))
            {
                Cleared?.Invoke(this, new LinkedListEventArgs(true, "The LinkedList has been already cleared."));
                return;
            }

            Count = 0;

            if (ReferenceEquals(First, Last))
            {
                First = null;
                Last = null;
                Cleared?.Invoke(this, new LinkedListEventArgs(true, "The LinkedList was cleared."));
                return;
            }

            LinkedListNode<T> deletedNode = First.Next;

            while (!ReferenceEquals(deletedNode, Last))
            {
                deletedNode.Previous.Next = null;
                deletedNode.Previous = null;
                deletedNode = deletedNode.Next;
            }
            First.Next = null;
            First = null;
            Last.Previous = null;
            Last = null;
            Cleared?.Invoke(this, new LinkedListEventArgs(true, "The LinkedList was cleared."));
            return;
        }

        public bool Contains(T value)
        {
            //if (ReferenceEquals(value, null)) throw new ArgumentNullException("value", "A null reference is passed to a method.");
            if (ReferenceEquals(First, null)) return false;
            if (ReferenceEquals(First, Last))
            {

                if (ReferenceEquals(First.Value, value)) return true;
                if (First.Value.GetType() != value.GetType()) return false;
                if (First.Value.Equals(value)) return true;
                return false;
            }

            LinkedListNode<T> iNode = First;
            while (!ReferenceEquals(iNode, Last))
            {
                if (ReferenceEquals(iNode.Value, value)) return true;
                if (iNode.Value.GetType() != value.GetType())
                {
                    iNode = iNode.Next;
                    continue;
                }
                if (iNode.Value.Equals(value)) return true;
                iNode = iNode.Next;
            }

            if (ReferenceEquals(Last.Value, value)) return true;
            if (iNode.Value.GetType() != value.GetType()) return false;
            if (iNode.Value.Equals(value)) return true;
            return false;
        }

        public void CopyTo(T[] array, int index)
        {
            if (ReferenceEquals(array, null)) throw new ArgumentNullException("array", "A null reference is passed to a method.");
            if (index < 0) throw new ArgumentOutOfRangeException();
            if (array.Rank > 1 || (this.Count > array.Length - index)) throw new ArgumentException();

            if (ReferenceEquals(First, null)) return;
            if (ReferenceEquals(First, Last))
            {
                array[index] = First.Value;
            }

            LinkedListNode<T> iNode = First;
            while (!ReferenceEquals(iNode, Last))
            {
                array[index] = iNode.Value;
                index++;
                iNode = iNode.Next;
            }

            array[index] = iNode.Value;
        }

        public bool Remove(T value)
        {
            if (ReferenceEquals(null, First))
            {
                Removed?.Invoke(this, new LinkedListEventArgs(false, "The LinkedList is empty. Removing is canceled."));
                return false;
            }

            bool isContain = false;

            LinkedListNode<T> iNode = First;
            while (!ReferenceEquals(iNode, Last))
            {
                if (ReferenceEquals(iNode.Value, value))
                {
                    isContain = true;
                    break;
                }
                if (iNode.Value.GetType() != value.GetType())
                {
                    iNode = iNode.Next;
                    continue;
                }
                if (iNode.Value.Equals(value))
                {
                    isContain = true;
                    break;
                }
                iNode = iNode.Next;
            }

            if (ReferenceEquals(Last.Value, value) && !isContain) isContain = true;
            if (iNode.Value.GetType() != value.GetType() && !isContain) isContain = false;
            if (iNode.Value.Equals(value) && !isContain) isContain = true;

            if (isContain)
            {
                Count--;
                if (ReferenceEquals(iNode, First) && ReferenceEquals(iNode, Last))
                {
                    First = null;
                    Last = null;
                    return true;
                }
                if (ReferenceEquals(iNode, First))
                {
                    First.Next.Previous = null;
                    First = First.Next;
                    return true;
                }
                if (ReferenceEquals(iNode, Last))
                {
                    Last.Previous.Next = null;
                    Last = Last.Previous;
                    return true;
                }

                iNode.Previous.Next = iNode.Next;
                iNode.Next.Previous = iNode.Previous;
                iNode.Previous = null;
                iNode.Next = null;
                iNode = null;
                Removed?.Invoke(this, new LinkedListEventArgs(true, "The passing element was removed."));
                return true;
            }

            Removed?.Invoke(this, new LinkedListEventArgs(false, "The passing element did not find. Removing is canceled."));
            return false;
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (ReferenceEquals(First, null)) yield break;

            LinkedListNode<T> iNode = First;
            while (!ReferenceEquals(iNode, Last))
            {
                yield return iNode.Value;
                iNode = iNode.Next;
            }
            yield return Last.Value;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Insert(T beforeValue, T value)
        {
            if (ReferenceEquals(First, null)) return;

            bool isContain = false;

            LinkedListNode<T> iNode = First;
            while (!ReferenceEquals(iNode, Last))
            {
                if (ReferenceEquals(iNode.Value, beforeValue))
                {
                    isContain = true;
                    break;
                }
                if (iNode.Value.GetType() != beforeValue.GetType())
                {
                    iNode = iNode.Next;
                    continue;
                }
                if (iNode.Value.Equals(beforeValue))
                {
                    isContain = true;
                    break;
                }
                iNode = iNode.Next;
            }

            if (ReferenceEquals(Last.Value, beforeValue) && !isContain) isContain = true;
            if (iNode.Value.GetType() != beforeValue.GetType() && !isContain) isContain = false;
            if (iNode.Value.Equals(beforeValue) && !isContain) isContain = true;

            if (isContain)
            {
                LinkedListNode<T> newNode = new LinkedListNode<T>(value, iNode, iNode.Previous, this);
                if (ReferenceEquals(First, iNode))
                {
                    First = newNode;
                    Count++;
                    return;
                }
                iNode.Previous.Next = newNode;
                iNode.Previous = newNode;
                Count++;
                return;
            }
        }
    }
}
