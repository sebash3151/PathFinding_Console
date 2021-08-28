using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial_1_Scripting_Aplicacion_de_consola
{
    public struct Vector2
    {
        public int x, y;

        public Vector2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector2 suma(Vector2 sumadoA)
        {
            int xsumada = x + sumadoA.x;
            int ysumada = y + sumadoA.y;
            Vector2 solucion = new Vector2(xsumada, ysumada);
            return solucion;
        }

        public override string ToString()
        {
            return ("(" + x.ToString() + " , " + y.ToString() + ")");
        }
    }
    public class Node
    {
        public bool explored = false;
        public Node exploredFrom;
        public Vector2 ubicacion;

        public Node(int x, int y)
        {
            ubicacion = new Vector2(x, y);
        }

        public Vector2 GetPosition()
        {
            return ubicacion;
        }

        public override string ToString()
        {
            return ("explorado: " + explored + "ubicacion: " + ubicacion);
        }
    }

    public class SearchPath
    {
        public SearchPath() { }
        //Inicio y Final del Camino
        private Node startPoint;
        private Node endPoint;

        //Hueco
        Vector2 heuco1 = new Vector2();
        int equis, ye;

        //Direcciones posibles a recorrer
        private Vector2[] directions = { new Vector2(0, 1), new Vector2(0, -1), new Vector2(1, 0), new Vector2(-1, 0) };

        //Se esta explorando y que esta explorando
        private bool isExploring = true;
        private Node searchingPoint;

        //Cola
        private Queue<Node> queue = new Queue<Node>();

        //Lista y Diccionarios
        private List<Node> path = new List<Node>();
        private Dictionary<Vector2, Node> diccionario = new Dictionary<Vector2, Node>();
        private Dictionary<Vector2, int> huecos = new Dictionary<Vector2, int>();
        private Dictionary<Vector2, int> inifina = new Dictionary<Vector2, int>();
        private Dictionary<Vector2, int> caminito = new Dictionary<Vector2, int>();

        //Tamaño del laberinto
        private int tamaño;

        //Valores iniciales del laberinto
        int xinicio = 0;
        int yinicio = 0;
        int xfinal = 0;
        int yfinal = 0;

        public void CreateMaze()
        {
            Console.WriteLine("De cuanto por cuanto desea tu laberinto");
            tamaño = int.Parse(Console.ReadLine());

            for (int i = 0; i < tamaño; i++)
            {
                for (int j = 0; j < tamaño; j++)
                {
                    Vector2 vectorcreated = new Vector2(i, j);
                    Node nodecreated = new Node(i, j);
                    diccionario.Add(vectorcreated, nodecreated);
                }
            }

            do
            {
                Console.WriteLine("Escriba la coordenada X del inicio: ");
                xinicio = int.Parse(Console.ReadLine());
                Console.WriteLine("Escriba la coordenada Y del inicio: ");
                yinicio = int.Parse(Console.ReadLine());
                Console.WriteLine("Escriba la coordenada X del final: ");
                xfinal = int.Parse(Console.ReadLine());
                Console.WriteLine("Escriba la coordenada Y del final: ");
                yfinal = int.Parse(Console.ReadLine());

                if(xinicio < 0 || xfinal < 0 || yinicio < 0 || yfinal < 0 || xinicio > tamaño || yinicio > tamaño || xfinal > tamaño || xfinal > tamaño)
                {
                    Console.WriteLine("Escriba valores dentro del limite establecido");
                }
                if (xinicio == yinicio && xfinal == yinicio)
                {
                    Console.WriteLine("El valor del inicio y el final son iguales, escriba valores adecuados");
                }

            } while ((xinicio < 0 || xfinal < 0 || yinicio < 0 || yfinal < 0||xinicio>tamaño || yinicio > tamaño || xfinal > tamaño || xfinal > tamaño)||(xinicio == yinicio && xfinal == yinicio));
                      
            Vector2 inicio = new Vector2(xinicio, yinicio);
            Vector2 final = new Vector2(xfinal, yfinal);
            inifina.Add(inicio, 1);
            inifina.Add(final, 1);
            startPoint = diccionario[inicio];
            endPoint = diccionario[final];

            Console.WriteLine("Cantidad de huecos que desea colocar");
            int cantidadDeHuecos = int.Parse(Console.ReadLine());

            for (int i = 1; i <= cantidadDeHuecos; i++)
            {
                do
                {
                    Console.WriteLine("Escriba la coordenada X del hueco: " + i);
                    equis = int.Parse(Console.ReadLine());
                    Console.WriteLine("Escriba la coordenada Y del hueco: " + i);
                    ye = int.Parse(Console.ReadLine());

                    heuco1 = new Vector2(equis, ye);

                    if (equis < 0 || ye < 0 || equis > tamaño || ye > tamaño)
                    {
                        Console.WriteLine("Escriba valores dentro del limite establecido");
                    }
                    else if (inifina.ContainsKey(heuco1))
                    {
                        Console.WriteLine("Ingreso un hueco en una ubicacion en el espacio del inicio o del final, porfavor ingrese un valor valido");
                    }
                    else if (diccionario.ContainsKey(heuco1))
                    {
                        diccionario.Remove(heuco1);
                        huecos.Add(heuco1, 1);
                    }
                } while (inifina.ContainsKey(heuco1) || equis < 0 || ye < 0 || equis > tamaño || ye > tamaño);
            }
            Console.WriteLine("Laberinto: ");
            DibujarMaze();
        }

        public void BFS()
        {
            queue.Enqueue(startPoint);
            while (queue.Count > 0 && isExploring)
            {
                searchingPoint = queue.Dequeue();
                OnReachingEnd();
                ExploreNeighbourNodes();
            }
        }

        public void OnReachingEnd()
        {
            if (searchingPoint == endPoint)
            {
                isExploring = false;
            }
            else
            {
                isExploring = true;
            }
        }

        public void ExploreNeighbourNodes()
        {
            if (!isExploring) { return; }
            foreach (var item in directions)
            {
                Vector2 vecinoPos = searchingPoint.GetPosition().suma(item);

                if (diccionario.ContainsKey(vecinoPos))
                {
                    Node nodito = diccionario[vecinoPos];
                    if (!nodito.explored)
                    {
                        nodito.explored = true;
                        nodito.exploredFrom = searchingPoint;
                        queue.Enqueue(nodito);                        
                    }
                }

            }
        }

        public void CreatePath()
        {
            SetPath(endPoint);
            Node previousNode = endPoint.exploredFrom;

            while (previousNode != startPoint)
            {
                caminito.Add(previousNode.ubicacion, 1);
                SetPath(previousNode);
                previousNode = previousNode.exploredFrom;               
            }

            SetPath(startPoint);
            path.Reverse();
        }

        public void SetPath(Node node)
        {
            path.Add(node);
        }

        public void ImprimirLista()
        {
            Console.WriteLine("Camino Recorrido: ");
            for (int i = 0; i < path.Count; i++)
            {
                Console.WriteLine(path[i].GetPosition());
            }
            DibujarMaze();
        }

        public void DibujarMaze()
        {
            for (int i = 0; i < tamaño; i++)
            {
                for (int j = 0; j < tamaño; j++)
                {
                    Vector2 dibujo = new Vector2(j, i);
                    if (inifina.ContainsKey(dibujo))
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("H");
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                    else if (caminito.ContainsKey(dibujo))
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.Write("V");
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                    else if (diccionario.ContainsKey(dibujo))
                    {
                        Console.Write("O");
                    }
                    else if (huecos.ContainsKey(dibujo))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("X");
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                }
                Console.WriteLine("");
            }
        }
    }
}
