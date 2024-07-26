//Javier Sanchez Collado 1º DAW

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace NaveEspacial
{
    internal class Nave
    {
        public float Vida { get; set; }
        public Point Posicion { get; set; }
        public ConsoleColor Color { get; set; }
        public Ventana VentanaC { get; set; }
        public List<Point> PosicionesNave { get; set; } //posiciones de la nave
        public List<Bala> Balas { get; set; }
        public float SobreCarga { get; set; }
        public bool SobreCargaCond { get; set; } //condicion para sobrecarga de disparo
        public float BalaEspecial { get; set; }
        public bool ColisionBala { get; set; }
        public List<Enemigo> Enemigos { get; set; }
        public ConsoleColor ColorAux { get; set; }
        public DateTime TiempoColision { get; set; }

        public Nave(Point posicion, ConsoleColor color, Ventana ventana)
        {
            Posicion = posicion;
            Color = color;
            VentanaC = ventana;
            Vida = 100;
            PosicionesNave = new List<Point>();
            Balas = new List<Bala>();
            ColisionBala = false;
            Enemigos = new List<Enemigo>();
            ColorAux = color;
            TiempoColision = DateTime.Now;
        }
        public void Dibujar()
        {
            if (DateTime.Now > TiempoColision.AddMilliseconds(1000))
                Console.ForegroundColor = Color;
            else
                Console.ForegroundColor = ColorAux;


            int x = Posicion.X;
            int y = Posicion.Y;

            Console.SetCursorPosition(x + 3, y);
            Console.Write("Â");
            Console.SetCursorPosition(x + 1, y + 1);
            Console.Write("▬{¤}▬");
            Console.SetCursorPosition(x, y + 2);
            Console.Write("Ï ┴ ┴ Ï");

            PosicionesNave.Clear();

            PosicionesNave.Add(new Point(x + 3, y));

            PosicionesNave.Add(new Point(x + 1, y + 1));
            PosicionesNave.Add(new Point(x + 2, y + 1));
            PosicionesNave.Add(new Point(x + 3, y + 1));
            PosicionesNave.Add(new Point(x + 4, y + 1));
            PosicionesNave.Add(new Point(x + 5, y + 1));

            PosicionesNave.Add(new Point(x, y + 2));
            PosicionesNave.Add(new Point(x + 2, y + 2));
            PosicionesNave.Add(new Point(x + 4, y + 2));
            PosicionesNave.Add(new Point(x + 6, y + 2));
        }
        public void Borrar() // borrado de posiciones
        {
            foreach (Point item in PosicionesNave)
            {
                Console.SetCursorPosition(item.X, item.Y);
                Console.Write(" ");
            }
        }
        public void Teclado(ref Point distancia, int velocidad)
        {
            ConsoleKeyInfo tecla = Console.ReadKey(true);

            // Comprueba las teclas de dirección y ajusta la distancia en consecuencia.
            switch (tecla.Key)
            {
                case ConsoleKey.UpArrow:
                    distancia = new Point(0, -1 * velocidad);
                    break;
                case ConsoleKey.DownArrow:
                    distancia = new Point(0, velocidad);
                    break;
                case ConsoleKey.RightArrow:
                    distancia = new Point(velocidad, 0);
                    break;
                case ConsoleKey.LeftArrow:
                    distancia = new Point(-1 * velocidad, 0);
                    break;
            }

            // Comprueba las teclas de disparo y añade las balas en consecuencia.
            if (!SobreCargaCond)
            {
                if (tecla.Key == ConsoleKey.M)
                {
                    Balas.Add(new Bala(new Point(Posicion.X + 6, Posicion.Y + 2), ConsoleColor.White, TipoBala.Normal));
                    SobreCarga = Math.Min(SobreCarga + 0.8f, 100);
                    SobreCargaCond = SobreCarga >= 100;
                }
                else if (tecla.Key == ConsoleKey.B)
                {
                    Balas.Add(new Bala(new Point(Posicion.X, Posicion.Y + 2), ConsoleColor.White, TipoBala.Normal));
                    SobreCarga = Math.Min(SobreCarga + 0.8f, 100);
                    SobreCargaCond = SobreCarga >= 100;
                }
            }

            // Comprueba la tecla de disparo especial y añade la bala en consecuencia.
            if (tecla.Key == ConsoleKey.Spacebar && BalaEspecial >= 100)
            {
                Balas.Add(new Bala(new Point(Posicion.X + 2, Posicion.Y - 2), ConsoleColor.White, TipoBala.Especial));
                BalaEspecial = 0;
            }
        }

        public void Colisiones(Point distancia)
        {
            Point posicionAux = new Point(Posicion.X + distancia.X, Posicion.Y + distancia.Y);
            if (posicionAux.X <= VentanaC.LimiteSuperior.X)
                posicionAux.X = VentanaC.LimiteSuperior.X + 1;
            if (posicionAux.X + 6 >= VentanaC.LimiteInferior.X)
                posicionAux.X = VentanaC.LimiteInferior.X - 7;
            if (posicionAux.Y <= (VentanaC.LimiteSuperior.Y) + 15)
                posicionAux.Y = (VentanaC.LimiteSuperior.Y + 1) + 15;
            if (posicionAux.Y + 2 >= VentanaC.LimiteInferior.Y)
                posicionAux.Y = VentanaC.LimiteInferior.Y - 3;

            Posicion = posicionAux;
        }
        public void Informacion()
        {
            // Establecer el color de la consola en blanco
            Console.ForegroundColor = ConsoleColor.White;

            // Mover el cursor a la posición superior de la ventana
            Console.SetCursorPosition(VentanaC.LimiteSuperior.X, VentanaC.LimiteSuperior.Y - 1);

            // Mostrar la información de vida
            Console.Write("VIDA: " + (int)Vida + " %  ");

            // Reducir la sobrecarga y asegurarse de que no sea inferior a 0
            if (SobreCarga <= 0)
                SobreCarga = 0;
            else
                SobreCarga -= 0.009f;

            // Verificar si la sobrecarga es menor o igual a 50
            if (SobreCarga <= 50)
                SobreCargaCond = false;

            // Cambiar el color del texto en función de SobreCargaCond
            if (SobreCargaCond)
                Console.ForegroundColor = ConsoleColor.Red;
            else
                Console.ForegroundColor = ConsoleColor.White;

            // Mover el cursor a la nueva posición y mostrar la información de sobrecarga
            Console.SetCursorPosition(VentanaC.LimiteSuperior.X + 13, VentanaC.LimiteSuperior.Y - 1);
            Console.Write("SOBRECARGA: " + (int)SobreCarga + " %  ");

            // Restablecer el color de la consola a blanco
            Console.ForegroundColor = ConsoleColor.White;

            // Mover el cursor a la nueva posición
            Console.SetCursorPosition(VentanaC.LimiteSuperior.X + 32, VentanaC.LimiteSuperior.Y - 1);

            // Mostrar la información de la bala especial
            Console.Write("BALA ESPECIAL: " + (int)BalaEspecial + " %  ");

            // Incrementar BalaEspecial si es menor que 100, si es mayor o igual a 100, mantenerlo en 100
            if (BalaEspecial >= 100)
                BalaEspecial = 100;
            else
                BalaEspecial += 0.009f;
        }

        public void Mover(int velocidad) // Logica del movimiento
        {
            if (Console.KeyAvailable)
            {
                Borrar();
                Point distancia = new Point();
                Teclado(ref distancia, velocidad);
                Colisiones(distancia);
            }
            Dibujar();
            Informacion();
        }
        public void Disparar()
        {
            for (int i = 0; i < Balas.Count; i++)
            {
                if (Balas[i].Mover(1, VentanaC.LimiteSuperior.Y, Enemigos))
                {
                    Balas.Remove(Balas[i]);
                }

            }
        }
        public void Muerte()
        {
            Console.ForegroundColor = Color;
            foreach (Point item in PosicionesNave)
            {
                Console.SetCursorPosition(item.X, item.Y);
                Console.Write("X");
                Thread.Sleep(200);
            }
            foreach (Point item in PosicionesNave)
            {
                Console.SetCursorPosition(item.X, item.Y);
                Console.Write(" ");
                Thread.Sleep(200);
            }
        }

    }
}