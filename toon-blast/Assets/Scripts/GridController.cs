using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{

    public static Action<TileBase> onAddBlock;
    public static Action<TileBase> onClickTile;

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

                tile.onBlockClicked = onClickTile;
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
            onAddBlock?.Invoke(tile);
        }

    }

    public void DestroyedBlocks(List<TileBase> tiles)
    {

    }

    public List<TileBase> GetAdjacentTiles(Vector2Int coordinate)
    {
        var tiles = new List<TileBase>();

        if (grid.TryGetValue(coordinate + Vector2Int.left, out var left))
            tiles.Add(left);
        if (grid.TryGetValue(coordinate + Vector2Int.right, out var right))
            tiles.Add(right);
        if (grid.TryGetValue(coordinate + Vector2Int.up, out var up))
            tiles.Add(up);
        if (grid.TryGetValue(coordinate + Vector2Int.down, out var down))
            tiles.Add(down);

        return tiles;
    }

    public List<TileBase> GetDifferentAdjacentTiles(TileBase tile)
    {
        var tiles = new List<TileBase>();
        var blockId = tile.currentBlock.blockId;

        if (grid.TryGetValue(tile.coordinate + Vector2Int.left, out var left))
            if(left.currentBlock != null && left.currentBlock.blockId != blockId)
                tiles.Add(left);
        if (grid.TryGetValue(tile.coordinate + Vector2Int.right, out var right))
            if(right.currentBlock != null && right.currentBlock.blockId != blockId)
                tiles.Add(right);
        if (grid.TryGetValue(tile.coordinate + Vector2Int.up, out var up))
            if (up.currentBlock != null && up.currentBlock.blockId != blockId)
                tiles.Add(up);
        if (grid.TryGetValue(tile.coordinate + Vector2Int.down, out var down))
            if (down.currentBlock != null && down.currentBlock.blockId != blockId)
                tiles.Add(down);

        return tiles;
    }

    public List<TileBase> GetAllAdjacentEqualBlocks(Vector2Int coordinate, string blockId)
    {

        var equalTiles = new List<TileBase>();

        equalTiles.AddRange(GetAdjacentBlocksOfId(coordinate + Vector2Int.up, blockId));
        equalTiles.AddRange(GetAdjacentBlocksOfId(coordinate + Vector2Int.right, blockId));
        equalTiles.AddRange(GetAdjacentBlocksOfId(coordinate + Vector2Int.down, blockId));
        equalTiles.AddRange(GetAdjacentBlocksOfId(coordinate + Vector2Int.left, blockId));

        return equalTiles;
    }

    public List<TileBase> GetAdjacentBlocksOfId(Vector2Int coordinate, string blockId)
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
