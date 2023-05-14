using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{

    public static Action<TileBase> onAddBlock;

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

        onAddBlock += OnAddBlock;

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
            onAddBlock?.Invoke(tile);
        }

    }

    private void OnAddBlock(TileBase tile)
    {

        if (tile.currentBlock is not NormalBlock)
            return;

        var adjacentEqualBlocks = GetAllAdjacentEqualBlocks(tile.coordinate, tile.currentBlock.blockId);

        var blocksAmount = adjacentEqualBlocks.Count;

        NormalBlock.ReadyState updatedState;

        if (blocksAmount >= GameSettings.GameConfig.globeCondition)
            updatedState = NormalBlock.ReadyState.globe;
        else if (blocksAmount >= GameSettings.GameConfig.bombCondition)
            updatedState = NormalBlock.ReadyState.bomb;
        else if (blocksAmount >= GameSettings.GameConfig.rocketCondition)
            updatedState = NormalBlock.ReadyState.rocket;
        else if (blocksAmount >= GameSettings.GameConfig.destroyCondition)
            updatedState = NormalBlock.ReadyState.destroyable;
        else
            updatedState = NormalBlock.ReadyState.normal;

        adjacentEqualBlocks.ForEach(t =>
        {
            ((NormalBlock)t.currentBlock).SetState(updatedState);
            t.busy = false;
        });

    }

    private List<TileBase> GetAllAdjacentEqualBlocks(Vector2Int coordinate, string blockId)
    {

        var equalTiles = new List<TileBase>();

        equalTiles.AddRange(GetAdjacentBlocksOfId(coordinate + Vector2Int.up, blockId));
        equalTiles.AddRange(GetAdjacentBlocksOfId(coordinate + Vector2Int.right, blockId));
        equalTiles.AddRange(GetAdjacentBlocksOfId(coordinate + Vector2Int.down, blockId));
        equalTiles.AddRange(GetAdjacentBlocksOfId(coordinate + Vector2Int.left, blockId));

        return equalTiles;
    }

    private List<TileBase> GetAdjacentBlocksOfId(Vector2Int coordinate, string blockId)
    {

        var equalTiles = new List<TileBase>();

        if (grid.TryGetValue(coordinate, out var tile))
        {

            if (!tile.busy && tile.currentBlock != null)
            {
                if (tile.currentBlock.blockId == blockId)
                {
                    equalTiles.Add(tile);
                    tile.busy = true;
                    equalTiles.AddRange(GetAllAdjacentEqualBlocks(tile.coordinate, blockId));
                }
            }
        }

        return equalTiles;
    }

}
