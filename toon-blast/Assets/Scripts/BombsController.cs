using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombsController : MonoBehaviour
{

    [SerializeField] private GridController gridController;

    private void Awake()
    {
        GridController.onClickTile += OnClickTile;
    }

    private void OnClickTile(TileBase tile)
    {
        if (tile.currentBlock is not BombBlock)
            return;

        var block = tile.currentBlock as BombBlock;

        var tilesAround = gridController.GetTilesAround(tile.coordinate);

        block.Play();

        foreach (var item in tilesAround)
        {
            if (item.currentBlock != null)
                item.currentBlock.Destroy();
        }

        block.Destroy();

        gridController.UpdateGrid();

    }
}
