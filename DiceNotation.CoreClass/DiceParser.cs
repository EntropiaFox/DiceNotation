using DiceNotation.Modifiers;
using System;
using System.Text.RegularExpressions;

namespace DiceNotation
{
    public class DiceParser : IDiceParser
    {
        private readonly Regex _whitespacePattern;

        public DiceParser()
        {
            _whitespacePattern = new Regex(@"\s+");
        }

        public DiceExpression Parse(string expression)
        {
            string cleanExpression = _whitespacePattern.Replace(expression, "");
            cleanExpression = cleanExpression.Replace("+-", "-");

            var parseValues = new ParseValues().Init();

            var dice = new DiceExpression();

            for (int i = 0; i < cleanExpression.Length; ++i)
            {
                char c = cleanExpression[i];

                if (char.IsDigit(c))
                {
                    parseValues.Constant += c;
                }
                else if (c == '*')
                {
                    parseValues.Scalar *= int.Parse(parseValues.Constant);
                    parseValues.Constant = "";
                }
                else if (c == 'd')
                {
                    if (parseValues.Constant == "")
                        parseValues.Constant = "1";
                    //Special case: FATE dice use capital F as they always have six sides
                    if (i + 1 < cleanExpression.Length && (cleanExpression[i+1] == 'F'))
                    {
                        parseValues.Multiplicity = int.Parse(parseValues.Constant);
                        parseValues.IsFate = true;
                        ++i;
                    }
                    parseValues.Multiplicity = int.Parse(parseValues.Constant);
                }
                else if (c == 'k')
                {
                    string chooseAccum = "";
                    while (i + 1 < cleanExpression.Length && char.IsDigit(cleanExpression[i + 1]))
                    {
                        chooseAccum += cleanExpression[i + 1];
                        ++i;
                    }
                    parseValues.Choose = int.Parse(chooseAccum);
                }
                else if (c == '!')
                {
                    int? parsedValue = null;
                    char? op = null;
                    //Is there a modifier already? Throw an exception
                    if (parseValues.Modifier != null)
                        throw new ArgumentException("Too many modifiers in the dice expression", "expression");

                    //Start testing if there are any more optional values to parse for the modifier

                    //TODO: Fix parameter parsing, let's work with normal exploding die case for now

                    //string operatorAccum = "";
                    //while(i + 1 < cleanExpression.Length && (char.IsDigit(cleanExpression[i + 1]) || Enum.IsDefined(typeof(CompareOperation), Char.GetNumericValue(cleanExpression[i + 1]))))
                    //{
                    //    if (!char.IsDigit(cleanExpression[i + 1]))
                    //        if (Enum.IsDefined(typeof(CompareOperation), Char.GetNumericValue(cleanExpression[i + 1])) && op == null)
                    //            op = cleanExpression[i + 1];
                    //        else
                    //            throw new ArgumentException("Invalid character in dice expression", "expression");
                    //    else
                    //        operatorAccum += cleanExpression[i + 1];
                    //    ++i;
                    //    parsedValue = int.Parse(operatorAccum);
                    //}
                    //if(op != null)
                    //    parseValues.Modifier = new ExplodingDiceModifier(parsedValue, (CompareOperation)op);
                    //else
                    //    parseValues.Modifier = new ExplodingDiceModifier(parsedValue);

                    parseValues.Modifier = new ExplodingDiceModifier();
                }
                else if (c == '+')
                {
                    Append(dice, parseValues);
                    parseValues = new ParseValues().Init();
                }
                else if (c == '-')
                {
                    Append(dice, parseValues);
                    parseValues = new ParseValues().Init();
                    parseValues.Scalar = -1;
                }
                else
                {
                    throw new ArgumentException("Invalid character in dice expression", "expression");
                }
            }
            Append(dice, parseValues);

            return dice;
        }

        private static void Append(DiceExpression dice, ParseValues parseValues)
        {
            int constant = int.Parse(parseValues.Constant);
            if (parseValues.Multiplicity == 0)
            {
                dice.Constant(parseValues.Scalar*constant);
            }
            else if(parseValues.IsFate)
            {
                dice.FateDice(parseValues.Multiplicity, parseValues.Scalar);
                //TODO: Throw an exception if constant has a value, as it's not a valid dice term
            }
            else if (parseValues.Modifier != null)
            {
                dice.Dice(parseValues.Multiplicity, constant, parseValues.Scalar, parseValues.Modifier);
            }
            else
            {
                dice.Dice(parseValues.Multiplicity, constant, parseValues.Scalar, parseValues.Choose);
            }
        }

        private struct ParseValues
        {
            public string Constant;
            public int Scalar;
            public int Multiplicity;
            public int? Choose;
            public IDieModifier Modifier;
            public CompareOperation? ModifierOperator;
            public int? ModifierValue;
            public Boolean IsFate;

            public ParseValues Init()
            {
                Scalar = 1;
                Constant = "";
                Modifier = null;
                ModifierOperator = null;
                IsFate = false;
                return this;
            }
        }
    }
}
