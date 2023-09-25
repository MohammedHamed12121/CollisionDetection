using System.ComponentModel;
using System.Globalization;
using System.Numerics;
using Microsoft.VisualBasic;
using Raylib_cs;


namespace HelloWorld;



public class Ball
{
    public float speed;
    public float acc;
    public Vector2 position = new ();
    public Vector2 velocity;
    public Vector2 acceleration ;
    public int raduis;
    int topPoint  ;
    int downPoint ;
    int rightPoint;
    int leftPoint ;

    Color ballColor;
    public IList<int> speeds = new List<int>{200,-200};

    public Ball(int x, int y, int r ,Color color)
    {
        // color = colors[new Random().Next( colors.Count )];
        position.X = x;
        position.Y = y;
        raduis = r;
        speed = speeds[new Random().Next(speeds.Count)];

        acc = 5;
        velocity = new Vector2(speed,speed);
        acceleration = new Vector2(acc);

        ballColor = color;
        
    }

    public void Draw()
    {
        Raylib.DrawCircle((int) position.X,(int) position.Y, raduis, ballColor);
    }

    public void Update(int fbs)
    {
        float dt =(float) 1/fbs;

        velocity +=  acceleration*dt ;
        position +=  velocity * dt ;
        WallCollision();
    }

    private void WallCollision()
    {

        topPoint   = (int)position.Y - raduis;
        downPoint  = (int)position.Y + raduis;
        rightPoint = (int)position.X + raduis;
        leftPoint  = (int)position.X - raduis;

        if(downPoint >= Raylib.GetScreenHeight())
            velocity.Y = -1 * Math.Abs(velocity.Y);
        
        if(topPoint <0)
            velocity.Y =  Math.Abs(velocity.Y);

        if(leftPoint <= 0)
            velocity.X = Math.Abs(velocity.X);
            
        if(rightPoint >= Raylib.GetScreenWidth())
            velocity.X = -1 * Math.Abs(velocity.X);
    }
    
    public void BallCollision(Ball ball1)
    {
        if(IsColliding(ball1))
        {
            if(position.X >= ball1.position.X)
                velocity.X = Math.Abs(velocity.X);

            if(position.X <= ball1.position.X)
                velocity.X = -1* Math.Abs(velocity.X);

            if(position.Y >= ball1.position.Y)
                velocity.Y = Math.Abs(velocity.Y);

            if(position.Y <= ball1.position.Y)
                velocity.Y = -1* Math.Abs(velocity.Y);
        }

    }

    private bool IsColliding(Ball b1)
    {
        float dx = b1.position.X - position.X;
        float dy = b1.position.Y - position.Y;
        double distance = Math.Sqrt(dx * dx + dy * dy);

        if (distance < b1.raduis + raduis)  return true;
        return false;
    }
}

class Program
{
    public static void Main()
    {
        int FPS = 60;
        var ballArray = new List<Ball>();
        int ballCount = 20;
        var colors = new List<Color> {Color.WHITE,Color.BEIGE,Color.VIOLET,Color.MAROON,Color.PURPLE,Color.BROWN,
                                        Color.BLUE, Color.LIME,Color.YELLOW,Color.DARKPURPLE};

        Raylib.InitWindow(800, 600, "Hello World");

        Raylib.SetTargetFPS(FPS);

        for(int i= 0; i< ballCount ; i++)
        {
            int randomPositionX = new Random().Next(800);
            int randomPositionY = new Random().Next(600);
            int randomRaduis = 20;
            Color randomColor = colors[new Random().Next(colors.Count)];

            ballArray.Add(new(randomPositionX,randomPositionY,randomRaduis,randomColor));
        }

        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.BLACK);
            
            for(int i = 0; i< ballArray.Count; i++)
            {
                ballArray[i].Update(FPS);

                for(int j =0; j< ballArray.Count; j++)
                {
                    if(i == j)  continue;
                    ballArray[i].BallCollision(ballArray[j]);
                }
                ballArray[i].Draw();
            }

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}