using NUnit.Framework;
using System;

namespace LinkedList.Test
{
    [TestFixture]
    public class LinkedListTest
    {
        private LinkedList<int> _linkedList;

        [SetUp]
        public void SetUp()
        {
            _linkedList = new LinkedList<int>();
        }

        [Test]
        public void NullTest()
        {
            var linkedList = new LinkedList<string>();
            linkedList.Add("Test");
            linkedList.Contains(null);
        }

        [Test]
        public void CountTest()
        {
            Assert.AreEqual(0, _linkedList.Count);

            for (int i = 0; i < 5; i++)
            {
                _linkedList.Add(i);
            }

            Assert.AreEqual(5, _linkedList.Count);
        }

        [Test]
        public void First()
        {
            Assert.AreEqual(0, _linkedList.Count);
            Assert.AreEqual(null, _linkedList.First);
            _linkedList.Add(5);
            _linkedList.Add(6);
            _linkedList.Add(7);
            Assert.AreEqual(5, _linkedList.First.Value);
        }

        [Test]
        public void Last()
        {
            Assert.AreEqual(0, _linkedList.Count);
            Assert.AreEqual(null, _linkedList.Last);
            _linkedList.Add(5);
            _linkedList.Add(6);
            _linkedList.Add(7);
            Assert.AreEqual(7, _linkedList.Last.Value);
        }

        [Test]
        public void IsReadOnlyTest()
        {
            Assert.AreEqual(_linkedList.IsReadOnly, false);
        }

        [Test]
        public void AddTest()
        {
            Assert.AreEqual(0, _linkedList.Count);
            for (int i = 0; i < 5; i++)
            {
               _linkedList.Add(i+2);
            }

            Assert.AreEqual(5, _linkedList.Count);

            LinkedListNode<int> tempNode = _linkedList.First;

            for (int i = 0; i < 5; i++)
            {
                Assert.AreEqual(i+2, tempNode.Value);
                tempNode = tempNode.Next;
            }

        }

        [Test]
        public void ClearTest()
        {

            Assert.AreEqual(0, _linkedList.Count);

            for (int i = 0; i < 5; i++)
            {
               _linkedList.Add(1);
            }

            Assert.AreEqual( 5, _linkedList.Count);

            _linkedList.Clear();

           Assert.AreEqual(0, _linkedList.Count);

        }

        [Test]
        public void ContainsTest()
        {
            Assert.AreEqual(false, _linkedList.Contains(10));
            _linkedList.Add(10);
            Assert.AreEqual(true, _linkedList.Contains(10));

            for (int i = 0; i < 5; i++)
            {
                _linkedList.Add(i);
            }

            for (int i = 0; i < 5; i++)
            {
                Assert.AreEqual(true, _linkedList.Contains(i));
            }
        }

        [Test]
        public void CopyToTest()
        {
            int[] array = new int[10];
            for (int i = 0; i < 2; i++)
            {
                array[i] = i;
            }

            for (int i = 2; i < 10; i++)
            {
                _linkedList.Add(i);
            }

            _linkedList.CopyTo(array, 2);

            for (int i = 0; i < 10; i++)
            {
                Assert.AreEqual(i,array.GetValue(i));
            }
        }

        [Test]
        public void RemoveTest()
        {
            for (int i = 0; i < 10; i++)
            {
                _linkedList.Add(i);
            }

            Assert.AreEqual(10, _linkedList.Count);

            for (int i = 0; i < 10; i++)
            {
                Assert.AreEqual(true,_linkedList.Contains(i));
            }

            for (int i = 0; i < 10; i++)
            {
               Assert.AreEqual(true,_linkedList.Remove(i));
            }

            Assert.AreEqual(0, _linkedList.Count);

            Assert.AreEqual(false, _linkedList.Remove(20));

        }

        [Test]
        public void GetEnumeratorTest()
        {
            string s = "";
            string trueS = "";
            for (int i = 0; i < 10; i++)
            {
                _linkedList.Add(i);
                s += i.ToString();
            }

            foreach (var item in _linkedList)
            {
                trueS += item.ToString();
            }

            Assert.AreEqual(trueS,s);
        }

