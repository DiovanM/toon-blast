using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Game Config")]
public class GameConfig : ScriptableObject
{

    public LevelDataReaderSO levelDataReader;

    public List<NormalBlock> normalBlocks;

}
