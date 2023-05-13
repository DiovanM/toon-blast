using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{

    [SerializeField] private List<TileBase> tiles;
    [SerializeField] private BlockSpawner blockSpawner;

    public Dictionary<Vector2Int, TileBase> grid = new ();

    private void Awake()
    {

        var tilesQueue = new Queue<TileBase>(tiles);

        for (int i = 0; i < 9; i++)
        {

            for (int j = 0; j < 9; j++)
            {

                var tile = tilesQueue.Dequeue();

                var coordinate = new Vector2Int(j, i);

                tile.coordinate = coordinate;
                tile.label.text = coordinate.ToString();
                grid.Add(coordinate, tile);
            }
        }

    }

    private void Start()
    {
        SetupLevel();
    }

    private void SetupLevel()
    {

        var levelData = GameSettings.GameConfig.levelDataReader.levelsData[0];

        foreach (var item in levelData.data)
        {

            var block = Instantiate(item.Value, transform);
            block.Setup();
            var tile = grid[item.Key];

            tile.AddBlock(block);
        }

    }

}
