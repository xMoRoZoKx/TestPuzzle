using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(menuName = "Configs/TileConnection", fileName = "TileConnection")]
public class WireConnection : ScriptableObject
{
    public Sprite icon;
    public TileType tileType;
    public List<Patch> patches;
}
[System.Serializable]
public class Patch
{
    public List<SideType> patch;
}
public enum SideType
{
    Left,
    Right,
    Up,
    Bottom
}
public enum TileType
{
    Default,
    Start,
    Finish
}