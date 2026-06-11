using System.Collections;

namespace СollectionsTask1
{
    internal class СollectionsTask1
    {
        public class SmartStack<T> : IEnumerable<T>
        {
            private T[] _array;
            private int _count; //Количество элементов в стеке.

            public int Count => _count;
            public int Capacity => _array.Length; //Длина внутреннего массива.


            public SmartStack() : this(4) { }

            public SmartStack(int capacity)
            {
                ArgumentOutOfRangeException.ThrowIfNegative(capacity);

                _array = new T[capacity];
                _count = 0;
            }

            public SmartStack(IEnumerable<T> collection)
            {
                if (!collection.Any())
                    throw new ArgumentNullException(nameof(collection));

                _count = collection.Count();
                _array = new T[_count];

                int index = 0;
                foreach (var element in collection)
                {
                    _array[index++] = element;
                }
            }

            /// <summary>
            /// Добавляет элемент на вершину стека. (При нехватке места для добавления элемента, ёмкость массива удваивается).
            /// </summary>
            /// <param name="item">элемент.</param>
            public void Push(T item)
            {
                if (Count == Capacity)
                {
                    var newArray = new T[Capacity * 2];
                    _array.CopyTo(newArray, 0);
                    _array = newArray;
                }

                _array[_count++] = item;
            }

            /// <summary>
            /// Добавляет на вершину стека содержимое коллекции, реализующей интерфейс <see cref="IEnumerable{T}"/>.
            /// </summary>
            /// <param name="collection">коллекция, реализующая интерфейс <see cref="IEnumerable{T}"/>.</param>
            public void PushRange(IEnumerable<T> collection)
            {
                var collectionArray = collection.ToArray();

                for (int i = collectionArray.Length - 1; i >= 0; i--)
                {
                    Push(collectionArray[i]);
                }
            }

            /// <summary>
            /// Удаляет и возвращает элемент с вершины стека. (Реальная ёмкость массива не уменьшаться при удалении).
            /// </summary>
            public T Pop()
            {
                if (_count == 0)
                    throw new InvalidOperationException("Стек пуст");

                T item = _array[_count - 1];
                _array[_count - 1] = default!; //Очистка ссылки для больших объектов (на всякий случай сделал)
                _count--;

                return item;
            }

            /// <summary>
            /// Возвращает элемент с вершины стека без его удаления.
            /// </summary>
            public T Peek()
            {
                if (_count == 0)
                    throw new InvalidOperationException("Стек пуст");

                return _array[_count - 1];
            }

            public bool Contains(T item)
            {
                for (int i = _count - 1; i >= 0; i--)
                {
                    if (item!.Equals(_array[i]))
                        return true;
                }

                return false;
            }

            public IEnumerator<T> GetEnumerator()
            {
                for (int i = _count - 1; i >= 0; i--)
                    yield return _array[i];
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            /// <summary>
            /// Индексатор, позволяющий работать с элементом по глубине (0 - вершина стека, _count - 1 - index - основание).
            /// </summary>
            public T this[int index]
            {
                get
                {
                    if (index < 0 || index >= Capacity)
                        throw new ArgumentOutOfRangeException(nameof(index));

                    return _array[_count - 1 - index];
                }
            }
        }

        static void Main()
        {
            //1.
            string text = "Пустой стек";
            Console.WriteLine("1.");
            Console.WriteLine($"========{text}========");

            SmartStack<int> testEmpty = new();
            foreach (var i in testEmpty)
                Console.WriteLine(i);

            Console.WriteLine($"\nВ нынешнем стеке количество элементов равно: {testEmpty.Count}");
            Console.WriteLine($"В нынешнем стеке длина внутреннего массива равна: {testEmpty.Capacity}");

            Console.WriteLine($"\n========{text}========\n");

            //2.
            text = "Объявление стека через массив";
            Console.WriteLine("2.");
            Console.WriteLine($"========{text}========\n");

            int[] arrayInt = [1, 2, 3, 4, 5, 6, 7];

            Console.WriteLine($"Иммеющийся массив для копирования [{string.Join(", ", arrayInt)}]");

            SmartStack<int> testCopy = new(arrayInt);
            Console.WriteLine($"\nСтек после объявления при помощи массива:");
            foreach (var item in testCopy)
                Console.WriteLine(item);

            Console.WriteLine($"\nВ нынешнем стеке количество элементов равно: {testCopy.Count}");
            Console.WriteLine($"В нынешнем стеке длина внутреннего массива равна: {testCopy.Capacity}");

            Console.WriteLine($"\n========{text}========\n");

            //3.
            text = "Пустой массив int длинной 5";
            Console.WriteLine("3.");
            Console.WriteLine($"========{text}========\n");

            SmartStack<int> testNumber = new(5);
            Console.WriteLine($"В нынешнем стеке количество элементов равно: {testNumber.Count}");
            Console.WriteLine($"В нынешнем стеке длина внутреннего массива равна: {testNumber.Capacity}");

            Console.WriteLine($"\nПроведём Push чисел 10, 9, 8 ");
            testNumber.Push(10);
            testNumber.Push(9);
            testNumber.Push(8);

            Console.WriteLine($"\nСтек после Push-a:");
            foreach (var item in testNumber)
                Console.WriteLine(item);

            Console.WriteLine($"\nПроведём Push чисел массива [{string.Join(", ", arrayInt)}]");
            testNumber.PushRange(arrayInt);

            Console.WriteLine($"\nСтек после Push-a массива:");
            foreach (var item in testNumber)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine($"\nВ нынешнем стеке количество элементов равно: {testNumber.Count}");
            Console.WriteLine($"В нынешнем стеке длина внутреннего массива равна: {testNumber.Capacity}");

            Console.WriteLine($"\nВызов Peek: {testNumber.Peek()}");
            Console.WriteLine($"Вызов Pop: {testNumber.Pop()}");
            Console.WriteLine($"Вызов Peek: {testNumber.Peek()}");

            Console.WriteLine($"\nСтек после Pop, Peek:");
            foreach (var item in testNumber)
            {
                Console.WriteLine(item);
            }

            int checkNumber = 1;
            Console.WriteLine($"\nПроверим существует ли в стеке {checkNumber}: {testNumber.Contains(checkNumber)}");
            Console.WriteLine($"В нынешнем стеке количество элементов равно: {testNumber.Count}");
            Console.WriteLine($"В нынешнем стеке длина внутреннего массива равна: {testNumber.Capacity}");

            Console.WriteLine($"\n========{text}========\n");
        }
    }
}
