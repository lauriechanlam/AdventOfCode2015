using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Signal = System.Int32;

namespace AdventOfCode2015
{
    public class Problem07
    {

        public static void part1()
        {
            var instructions = Problem07.text().ToList();
            var sourcePerWire = Problem07.GetSourcePerWire(instructions);
            var signalPerWire = new Dictionary<string, Signal>();
            var signal = Problem07.GetSignal("a", ref sourcePerWire, ref signalPerWire);
            Console.WriteLine(signal);
        }

        public static void part2()
        {
            var instructions = Problem07.text().ToList();
            var sourcePerWire = Problem07.GetSourcePerWire(instructions);
            var signalPerWire = new Dictionary<string, Signal>();
            sourcePerWire.Remove("b");
            signalPerWire["b"] = 16076;
            var signal = Problem07.GetSignal("a", ref sourcePerWire, ref signalPerWire);
            Console.WriteLine(signal);
        }

        static String[] text()
        {
            return System.IO.File.ReadAllLines("resources/07.txt");
        }

        static Dictionary<string, string> GetSourcePerWire(List<string> instructions)
        {
            var regex = new Regex(@"^(?<source>.+) -> (?<destination>[a-z]+)$", RegexOptions.None);
            var sourcePerWire = new Dictionary<string, string>();
            instructions.ForEach(instruction => {
                var matches = regex.Matches(instruction);
                if (matches.Count() != 1)
                {
                    throw new Exception(String.Format("Wrong number of matches for instruction {0}", instruction));
                }
                var groups = matches[0].Groups;
                var wire = groups["destination"].Value;
                var source = groups["source"].Value;
                if (sourcePerWire.ContainsKey(wire)) {
                    throw new Exception(String.Format("Several entries for wire {0} ({1}, {2})", wire, sourcePerWire[wire], source));
                }
                sourcePerWire[wire] = source;
            });
            return sourcePerWire;
        }

        static Signal GetSignal(string wire, ref Dictionary<string, string> sourcePerWire, ref Dictionary<string, Signal> signalPerWire)
        {
            if (signalPerWire.ContainsKey(wire))
            {
                return signalPerWire[wire];
            }
            var source = sourcePerWire[wire];
            sourcePerWire.Remove(wire);
            Regex regex;
            MatchCollection matches;

            Signal Value(Group valueGroup, Group wireGroup, ref Dictionary<string, string> sourcePerWire, ref Dictionary<string, Signal> signalPerWire)
            {
                if (valueGroup.Value != "")
                {
                    return Signal.Parse(valueGroup.Value);
                }
                return GetSignal(wireGroup.Value, ref sourcePerWire, ref signalPerWire);
            }

            Signal SetSignal(string wire, Signal signal, ref Dictionary<string, Signal> signalPerWire)
            {
                if (signalPerWire.ContainsKey(wire))
                {
                    throw new Exception(String.Format("SignalPerWire already contains value {0} for wire {1}. Cannot insert value {2}", signalPerWire[wire], wire, signal));
                }
                signalPerWire[wire] = signal;
                return signal;
            }

            // BINARY OPERATORS
            regex = new Regex(@"^((?<lhs_value>\d+)|(?<lhs_wire>[a-z]+)) (?<operator>AND|OR|LSHIFT|RSHIFT) ((?<rhs_value>\d+)|(?<rhs_wire>[a-z]+))$", RegexOptions.None);
            matches = regex.Matches(source);
            if (matches.Count() == 1)
            {
                Signal value;
                var groups = matches[0].Groups;
                var op = groups["operator"].Value;
                var lhs = Value(groups["lhs_value"], groups["lhs_wire"], ref sourcePerWire, ref signalPerWire);
                var rhs = Value(groups["rhs_value"], groups["rhs_wire"], ref sourcePerWire, ref signalPerWire);
                switch (op)
                {
                    case "AND":
                        value = lhs & rhs;
                        break;
                    case "OR":
                        value = lhs | rhs;
                        break;
                    case "LSHIFT":
                        value = (lhs << rhs) & 0xffff;
                        break;
                    case "RSHIFT": 
                        value = lhs >> rhs;
                        break;
                    default:
                        throw new Exception(String.Format("Unknown binary operatior {0}", op));
                }
                return SetSignal(wire, value, ref signalPerWire);
            }

            // UNARY OPERATOR
            regex = new Regex(@"^NOT ((?<value>\d+)|(?<wire>[a-z]+))$", RegexOptions.None);
            matches = regex.Matches(source);
            if (matches.Count() == 1)
            {
                var groups = matches[0].Groups;
                var value = (~Value(groups["value"], groups["wire"], ref sourcePerWire, ref signalPerWire)) & 0xffff;
                return SetSignal(wire, value, ref signalPerWire);
            }

            // ASSIGNMENT
            regex = new Regex(@"^(?<value>\d+)|(?<wire>[a-z]+)$", RegexOptions.None);
            matches = regex.Matches(source);
            if (matches.Count() == 1)
            {
                var groups = matches[0].Groups;
                var value = Value(groups["value"], groups["wire"], ref sourcePerWire, ref signalPerWire);
                return SetSignal(wire, value, ref signalPerWire);
            }

            throw new Exception(String.Format("Unknown instruction {0} for wire {1}", source, wire));
        }
    }
}
