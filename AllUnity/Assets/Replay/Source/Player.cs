using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private TickManager tickManager;
    [SerializeField] private CommandSource commandSource;
    private bool isPlaying = false;
    private ReplayData currentReplay;
    private int currentCommandIndex;
    private Dictionary<int, List<Command>> tickCommandMap;
    public void Play(ReplayData replay)
    {
        if (isPlaying) return;

        currentReplay = replay;
        isPlaying = true;
        currentCommandIndex = 0;
        
        commandSource?.SetInputEnabled(false);
        
        tickManager.ResetTick();
        tickManager.SetRunning(true);
        
        tickCommandMap = new Dictionary<int, List<Command>>();
        foreach (var cmd in replay.Commands)
        {
            if (!tickCommandMap.ContainsKey(cmd.Tick))
                tickCommandMap[cmd.Tick] = new List<Command>();
            tickCommandMap[cmd.Tick].Add(cmd);
        }
        
        tickManager.OnTick += OnTick;
    }

    private void OnTick(int tick)
    {
        if (!isPlaying) return;
        
        if (tickCommandMap.ContainsKey(tick))
        {
            foreach (var cmd in tickCommandMap[tick])
            {
                commandSource?.ExecuteCommand(cmd);
            }
        }

        if (tick > currentReplay.DurationTicks + 10) 
        {
            StopPlayback();
        }
    }

    public void StopPlayback()
    {
        if (!isPlaying) return;

        isPlaying = false;
        tickManager.OnTick -= OnTick;
        commandSource?.SetInputEnabled(true);
    }
    public bool IsPlaying => isPlaying;
}