using DiceNotation.Modifiers;
using DiceNotation.Rollers;
using DiceNotation.Terms;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiceNotation.Modifiers
{
    class ExplodingDiceModifier : IDieModifier
    {
        public CompareOperation Operator { get; private set; }
        public int? Value { get; private set; }

        public ExplodingDiceModifier(int? value = null, CompareOperation op = CompareOperation.Equals)
        {
            this.Operator = op;
            this.Value = value;
        }

        public IEnumerable<TermResult> ApplyModifier(IDieRoller roller, ModifiedDiceTerm term)
        {
            var times = term.Multiplicity;
            var result = new List<TermResult>();
            for (int i = 0; i < times; i++)
            {
                var resultEntry = new TermResult
                {
                    Scalar = term.Scalar,
                    Value = roller.RollDie(term.Sides),
                    Type = "d" + term.Sides
                };
                if (resultEntry.Value == term.Sides)
                    times++;
                result.Add(resultEntry);
            }
            return result;
        }
    }
}
