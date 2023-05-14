using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RocketsController : MonoBehaviour
{

    [SerializeField] private GridController gridController;

    private void Awake()
    {
        GridController.onClickTile += OnClickTile;
    }

    private void OnClickTile(TileBase tile)
    {
        if (tile.currentBlock is not RocketBlock)
            return;

        var block = tile.currentBlock as RocketBlock;

        var higherDistance = 0;
        var speedPerBlock = 0.07f;

        for (int i = 0; i < 9; i++)
        {

            if(block.orientation == RocketBlock.Orientation.Horizontal)
            {
                var lineTile = gridController.grid[new Vector2Int(i, tile.coordinate.y)];
                var distance = Mathf.Abs(tile.coordinate.x - i);

                if (distance > higherDistance)
                    higherDistance = distance;

                if (distance != 0)
                {
                    GenericCoroutines.DoAfterTime(speedPerBlock * distance, () =>
                    {
                        if(lineTile.currentBlock != null)
                            lineTile.currentBlock.Destroy();
                    }, this);
                }

            }
            else
            {

                var columnTile = gridController.grid[new Vector2Int(tile.coordinate.x, i)];
                var distance = Mathf.Abs(tile.coordinate.y - i);

                if (distance > higherDistance)
                    higherDistance = distance;

                if (distance != 0)
                {
                    GenericCoroutines.DoAfterTime(speedPerBlock * distance, () =>
                    {
                        if (columnTile.currentBlock != null)
                            columnTile.currentBlock.Destroy();
                    }, this);
                }

            }

        }

        block.Play();

        GenericCoroutines.DoAfterTime(higherDistance * speedPerBlock, () =>
        {
            block.Destroy();
            gridController.UpdateGrid();
        }, this);
    }

}
