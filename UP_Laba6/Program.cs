using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

/// <summary>
/// Расположения узла относительно родителя
/// </summary>
public enum Side
{
    Left,
    Right
}

/// <summary>
/// Узел бинарного дерева
/// </summary>
/// <typeparam name="T"></typeparam>
public class BinaryTreeNode<T> where T : IComparable
{
    /// <summary>
    /// Конструктор класса
    /// </summary>
    /// <param name="data">Данные</param>
    public BinaryTreeNode(T data)
    {
        Data = data;
    }

    /// <summary>
    /// Данные которые хранятся в узле
    /// </summary>
    public T Data { get; set; }

    /// <summary>
    /// Левая ветка
    /// </summary>
    public BinaryTreeNode<T> LeftNode { get; set; }

    /// <summary>
    /// Правая ветка
    /// </summary>
    public BinaryTreeNode<T> RightNode { get; set; }

    /// <summary>
    /// Родитель
    /// </summary>
    public BinaryTreeNode<T> ParentNode { get; set; }

    /// <summary>
    /// Расположение узла относительно его родителя
    /// </summary>
    public Side? NodeSide =>
        ParentNode == null
        ? (Side?)null
        : ParentNode.LeftNode == this
            ? Side.Left
            : Side.Right;

    /// <summary>
    /// Преобразование экземпляра класса в строку
    /// </summary>
    /// <returns>Данные узла дерева</returns>
    public override string ToString() => Data.ToString();
}

/// <summary>
/// Бинарное дерево
/// </summary>
/// <typeparam name="T">Тип данных хранящихся в узлах</typeparam>
public class BinaryTree<T> where T : IComparable
{
    /// <summary>
    /// Корень бинарного дерева
    /// </summary>
    public BinaryTreeNode<T> RootNode { get; set; }

    /// <summary>
    /// Добавление нового узла в бинарное дерево
    /// </summary>
    /// <param name="node">Новый узел</param>
    /// <param name="currentNode">Текущий узел</param>
    /// <returns>Узел</returns>
    public BinaryTreeNode<T> Add(BinaryTreeNode<T> node, BinaryTreeNode<T> currentNode = null)
    {
        if (RootNode == null)
        {
            node.ParentNode = null;
            return RootNode = node;
        }

        currentNode = currentNode ?? RootNode;
        node.ParentNode = currentNode;
        int result;
        return (result = node.Data.CompareTo(currentNode.Data)) == 0
            ? currentNode
            : result < 0
                ? currentNode.LeftNode == null
                    ? (currentNode.LeftNode = node)
                    : Add(node, currentNode.LeftNode)
                : currentNode.RightNode == null
                    ? (currentNode.RightNode = node)
                    : Add(node, currentNode.RightNode);
    }

    /// <summary>
    /// Добавление данных в бинарное дерево
    /// </summary>
    /// <param name="data">Данные</param>
    /// <returns>Узел</returns>
    public BinaryTreeNode<T> Add(T data)
    {
        return Add(new BinaryTreeNode<T>(data));
    }

    /// <summary>
    /// Поиск узла по значению
    /// </summary>
    /// <param name="data">Искомое значение</param>
    /// <param name="startWithNode">Узел начала поиска</param>
    /// <returns>Найденный узел</returns>
    public BinaryTreeNode<T> FindNode(T data, BinaryTreeNode<T> startWithNode = null)
    {
        startWithNode = startWithNode ?? RootNode;
        int result;
        return (result = data.CompareTo(startWithNode.Data)) == 0
            ? startWithNode
            : result < 0
                ? startWithNode.LeftNode == null
                    ? null
                    : FindNode(data, startWithNode.LeftNode)
                : startWithNode.RightNode == null
                    ? null
                    : FindNode(data, startWithNode.RightNode);
    }

