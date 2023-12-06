using System.Drawing;

namespace Tests1
{
    [TestClass]
    public class UnitTest1
    {
        public class PointHelper
        {
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
        }


        [TestMethod]
        public void TestMethod1()
        {
            // Arrange
            Point currentPoint = new Point(0, 0);
            List<Point> allPoints = new List<Point>
            {
                new Point(1, 1),
                new Point(2, 2),
                new Point(3, 3)
            };
            HashSet<Point> visitedPoints = new HashSet<Point>();

            // Act
            PointHelper pointHelper = new PointHelper();
            Point result = pointHelper.FindNearestPoint(currentPoint, allPoints, visitedPoints);

            // Assert
            Assert.AreEqual(new Point(1, 1), result);
        }

        [TestMethod]
        public void TestMethod2()
        {
            // Arrange
            Point currentPoint = new Point(0, 0);
            List<Point> allPoints = new List<Point>
            {
                new Point(10, 10),
                new Point(20, 20),
                new Point(30, 30)
            };
            HashSet<Point> visitedPoints = new HashSet<Point>();

            // Act
            PointHelper pointHelper = new PointHelper();
            Point result = pointHelper.FindNearestPoint(currentPoint, allPoints, visitedPoints);

            // Assert
            Assert.AreEqual(new Point(10, 10), result);
        }

        [TestMethod]
        public void TestMethod3()
        {
            // Arrange
            Point currentPoint = new Point(0, 0);
            List<Point> allPoints = new List<Point>
            {
                new Point(11, 11),
                new Point(22, 22),
                new Point(33, 33)
            };
            HashSet<Point> visitedPoints = new HashSet<Point>();

            // Act
            PointHelper pointHelper = new PointHelper();
            Point result = pointHelper.FindNearestPoint(currentPoint, allPoints, visitedPoints);

            // Assert
            Assert.AreEqual(new Point(11, 11), result);
        }
    }
}