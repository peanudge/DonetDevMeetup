using System.Text.RegularExpressions;

public class DomainLanguageExample
{
    public static void Run()
    {
        var mySimpleDomLang = @"
ADD 1 4.0
DIV 1 0
SUB 6 1.9
MUL 4.1 8
DIV 2 2.3
MOD 3.5 3
HLO 4 6
Z A A
";

        var matches = Regex.Matches(
            mySimpleDomLang,
            @"(?<Instruction>[^\s]+)\s+(?<Op1>[^\s]+)\s+(?<Op2>[^\s]+)",
            RegexOptions.Compiled);

        // 행을 나누는 Split 메서드를 쓰지 않아도 자동으로 그룹핑이 이루어집니다.
        //matches.Dump();

        for (var i = 0; i < matches.Count; i++)
        {
            var currentLine = matches[i].Groups.AsReadOnly();

            // Index 0은 전체 문자열을 반환합니다.
            //currentLine.Skip(1).Dump();

            var instruction = currentLine.ElementAtOrDefault(1)?.Value;
            var firstOp = currentLine.ElementAtOrDefault(2)?.Value;
            var secondOp = currentLine.ElementAtOrDefault(3)?.Value;

            switch (instruction)
            {
                case "ADD":
                    if (decimal.TryParse(firstOp, out decimal addArg1) &&
                        decimal.TryParse(secondOp, out decimal addArg2))
                        Console.Out.WriteLine(
                            $"{firstOp} + {secondOp} = {addArg1 + addArg2:#,##0.00}");
                    else
                        Console.Out.WriteLine($"Cannot process ADD instruction. (Line {i + 1})");
                    break;
                case "SUB":
                    if (decimal.TryParse(firstOp, out decimal subArg1) &&
                        decimal.TryParse(secondOp, out decimal subArg2))
                        Console.Out.WriteLine(
                            $"{firstOp} - {secondOp} = {subArg1 - subArg2:#,##0.00}");
                    else
                        Console.Out.WriteLine($"Cannot process SUB instruction. (Line {i + 1})");
                    break;
                case "MUL":
                    if (decimal.TryParse(firstOp, out decimal mulArg1) &&
                        decimal.TryParse(secondOp, out decimal mulArg2))
                        Console.Out.WriteLine(
                            $"{firstOp} * {secondOp} = {mulArg1 * mulArg2:#,##0.00}");
                    else
                        Console.Out.WriteLine($"Cannot process MUL instruction. (Line {i + 1})");
                    break;
                case "DIV":
                    if (decimal.TryParse(firstOp, out decimal divArg1) &&
                        decimal.TryParse(secondOp, out decimal divArg2) &&
                        divArg2 != decimal.Zero)
                        Console.Out.WriteLine(
                            $"{firstOp} / {secondOp} = {divArg1 / divArg2:#,##0.00}");
                    else
                        Console.Out.WriteLine($"Cannot process DIV instruction. (Line {i + 1})");
                    break;
                case "MOD":
                    if (decimal.TryParse(firstOp, out decimal modArg1) &&
                        decimal.TryParse(secondOp, out decimal modArg2))
                        Console.Out.WriteLine(
                            $"{firstOp} % {secondOp} = {modArg1 % modArg2:#,##0.00}");
                    else
                        Console.Out.WriteLine($"Cannot process MOD instruction. (Line {i + 1})");
                    break;
                default:
                    Console.Out.WriteLine($"Cannot process '{instruction}' instruction. (Line {i + 1})");
                    break;
            }
        }
    }
}
