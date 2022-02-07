using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CybernautX/Weapon")]
public class Weapon : ScriptableObject
{
    public enum Type { Shotgun, Flamethrower, Chainsaw, GrenadeLauncher }

    public Type type;
}
