//Javier Sanchez Collado 1º DAW

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace NaveEspacial
{
    public enum TipoBala
    {
        Normal, Especial, Enemigo, Menu
    }
    internal class Bala
    {
        public Point Posicion { get; set; }
        public ConsoleColor Color { get; set; }
        public TipoBala TipoBalaB { get; set; }
        public List<Point> PosicionesBala { get; set; } // Lista de almacenamiento de posiciones de balas
        private DateTime _tiempo;
        public Bala(Point posicion, ConsoleColor color, TipoBala tipoBala)
        {
            Posicion = posicion;
            Color = color;
            TipoBalaB = tipoBala;
            PosicionesBala = new List<Point>();
            _tiempo = DateTime.Now;//devuelve fecha y hora actual
        }
        public void Dibujar()
        {
            Console.ForegroundColor = Color;
            int x = Posicion.X;
            int y = Posicion.Y;

            PosicionesBala.Clear();

            switch (TipoBalaB)
            {
                case TipoBala.Normal:
                    Console.SetCursorPosition(x, y);
                    Console.Write("☼");
                    PosicionesBala.Add(new Point(x, y));
                    break;
                case TipoBala.Especial:
                    Console.SetCursorPosition(x + 1, y);
                    Console.Write("▲");
                    Console.SetCursorPosition(x, y + 1);
                    Console.Write("{☻}");
                    Console.SetCursorPosition(x + 1, y + 2);
                    Console.Write("¥");

                    PosicionesBala.Add(new Point(x + 1, y));
                    PosicionesBala.Add(new Point(x, y + 1));
                    PosicionesBala.Add(new Point(x + 2, y + 1));
                    PosicionesBala.Add(new Point(x + 1, y + 2));
                    break;
                case TipoBala.Enemigo:
                    Console.SetCursorPosition(x, y);
                    Console.Write("█");
                    PosicionesBala.Add(new Point(x, y));
                    break;
                case TipoBala.Menu:
                    Console.SetCursorPosition(x, y);
                    Console.Write("!");
                    PosicionesBala.Add(new Point(x, y));
                    break;

            }
        }
        public void Borrar()
        {
            foreach (Point item in PosicionesBala)
            {
                Console.SetCursorPosition(item.X, item.Y);
                Console.Write(" ");
            }
        }
        public bool Mover(int velocidad, int limite, List<Enemigo> enemigos)
        {
            // Comprueba si ha pasado el tiempo suficiente desde la última vez
            if (DateTime.Now > _tiempo.AddMilliseconds(20))
            {
                Borrar();

                // Mueve la bala hacia arriba, independientemente de su tipo
                Posicion = new Point(Posicion.X, Posicion.Y - velocidad);
                // Si la bala ha alcanzado el límite superior, termina el movimiento
                if (Posicion.Y <= limite)
                    return true;

                // Determina el daño y las posiciones de la bala basándose en su tipo
                int dano;
                List<Point> posiciones;
                switch (TipoBalaB)
                {
                    case TipoBala.Normal:
                        dano = 5; // Daño de la bala normal
                        posiciones = new List<Point> { Posicion }; // La bala normal solo tiene una posición
                        break;
                    case TipoBala.Especial:
                        dano = 40; // Daño de la bala especial
                        posiciones = PosicionesBala; // La bala especial puede tener varias posiciones
                        break;
                    default:
                        throw new InvalidOperationException("Tipo de bala desconocido.");
                }

                // Verifica si la bala ha golpeado a algún enemigo
                foreach (Enemigo enemigo in enemigos)
                {
                    foreach (Point posicionE in enemigo.PosicionesEnemigo)
                    {
                        foreach (Point posicionB in posiciones)
                        {
                            if (posicionE.X == posicionB.X && posicionE.Y == posicionB.Y)
                            {
                                // Si se golpea a un enemigo, le reduce la vida
                                enemigo.Vida -= dano;
                                if (enemigo.Vida <= 0)
                                {
                                    // Si la vida del enemigo llega a 0, marca al enemigo como muerto
                                    enemigo.Vida = 0;
                                    enemigo.Vivo = false;
                                    enemigo.Muerte();
                                }
                                return true;
                            }
                        }
                    }
                }

                Dibujar();
                _tiempo = DateTime.Now;
            }
            return false;
        }

        // Esta función maneja el movimiento de un objeto (como una bala) que se mueve hacia abajo en la pantalla.
        public bool Mover(int velocidad, int limite, Nave nave)
        {
            // Verifica si ha pasado el tiempo suficiente desde la última vez que se movió el objeto.
            if (DateTime.Now > _tiempo.AddMilliseconds(20))
            {
                Borrar();

                // Mueve el objeto hacia abajo en la pantalla.
                Posicion = new Point(Posicion.X, Posicion.Y + velocidad);

                // Si el objeto ha alcanzado o superado el límite inferior, termina el movimiento.
                if (Posicion.Y >= limite)
                    return true;

                // Verifica si el objeto ha colisionado con la nave.
                foreach (Point posicionNave in nave.PosicionesNave)
                {
                    if (posicionNave.X == Posicion.X && posicionNave.Y == Posicion.Y)
                    {
                        // Si el objeto colisiona con la nave, reduce la vida de la nave y actualiza el color y el tiempo de la colisión.
                        nave.Vida -= 5;//daño recibido por impacto
                        nave.ColorAux = Color;
                        nave.TiempoColision = DateTime.Now;
                        return true;
                    }
                }

                Dibujar();

                // Actualiza el tiempo para la próxima invocación de la función.
                _tiempo = DateTime.Now;
            }
            // Devuelve false si el objeto no ha alcanzado el límite ni ha colisionado con la nave.
            return false;
        }

        // Esta función maneja el movimiento de un objeto (como una bala) que se mueve hacia arriba en la pantalla.
        public bool Mover(int velocidad, int limite)
        {
            // Verifica si ha pasado el tiempo suficiente desde la última vez que se movió el objeto.
            if (DateTime.Now > _tiempo.AddMilliseconds(20))
            {
                Borrar();

                // Mueve el objeto hacia arriba en la pantalla.
                Posicion = new Point(Posicion.X, Posicion.Y - velocidad);

                // Si el objeto ha alcanzado o superado el límite superior, termina el movimiento.
                if (Posicion.Y <= limite)
                    return true;

                
                Dibujar();

                // Actualiza el tiempo para la próxima invocación de la función.
                _tiempo = DateTime.Now;
            }
            // Devuelve false si el objeto no ha alcanzado el límite.
            return false;
        }



    }
}