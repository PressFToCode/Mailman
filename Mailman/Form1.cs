using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Mailman
{
    public partial class Form1 : Form
    {
        private List<Point> optimizedRoute; // Переменная для хранения оптимального маршрута
        private int currentStep = 0; // Текущий шаг в маршруте
        private Point currentPosition; // Текущая позиция квадрата
        private System.Windows.Forms.Timer moveTimer;
        private Point currentSquarePosition;
        private Point postOfficeLocation;
        private List<Point> mailboxLocations = new List<Point>();
        private int bagCapacity; // Вместимость сумки почтальона
        private int lettersDelivered = 0; // Количество доставленных писем
        private bool isReturning = false; // Флаг возврата к почтовому отделению
        private List<Point> positions = new List<Point>(); // Хранение всех позиций квадрата
        private List<Point> returnedPositions = new List<Point>(); // Хранение всех позиций возвращения к почтовому отделению
        private List<Point> officeToMailbox = new List<Point>(); // Маршрут от почтового отделения к последнему посещенному ящику
        private bool isOfficeToMailboxDrawn = false; // Флаг для отображения маршрута от офиса к ящику

        public Form1()
        {
            // Инициализация переменных
            moveTimer = new System.Windows.Forms.Timer();
            moveTimer.Interval = 1000; // Интервал движения (1 секунда)
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void DrawMarker(Point location, Color color, int width, int height)
        {
            Graphics g = pictureBoxMap.CreateGraphics();
            using (SolidBrush brush = new SolidBrush(color))
            {
                g.FillRectangle(brush, location.X - width / 2, location.Y - height / 2, width, height);
            }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (postOfficeLocation.IsEmpty || (postOfficeLocation.X == 0 && postOfficeLocation.Y == 0))
                {
                    postOfficeLocation = e.Location;
                    DrawMarker(postOfficeLocation, Color.Blue, 50, 50);
                }
                else if (mailboxLocations.Count < 12)
                {
                    mailboxLocations.Add(e.Location);
                    DrawMarker(e.Location, Color.Green, 20, 20);
                }
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (bagCapacity <= 0 || postOfficeLocation.IsEmpty || mailboxLocations.Count == 0)
            {
                MessageBox.Show("Пожалуйста, убедитесь, что все данные указаны корректно.");
                return;
            }
            // Вызываем метод оптимизации маршрута
            optimizedRoute = CalculateNearestNeighborRoute(mailboxLocations.ToList());

            if (int.TryParse(textBoxCapacity.Text, out int capacity))
            {
                bagCapacity = capacity;
            }
            else
            {
                MessageBox.Show("Введите корректную вместимость сумки!");
                return;
            }

            // Инициализируем начальные значения
            currentStep = 0;
            currentPosition = postOfficeLocation;
            DrawMarkers(); // Отрисовываем начальные маркеры

            // Начинаем таймер для перемещения по маршруту
            MoveTimer.Interval = 1000; // Установите интервал таймера в миллисекундах по вашему усмотрению
            MoveTimer.Start();
            positions.Add(currentPosition); // Добавляем начальную позицию
        }


        private List<Point> CalculateNearestNeighborRoute(List<Point> points)
        {
            List<Point> route = new List<Point>();
            HashSet<Point> visitedPoints = new HashSet<Point>();

            Point currentPoint = postOfficeLocation;
            route.Add(currentPoint);

            while (visitedPoints.Count < points.Count)
            {
                Point nearestPoint = FindNearestPoint(currentPoint, points, visitedPoints);
                if (nearestPoint == Point.Empty)
                {
                    // Возврат к почтовому отделению, если не найден непосещенный ящик
                    nearestPoint = postOfficeLocation;
                }

                route.Add(nearestPoint);
                visitedPoints.Add(nearestPoint);
                currentPoint = nearestPoint;
            }

            return route;
        }


        private Point FindNearestPoint(Point currentPoint, List<Point> allPoints, HashSet<Point> visitedPoints)
        {
            Point nearestPoint = Point.Empty;
            int shortestDistance = int.MaxValue;

            foreach (Point point in allPoints)
            {
                if (!visitedPoints.Contains(point))
                {
                    int distance = CalculateDistance(currentPoint, point);

                    if (distance < shortestDistance)
                    {
                        shortestDistance = distance;
                        nearestPoint = point;
                    }
                }
            }

            return nearestPoint;
        }

        private int CalculateDistance(Point point1, Point point2)
        {
            // Рассчет расстояния между двумя точками (например, Евклидово расстояние)
            return (int)Math.Sqrt(Math.Pow(point2.X - point1.X, 2) + Math.Pow(point2.Y - point1.Y, 2));
        }

        private void textBoxCapacity_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(textBoxCapacity.Text, out int capacity))
            {
                bagCapacity = capacity;
            }
        }

        private void MoveTimer_Tick(object sender, EventArgs e)
        {
            Point prevPositon = currentPosition;

            if (currentStep < optimizedRoute.Count)
            {
                currentPosition = optimizedRoute[currentStep]; // Обновляем текущую позицию квадрата

                if (!isReturning)
                {
                    if (lettersDelivered < bagCapacity)
                    {
                        lettersDelivered++; // Увеличиваем количество доставленных писем
                        officeToMailbox.Add(currentPosition); // Добавляем позицию к маршруту от офиса к ящику
                    }
                    else
                    {
                        isReturning = true; // Устанавливаем флаг возврата к почтовому отделению
                        returnedPositions.Add(currentPosition); // Добавляем текущую позицию в список позиций возврата к отделению
                        officeToMailbox.Reverse(); // Переворачиваем маршрут от офиса к ящику
                        currentStep--; // Возвращаемся на один шаг назад
                    }
                }
                else
                {
                    currentPosition = postOfficeLocation;
                    if (currentPosition == postOfficeLocation)
                    {
                        isReturning = false; // Сбрасываем флаг возврата к почтовому отделению
                        lettersDelivered = 0; // Обнуляем количество доставленных писем
                        officeToMailbox.Clear(); // Очищаем маршрут от офиса к ящику
                        prevPositon = postOfficeLocation;
                    }
                }

                MoveMarker(currentPosition); // Перемещаем квадрат по текущему шагу в маршруте
                currentStep++; // Переходим к следующему шагу в маршруте
            }
            else
            {
                MoveTimer.Stop(); // Если достигнут конец маршрута, останавливаем таймер
                                  // Проверяем, есть ли оставшийся маршрут от офиса к последнему посещенному ящику
                if (officeToMailbox.Count > 0 && !isOfficeToMailboxDrawn)
                {
                    using (Pen pen = new Pen(Color.Black, 2))
                    {
                        Graphics g = pictureBoxMap.CreateGraphics();
                        List<Point> pointsToDraw = new List<Point>();

                        if (isReturning)
                        {
                            pointsToDraw.AddRange(returnedPositions);
                            pointsToDraw.Add(postOfficeLocation); // Добавляем почтовое отделение для завершения пути
                        }
                        else
                        {   
                            pointsToDraw.AddRange(officeToMailbox);
                        }

                        // Если последняя точка является почтовым отделением, добавляем её в путь
                        if (pointsToDraw.Last() != postOfficeLocation && pointsToDraw.Last() != optimizedRoute.Last())
                        {
                            pointsToDraw.Add(optimizedRoute.Last());
                        }

                        for (int i = 1; i < pointsToDraw.Count; i++)
                        {
                            g.DrawLine(pen, pointsToDraw[i - 1], pointsToDraw[i]); // Рисуем линии между позициями
                        }
                    }
                    isOfficeToMailboxDrawn = true; // Устанавливаем флаг, что маршрут отрисован
                }
            }
        }

        private void pictureBoxMap_Paint(object sender, PaintEventArgs e)
        {
            // Отображение квадрата на PictureBox
            if (!currentSquarePosition.IsEmpty || (currentPosition.X ==0 && currentPosition.Y == 0))
            {
                e.Graphics.FillRectangle(Brushes.Black, currentSquarePosition.X - 7, currentSquarePosition.Y - 7, 15, 15);
            }
        }
        private void MoveMarker(Point location)
        {
            pictureBoxMap.Refresh(); // Очищаем pictureBox для обновления маркера

            DrawMarkers(); // Повторно отрисовываем офис и ящики

            using (Pen pen = new Pen(Color.Black, 2))
            {
                Graphics g = pictureBoxMap.CreateGraphics();
                for (int i = 1; i < positions.Count; i++)
                {
                    if (!isReturning || i <= positions.IndexOf(returnedPositions.LastOrDefault()))
                    {
                        g.DrawLine(pen, positions[i - 1], positions[i]); // Рисуем линии между позициями квадрата
                    }
                    else
                    {
                        g.DrawLine(pen, positions[i - 1], positions[i]);
                    }
                }
            }

            DrawMarker(location, Color.Black, 15, 15); // Рисуем квадрат в новой позиции
            positions.Add(location); // Добавляем текущую позицию в список позиций
        }

        private void DrawMarkers()
        {
            // Отрисовываем офис и ящики
            DrawMarker(postOfficeLocation, Color.Blue, 30, 30); // Почтовый офис
            foreach (Point mailbox in mailboxLocations)
            {
                DrawMarker(mailbox, Color.Green, 15, 15); // Почтовые ящики
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Очистка pictureBoxMap
                pictureBoxMap.Refresh();
                postOfficeLocation.X = 0; postOfficeLocation.Y = 0;
                // Очистка списков и сброс переменных
                optimizedRoute.Clear();
                positions.Clear();
                officeToMailbox.Clear();
                returnedPositions.Clear();
                currentStep = 0;
                lettersDelivered = 0;
                isReturning = false;
                // Очистка текстовых полей
                textBoxCapacity.Text = string.Empty;

                currentStep = 0;
                currentPosition.X =0; currentPosition.Y = 0;
                currentSquarePosition.X = 0; currentSquarePosition.Y = 0;
                mailboxLocations.Clear();
                bagCapacity = 0;
                lettersDelivered = 0;
                isReturning = false;
                positions.Clear();
                returnedPositions.Clear();
                officeToMailbox.Clear();
                isOfficeToMailboxDrawn = false;

                // Остановка таймера
                MoveTimer.Stop();
            }
            catch
            {
                MessageBox.Show("Для очистки программы нужно хотя бы раз её запустить!");
            }
        }
    }
}
