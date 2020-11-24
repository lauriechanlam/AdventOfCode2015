using System;
using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AdventOfCode2015
{
    public class Problem12
    {
        public static void part1()
        {
            var text = Problem12.text();
            
            int Visit(JToken json)
            {
                switch (json.Type)
                {
                    case JTokenType.Array:
                        return json.Aggregate(0, (sum, token) => sum + Visit(token));
                    case JTokenType.Object:
                        return json.Aggregate(0, (sum, token) => sum + Visit(token));
                    case JTokenType.Property:
                        return Visit(json.ToObject<JProperty>().Value);
                    case JTokenType.Integer:
                        return json.ToObject<int>();
                }
                return 0;
            }

            Console.WriteLine(Visit(JToken.Parse(text)));
        }

        public static void part2()
        {
            Boolean IsRedProperty(JToken token)
            {
                if (token.Type != JTokenType.Property)
                {
                    return false;
                }
                var value = token.ToObject<JProperty>().Value;
                if (value.Type != JTokenType.String)
                {
                    return false;
                }
                return value.ToObject<string>() == "red";
            }

            int Visit(JToken json)
            {
                switch (json.Type)
                {
                    case JTokenType.Array:
                        return json.Aggregate(0, (sum, token) => sum + Visit(token));
                    case JTokenType.Object:
                        {
                            var isRed = false;
                            var sum = json.Aggregate(0, (sum, token) => {
                                isRed |= IsRedProperty(token);
                                return sum + Visit(token);
                            });
                            return isRed ? 0 : sum;
                        }
                    case JTokenType.Property:
                        return Visit(json.ToObject<JProperty>().Value);
                    case JTokenType.Integer:
                        return json.ToObject<int>();
                }
                return 0;
            }

            Console.WriteLine(Visit(JToken.Parse(text())));
        }

        static String text()
        {
            return System.IO.File.ReadAllText("resources/12.txt");
        }
    }
}
