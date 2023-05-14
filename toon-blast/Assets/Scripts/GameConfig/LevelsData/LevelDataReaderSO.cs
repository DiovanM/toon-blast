using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDataReader", menuName = "Level Data Reader")]
public class LevelDataReaderSO : ScriptableObject
{

    public StringBlockBaseDictionary keyBlockMap;

    public List<TextAsset> levelsDataFiles;

    public List<LevelData> levelsData;

}
