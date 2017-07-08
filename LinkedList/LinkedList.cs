using System;
using System.Collections;
using System.Collections.Generic;


namespace LinkedList
{
    /// <summary>
    /// Provides data for the LinkedList event.
    /// </summary>
    public class LinkedListEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the date whether method was executed.
        /// </summary>
        public bool IsExecuted { get; }
        /// <summary>
        /// Gets the message with specified notes about method.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Initializes a new instance of the LinkedListEventArgs class, specifying the executing and message.
        /// </summary>
        /// <param name="isExecuted"></param>
        /// <param name="message"></param>
        public LinkedListEventArgs(bool isExecuted, string message)
        {
            IsExecuted = isExecuted;
            Message = message;
        }
    }

    /// <summary>
    /// Represents a node in a LinkedList T .This class cannot be inherited.
    /// </summary>
    /// <typeparam name="T">Specifies the element type of the linked list.</typeparam>
    public sealed class LinkedListNode<T>
    {
        /// <summary>
        /// Initializes a new instance of the LinkedListNode T  class, containing the specified value.
        /// </summary>
        /// <param name="value">The value to contain in the LinkedListNode T .</param>
        public LinkedListNode(T value) : this(value, null, null, null) { }
        internal LinkedListNode(T value, LinkedListNode<T> next, LinkedListNode<T> previous, LinkedList<T> list)
        {
            this.Value = value;
            this.List = list;
            this.Next = next;
            this.Previous = previous;
        }
        /// <summary>
        /// Gets the LinkedList T  that the LinkedListNode T  belongs to.
        /// </summary>
        public LinkedList<T> List { get; internal set; }
        /// <summary>
        /// Gets the value contained in the node.
        /// </summary>
        public T Value { get; set; }
        /// <summary>
        /// Gets the next node in the LinkedList T .
        /// </summary>
        public LinkedListNode<T> Next { get; internal set; }
        /// <summary>
        /// Gets the previous node in the LinkedList T .
        /// </summary>
        public LinkedListNode<T> Previous { get; internal set; }
    }


    /// <summary>
    /// Represents a doubly linked list.
    /// </summary>
    /// <typeparam name="T">Specifies the element type of the linked list.</typeparam>
    public sealed class LinkedList<T> : IEnumerable<T>, ICollection<T>
    {
        /// <summary>
        /// Initializes a new instance of the LinkedList T  class that is empty.
        /// </summary>
        public LinkedList()
        {
            this.Count = 0;
            this.First = null;
            this.Last = null;
        }

        /// <summary>
        /// Gets the number of nodes actually contained in the LinkedList T 
        /// </summary>
        public int Count { get; private set; }
        /// <summary>
        /// Gets the first node of the LinkedList T .
        /// </summary>
        public LinkedListNode<T> First { get; private set; }
        /// <summary>
        /// Gets the last node of the LinkedList T .
        /// </summary>
        public LinkedListNode<T> Last { get; private set; }
        /// <summary>
        /// Gets a value indicating whether the LinkedList T  is read-only.
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// Occurs directly after Add is called, and can be handled to get information about executed and the message about executing.
        /// </summary>
        public event EventHandler<LinkedListEventArgs> Added;
        /// <summary>
        /// Occurs directly after Remove is called, and can be handled to get information about executed and the message about executing.
        /// </summary>
        public event EventHandler<LinkedListEventArgs> Removed;
        /// <summary>
        /// Occurs directly after Clear is called, and can be handled to get information about executed and the message about executing.
        /// </summary>
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

        /// <summary>
        /// Adds a new node containing the specified value at the end of the LinkedList T .
        /// </summary>
        /// <param name="value">The value to add at the end of the LinkedList T .</param>
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

        /// <summary>
        /// Removes all nodes from the LinkedList T .
        /// </summary>
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

        /// <summary>
        /// Determines whether a value is in the LinkedList T .
        /// </summary>
        /// <param name="value">The value to locate in the LinkedList T .</param>
        /// <returns>true if value is found in the LinkedList T ; otherwise, false.</returns>
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

        /// <summary>
        /// Copies the entire LinkedList T  to a compatible one-dimensional Array, starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">The one-dimensional Array that is the destination of the elements copied from LinkedList T .</param>
        /// <param name="index">The zero-based index in array at which copying begins.</param>
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

        /// <summary>
        /// Removes the first occurrence of the specified value from the LinkedList T .
        /// </summary>
        /// <param name="value">The value to remove from the LinkedList T .</param>
        /// <returns>true if the element containing value is successfully removed; otherwise, false.</returns>
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

        /// <summary>
        /// Returns an enumerator that iterates through the LinkedList T .
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Adds a new node containing the specified value before the specified existing value in the LinkedList T .
        /// </summary>
        /// <param name="beforeValue">The value before which to insert a new LinkedListNode T  containing value.</param>
        /// <param name="value">The value to add to the LinkedList T .</param>
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
