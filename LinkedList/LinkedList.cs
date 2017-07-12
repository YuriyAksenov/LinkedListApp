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
    /// Provides data for the LinkedList T event with adding value T.
    /// </summary>
    public class AddedLinkedListEventArgs<T> : LinkedListEventArgs
    {
        /// <summary>
        /// Adding value to the LinkedList T.
        /// </summary>
        public T Value { get; }
        /// <summary>
        /// Initializes a new instance of the AddedLinkedListEventArgs class, specifying the executing and message and containing the specified value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isExecuted"></param>
        /// <param name="message"></param>
        public AddedLinkedListEventArgs(T value, bool isExecuted, string message) : base(isExecuted,message)
        {
            Value = value;
        }
    }

    /// <summary>
    /// Provides data for the LinkedList T event with removing value T.
    /// </summary>
    public class RemovedLinkedListEventArgs<T> : LinkedListEventArgs
    {
        /// <summary>
        /// Removing value from the LinkedList T.
        /// </summary>
        public T Value { get; }
        /// <summary>
        /// Initializes a new instance of the RemovedLinkedListEventArgs class, specifying the executing and message and containing the specified value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isExecuted"></param>
        /// <param name="message"></param>
        public RemovedLinkedListEventArgs(T value, bool isExecuted, string message) : base(isExecuted, message)
        {
            Value = value;
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

        /// <summary>
        /// Initializes a new instance of the LinkedListEventArgs class, specifying the executing and message and containing the specified value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isExecuted"></param>
        /// <param name="message"></param>
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
        public event EventHandler<AddedLinkedListEventArgs<T>> Added;
        /// <summary>
        /// Occurs directly after Remove is called, and can be handled to get information about executed and the message about executing.
        /// </summary>
        public event EventHandler<RemovedLinkedListEventArgs<T>> Removed;
        /// <summary>
        /// Occurs directly after Clear is called, and can be handled to get information about executed and the message about executing.
        /// </summary>
        public event EventHandler<LinkedListEventArgs> Cleared;

        /// <summary>
        /// Adds a new node containing the specified value at the end of the LinkedList T .
        /// </summary>
        /// <param name="value">The value to add at the end of the LinkedList T .</param>
        public void Add(T value)
        {
            LinkedListNode<T> newNode = new LinkedListNode<T>(value, null, Last, this);

            if (null == First)
            {
                First = newNode;
                Count = 1;
            }
            else
            {
                Last.Next = newNode;
                Count++;
            }
            Last = newNode;
            Added?.Invoke(this, new AddedLinkedListEventArgs<T>(value, true, "The value was added into the LinkedList."));
        }

        /// <summary>
        /// Removes all nodes from the LinkedList T .
        /// </summary>
        public void Clear()
        {
            Count = 0;
            First = null;
            Last = null;
            Cleared?.Invoke(this, new LinkedListEventArgs(true, "The LinkedList was cleared."));
        }

        /// <summary>
        /// Determines whether a value is in the LinkedList T .
        /// </summary>
        /// <param name="value">The value to locate in the LinkedList T .</param>
        /// <returns>true if value is found in the LinkedList T ; otherwise, false.</returns>
        public bool Contains(T value)
        {
            foreach (var item in this)
            {
                if (Equals(item, value))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Copies the entire LinkedList T  to a compatible one-dimensional Array, starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">The one-dimensional Array that is the destination of the elements copied from LinkedList T .</param>
        /// <param name="index">The zero-based index in array at which copying begins.</param>
        public void CopyTo(T[] array, int index)
        {
            if (array == null) throw new ArgumentNullException("array", "A null reference is passed to a method.");
            if (index < 0) throw new ArgumentOutOfRangeException();
            if (array.Rank > 1 || (this.Count > array.Length - index)) throw new ArgumentException();

            foreach (var item in this)
            {
                array[index] = item;
                index++;
            }
        }

        /// <summary>
        /// Removes the first occurrence of the specified value from the LinkedList T .
        /// </summary>
        /// <param name="value">The value to remove from the LinkedList T .</param>
        /// <returns>true if the element containing value is successfully removed; otherwise, false.</returns>
        public bool Remove(T value)
        {
            LinkedListNode<T> matchingNode = FindAMatchingNode(value);

            if (matchingNode != null)
            {
                Count--;
                var prev = matchingNode.Previous;
                var next = matchingNode.Next;

                if (prev == null)
                    First = next;
                else
                    prev.Next = next;

                if (next == null)
                    Last = prev;
                else
                    next.Previous = prev;

                Removed?.Invoke(this, new RemovedLinkedListEventArgs<T>(value, true, "The passing element was deleted."));
                return true;
            }

            Removed?.Invoke(this, new RemovedLinkedListEventArgs<T>(value, false, "The passing element did not find. Removing is canceled."));
            return false;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the LinkedList T .
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            LinkedListNode<T> Node = First;
            while (Node != null)
            {
                yield return Node.Value;
                Node = Node.Next;
            }
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
        public bool Insert(T beforeValue, T value)
        {

            LinkedListNode<T> matchingNode = FindAMatchingNode(beforeValue);

            if (matchingNode != null)
            {
                LinkedListNode<T> newNode = new LinkedListNode<T>(value, matchingNode, matchingNode.Previous, this);

                var prev = matchingNode.Previous;

                if (prev == null)
                {
                    First.Previous = newNode;
                    First = newNode;

                }
                else
                {
                    prev.Next = newNode;
                    prev = newNode;
                }
                Count++;
                return true;
            }
            return false;
        }

        private LinkedListNode<T> FindAMatchingNode(T value)
        {
            LinkedListNode<T> Node = First;
            while (Node != null)
            {
                if (Equals(Node.Value, value))
                    return Node;
                Node = Node.Next;
            }
            return null;
        }
    } 


}


