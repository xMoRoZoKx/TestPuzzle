using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Configs/TileGrid", fileName = "TileGrid")]
public class TileGridSettings : ScriptableObject
{
    public Serializable2DArray<TileSetting> tilesData;
    public float timeForSolve = 45;
}
[System.Serializable]
public class TileSetting
{
    public WireConnection tile;
    public RotateType rotate;
}
public enum RotateType
{
    R0 = 0,
    R90 = 90,
    R180 = 180,
    R270 = 270
}