    /// <summary>
    /// Удаление узла бинарного дерева
    /// </summary>
    /// <param name="node">Узел для удаления</param>
    public void Remove(BinaryTreeNode<T> node)
    {
        if (node == null)
        {
            return;
        }

        var currentNodeSide = node.NodeSide;
        //если у узла нет подузлов, можно его удалить
        if (node.LeftNode == null && node.RightNode == null)
        {
            if (currentNodeSide == Side.Left)
            {
                node.ParentNode.LeftNode = null;
            }
            else
            {
                node.ParentNode.RightNode = null;
            }
        }
        //если нет левого, то правый ставим на место удаляемого 
        else if (node.LeftNode == null)
        {
            if (currentNodeSide == Side.Left)
            {
                node.ParentNode.LeftNode = node.RightNode;
            }
            else
            {
                node.ParentNode.RightNode = node.RightNode;
            }

            node.RightNode.ParentNode = node.ParentNode;
        }
        //если нет правого, то левый ставим на место удаляемого 
        else if (node.RightNode == null)
        {
            if (currentNodeSide == Side.Left)
            {
                node.ParentNode.LeftNode = node.LeftNode;
            }
            else
            {
                node.ParentNode.RightNode = node.LeftNode;
            }

            node.LeftNode.ParentNode = node.ParentNode;
        }
        //если оба дочерних присутствуют, 
        //то правый становится на место удаляемого,
        //а левый вставляется в правый
        else
        {
            switch (currentNodeSide)
            {
                case Side.Left:
                    node.ParentNode.LeftNode = node.RightNode;
                    node.RightNode.ParentNode = node.ParentNode;
                    Add(node.LeftNode, node.RightNode);
                    break;
                case Side.Right:
                    node.ParentNode.RightNode = node.RightNode;
                    node.RightNode.ParentNode = node.ParentNode;
                    Add(node.LeftNode, node.RightNode);
                    break;
                default:
                    var bufLeft = node.LeftNode;
                    var bufRightLeft = node.RightNode.LeftNode;
                    var bufRightRight = node.RightNode.RightNode;
                    node.Data = node.RightNode.Data;
                    node.RightNode = bufRightRight;
                    node.LeftNode = bufRightLeft;
                    Add(bufLeft, node);
                    break;
            }
        }
    }

    /// <summary>
    /// Удаление узла дерева
    /// </summary>
    /// <param name="data">Данные для удаления</param>
    public void Remove(T data)
    {
        var foundNode = FindNode(data);
        Remove(foundNode);
    }

    /// <summary>
    /// Вывод бинарного дерева
    /// </summary>
    public void PrintTree()
    {
        PrintTree(RootNode);
    }

    /// <summary>
    /// Вывод бинарного дерева начиная с указанного узла
    /// </summary>
    /// <param name="startNode">Узел с которого начинается печать</param>
    /// <param name="indent">Отступ</param>
    /// <param name="side">Сторона</param>
    private void PrintTree(BinaryTreeNode<T> startNode, string indent = "", Side? side = null)
    {
        if (startNode != null)
        {
            var nodeSide = side == null ? "+" : side == Side.Left ? "L" : "R";
            Console.WriteLine($"{indent} [{nodeSide}]- {startNode.Data}");
            indent += new string(' ', 3);
            //рекурсивный вызов для левой и правой веток
            PrintTree(startNode.LeftNode, indent, Side.Left);
            PrintTree(startNode.RightNode, indent, Side.Right);
        }
    }
}



namespace UP_Laba6
{
    class Program
    {
        static void Main(string[] args)
        {
            //Zadanie1();

            //Zadanie2();

            //Zadanie3();

            //Zadanie4();

            int[,] mas = new int[5, 5];
            Random random = new Random();

            for (int i = 0; i < mas.GetLength(0); i++)
            {
                for (int j = 0; j < mas.GetLength(1); j++)
                {
                    mas[i, j] = random.Next(-10, 11);
                }
            }

            int[,] masRez = new int[5, 5];

            int min = mas[0, 0];

            for (int i = 0; i < mas.GetLength(1); i++)
            {
                if (mas[0, i] < min)
                {
                    min = mas[0,i];
                }
            }

            int num=0,counter=0,numVcif;
            int[] masKolvoMin = new int[mas.GetLength(0)];
            for (int i = 0; i < mas.GetLength(0); i++)
            {
                for (int j = 0; j < mas.GetLength(1); j++)
                {
                    num = mas[i, j];
                    while (num>0)
                    {
                        numVcif = num % 10;
                        if (numVcif==min)
                        {
                            counter++;
                            break;
                        }
                    }
                }
                masKolvoMin[i] = counter;
                counter = 0;
            }



            Console.ReadLine();
        }

