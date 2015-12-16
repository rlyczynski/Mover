namespace Mover
{
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Mover App");
            
            var random = new Random();
            int x;
            int y;

            while (true)
            {

                var bounds = Screen.PrimaryScreen.Bounds;

                //x = random.Next(bounds.Width);
                //y = random.Next(bounds.Height);

                //ChangePosition(new Point(x, y), new TimeSpan(0, 0, 1));

                Point oldPoint = Cursor.Position;
                Point positivePoint = oldPoint;
                positivePoint.X += 10;

                ChangePosition(positivePoint, new TimeSpan(0, 0, 0, 0, 25));
                ChangePosition(oldPoint, new TimeSpan(0, 0, 0, 0, 25));
                
                Thread.Sleep(new TimeSpan(0, 0, 5));
            }
        }

        private static void ChangePosition(Point dest, TimeSpan timeSpan)
        {
            var sw = new Stopwatch();

            Point initPos = Cursor.Position;
            Point currPos = initPos;

            var xDir = dest.X > initPos.X ? 1 : -1;
            var yDir = dest.Y > initPos.Y ? 1 : -1;
            var xDist = Math.Abs(dest.X - initPos.X);
            var yDist = Math.Abs(dest.Y - initPos.Y);

            var input = GetCursorInput();

            sw.Start();
            while (sw.ElapsedMilliseconds < timeSpan.TotalMilliseconds)
            {
                var percentComplete = sw.ElapsedMilliseconds / timeSpan.TotalMilliseconds;
                var moveXDist = xDist * percentComplete;
                var moveYDist = yDist * percentComplete;

                var newX = (int)(initPos.X + (moveXDist * xDir));
                var newY = (int)(initPos.Y + (moveYDist * yDir));

                newX = xDir > 0 ? newX > dest.X ? dest.X : newX : newX < dest.X ? dest.X : newX;
                newY = yDir > 0 ? newY > dest.Y ? dest.Y : newY : newY < dest.Y ? dest.Y : newY;

                input.Input.mouseInput.dx = newX - currPos.X;
                input.Input.mouseInput.dy = newY - currPos.Y;

                Win32.SendInput(1, new[] { input }, Marshal.SizeOf(input));

                currPos = Cursor.Position;
            }
            sw.Stop();

            // One last move just to make sure we landed in the right spot.
            input.Input.mouseInput.dx = dest.X - currPos.X;
            input.Input.mouseInput.dy = dest.Y - currPos.Y;

            Win32.SendInput(1, new[] { input }, Marshal.SizeOf(input));
        }

        public static Win32.InputWrapper GetCursorInput()
        {
            return 
                new Win32.InputWrapper()
                {
                    dwType = Win32.InputType.Mouse,
                    Input = new Win32.Input()
                    {
                        mouseInput = new Win32.MouseInput()
                        {
                            dx = 0,
                            dy = 0,
                            mouseData = 0,
                            dwFlags = Win32.MouseEventFlags.Move,
                            time = 0,
                            dwExtraInfo = IntPtr.Zero
                        }
                    }
                };
        }
    }
}
