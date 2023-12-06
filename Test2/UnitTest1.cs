using System.Drawing;

namespace Test2
{
    [TestClass]
    public class UnitTest1
    {
        private Point postOfficeLocation = new Point(0, 0);
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

        public Point FindNearestPoint(Point currentPoint, List<Point> allPoints, HashSet<Point> visitedPoints)
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

        [TestMethod]
        public void TestMethod1()
        {
                // Создание списка точек
            List<Point> points = new List<Point>()
            {
                new Point(2, 2),
                new Point(5, 3),
                new Point(-1, 4)
            };

            // Ожидаемый маршрут
            List<Point> expectedRoute = new List<Point>()
            {
                new Point(0, 0),
                new Point(2, 2),
                new Point(5, 3),
                new Point(-1, 4)
            };

            // Выполнение метода
            List<Point> actualRoute = CalculateNearestNeighborRoute(points);

            // Проверка результатов
            Assert.AreEqual(expectedRoute.Count, actualRoute.Count);
            for (int i = 0; i < expectedRoute.Count; i++)
            {
                Assert.AreEqual(expectedRoute[i], actualRoute[i]);
            }
        }

        [TestMethod]
        public void TestMethod2()
        {
            // Создание списка точек
            List<Point> points = new List<Point>()
            {
                new Point(20, 20),
                new Point(50, 30),
                new Point(-10, 40)
            };

            // Ожидаемый маршрут
            List<Point> expectedRoute = new List<Point>()
            {
                new Point(0, 0),
                new Point(20, 20),
                new Point(50, 30),
                new Point(-10, 40)
            };

            // Выполнение метода
            List<Point> actualRoute = CalculateNearestNeighborRoute(points);

            // Проверка результатов
            Assert.AreEqual(expectedRoute.Count, actualRoute.Count);
            for (int i = 0; i < expectedRoute.Count; i++)
            {
                Assert.AreEqual(expectedRoute[i], actualRoute[i]);
            }
        }
    }
}