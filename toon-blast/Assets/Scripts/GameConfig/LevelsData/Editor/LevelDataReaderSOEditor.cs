using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(LevelDataReaderSO))]
public class LevelDataReaderSOEditor : Editor
{

    private LevelDataReaderSO dataReader => (LevelDataReaderSO)target;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        using(new GUILayout.HorizontalScope())
        {
            if (GUILayout.Button("Load Levels Data"))
            {
                LoadLevelsData();
            }
        }
    }

    public void LoadLevelsData()
    {

        dataReader.levelsData = new List<LevelData>();

        foreach (var file in dataReader.levelsDataFiles)
        {

            var levelData = new LevelData();

            var rows = file.text.Split("\n");

            var header = rows[0].Split(",");

            levelData.goalBlock = dataReader.keyBlockMap[header[0]];
            levelData.goalValue = int.Parse(header[1]);
            levelData.moves = int.Parse(header[2]);

            for (int i = 1; i < 10; i++)
            {

                var row = rows[i].Split(",");

                for (int j = 0; j < 9; j++)
                {

                    var cell = row[j];

                    var block = dataReader.keyBlockMap[cell.ToString()];

                    levelData.data.Add(new Vector2Int(j, 9 - i), block);
                }

            }

            dataReader.levelsData.Add(levelData);

        }

        EditorUtility.SetDirty(target);
        AssetDatabase.SaveAssets();

    }

}