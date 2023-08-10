using CommandLine;

namespace WaveRecorder.Infra;

public abstract class App<TArgs>
{
    const int DelayTime = 500;
    public void Start(params string[] args)
    {   
        Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight * 2);
        var parsedArgs = Parser.Default.ParseArguments<TArgs>(args).Value;
        Start(parsedArgs);
    }

    public abstract void Init(TArgs args);

    public abstract void Update();

    public abstract void Draw();

    private void Start(TArgs args)
    {
        Init(args);

        var uiThread = new Thread(() =>
        {
            while (true)
            {
                ConsoleDraw(() => Draw());
                Thread.Sleep(DelayTime);
            }
        });

        uiThread.Start();

        while (true)
        {
            Update();

            Thread.Sleep(DelayTime);
        }
    }

    private void ConsoleDraw(Action action)
    {
        Console.CursorVisible = false;
        Console.CursorLeft = 0;
        Console.CursorTop = Console.WindowHeight;

        action();

        Console.MoveBufferArea(0, Console.WindowHeight, Console.WindowWidth, Console.WindowHeight, 0, 0);

        Console.CursorLeft = 0;
        Console.CursorTop = 0;
    }
}
