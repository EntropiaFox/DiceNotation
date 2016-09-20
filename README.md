# DiceNotation #
**DiceNotation** is a simple parser / evaluator for a variant of "dice notation" similar to the version used in Dungeons and Dragons 3.5 that allows developers to support the use of dice notation in their own programs. It is written in C# and depends only on Portable Class Library functionality.

This is a fork of [EdCanHack](https://twitter.com/edropple) DiceNotation which is a fork of [Chris Wagner](https://github.com/cawagner)'s [DiceNotation](http://dicenotation.codeplex.com) library, maintained by [Ed Ropple](https://twitter.com/edropple).

# Installation #
DiceNotation is available [on NuGet](https://www.nuget.org/packages/m1tche11j.DiceNotation/0.3.0). To install, run the following command in the Package Manager Console:

```
PM> Install-Package DiceNotation.CoreClass
```

# Usage #
```csharp	
IDiceParser parser = new DiceParser();
DiceExpression dice = parser.Parse("3d6 + 12");

Int32 min = dice.Roll(new MinRoller); // 15
Int32 max = dice.Roll(new MaxRoller); // 30)

// This uses a singleton instance of System.Random across all rolls
// for all DiceExpressions; it's recommended that you instantiate an
// IDieRoller that preserves a given PRNG's seed and sequence.
Int32 ret = dice.Roll(); 
```

```csharp
// Equivalent to "5 + d8 + 4d6k3"
var expression = new DiceExpression().Constant(5).Die(8).Dice(4, 6, choose: 3);
```

# Dice Notation Examples #
| Expression | Meaning                                                    |
| ---------- | ---------------------------------------------------------- |
| `3d6`      | Roll three six-sided dice                                  |
| `4d6k3`    | Roll four six-sided dice, keep the three highest           |
| `2*2d8`    | Roll two eight-sided dice and multiply the result by two   |
| `5+d2`     | Roll a two-sided die (flip a coin) and add 5 to the result |