//Javier Sanchez Collado 1º DAW

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace NaveEspacial
{
    public enum TipoEnemigo
    {
        Normal, Boss, Menu
    }
    internal class Enemigo
    {
        enum Direccion
        {
            Derecha, Izquierda, Arriba, Abajo
        }

        public bool Vivo { get; set; }// si es verdader esta vivo
        public float Vida { get; set; }//almacena la vida del enemigo
        public Point Posicion { get; set; }
        public Ventana VentanaC { get; set; }//colisiones
        public ConsoleColor Color { get; set; }//color enemigo
        public TipoEnemigo TipoEnemigoE { get; set; }
        public List<Point> PosicionesEnemigo { get; set; }// posiciones enemigo
        public List<Bala> Balas { get; set; }
        public Nave NaveC { get; set; }

        private Direccion _direccion;//almacena la direccion actual a la cual se mueve el enemigo
        private DateTime _tiempoDireccion;//alamacena el tiempo de las posiciones
        private float _tiempoDireccionAleatorio;//tiempo en el que se genera nueva direccion

        private DateTime _tiempoMovimiento;

        private DateTime _tiempoDisparo;
        private float _tiempoDisparoAleatorio;

        public Enemigo(Point posicion, ConsoleColor color, Ventana ventana,
            TipoEnemigo tipoEnemigo, Nave nave)
        {
            Posicion = posicion;
            Color = color;
            VentanaC = ventana;
            TipoEnemigoE = tipoEnemigo;
            Vivo = true;
            Vida = 100;
            _direccion = Direccion.Derecha;//direccion inicial
            _tiempoDireccion = DateTime.Now;
            _tiempoDireccionAleatorio = 1000;//se inicia en 1000 por que estamos trabajando con milisegundos
            _tiempoMovimiento = DateTime.Now;
            _tiempoDisparo = DateTime.Now;
            _tiempoDisparoAleatorio = 200;
            PosicionesEnemigo = new List<Point>();
            Balas = new List<Bala>();
            NaveC = nave;
        }
        public void Dibujar()
        {
            switch (TipoEnemigoE)
            {
                case TipoEnemigo.Normal:
                    DibujoNormal();
                    break;
                case TipoEnemigo.Boss:
                    DibujoBoss();
                    break;
                case TipoEnemigo.Menu:
                    DibujoNormal();
                    break;
            }
        }
        public void DibujoNormal()
        {
            // Configura el color de la consola
            Console.ForegroundColor = Color;

            // Posiciones iniciales del enemigo
            int x = Posicion.X;
            int y = Posicion.Y;

            // Diseño del enemigo
            string[] disenoEnemigo = { " ▲▲ ", "▌██▐", "▼  ▼" };

            // Limpia la lista de posiciones de enemigos
            PosicionesEnemigo.Clear();

            // Itera a través del diseño del enemigo
            for (int i = 0; i < disenoEnemigo.Length; i++)
            {
                // Dibuja la línea actual del diseño del enemigo en la consola
                Console.SetCursorPosition(x, y + i);
                Console.Write(disenoEnemigo[i]);

                // Itera a través de los caracteres en la línea actual del diseño
                for (int j = 0; j < disenoEnemigo[i].Length; j++)
                {
                    // Si el caracter no es un espacio, añade su posición a la lista PosicionesEnemigo
                    if (disenoEnemigo[i][j] != ' ')
                    {
                        PosicionesEnemigo.Add(new Point(x + j, y + i));
                    }
                }
            }
        }


        public void DibujoBoss()
        {
            Console.ForegroundColor = Color;
            int x = Posicion.X;
            int y = Posicion.Y;

            // Diseño del boss
            string[] disenoBoss =
            {
        " █▄▄▄▄█ ",
        "██ ██ ██",
        "╔▀╦▀▀╦▀╗"
    };

            // Limpia la lista de posiciones del enemigo
            PosicionesEnemigo.Clear();

            // Itera a través del diseño del boss
            for (int i = 0; i < disenoBoss.Length; i++)
            {
                // Dibuja la línea actual del diseño del boss en la consola
                Console.SetCursorPosition(x, y + i);
                Console.Write(disenoBoss[i]);

                // Itera a través de los caracteres en la línea actual del diseño
                for (int j = 0; j < disenoBoss[i].Length; j++)
                {
                    // Si el caracter no es un espacio, añade su posición a la lista PosicionesEnemigo
                    if (disenoBoss[i][j] != ' ')
                    {
                        PosicionesEnemigo.Add(new Point(x + j, y + i));
                    }
                }
            }
        }

        public void Muerte()
        {
            if (TipoEnemigoE == TipoEnemigo.Normal)
            {
                MuerteNormal();
            }
            if (TipoEnemigoE == TipoEnemigo.Boss)
            {
                MuerteBoss();
            }
        }
        public void MuerteBoss()
        {
            Console.ForegroundColor = Color;
            foreach (Point item in PosicionesEnemigo)
            {
                Console.SetCursorPosition(item.X, item.Y);
                Console.Write("▓");
                Thread.Sleep(200);
            }
            foreach (Point item in PosicionesEnemigo)
            {
                Console.SetCursorPosition(item.X, item.Y);
                Console.Write(" ");
                Thread.Sleep(200);
            }
            PosicionesEnemigo.Clear();
            foreach (Bala item in Balas)
            {
                item.Borrar();
            }
            Balas.Clear();
        }
        public void MuerteNormal()
        {
            Console.ForegroundColor = ConsoleColor.White;
            int x = Posicion.X;
            int y = Posicion.Y;

            Console.SetCursorPosition(x + 1, y);
            Console.Write("▄▄Zzz");
            Console.SetCursorPosition(x, y + 1);
            Console.Write("████");
            Console.SetCursorPosition(x, y + 2);
            Console.Write("▀  ▀");
            PosicionesEnemigo.Clear();

            foreach (Bala item in Balas)
            {
                item.Borrar();
            }
            Balas.Clear();
        }


        public void Borrar()
        {
            foreach (Point item in PosicionesEnemigo)
            {
                Console.SetCursorPosition(item.X, item.Y);
                Console.Write(" ");
            }
        }
        public void Mover()
        {
            if (!Vivo)
            {
                Muerte();
                return;
            }

            int tiempo = 30;
            if (TipoEnemigoE == TipoEnemigo.Boss)
                tiempo = 20;
            if (DateTime.Now > _tiempoMovimiento.AddMilliseconds(tiempo))
            {
                Borrar();
                DireccionAleatoria();
                Point posicionAux = Posicion;
                Movimiento(ref posicionAux);
                Colisiones(posicionAux);
                Dibujar();
                _tiempoMovimiento = DateTime.Now;
            }
            if (TipoEnemigoE != TipoEnemigo.Menu)
            {
                CrearBalas();
                Disparar();
            }
        }
        public void Colisiones(Point posicionAux)
        {
            int ancho = TipoEnemigoE == TipoEnemigo.Boss ? 7 : 3;

            int limiteInferior = TipoEnemigoE == TipoEnemigo.Menu ? VentanaC.LimiteInferior.Y - 1 : VentanaC.LimiteSuperior.Y + 15;//pongo 15 como limite superior para que la nave no collisione con los enemigos

            // Verificación de los límites X
            if (posicionAux.X <= VentanaC.LimiteSuperior.X)
            {
                _direccion = Direccion.Derecha;
                posicionAux.X = VentanaC.LimiteSuperior.X + 1;
            }
            else if (posicionAux.X + ancho >= VentanaC.LimiteInferior.X)
            {
                _direccion = Direccion.Izquierda;
                posicionAux.X = VentanaC.LimiteInferior.X - ancho - 1;
            }

            // Verificación de los límites Y
            if (posicionAux.Y <= VentanaC.LimiteSuperior.Y)
            {
                _direccion = Direccion.Abajo;
                posicionAux.Y = VentanaC.LimiteSuperior.Y + 1;
            }
            else if (posicionAux.Y + 2 >= limiteInferior)
            {
                _direccion = Direccion.Arriba;
                posicionAux.Y = limiteInferior - 2;
            }

            Posicion = posicionAux;
        }

        public void Movimiento(ref Point posicionAux)
        {
            switch (_direccion)
            {
                case Direccion.Derecha:
                    posicionAux.X += 1;
                    break;
                case Direccion.Izquierda:
                    posicionAux.X -= 1;
                    break;
                case Direccion.Arriba:
                    posicionAux.Y -= 1;
                    break;
                case Direccion.Abajo:
                    posicionAux.Y += 1;
                    break;
            }
        }
        public void DireccionAleatoria()
        {
            if (DateTime.Now > _tiempoDireccion.AddMilliseconds(_tiempoDireccionAleatorio)
                && (_direccion == Direccion.Derecha || _direccion == Direccion.Izquierda))
            {
                Random random = new Random();
                int numAleatorio = random.Next(1, 5);
                switch (numAleatorio)
                {
                    case 1:
                        _direccion = Direccion.Derecha;
                        break;
                    case 2:
                        _direccion = Direccion.Izquierda;
                        break;
                    case 3:
                        _direccion = Direccion.Arriba;
                        break;
                    case 4:
                        _direccion = Direccion.Abajo;
                        break;
                }
                _tiempoDireccion = DateTime.Now;
                _tiempoDireccionAleatorio = random.Next(1000, 2000);
            }

            if (DateTime.Now > _tiempoDireccion.AddMilliseconds(50)
                && (_direccion == Direccion.Arriba || _direccion == Direccion.Abajo))
            {
                Random random = new Random();
                int numAleatorio = random.Next(1, 3);
                switch (numAleatorio)
                {
                    case 1:
                        _direccion = Direccion.Derecha;
                        break;
                    case 2:
                        _direccion = Direccion.Izquierda;
                        break;
                }
                _tiempoDireccion = DateTime.Now;
            }
        }
        private static readonly Random random = new Random();

        public void CrearBalas()
        {
            if (DateTime.Now > _tiempoDisparo.AddMilliseconds(_tiempoDisparoAleatorio))
            {
                Point balaPoint;
                if (TipoEnemigoE == TipoEnemigo.Normal)
                {
                    balaPoint = new Point(Posicion.X + 1, Posicion.Y + 2);
                    _tiempoDisparoAleatorio = random.Next(200, 500);
                }
                else if (TipoEnemigoE == TipoEnemigo.Boss)
                {
                    balaPoint = new Point(Posicion.X + 4, Posicion.Y + 2);
                    _tiempoDisparoAleatorio = random.Next(100, 150);
                }
                else
                {
                    return;
                }

                Bala bala = new Bala(balaPoint, Color, TipoBala.Enemigo);
                Balas.Add(bala);
                _tiempoDisparo = DateTime.Now;//capturo fecha y hora actual en la que se creo la bala
            }
        }

        public void Disparar()
        {
            for (int i = 0; i < Balas.Count; i++)
            {
                if (Balas[i].Mover(1, VentanaC.LimiteInferior.Y, NaveC))//si la bala llega al limite se borra de la lista
                    Balas.Remove(Balas[i]);

            }
        }
        public void Informacion(int distanciaX)
        {
            Console.ForegroundColor = Color;
            Console.SetCursorPosition(VentanaC.LimiteSuperior.X + distanciaX, VentanaC.LimiteSuperior.Y - 1);
            Console.Write("Enemigo: " + (int)Vida + " %  ");
        }



    }
}