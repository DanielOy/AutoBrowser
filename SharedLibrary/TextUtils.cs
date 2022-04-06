using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public class TextUtils
    {
        public static int GetLevenshteinDistance(string Cadena1, string Cadena2)
        {
            int Length1 = Cadena1.Length;
            int Length2 = Cadena2.Length;
            int[,] distance = new int[Length1 + 1, Length2 + 1];

            // Step 1
            if (Length1 == 0) { return Length2; }
            else if (Length2 == 0) { return Length1; }

            // Step 2
            for (int i = 0; i <= Length1; distance[i, 0] = i++) { }

            for (int j = 0; j <= Length2; distance[0, j] = j++) { }

            // Step 3
            for (int i = 1; i <= Length1; i++)
            {
                //Step 4
                for (int j = 1; j <= Length2; j++)
                {
                    // Step 5
                    int cost = (Cadena2[j - 1] == Cadena1[i - 1]) ? 0 : 1;

                    // Step 6
                    distance[i, j] = System.Math.Min(
                        System.Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1),
                        distance[i - 1, j - 1] + cost);
                }
            }
            // Step 7
            return distance[Length1, Length2];
        }
    }
}
