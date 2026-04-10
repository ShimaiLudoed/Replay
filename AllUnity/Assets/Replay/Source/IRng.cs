public interface IRng
{
  int Next(int min, int max);
  float NextFloat();
  void SetSeed(int seed);
  int GetSeed();
}
