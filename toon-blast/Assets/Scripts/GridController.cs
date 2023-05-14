using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GridController : MonoBehaviour
{

    public static Action onPerformMove;
    public static Action<TileBase> onAddBlock;
    public static Action<TileBase> onClickTile;
    public static Action<TileBase> onBlockDestroyed;

    public static Action<TileBase> updateTile;

    [SerializeField] private List<TileBase> tiles;
    [SerializeField] private BlockSpawner blockSpawner;

    public Dictionary<Vector2Int, TileBase> grid = new ();

    private bool clickEnabled;

    private void Awake()
    {
        onPerformMove = null;
        onAddBlock = null;
        onClickTile = null;
        onBlockDestroyed = null;

        GameController.levelFailed += () => clickEnabled = false;
        GameController.finishLevel += () => clickEnabled = false;

        var tilesQueue = new Queue<TileBase>(tiles);

        for (int i = 0; i < 9; i++)
        {

            for (int j = 0; j < 9; j++)
            {

                var tile = tilesQueue.Dequeue();

                var coordinate = new Vector2Int(j, i);

                tile.pointerHandler.onPointerClick += (e) =>
                {
                    if (clickEnabled)
                        onPerformMove?.Invoke();
                };

                tile.onBlockClicked = (t) =>
                {
                    if(clickEnabled)
                        onClickTile?.Invoke(t);
                };
                tile.coordinate = coordinate;
                tile.onBlockDestroyed = (t) => 
                {
                    onBlockDestroyed?.Invoke(t);
                };
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

        clickEnabled = true;

    }

    public void UpdateGrid()
    {

        clickEnabled = false;

        var tilesToUpdate = 0;

        for (int i = 0; i < 9; i++)
        {

            var emptyTiles = new List<TileBase>();
            var tilesToMove = new List<TileBase>();

            for (int j = 0; j < 9; j++)
            {

                var tile = grid[new Vector2Int(i, j)];

                if (tile.currentBlock == null)
                    emptyTiles.Add(tile);
                else if (emptyTiles.Count > 0)
                    tilesToMove.Add(tile);

                if(j == 8)
                {
                    tilesToUpdate += emptyTiles.Count + tilesToMove.Count;

                    if(emptyTiles.Count > 0)
                    {
                        var startingCoordinate = emptyTiles[0].coordinate;

                        var l = 0;
                        for (int k = startingCoordinate.y; k < 9; k++)
                        {
                            var targetCoordinate = startingCoordinate;
                            targetCoordinate.y += l;

                            var target = grid[targetCoordinate];

                            if (l > tilesToMove.Count - 1)
                            {
                                var newBlock = blockSpawner.SpawnBlock(targetCoordinate.x);

                                newBlock.transform.DOMove(target.transform.position, .2f)
                                    .SetEase(Ease.Linear)
                                    .OnComplete(() =>
                                    {
                                        target.AddBlock(newBlock);

                                        tilesToUpdate--;
                                        if (tilesToUpdate == 0)
                                            UpdateBlocks();
                                    });
                            }
                            else
                            {

                                var tileToMove = tilesToMove[l];

                                var block = tileToMove.currentBlock;
                                tileToMove.RemoveBlock();

                                block.transform.DOMove(target.transform.position, .2f)
                                    .SetEase(Ease.Linear)
                                    .OnComplete(() =>
                                    {
                                        target.AddBlock(block);

                                        tilesToUpdate--;
                                        if (tilesToUpdate == 0)
                                            UpdateBlocks();
                                    });
                            }
                            l++;
                        }
                    }
                    else
                    {
                        clickEnabled = true;
                    }

                }

            }
        }

    }

    public void UpdateBlocks()
    {
        foreach(var item in grid)
        {
            if(item.Value.currentBlock != null)
                item.Value.currentBlock.updated = false;
        }

        foreach (var item in grid)
        {
            updateTile?.Invoke(item.Value);
        }

        clickEnabled = true;
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

    public List<TileBase> GetTilesAround(Vector2Int coordinate)
    {
        var tiles = new List<TileBase>();

        tiles.AddRange(GetAdjacentTiles(coordinate));

        if (grid.TryGetValue(coordinate + Vector2Int.up + Vector2Int.left, out var upLeft))
            tiles.Add(upLeft);
        if (grid.TryGetValue(coordinate + Vector2Int.up + Vector2Int.right, out var upRight))
            tiles.Add(upRight);
        if (grid.TryGetValue(coordinate + Vector2Int.down + Vector2Int.left, out var downLeft))
            tiles.Add(downLeft);
        if (grid.TryGetValue(coordinate + Vector2Int.down + Vector2Int.right, out var downRight))
            tiles.Add(downRight);

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

    public List<TileBase> GetTilesOfId(string blockId)
    {

        var tiles = new List<TileBase>();

        foreach(var item in grid)
        {
            if(item.Value.currentBlock != null)
            {
                if (item.Value.currentBlock.blockId == blockId)
                    tiles.Add(item.Value);
            }
        }

        return tiles;

    }

}
