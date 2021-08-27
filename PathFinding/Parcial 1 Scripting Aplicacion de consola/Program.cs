using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial_1_Scripting_Aplicacion_de_consola
{
    class Program
    {
        
        static Stopwatch stopwatch = new Stopwatch();
        static void Main(string[] args)
        {
            stopwatch.Start();           
            SearchPath program = new SearchPath();
            program.CreateMaze();
            program.BFS();
            program.CreatePath();
            program.ImprimirLista();
            TimeSpan ts = stopwatch.Elapsed;
            Console.WriteLine("Tiempo Transcurrido: " + ts.ToString("mm\\:ss\\.ff"));
            Console.ReadKey();             
        }
    }
}
