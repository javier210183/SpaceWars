//Javier Sanchez Collado 1º DAW

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace NaveEspacial
{
    internal class Ventana
    {
        public int Ancho { get; set; }
        public int Altura { get; set; }
        public Point LimiteSuperior { get; set; }
        public Point LimiteInferior { get; set; }
        public ConsoleColor Color { get; set; }

        private Enemigo _enemigo1;
        private Enemigo _enemigo2;
        private Enemigo _enemigo3;
        private List<Bala> _balas;

        public Ventana(int ancho, int altura, ConsoleColor color, Point limiteSuperior,
            Point limiteInferior)
        {
            Ancho = ancho;
            Altura = altura;
            Color = color;
            LimiteSuperior = limiteSuperior;
            LimiteInferior = limiteInferior;
            Init();
        }
        private void Init()
        {
            Console.SetWindowSize(Ancho, Altura);
            Console.Title = "Nave";
            Console.CursorVisible = false;
            Console.BackgroundColor = Color;
            Console.Clear();
            _enemigo1 = new Enemigo(new Point(50, 10), ConsoleColor.Cyan, this, TipoEnemigo.Menu, null);
            _enemigo2 = new Enemigo(new Point(100, 30), ConsoleColor.Red, this, TipoEnemigo.Menu, null);
            _enemigo3 = new Enemigo(new Point(80, 200), ConsoleColor.DarkGreen, this, TipoEnemigo.Menu, null);
            _balas = new List<Bala>();
            CrearBalas();
        }
        public void DibujarMarco()
        {
            Console.ForegroundColor = ConsoleColor.White;

            for (int i = LimiteSuperior.X; i <= LimiteInferior.X; i++)
            {
                Console.SetCursorPosition(i, LimiteSuperior.Y);
                Console.Write("═");
                Console.SetCursorPosition(i, LimiteInferior.Y);
                Console.Write("═");
            }
            for (int i = LimiteSuperior.Y; i <= LimiteInferior.Y; i++)
            {
                Console.SetCursorPosition(LimiteSuperior.X, i);
                Console.Write("║");
                Console.SetCursorPosition(LimiteInferior.X, i);
                Console.Write("║");
            }

            Console.SetCursorPosition(LimiteSuperior.X, LimiteSuperior.Y);
            Console.Write("╔");
            Console.SetCursorPosition(LimiteSuperior.X, LimiteInferior.Y);
            Console.Write("╚");
            Console.SetCursorPosition(LimiteInferior.X, LimiteSuperior.Y);
            Console.Write("╗");
            Console.SetCursorPosition(LimiteInferior.X, LimiteInferior.Y);
            Console.Write("╝");
        }
        public void Peligro()//paso al segundo nivel
        {
            Console.Clear();
            DibujarMarco();
            for (int i = 0; i < 6; i++)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(LimiteInferior.X / 2 - 5, LimiteInferior.Y / 2);
                Console.Write("¡¡PELIGRO!!");
                Thread.Sleep(200);
                Console.SetCursorPosition(LimiteInferior.X / 2 - 5, LimiteInferior.Y / 2);
                Console.Write("           ");
                Thread.Sleep(200);
            }
        }
        public void Menu()
        {
            _enemigo1.Mover();
            _enemigo2.Mover();
            _enemigo3.Mover();
            MoverBalas();
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(LimiteInferior.X / 2 - 5, LimiteInferior.Y / 2 - 1);
            Console.Write("[Enter] JUGAR");
            Console.SetCursorPosition(LimiteInferior.X / 2 - 5, LimiteInferior.Y / 2);
            Console.Write("[Esc] SALIR");
        }
        public void Teclado(ref bool ejecucion, ref bool jugar)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo tecla = Console.ReadKey(true);
                if (tecla.Key == ConsoleKey.Enter)
                {
                    Console.Clear();
                    DibujarMarco();
                    jugar = true;
                }
                if (tecla.Key == ConsoleKey.Escape)
                    ejecucion = false;
            }
        }
        public void CrearBalas()
        {
            var colores = new ConsoleColor[]
            {
        ConsoleColor.Red, ConsoleColor.DarkMagenta, ConsoleColor.Cyan,
        ConsoleColor.DarkBlue, ConsoleColor.Gray, ConsoleColor.Blue,
        ConsoleColor.DarkRed, ConsoleColor.Yellow, ConsoleColor.White,
        ConsoleColor.Green, ConsoleColor.DarkCyan, ConsoleColor.DarkGray,
        ConsoleColor.DarkGreen, ConsoleColor.Blue, ConsoleColor.Cyan,
        ConsoleColor.Yellow, ConsoleColor.Green, ConsoleColor.White,
        ConsoleColor.Magenta, ConsoleColor.DarkMagenta
            };

            Random random = new Random();

            for (int i = 0; i < colores.Length; i++)
            {
                Bala bala = new Bala(new Point(0, 0), colores[i], TipoBala.Menu);
                PosicionesAleatorias(bala);
                int numeroAleatorio = random.Next(LimiteSuperior.Y + 1, LimiteInferior.Y);
                bala.Posicion = new Point(bala.Posicion.X, numeroAleatorio);
                _balas.Add(bala);
            }
        }

        public void PosicionesAleatorias(Bala bala)
        {
            Random random = new Random();
            int numeroAleatorio = random.Next(LimiteSuperior.X + 1, LimiteInferior.X);
            bala.Posicion = new Point(numeroAleatorio, LimiteInferior.Y);
        }
        public void MoverBalas()
        {
            for (int i = 0; i < _balas.Count; i++)
            {
                if (_balas[i].Mover(1, LimiteSuperior.Y))
                    PosicionesAleatorias(_balas[i]);
            }
        }
    }

}