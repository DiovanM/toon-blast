using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Game Config")]
public class GameConfig : ScriptableObject
{

    public LevelDataReaderSO levelDataReader;

    public List<NormalBlock> normalBlocks;

    public RocketBlock horizontalRocket;
    public RocketBlock verticalRocket;

    public BombBlock bomb;

    public StringGlobeBlockDictionary globeBlocks;

    public int destroyCondition = 2;
    public int rocketCondition = 5;
    public int bombCondition = 7;
    public int globeCondition = 9;

}
