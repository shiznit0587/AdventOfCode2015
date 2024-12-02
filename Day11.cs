using System;
using System.Collections.Generic;

namespace AdventOfCode
{
    public class Day11
    {
        public Day11()
        {
            string input = "hepxcrrq";

            Console.WriteLine("Running Day 11 - a");

            char[] password = input.ToCharArray();
            do
            {
                incrementPassword(password);
            }
            while (!testPassword(password));

            Console.WriteLine("next password = " + String.Join("", password));

            Console.WriteLine("Running Day 11 - b");

            do
            {
                incrementPassword(password);
            }
            while (!testPassword(password));

            Console.WriteLine("next password = " + String.Join("", password));
        }

        private bool testPassword(char[] password)
        {
            // - Passwords may not contain the letters i, o, or l, as these letters can be mistaken for other characters and are therefore confusing.
            for (int i = 0; i < password.Length; ++i)
            {
                if (password[i] == 'i' ||
                    password[i] == 'o' ||
                    password[i] == 'l')
                {
                    return false;
                }
            }

            // - Passwords must include one increasing straight of at least three letters, like abc, bcd, cde, and so on, up to xyz. They cannot skip letters; abd doesn't count.
            bool straightFound = false;
            for (int i = 0; i < password.Length - 2; ++i)
            {
                if (password[i] + 1 == password[i + 1] &&
                    password[i + 1] + 1 == password[i + 2])
                {
                    straightFound = true;
                    break;
                }
            }

            if (!straightFound)
            {
                return false;
            }

            // - Passwords must contain at least two different, non-overlapping pairs of letters, like aa, bb, or zz.
            HashSet<char> pairChars = new HashSet<char>();
            int pairCount = 0;

            for (int i = 0; i < password.Length - 1; ++i)
            {
                if (password[i] == password[i + 1] && !pairChars.Contains(password[i]))
                {
                    ++pairCount;
                    ++i;
                    pairChars.Add(password[i]);
                }
            }

            if (pairCount < 2)
            {
                return false;
            }

            return true;
        }

        private void incrementPassword(char[] password)
        {
            int index = password.Length - 1;
            bool didRollOver = false;

            do
            {
                didRollOver = false;

                if (password[index] == 'z')
                {
                    password[index] = 'a';
                    --index;
                    didRollOver = true;
                }
                else
                {
                    ++password[index];
                }
            }
            while (didRollOver && index >= 0);
        }
    }
}
