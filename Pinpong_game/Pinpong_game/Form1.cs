using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pinpong_game
{
    public partial class Form1 : Form
    {
        // Variables para la pelota
        private int ballX = 390, ballY = 240;  // Posición inicial de la pelota
        private int ballSpeedX = 6, ballSpeedY = 6;  // Velocidad de la pelota
        // Variables para las paletas
        private int paddle1X = 20, paddle1Y = 150; // Posición inicial de la paleta del jugador 1
        private int paddle2X = 750, paddle2Y = 150; // Posición inicial de la paleta del jugador 2
        private int paddleSpeed = 30; // Velocidad de las paletas
        // Tamaños de la pelota y las paletas
        private readonly int ballSize = 30;
        private readonly int paddleWidth = 20, paddleHeight = 150;
        // Variables de puntuación
        private int scorePlayer1 = 0, scorePlayer2 = 0;
        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;  // Evitar parpadeo
            // Inicializa el Timer manualmente
            timer1 = new Timer();
            timer1.Interval = 20;  // Actualización del juego cada 20 ms
            timer1.Tick += Timer1_Tick;  // Asocia el evento Tick
            timer1.Start();  // Inicia el Timer
            this.KeyDown += Form1_KeyDown;  // Evento para controlar las teclas
        }
        private void Timer1_Tick(object sender, EventArgs e)
        {
            MoveBall();  // Mueve la pelota
            CheckCollisions();  // Verifica colisiones
            this.Invalidate();  // Redibuja el formulario
        }
        private void MoveBall()
        {
            // Mueve la pelota
            ballX += ballSpeedX;
            ballY += ballSpeedY;
            // Rebota en los bordes superior e inferior
            if (ballY <= 0 || ballY + ballSize >= this.ClientSize.Height)
            {
                ballSpeedY = -ballSpeedY;
            }
        }
        private void CheckCollisions()
        {
            // Colisiones con las paletas
            Rectangle ballRect = new Rectangle(ballX, ballY, ballSize, ballSize);
            Rectangle paddle1Rect = new Rectangle(paddle1X, paddle1Y, paddleWidth, paddleHeight);
            Rectangle paddle2Rect = new Rectangle(paddle2X, paddle2Y, paddleWidth, paddleHeight);
            // Si la pelota golpea la paleta del jugador 1
            if (ballRect.IntersectsWith(paddle1Rect))
            {
                ballSpeedX = -ballSpeedX;
            }
            // Si la pelota golpea la paleta del jugador 2
            if (ballRect.IntersectsWith(paddle2Rect))
            {
                ballSpeedX = -ballSpeedX;
            }
            // Si la pelota pasa por la izquierda (punto para el jugador 2)
            if (ballX <= 0)
            {
                scorePlayer2++;
                ResetBall();
                CheckForWinner(); // Verifica si hay un ganador después de cada punto
            }
            // Si la pelota pasa por la derecha (punto para el jugador 1)
            if (ballX + ballSize >= this.ClientSize.Width)
            {
                scorePlayer1++;
                ResetBall();
                CheckForWinner(); // Verifica si hay un ganador después de cada punto
            }
        }
        private void CheckForWinner()
        {
            // Condición para que el jugador 1 gane
            if (scorePlayer1 >= 5)
            {
                BackColor = Color.CornflowerBlue;
                timer1.Stop(); // Detener el juego
                MessageBox.Show("¡Jugador 1 gana!", "Fin del juego");
                ResetGame(); // Opción para reiniciar o cerrar el juego
                BackColor = Color.LightBlue;
            }
            // Condición para que el jugador 2 gane
            if (scorePlayer2 >= 5)
            {
                BackColor = Color.Peru;
                timer1.Stop(); // Detener el juego
                MessageBox.Show("¡Jugador 2 gana!", "Fin del juego");
                ResetGame(); // Opción para reiniciar o cerrar el juego
                BackColor = Color.LightBlue;
            }
        }
        private void ResetGame()
        {
            // Reinicia el puntaje de ambos jugadores
            scorePlayer1 = 0;
            scorePlayer2 = 0;
            // Reinicia la posición de la pelota y las paletas
            ballX = this.ClientSize.Width / 2 - ballSize / 2;
            ballY = this.ClientSize.Height / 2 - ballSize / 2;
            paddle1X = 20;
            paddle1Y = 150;
            paddle2X = this.ClientSize.Width - 40;
            paddle2Y = 150;
            // Restablece la velocidad de la pelota
            ballSpeedX = 0;
            ballSpeedY = 0;
            // Reinicia el juego
            timer1.Start();
        }
        private void ResetBall()
        {
            // Reinicia la pelota en el centro
            ballX = this.ClientSize.Width / 2 - ballSize / 2;
            ballY = this.ClientSize.Height / 2 - ballSize / 2;
            // Cambia la dirección de la pelota al reiniciar
            ballSpeedX = -ballSpeedX;
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // Movimiento de la paleta del jugador 1 (W, S, A, D)
            if (e.KeyCode == Keys.W && paddle1Y > 0)
            {
                paddle1Y -= paddleSpeed;  // Mover hacia arriba
            }
            if (e.KeyCode == Keys.S && paddle1Y + paddleHeight < this.ClientSize.Height)
            {
                paddle1Y += paddleSpeed;  // Mover hacia abajo
            }
            if (e.KeyCode == Keys.A && paddle1X > 0)
            {
                paddle1X -= paddleSpeed;  // Mover hacia la izquierda
            }
            if (e.KeyCode == Keys.D && paddle1X < 0)
            {
                paddle1X += paddleSpeed;  // Mover hacia la derecha
            }
            // Movimiento de la paleta del jugador 2 (Flechas: Arriba, Abajo, Izquierda, Derecha)
            if (e.KeyCode == Keys.Up && paddle2Y > 0)
            {
                paddle2Y -= paddleSpeed;  // Mover hacia arriba
            }
            if (e.KeyCode == Keys.Down && paddle2Y + paddleHeight < this.ClientSize.Height)
            {
                paddle2Y += paddleSpeed;  // Mover hacia abajo
            }
            if (e.KeyCode == Keys.Left && paddle2X > 0)
            {
                paddle2X -= paddleSpeed;  // Mover hacia la izquierda
            }
            if (e.KeyCode == Keys.Right && paddle2X + paddleWidth < this.ClientSize.Width)
            {
                paddle2X += paddleSpeed;  // Mover hacia la derecha
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            // Dibuja la pelota
            g.FillEllipse(Brushes.White, ballX, ballY, ballSize, ballSize);
            // Dibuja las paletas
            g.FillRectangle(Brushes.CornflowerBlue, paddle1X, paddle1Y, paddleWidth, paddleHeight);
            g.FillRectangle(Brushes.Peru, paddle2X, paddle2Y, paddleWidth, paddleHeight);
            // Dibuja la puntuación
            g.DrawString(scorePlayer1.ToString(), new Font("Arial", 16), Brushes.CornflowerBlue, 100, 20);
            g.DrawString(scorePlayer2.ToString(), new Font("Arial", 16), Brushes.Peru, this.ClientSize.Width - 120, 20);
        }
    }
}
