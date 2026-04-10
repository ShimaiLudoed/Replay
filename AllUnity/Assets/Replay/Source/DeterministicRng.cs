using UnityEngine;

public class DeterministicRng : IRng
{
  private System.Random random;
  private int seed;

  public DeterministicRng(int seed)
  {
    SetSeed(seed);
  }

  public void SetSeed(int seed)
  {
    this.seed = seed;
    random = new System.Random(seed);
  }

  public int GetSeed() => seed;

  public int Next(int min, int max)
  {
    return random.Next(min, max);
  }

  public float NextFloat()
  {
    return (float)random.NextDouble();
  }
}