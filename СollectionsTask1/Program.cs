using System.Collections;

namespace СollectionsTask1
{
    internal class Program
    {
        //      Stack<T> - стек LIFO(Last-In-First-Out)
        //      Реализует принцип "последним пришел - первым вышел".

        //      Пример
        //      // Использование стека
        //      var stack = new Stack<int>();
        //              stack.Push(1);
        //      stack.Push(2);
        //      stack.Push(3);

        //      int topItem = stack.Pop(); // 3
        //              int nextItem = stack.Peek(); // 2 (без удаления)

        //              // Типичный сценарий - отмена операций
        //              var actions = new Stack<Action>();
        //              actions.Push(() => Console.WriteLine("Undo action 1"));
        //      actions.Push(() => Console.WriteLine("Undo action 2"));

        //      // Выполнение отмены в обратном порядке
        //      while (actions.Count > 0)
        //      {
        //          Action undoAction = actions.Pop();
        //              undoAction.Invoke();
        //      }

        //      Основные операции:

        //      Push - добавление на вершину стека

        //      Pop - извлечение с вершины стека

        public class SmartStack<T> : IEnumerable<T>
        {
            private T[] _array;
            private int _arraySize;

            public SmartStack()
            {
                _array = new T[4];
            }

            public SmartStack(int arraySize)
            {
                _array = new T[arraySize];
                _arraySize = arraySize;
            }

            public SmartStack(IEnumerable<T> collections)
            {
                int arraySize = collections.Count();
                _array = new T[arraySize];
                _arraySize = arraySize;

                int index = arraySize-1;
                foreach (var element in collections)
                {
                    _array[index] = element;
                    index--;
                }
            }

            /// <summary>
            /// Добавляет элемент на вершину стека. (При нехватке места для добавления элемента, ёмкость массива удваивается)
            /// </summary>
            public void Push()
            { 
                
            }

            public IEnumerator<T> GetEnumerator()
            {
                foreach (var item in _array)
                {
                    yield return item;
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public T this[int index]
            {
                get
                {
                    if (index < 0 || index >= _arraySize)
                        throw new ArgumentOutOfRangeException(nameof(index));

                    return _array[index];
                }
            }
        }

        static void Main()
        {
            int[] arrayInt = [1, 2, 3, 4];
            SmartStack<int> testEmpty = new();
            SmartStack<int> testNumber = new(5);
            SmartStack<int> testCopy = new(arrayInt);

            Console.WriteLine(testCopy[0]);
        }
    }
}
