using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace UP_Laba6
{
    class StupMatrix
    {
        public List<int>[] stupMatrix;

        public StupMatrix(string fileName)
        {
            string[] strings = File.ReadAllLines(fileName);
            stupMatrix = new List<int>[strings.Count()];
            for (int i = 0; i < strings.Length; i++)
            {
                stupMatrix[i] = new List<int>();
                int[] nums = strings[i].Select(x => int.Parse(Convert.ToString(x))).ToArray();
                stupMatrix[i].AddRange(nums);
            }
        }

        public void Output()
        {
            for (int i = 0; i < stupMatrix.Length; i++)
            {
                foreach (var num in stupMatrix[i])
                {
                    Console.Write(num + "\t");
                }
                Console.WriteLine();
            }
        }
    }
}
