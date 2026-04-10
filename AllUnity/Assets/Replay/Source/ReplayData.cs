using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

[Serializable]
public class ReplayData
{
  public int Version = 1;
  public int TickRate;
  public int Seed;
  public int DurationTicks;
  public List<Command> Commands = new List<Command>();

  public ReplayData(int tickRate, int seed)
  {
    TickRate = tickRate;
    Seed = seed;
  }
}