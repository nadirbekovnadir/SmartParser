using SmartParser.Domain.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParser.Domain.Services
{
    public enum ParserTokens
    {
        LB = 1,
        RB = 0,
        AND = 4,
        OR = 3,
        OPERAND = 5,
    }

    public class ParserService : IParserService
    {
        private static readonly Dictionary<char, ParserTokens> stringToToken = new Dictionary<char, ParserTokens>
        {
            {')', ParserTokens.RB },
            {'(', ParserTokens.LB },
            {'#', ParserTokens.AND },
            {'@',  ParserTokens.OR }
        };

        private List<ParserTokens> expression = new List<ParserTokens>();

        public List<string> Decompose(string expr)
        {
            expression.Clear();
            var tokens = SplitStringToTokens(expr);

            expression.AddRange(
                tokens
                    .Select(t =>
                        stringToToken.TryGetValue(t[0], out _) ?
                            stringToToken.GetValueOrDefault(t[0]) :
                            ParserTokens.OPERAND));

            return tokens
                .Where(t =>
                    t.Length > 1 ||
                    (t.Length == 1 && !stringToToken.ContainsKey(t[0])))
                .ToList();
        }

        private List<string> SplitStringToTokens(string expr)
        {
            List<string> tokens = new List<string>();
            var splitters = stringToToken.Keys.ToArray();

            int lastIndex = 0;
            while (true)
            {
                int index = expr.IndexOfAny(splitters, lastIndex);
                if (index == -1)
                {
                    tokens.Add(
                        expr.Substring(lastIndex, expr.Length - lastIndex).Trim());
                    break;
                }

                tokens.Add(
                    expr.Substring(lastIndex, index - lastIndex).Trim());
                tokens.Add(
                    expr.Substring(index, 1).Trim());

                lastIndex = index + 1;
            }

            tokens = tokens.Where(t => t != string.Empty).ToList();

            return tokens;
        }

        public bool Compute(List<bool> values)
        {
            if (values.Count != expression.Count(t => t == ParserTokens.OPERAND))
                throw new ArgumentException("Values size is not correct");

            var postfix = ToPostfix();
            bool result = SolvePostFix(postfix, values);

            return result;
        }

        private Queue<ParserTokens> ToPostfix()
        {
            Queue<ParserTokens> postfix = new Queue<ParserTokens>();
            Stack<ParserTokens> operators = new Stack<ParserTokens>();

            foreach (var t in expression)
            {
                if (t == ParserTokens.OPERAND)
                {
                    postfix.Enqueue(t);
                    continue;
                }

                int priority = (int)t;
                while (
                    operators.Count > 0 &&
                    (int)operators.Peek() >= priority &&
                    t != ParserTokens.LB)
                {
                    var op = operators.Pop();

                    if (op == ParserTokens.LB)
                        continue;

                    postfix.Enqueue(op);
                }

                if (t == ParserTokens.RB)
                    continue;

                operators.Push(t);
            }

            while (operators.Count > 0)
            {
                var op = operators.Pop();

                if (op == ParserTokens.LB)
                    continue;

                postfix.Enqueue(op);
            }

            return postfix;
        }

        private bool SolvePostFix(Queue<ParserTokens> postfix, List<bool> values)
        {
            Stack<bool> valStack = new Stack<bool>();
            int valuesPos = 0;
            while (postfix.Count > 0)
            {
                var t = postfix.Dequeue();

                if (t == ParserTokens.OPERAND)
                {
                    valStack.Push(values[valuesPos]);
                    valuesPos++;
                    continue;
                }

                var secondOp = valStack.Pop();
                var firstOp = valStack.Pop();

                switch (t)
                {
                    case ParserTokens.AND:
                        valStack.Push(firstOp & secondOp);
                        break;
                    case ParserTokens.OR:
                        valStack.Push(firstOp | secondOp);
                        break;
                    default:
                        break;
                }
            }

            return valStack.Pop();
        }
    }
}
