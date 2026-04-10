public static class EventBus
{
  private static System.Collections.Generic.Dictionary<string, System.Delegate> events =
    new System.Collections.Generic.Dictionary<string, System.Delegate>();

  public static void Trigger(string eventName)
  {
    if (events.ContainsKey(eventName))
      events[eventName]?.DynamicInvoke();
  }

  public static void Trigger<T>(string eventName, T param)
  {
    if (events.ContainsKey(eventName))
      events[eventName]?.DynamicInvoke(param);
  }

  public static void Register(string eventName, System.Action handler)
  {
    if (!events.ContainsKey(eventName))
      events[eventName] = handler;
    else
      events[eventName] = System.Delegate.Combine(events[eventName], handler);
  }

  public static void Register<T>(string eventName, System.Action<T> handler)
  {
    if (!events.ContainsKey(eventName))
      events[eventName] = handler;
    else
      events[eventName] = System.Delegate.Combine(events[eventName], handler);
  }
}