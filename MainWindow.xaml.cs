using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Pong_Game_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();

        int speedX = 5, speedY = 5;

        double canvasHeight, canvasWidth;

        Rect ballHitBox, platformHitBox;

        public MainWindow()
        {
            InitializeComponent();
            MyCanvas.Focus();

            Loaded += (sender, e) =>
            {
                canvasHeight = MyCanvas.ActualHeight;
                canvasWidth = MyCanvas.ActualWidth; // Отримуємо розмір канвасу після ініціалізації
            };

            timer.Start();
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += GameLogic;

        }

        public void GameLogic(object sender, EventArgs e)
        {
            MovePlatform();
            MoveTheBall();

            if (Canvas.GetBottom(ball) < 0)
            {
                GameOver();
            }
        }

        private void MovePlatform()
        {
            Point mousePosition = Mouse.GetPosition(MyCanvas); // Отримати позицію миші відносно канвасу

            // Оновити положення платформи відповідно до позиції миші
            double platformWidth = platform.Width;
            double newLeft = mousePosition.X - platformWidth / 2; // Центруємо платформу відносно миші
            if (newLeft >= 0 && newLeft + platformWidth <= canvasWidth)
            {
                Canvas.SetLeft(platform, newLeft);
            }
        }

        public void MoveTheBall()
        {
            Canvas.SetLeft(ball, Canvas.GetLeft(ball) + speedX);
            Canvas.SetBottom(ball, Canvas.GetBottom(ball) - speedY);

            if (Canvas.GetBottom(ball) + ball.Height > canvasHeight)
            {
                speedY = -speedY;
            }

            if (Canvas.GetLeft(ball) <= 0 || Canvas.GetLeft(ball) + ball.Width > canvasWidth)
            {
                speedX = -speedX;
            }

            ballHitBox = new Rect(Canvas.GetLeft(ball), Canvas.GetBottom(ball), ball.Width, ball.Height);
            platformHitBox = new Rect(Canvas.GetLeft(platform), Canvas.GetBottom(platform), platform.Width, platform.Height);
            if (ballHitBox.IntersectsWith(platformHitBox))
            {
                speedY = -speedY;
            }
        }

        public void GameOver()
        {
            timer.Stop();
            ball.Visibility = Visibility.Hidden;
            platform.Visibility = Visibility.Hidden;
            GameOverText.Visibility = Visibility.Visible;
            MyCanvas.Background = new SolidColorBrush(Colors.Red);

            gameOver = true;
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)// && gameOver)
            {
                StartGame();
            }
        }

        public void StartGame()
        {
            Canvas.SetBottom(ball, 200);
            Canvas.SetLeft(ball, 190);

            Canvas.SetBottom(platform, 20);
            Canvas.SetLeft(platform, 140);

            speedY = -speedY;

            ball.Visibility = Visibility.Visible;
            platform.Visibility = Visibility.Visible;
            GameOverText.Visibility = Visibility.Hidden;
            MyCanvas.Background = new SolidColorBrush(Colors.White);

            gameOver = false;

            timer.Start();
        }
    }
}
