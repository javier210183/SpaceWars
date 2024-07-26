//Javier Sanchez Collado 1º DAW

using NaveEspacial;
using System.Drawing;

Ventana ventana;
Nave nave;
Enemigo enemigo1;
Enemigo enemigo2;
Enemigo enemigo3;
Enemigo enemigo4;
Enemigo enemigoBoss;
bool jugar = false;//creo esta variable : bool jugar = true;//el juego se ejcutara mientras estas variable este en true
bool enemigoFinal = false;
bool bossFinal = false;
bool ejecucion = true;
void Inicar()
{
    ventana = new Ventana(170, 45, ConsoleColor.Black, new Point(5, 3), new Point(165, 43));
    ventana.DibujarMarco();
    nave = new Nave(new Point(80, 30), ConsoleColor.White, ventana);


    enemigo1 = new Enemigo(new Point(50, 10), ConsoleColor.Cyan, ventana, TipoEnemigo.Normal,
        nave);
    enemigo2 = new Enemigo(new Point(20, 12), ConsoleColor.Red, ventana, TipoEnemigo.Normal,
        nave);
    enemigo3 = new Enemigo(new Point(29, 20), ConsoleColor.Red, ventana, TipoEnemigo.Normal,
       nave);
    enemigo4 = new Enemigo(new Point(50, 20), ConsoleColor.Yellow, ventana, TipoEnemigo.Normal,
      nave);
    enemigoBoss = new Enemigo(new Point(100, 10), ConsoleColor.Magenta, ventana, TipoEnemigo.Boss,
        nave);

    nave.Enemigos.Add(enemigo1);
    nave.Enemigos.Add(enemigo2);
    nave.Enemigos.Add(enemigo3);
    nave.Enemigos.Add(enemigo4);
    nave.Enemigos.Add(enemigoBoss);

}
void Reiniciar()
{
    Console.Clear();
    ventana.DibujarMarco();

    nave.Vida = 100;
    nave.SobreCarga = 0;
    nave.BalaEspecial = 0;
    nave.Balas.Clear();

    enemigo1.Vida = 100;
    enemigo1.Vivo = true;
    enemigo2.Vida = 100;
    enemigo2.Vivo = true;
    enemigo3.Vivo = true;
    enemigo3.Vida = 100;
    enemigo4.Vivo = true;
    enemigo4.Vida = 150;

    enemigoBoss.Vida = 200;
    enemigoBoss.Vivo = true;
    enemigoBoss.PosicionesEnemigo.Clear();
    enemigo4.Vida = 150;
    enemigo4.Vivo = true;
    enemigo4.PosicionesEnemigo.Clear();

}

void Game()
{
    while (ejecucion)
    {
        ventana.Menu();
        ventana.Teclado(ref ejecucion, ref jugar);
        while (jugar)
        {
            if (!enemigo1.Vivo && !enemigo2.Vivo && !enemigo3.Vivo && !bossFinal && !enemigoFinal)
            {
                enemigoFinal = true;
                bossFinal = true;
                ventana.Peligro();
            }
            if (bossFinal && enemigoFinal)
            {
                enemigo4.Mover();
                enemigo4.Informacion(100);
                enemigoBoss.Mover();
                enemigoBoss.Informacion(140);
            }
            else
            {
                enemigo1.Mover();
                enemigo1.Informacion(100);
                enemigo2.Mover();
                enemigo2.Informacion(120);
                enemigo3.Mover();
                enemigo3.Informacion(140);

            }
            nave.Mover(2);//VELOCIDAD DE LA NAVE 2 espacios en cada movimiento
            nave.Disparar();
            if (nave.Vida <= 0)
            {
                jugar = false;
                nave.Muerte();
                Reiniciar();
            }
            if (!enemigoBoss.Vivo && !enemigo4.Vivo)
            {
                jugar = false;
                Reiniciar();
            }

        }
    }


}
Inicar();
Game();