        [Test]
        public void InsertTest()
        {
            Assert.AreEqual(0, _linkedList.Count);
            string s = "";
            string trueS = "";
            for (int i = 1; i < 6; i++)
            {
                _linkedList.Add(i);
                trueS +=(i*10).ToString()+" "+ i.ToString()+" ";
            }

            for (int i = 1; i < 6; i++)
            {
               _linkedList.Insert(i,i*10);
            }

            //LinkedListNode<int> tempNode = _linkedList.First;

            foreach (var item in _linkedList)
            {
                s += item.ToString() + " ";
            }


            Assert.AreEqual(trueS,s);
            Assert.AreEqual(10, _linkedList.Count);

            s = "";

            LinkedListNode<int> Node = _linkedList.First;

            while (Node != null)
            {
                s += Node.Value.ToString() + " ";
                Node = Node.Next;
            }

            Assert.AreEqual(trueS, s);

            s = "";

            Node = _linkedList.Last;

            while (Node != null)
            {
                s += Node.Value.ToString() + " ";
                Node = Node.Previous;
            }
            var trueSChars = trueS.Split(' ');
            Array.Reverse(trueSChars);
            trueS = string.Join(" ",trueSChars).Trim();
            Assert.AreEqual(trueS, s.Trim());
        }

        

        [Test]
        public void AddedTest()
        {
            int addedValueInt = 5;
            LinkedList<int> linkedListInt = new LinkedList<int>();
            linkedListInt.Add(addedValueInt);

            linkedListInt.Added += delegate (object sender, AddedLinkedListEventArgs<int> e)
            {
                if (ReferenceEquals(null, sender) || ReferenceEquals(null, e))
                {
                    Assert.Fail();
                }

                Assert.AreEqual(true, e.IsExecuted);
                Assert.AreEqual(addedValueInt, e.Value);

            };

            linkedListInt.Remove(addedValueInt);

            string addedValueString = "5";

            LinkedList<string> linkedListString = new LinkedList<string>();
            linkedListString.Add(addedValueString);

            linkedListString.Added += delegate (object sender, AddedLinkedListEventArgs<string> e)
            {
                if (ReferenceEquals(null, sender) || ReferenceEquals(null, e))
                {
                    Assert.Fail();
                }

                Assert.AreEqual(true, e.IsExecuted);
                Assert.AreEqual(addedValueInt, e.Value);

            };

            linkedListString.Remove(addedValueString);
        }

        [Test]
        public void RemovedTest()
        {
            int removedValueInt = 5;
            LinkedList<int> linkedListInt = new LinkedList<int>();
            linkedListInt.Add(removedValueInt);
        
            linkedListInt.Removed += delegate (object sender, RemovedLinkedListEventArgs<int> e)
            {
                if (ReferenceEquals(null, sender) || ReferenceEquals(null, e))
                {
                    Assert.Fail();
                }

                Assert.AreEqual(true, e.IsExecuted);
                Assert.AreEqual(removedValueInt, e.Value);

            };

            linkedListInt.Remove(removedValueInt);

            string removedValueString = "5";

            LinkedList<string> linkedListString = new LinkedList<string>();
            linkedListString.Add(removedValueString);

            linkedListString.Removed += delegate (object sender, RemovedLinkedListEventArgs<string> e)
            {
                if (ReferenceEquals(null, sender) || ReferenceEquals(null, e))
                {
                    Assert.Fail();
                }

                Assert.AreEqual(true, e.IsExecuted);
                Assert.AreEqual(removedValueString, e.Value);

            };

            linkedListString.Remove(removedValueString);
        }

        [Test]
        public void ClearedTest()
        {
            _linkedList.Cleared += delegate (object sender, LinkedListEventArgs e)
            {
                if (ReferenceEquals(null, sender) || ReferenceEquals(null, e))
                {
                    Assert.Fail();
                }
            };

            _linkedList.Clear();
        }

        

    }
}
