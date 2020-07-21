using DiceNotation.Exceptions;
using DiceNotation.Modifiers;
using DiceNotation.Rollers;
using DiceNotation.Terms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiceNotation.Terms
{
    public class ModifiedDiceTerm : IDiceExpressionTerm
    {
        public int Multiplicity { get; private set; }
        public int Sides { get; private set; }
        public int Scalar { get; private set; }
        public IDieModifier Modifier { get; private set; } = null;

        public ModifiedDiceTerm(int multiplicity, int sides, int scalar, IDieModifier mod = null)
        {
            if (sides <= 0)
            {
                throw new ImpossibleDieException(sides);
            }
            if (multiplicity < 0)
            {
                throw new InvalidMultiplicityException(multiplicity);
            }

            Sides = sides;
            Multiplicity = multiplicity;
            Scalar = scalar;
            Modifier = mod;
        }

        public IEnumerable<TermResult> GetResults(IDieRoller dieRoller)
        {
            if (this.Modifier != null)
                return this.GetResults(dieRoller, this.Modifier);
            else
            {
                IEnumerable<TermResult> results =
                from i in Enumerable.Range(0, Multiplicity)
                select new TermResult
                {
                    Scalar = Scalar,
                    Value = dieRoller.RollDie(Sides),
                    Type = "d" + Sides
                };
                return results.OrderByDescending(d => d.Value);
            }
        }

        public IEnumerable<TermResult> GetResults(IDieRoller dieRoller, IDieModifier modifier)
        {
            IEnumerable<TermResult> results = modifier.ApplyModifier(dieRoller, this);
            return results.OrderByDescending(d => d.Value);
        }

        public override string ToString()
        {
            return Scalar == 1
                ? string.Format("{0}d{1}{2}", Multiplicity, Sides, Modifier)
                : string.Format("{0}*{1}d{2}{3}", Scalar, Multiplicity, Sides, Modifier);
        }
    }
}
