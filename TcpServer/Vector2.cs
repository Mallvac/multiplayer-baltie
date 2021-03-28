public class Vector2
{
    private static int _avalibleID = 0;
    public int X { get; private set; }
    public int Y { get; private set; }
    public int A { get; private set; }
    public int ID { get; private set; }

    public Vector2()
    {
        this.X = 127;
        this.Y = 127;
        this.A = 0;
        this.ID = _avalibleID++;
    }

    public Vector2(byte x, byte y)
    {
        this.X = x;
        this.Y = y;
        this.A = 0;
        this.ID = _avalibleID++;
    }

    public void Move(bool forward)
    {
        int shift = forward ? 1 : -1;
        if (A == 0)
            Y += shift;
        if (A == 1)
            X += shift;
        if (A == 2)
            Y -= shift;
        if (A == 3)
            X -= shift;
    }

    public void TurnLeft() => A = A == 0 ? 3 : A + 1;

    public void TurnRight() => A = A == 3 ? 0 : A - 1;

    public byte[] MoveCord(Command command)
    {
        byte[] buffer = new byte[4];
        buffer[0] = (byte)command;
        buffer[1] = (byte)ID;
        return buffer;
    }

    public byte[] SpawnCord()
    {
        byte[] buffer = new byte[4];
        buffer[0] = (byte)Command.New;
        buffer[1] = (byte)ID;
        buffer[2] = (byte)X;
        buffer[3] = (byte)Y;
        return buffer;
    }
}