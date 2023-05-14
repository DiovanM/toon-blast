using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelData
{
    public BlockBase goalBlock;
    public int goalValue;
    public int moves;
    public CoordinateBlockBaseDictionary data = new ();
}
