using SGP.XUtility;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class Program
{
    private readonly Dictionary<string, Vector2> _players = new Dictionary<string, Vector2>();
    private string error;
    private static void Main() => new Program().Handler();

    private void Handler()
    {
        XNetworking.ListeningPort = 8081;

        if (XNetworking.StartListener(out error))
        {
            while (true)
            {
                if (XNetworking.Pending)
                {
                    Log("Incoming new connection!");
                }
                else if (_players.Count == 0)
                {
                    Thread.Sleep(100);
                    continue;
                }
                byte[] buffer = XNetworking.ReadBytes(out string sender, out error);
                if (string.IsNullOrEmpty(error))
                {
                    Command command = (Command)buffer[0];

                    if (!_players.ContainsKey(sender))
                    {
                        Log($"Player {sender} joined!");
                        _players.Add(sender, new Vector2());
                        SpawnNewPlayer(sender);
                    }
                    else if(command != Command.New)
                    {
                        PlayerMove(sender, command);
                    }

                }
                else
                {

                }

            }
            Log("Stopping listener");
            XNetworking.CloseListener(out error);
            if (!string.IsNullOrEmpty(error))
                Log(error, true);
        }
        else 
        {
            if (!string.IsNullOrEmpty(error))
                Log(error, true);
        }
        Console.ReadKey();
    }

    private void PlayerMove(string sender, Command command)
    {
        switch(command)
        {
            case Command.Forward:
                _players[sender].Move(true);
                break;
            case Command.Backward:
                _players[sender].Move(false);
                break;
            case Command.TurnLeft:
                _players[sender].TurnLeft();
                break;
            case Command.TurnRight:
                _players[sender].TurnRight();
                break;
        }


        foreach (KeyValuePair<string, Vector2> player in _players)
        {
            XNetworking.SendTo(player.Value.MoveCord(command), sender, out error);
            if (!string.IsNullOrEmpty(error))
                Log(error, true);
        }
    }

    private void SpawnNewPlayer(string sender)
    {
        Log($"Spawning {sender}");
        foreach (KeyValuePair<string, Vector2> player in _players)
        {
            XNetworking.SendTo(_players[sender].SpawnCord(), player.Key, out error);
            if (!string.IsNullOrEmpty(error))
                Log(error, true);
            XNetworking.SendTo(player.Value.SpawnCord(), sender, out error);
            if (!string.IsNullOrEmpty(error))
                Log(error, true);
        }

    }

    private void Log(string message, bool error = false)
    {
        if (error)
            Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[{DateTime.Now:G}] {message}");
        if (error)
            Console.ForegroundColor = ConsoleColor.White;
    }
}
