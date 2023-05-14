using System;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public static Action finishLevel;
    public static Action levelFailed;

    private int movesCounter;
    private int goalCounter;

    private LevelData currentLevelData;

    private void Awake()
    {
        finishLevel = null;
        levelFailed = null;
    }

    private void Start()
    {
        currentLevelData = GameSettings.GameConfig.levelDataReader.levelsData[0];

        movesCounter = currentLevelData.moves;
        goalCounter = currentLevelData.goalValue;

        GridController.onPerformMove += OnPerformMove;
        GridController.onBlockDestroyed += OnBlockDestroyed;

        GameAnalyticsSDK.GameAnalytics.NewProgressionEvent(GameAnalyticsSDK.GAProgressionStatus.Start, "Level_1");
    }

    private void OnPerformMove()
    {
        movesCounter--;

        if (movesCounter <= 0)
        {
            if (goalCounter >= 0)
            {
                levelFailed?.Invoke();
                GameAnalyticsSDK.GameAnalytics.NewProgressionEvent(GameAnalyticsSDK.GAProgressionStatus.Fail, "Level_1");
            }
        }

    }

    private void OnBlockDestroyed(TileBase tile)
    {
        if (tile.currentBlock.blockId != currentLevelData.goalBlock.blockId)
            return;

        goalCounter--;

        if (goalCounter <= 0)
        {
            finishLevel?.Invoke();
            GameAnalyticsSDK.GameAnalytics.NewProgressionEvent(GameAnalyticsSDK.GAProgressionStatus.Complete, "Level_1");
        }
    }

}
