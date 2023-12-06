using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Mailman
{
    public partial class Form1 : Form
    {
        private List<Point> optimizedRoute; // ���������� ��� �������� ������������ ��������
        private int currentStep = 0; // ������� ��� � ��������
        private Point currentPosition; // ������� ������� ��������
        private System.Windows.Forms.Timer moveTimer;
        private Point currentSquarePosition;
        private Point postOfficeLocation;
        private List<Point> mailboxLocations = new List<Point>();
        private int bagCapacity; // ����������� ����� ����������
        private int lettersDelivered = 0; // ���������� ������������ �����
        private bool isReturning = false; // ���� �������� � ��������� ���������
        private List<Point> positions = new List<Point>(); // �������� ���� ������� ��������
        private List<Point> returnedPositions = new List<Point>(); // �������� ���� ������� ����������� � ��������� ���������
        private List<Point> officeToMailbox = new List<Point>(); // ������� �� ��������� ��������� � ���������� ����������� �����
        private bool isOfficeToMailboxDrawn = false; // ���� ��� ����������� �������� �� ����� � �����

        public Form1()
        {
            // ������������� ����������
            moveTimer = new System.Windows.Forms.Timer();
            moveTimer.Interval = 1000; // �������� �������� (1 �������)
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
                MessageBox.Show("����������, ���������, ��� ��� ������ ������� ���������.");
                return;
            }
            // �������� ����� ����������� ��������
            optimizedRoute = CalculateNearestNeighborRoute(mailboxLocations.ToList());

            if (int.TryParse(textBoxCapacity.Text, out int capacity))
            {
                bagCapacity = capacity;
            }
            else
            {
                MessageBox.Show("������� ���������� ����������� �����!");
                return;
            }

            // �������������� ��������� ��������
            currentStep = 0;
            currentPosition = postOfficeLocation;
            DrawMarkers(); // ������������ ��������� �������

            // �������� ������ ��� ����������� �� ��������
            MoveTimer.Interval = 1000; // ���������� �������� ������� � ������������� �� ������ ����������
            MoveTimer.Start();
            positions.Add(currentPosition); // ��������� ��������� �������
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
                    // ������� � ��������� ���������, ���� �� ������ ������������ ����
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
            // ������� ���������� ����� ����� ������� (��������, ��������� ����������)
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
                currentPosition = optimizedRoute[currentStep]; // ��������� ������� ������� ��������

                if (!isReturning)
                {
                    if (lettersDelivered < bagCapacity)
                    {
                        lettersDelivered++; // ����������� ���������� ������������ �����
                        officeToMailbox.Add(currentPosition); // ��������� ������� � �������� �� ����� � �����
                    }
                    else
                    {
                        isReturning = true; // ������������� ���� �������� � ��������� ���������
                        returnedPositions.Add(currentPosition); // ��������� ������� ������� � ������ ������� �������� � ���������
                        officeToMailbox.Reverse(); // �������������� ������� �� ����� � �����
                        currentStep--; // ������������ �� ���� ��� �����
                    }
                }
                else
                {
                    currentPosition = postOfficeLocation;
                    if (currentPosition == postOfficeLocation)
                    {
                        isReturning = false; // ���������� ���� �������� � ��������� ���������
                        lettersDelivered = 0; // �������� ���������� ������������ �����
                        officeToMailbox.Clear(); // ������� ������� �� ����� � �����
                        prevPositon = postOfficeLocation;
                    }
                }

                MoveMarker(currentPosition); // ���������� ������� �� �������� ���� � ��������
                currentStep++; // ��������� � ���������� ���� � ��������
            }
            else
            {
                MoveTimer.Stop(); // ���� ��������� ����� ��������, ������������� ������
                                  // ���������, ���� �� ���������� ������� �� ����� � ���������� ����������� �����
                if (officeToMailbox.Count > 0 && !isOfficeToMailboxDrawn)
                {
                    using (Pen pen = new Pen(Color.Black, 2))
                    {
                        Graphics g = pictureBoxMap.CreateGraphics();
                        List<Point> pointsToDraw = new List<Point>();

                        if (isReturning)
                        {
                            pointsToDraw.AddRange(returnedPositions);
                            pointsToDraw.Add(postOfficeLocation); // ��������� �������� ��������� ��� ���������� ����
                        }
                        else
                        {   
                            pointsToDraw.AddRange(officeToMailbox);
                        }

                        // ���� ��������� ����� �������� �������� ����������, ��������� � � ����
                        if (pointsToDraw.Last() != postOfficeLocation && pointsToDraw.Last() != optimizedRoute.Last())
                        {
                            pointsToDraw.Add(optimizedRoute.Last());
                        }

                        for (int i = 1; i < pointsToDraw.Count; i++)
                        {
                            g.DrawLine(pen, pointsToDraw[i - 1], pointsToDraw[i]); // ������ ����� ����� ���������
                        }
                    }
                    isOfficeToMailboxDrawn = true; // ������������� ����, ��� ������� ���������
                }
            }
        }

        private void pictureBoxMap_Paint(object sender, PaintEventArgs e)
        {
            // ����������� �������� �� PictureBox
            if (!currentSquarePosition.IsEmpty || (currentPosition.X ==0 && currentPosition.Y == 0))
            {
                e.Graphics.FillRectangle(Brushes.Black, currentSquarePosition.X - 7, currentSquarePosition.Y - 7, 15, 15);
            }
        }
        private void MoveMarker(Point location)
        {
            pictureBoxMap.Refresh(); // ������� pictureBox ��� ���������� �������

            DrawMarkers(); // �������� ������������ ���� � �����

            using (Pen pen = new Pen(Color.Black, 2))
            {
                Graphics g = pictureBoxMap.CreateGraphics();
                for (int i = 1; i < positions.Count; i++)
                {
                    if (!isReturning || i <= positions.IndexOf(returnedPositions.LastOrDefault()))
                    {
                        g.DrawLine(pen, positions[i - 1], positions[i]); // ������ ����� ����� ��������� ��������
                    }
                    else
                    {
                        g.DrawLine(pen, positions[i - 1], positions[i]);
                    }
                }
            }

            DrawMarker(location, Color.Black, 15, 15); // ������ ������� � ����� �������
            positions.Add(location); // ��������� ������� ������� � ������ �������
        }

        private void DrawMarkers()
        {
            // ������������ ���� � �����
            DrawMarker(postOfficeLocation, Color.Blue, 30, 30); // �������� ����
            foreach (Point mailbox in mailboxLocations)
            {
                DrawMarker(mailbox, Color.Green, 15, 15); // �������� �����
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // ������� pictureBoxMap
                pictureBoxMap.Refresh();
                postOfficeLocation.X = 0; postOfficeLocation.Y = 0;
                // ������� ������� � ����� ����������
                optimizedRoute.Clear();
                positions.Clear();
                officeToMailbox.Clear();
                returnedPositions.Clear();
                currentStep = 0;
                lettersDelivered = 0;
                isReturning = false;
                // ������� ��������� �����
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

                // ��������� �������
                MoveTimer.Stop();
            }
            catch
            {
                MessageBox.Show("��� ������� ��������� ����� ���� �� ��� � ���������!");
            }
        }
    }
}
