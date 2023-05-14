using System.Linq;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBlocksController : MonoBehaviour
{

    [SerializeField] private GridController gridController;

    private void Awake()
    {
        GridController.onAddBlock += UpdateTiles;
        GridController.updateTile += UpdateTiles;
        GridController.onClickTile += OnClickTile;
    }

    private void UpdateTiles(TileBase tile)
    {

        if (tile.currentBlock is not NormalBlock)
            return;

        if (tile.currentBlock.updated)
            return;

        var adjacentEqualBlocks = gridController.GetAllAdjacentEqualBlocks(tile.coordinate, tile.currentBlock.blockId);

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
            var normalBlock = (NormalBlock)t.currentBlock;
            normalBlock.SetState(updatedState);
            normalBlock.adjacentTiles = adjacentEqualBlocks;
            normalBlock.updated = true;
            t.busy = false;
        });

    }

    private void OnClickTile(TileBase tile)
    {

        if (tile.currentBlock is not NormalBlock)
            return;

        var block = tile.currentBlock as NormalBlock;

        if (block.readyState == NormalBlock.ReadyState.normal)
            return;

        switch(block.readyState)
        {
            case NormalBlock.ReadyState.destroyable:
                {

                    var differentAdjacentTiles = new List<TileBase>();

                    block.adjacentTiles.ForEach(t =>
                    {
                        differentAdjacentTiles.AddRange(gridController.GetDifferentAdjacentTiles(t));
                        t.currentBlock.Destroy();
                    });

                    differentAdjacentTiles.ForEach(t =>
                    {
                        if(t.currentBlock != null)
                        {
                            t.currentBlock.OnNeighbourDestroyed();
                        }
                    });

                    gridController.UpdateGrid();
                }
                break;
            case NormalBlock.ReadyState.rocket:
                {
                    var differentAdjacentTiles = new List<TileBase>();

                    block.adjacentTiles.Remove(tile);

                    block.adjacentTiles.ForEach(t =>
                    {
                        differentAdjacentTiles.AddRange(gridController.GetDifferentAdjacentTiles(t));
                        t.currentBlock.Destroy();
                    });

                    differentAdjacentTiles.ForEach(t =>
                    {
                        if (t.currentBlock != null)
                        {
                            t.currentBlock.OnNeighbourDestroyed();
                        }
                    });

                    block.Destroy();

                    var rocketPrefab = Random.Range(0, 2) == 0 ? GameSettings.GameConfig.horizontalRocket : GameSettings.GameConfig.verticalRocket;

                    var rocket = Instantiate(rocketPrefab, tile.transform);
                    rocket.Setup();
                    tile.AddBlock(rocket);

                    gridController.UpdateGrid();
                }
                break;
            case NormalBlock.ReadyState.bomb:
                {
                    var differentAdjacentTiles = new List<TileBase>();

                    block.adjacentTiles.Remove(tile);

                    block.adjacentTiles.ForEach(t =>
                    {
                        differentAdjacentTiles.AddRange(gridController.GetDifferentAdjacentTiles(t));
                        t.currentBlock.Destroy();
                    });

                    differentAdjacentTiles.ForEach(t =>
                    {
                        if (t.currentBlock != null)
                        {
                            t.currentBlock.OnNeighbourDestroyed();
                        }
                    });

                    block.Destroy();

                    var bomb = Instantiate(GameSettings.GameConfig.bomb, tile.transform);
                    bomb.Setup();
                    tile.AddBlock(bomb);

                    gridController.UpdateGrid();
                }
                break;
            case NormalBlock.ReadyState.globe:
                break;
        }

    }

}
