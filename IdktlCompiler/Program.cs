
namespace IdktlCompiler;
internal class Program
{
    private static void Main()
    {
        var inputProgram = InputReader.Read();
        var myLexer = new Lexer(inputProgram);

        foreach (var token in myLexer.GetTokenIterator()) {
            Console.WriteLine(token);
        }
    }
}