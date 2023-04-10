
namespace IdktlCompiler;
internal class Program
{
    private static void Main()
    {
        var inputProgram = InputReader.Read();
        var myLexer = new Lexer(inputProgram);

        var tokenTuple = myLexer.GetNextToken();
        while (tokenTuple.Success && tokenTuple.Token != "")
        {
            Console.WriteLine($"{tokenTuple}");
            tokenTuple = myLexer.GetNextToken();
            System.Threading.Thread.Sleep(1000);
        }

        //foreach (var token in myLexer.GetNextToken())
        //{
        //    Console.WriteLine(token);
        //}
    }
}