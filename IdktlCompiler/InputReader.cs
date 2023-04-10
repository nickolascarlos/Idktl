using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdktlCompiler
{
    internal class InputReader
    {
        public static String Read()
        {
            String InputProgram = new String("");

            while (true)
            {
                String? Chunk = Console.ReadLine();
                if (Chunk == null) break;
                InputProgram += Chunk + "\n";
            }

            return InputProgram;
        }
    }
}
