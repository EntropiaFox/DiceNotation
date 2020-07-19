# DiceNotation #
**DiceNotation** is a simple parser / evaluator for a variant of "dice notation" similar to the version used in Dungeons and Dragons 3.5 that allows developers to support the use of dice notation in their own programs. It is written in C# and depends only on Portable Class Library functionality. In the future, it should be possible for the library to understand different dialects of dice notation.

In addition, **DiceNotation.MathNet** uses the awesome [MathNet.Numerics](http://numerics.mathdotnet.com) API to support pluggable random number generators; it too is PCL.

This is a fork of [Ed Ropple](https://twitter.com/edropple)'s DiceNotation, which is in turn a fork of [Chris Wagner](https://github.com/cawagner)'s [DiceNotation](http://dicenotation.codeplex.com) library, now maintained by [EntropiaFox](https://twitter.com/NeutrinoX).

# Installation #
(Pending, there is no NuGet package for this new version yet)

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

# Future Work #
- Results of dice rolls could be used as parameters to dice rolls (scalar, multiplicity, or choose).
- The distributive property (`3 * (2 + d6)`) could be supported.
- Better error handling on malformed expressions.
- Function parity with [Roll20](https://roll20.net/)'s [dice parser](https://roll20.zendesk.com/hc/en-us/articles/360037773133-Dice-Reference)