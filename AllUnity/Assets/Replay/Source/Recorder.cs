using System.Collections.Generic;
using UnityEngine;

public class Recorder : MonoBehaviour
{
  private bool isRecording = false;
  private List<Command> recordedCommands = new List<Command>();
  private TickManager tickManager;
  private int startTick;
  private int seed;

  void Start()
  {
    tickManager = FindObjectOfType<TickManager>();
  }

  public void StartRecording(int seed)
  {
    isRecording = true;
    recordedCommands.Clear();
    this.seed = seed;
    startTick = tickManager.CurrentTick;
    Debug.Log($"[Recorder] Recording started at tick {startTick}, seed {seed}");
  }

  public void StopRecording()
  {
    isRecording = false;
    Debug.Log($"[Recorder] Recording stopped. Commands recorded: {recordedCommands.Count}");
  }

  public void RecordCommand(string type, object payload = null)
  {
    if (!isRecording) return;

    var cmd = new Command(tickManager.CurrentTick - startTick, type, payload);
    recordedCommands.Add(cmd);
  }

  public ReplayData GetReplayData()
  {
    var data = new ReplayData(tickManager.TickRate, seed);
    data.Commands = recordedCommands;
    data.DurationTicks = recordedCommands.Count > 0 ? recordedCommands[recordedCommands.Count - 1].Tick : 0;
    return data;
  }

  public bool IsRecording => isRecording;
}