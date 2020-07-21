using DiceNotation.Rollers;
using DiceNotation.Terms;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiceNotation.Modifiers
{
    public interface IDieModifier
    {
        public IEnumerable<TermResult> ApplyModifier(IDieRoller roller, ModifiedDiceTerm term);
    }
}
