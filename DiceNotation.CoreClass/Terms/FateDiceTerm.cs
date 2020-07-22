using DiceNotation.Exceptions;
using DiceNotation.Rollers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiceNotation.Terms
{
    class FateDiceTerm : IDiceExpressionTerm
    {
        public int Multiplicity { get; private set; }
        public int Sides { get; private set; }
        public int Scalar { get; private set; }

        public FateDiceTerm(int multiplicity, int scalar)
        {
            if (multiplicity < 0)
            {
                throw new InvalidMultiplicityException(multiplicity);
            }

            Sides = 6;
            Multiplicity = multiplicity;
            Scalar = scalar;
        }

        public IEnumerable<TermResult> GetResults(IDieRoller dieRoller)
        {
            IEnumerable<TermResult> results =
                from i in Enumerable.Range(0, Multiplicity)
                select new TermResult
                {
                    Scalar = Scalar,
                    Value = dieRoller.RollDie(Sides)%3-1,
                    Type = "dF"
                };
            return results.OrderByDescending(d => d.Value);
        }

        public override string ToString()
        {
            return Scalar == 1
                ? string.Format("{0}dF", Multiplicity)
                : string.Format("{0}*{1}dF", Scalar, Multiplicity);
        }
    }
}
