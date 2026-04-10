using UnityEngine;

public class TickManager : MonoBehaviour
{
    [SerializeField] private int tickRate = 20;
    private float tickDelta;
    private float timer;
    private int currentTick;
    private bool isRunning = true;

    public int CurrentTick => currentTick;
    public int TickRate => tickRate;
    public float TickDelta => tickDelta;

    public System.Action<int> OnTick;

    void Awake()
    {
        tickDelta = 1f / tickRate;
    }

    void Update()
    {
        if (!isRunning) return;

        timer += Time.deltaTime;
        while (timer >= tickDelta)
        {
            timer -= tickDelta;
            currentTick++;
            OnTick?.Invoke(currentTick);
        }
    }

    public void ResetTick()
    {
        currentTick = 0;
        timer = 0;
    }

    public void SetRunning(bool running)
    {
        isRunning = running;
    }
}
