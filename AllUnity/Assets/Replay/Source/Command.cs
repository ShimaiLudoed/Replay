using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Command
{
    public int Tick { get; private set; }
    public string Type{ get; private set; }
    public string PayLoad{ get; private set; }

    public Command(int tick, string type, object payloadObj = null)
    {
        Tick = tick;
        Type = type;
        PayLoad = payloadObj != null ? JsonUtility.ToJson(payloadObj) : "";
    }

    public T GetPayload<T>() where T : class
    {
        if (string.IsNullOrEmpty(PayLoad)) return null;
        return JsonUtility.FromJson<T>(PayLoad);
    }
}
