using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelFinishPopups : MonoBehaviour
{

    [SerializeField] private GameObject content;
    [SerializeField] private GameObject levelFinish;
    [SerializeField] private Button continueButton;
    [SerializeField] private GameObject levelFailed;
    [SerializeField] private Button tryAgainButton;

    private void Awake()
    {
        GameController.finishLevel += OpenFinish;
        GameController.levelFailed += OpenFailed;

        continueButton.onClick.AddListener(ReloadLevel);
        tryAgainButton.onClick.AddListener(ReloadLevel);
    }

    private void ReloadLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void OpenFinish()
    {
        levelFinish.SetActive(true);
        levelFailed.SetActive(false);

        content.SetActive(true);
    }

    private void OpenFailed()
    {
        levelFinish.SetActive(false);
        levelFailed.SetActive(true);

        content.SetActive(true);
    }

}
