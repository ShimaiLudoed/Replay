using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class AttackPayload
{
  public int Damage;
  public string TargetTag;
}