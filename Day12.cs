using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace AdventOfCode
{
    public class Day12
    {
        int sum = 0;
        bool skipReds = false;

        public Day12()
        {
            Console.WriteLine("Running Day 12 - a");

            JObject jsonData = null;
            FileUtil.Parse(12, delegate(string input)
            {
                jsonData = Newtonsoft.Json.Linq.JObject.Parse(input);
            });

            walkObject(jsonData);

            Console.WriteLine("final sum = " + sum);

            Console.WriteLine("Running Day 12 - b");

            sum = 0;
            skipReds = true;
            walkObject(jsonData);

            Console.WriteLine("final sum = " + sum);
        }

        private void walkObject(JObject json)
        {
            int sumBackup = sum;
            foreach (KeyValuePair<string, JToken> child in json)
            {
                if (skipReds && child.Value.Type == JTokenType.String && child.Value.ToString() == "red")
                {
                    sum = sumBackup;
                    return;
                }

                determineAction(child.Value);
            }
        }

        private void walkArray(JArray json)
        {
            foreach (JToken token in json)
            {
                determineAction(token);
            }
        }

        private void determineAction(JToken token)
        {
            switch (token.Type)
            {
                case JTokenType.Object:
                    walkObject((JObject)token);
                    break;
                case JTokenType.Array:
                    walkArray((JArray)token);
                    break;
                case JTokenType.Integer:
                    sum += token.Value<Int32>();
                    break;
                default:
                    break;
            }
        }
    }
}