        private static void Zadanie4()
        {
            var binaryTree = new BinaryTree<int>();

            binaryTree.Add(8);
            binaryTree.Add(3);
            binaryTree.Add(10);
            binaryTree.Add(1);
            binaryTree.Add(6);
            binaryTree.Add(4);
            binaryTree.Add(7);
            binaryTree.Add(14);
            binaryTree.Add(16);

            binaryTree.PrintTree();

            Console.WriteLine(new string('-', 40));
            binaryTree.Remove(3);
            binaryTree.PrintTree();

            Console.WriteLine(new string('-', 40));
            binaryTree.Remove(8);
            binaryTree.PrintTree();
        }

        private static void Zadanie3()
        {
            Console.WriteLine("В дебаге файл, там изменяйте значения.");
            StupMatrix stupMatrix = new StupMatrix("stup" + ".txt");
            stupMatrix.Output();
            int proizvedenie = stupMatrix.stupMatrix.Where((rows, rowIndex) => rowIndex % 3 == 0).Select(x => x.Where(num => num % 2 != 0).Aggregate((ak, e) => ak * e)).Where(x => x % 2 != 0).Max();
            Console.WriteLine("Произведение:"+proizvedenie);
        }

        private static void Zadanie2()
        {
            Random random = new Random();
            int[,] matrix2 = new int[6, 6];
            Console.WriteLine("Матрица 6 на 6, введите занчения:");
            for (int i = 0; i < matrix2.GetLength(0); i++)
            {
                for (int j = 0; j < matrix2.GetLength(1); j++)
                {
                    matrix2[i, j] = random.Next(-10, 11);
                }
            }

            for (int i = 0; i < matrix2.GetLength(0); i++)
            {
                for (int j = 0; j < matrix2.GetLength(1); j++)
                {
                    Console.Write(matrix2[i, j] + "\t");
                }
                Console.WriteLine();
            }

            int sum = 0;
            foreach (var num in matrix2)
            {
                sum += num;
            }
            Console.WriteLine($"Сумма всех элементов массива {sum}");

            for (int i = 0; i < matrix2.GetLength(0); i++)
            {
                for (int j = 0; j < matrix2.GetLength(1); j++)
                {
                    if (i == j)
                    {
                        matrix2[i, j] = 1;
                    }
                }
            }

            Console.WriteLine("Замена главной диогонали на 1:");

            for (int i = 0; i < matrix2.GetLength(0); i++)
            {
                for (int j = 0; j < matrix2.GetLength(1); j++)
                {
                    Console.Write(matrix2[i, j] + "\t");
                }
                Console.WriteLine();
            }
        }

        private static void Zadanie1()
        {
            Console.WriteLine("Введите размерность:");
            int n = int.Parse(Console.ReadLine());
            int[] mas = new int[n];
            for (int i = 0; i < n; i++)
            {
                mas[i] = int.Parse(Console.ReadLine());
            }
            Console.WriteLine("Что найти:");
            int searchedValue = int.Parse(Console.ReadLine());
            Array.Sort(mas);
            Console.WriteLine(string.Join(" ",mas));
            Console.WriteLine(BinarySearch(mas, searchedValue, 0, mas.Length));
        }

        static int BinarySearch(int[] mas,int searchedValue,int leftBorder,int rightBorder)
        {
            if (leftBorder>rightBorder)
            {
                return -1;
            }

            var sredInd = leftBorder+(rightBorder-leftBorder) / 2;
            var midValue = mas[sredInd];

            if (midValue == searchedValue)
            {
                return midValue;
            }
            else
            {
                if (midValue>searchedValue)
                {
                    return BinarySearch(mas,searchedValue,leftBorder,midValue-1);
                }
                else
                {
                    return BinarySearch(mas, searchedValue, midValue+1, rightBorder);
                }
            }
        }
    }
}
