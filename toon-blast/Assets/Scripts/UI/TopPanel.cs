using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TopPanel : MonoBehaviour
{

    [SerializeField] private Image goalIcon;
    [SerializeField] private TextMeshProUGUI goalCounter;
    [SerializeField] private TextMeshProUGUI movesCounter;
    [SerializeField] private GameObject finishedGoalIcon;

    private int goal;
    private int moves;

    private LevelData currentLevelData;

    private void Start()
    {
        currentLevelData = GameSettings.GameConfig.levelDataReader.levelsData[0];

        goalIcon.sprite = currentLevelData.goalBlock.startingSprite;
        goal = currentLevelData.goalValue;
        moves = currentLevelData.moves;

        GridController.onPerformMove += OnPerformMove;
        GridController.onBlockDestroyed += OnBlockDestroyed;

        SetUIValues();
    }

    private void OnPerformMove()
    {
        moves--;

        SetUIValues();
    }

    private void OnBlockDestroyed(TileBase tile)
    {
        if (tile.currentBlock.blockId != currentLevelData.goalBlock.blockId)
            return;

        goal--;

        SetUIValues();
    }

    private void SetUIValues()
    {
        if(goal <= 0)
        {
            goalCounter.gameObject.SetActive(false);
            finishedGoalIcon.SetActive(true);
        }
        else
        {
            goalCounter.text = goal.ToString();
            movesCounter.text = moves.ToString();
        }
    }

}
