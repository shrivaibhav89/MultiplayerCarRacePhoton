using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JoinScreen : MonoBehaviour
{
    public InputField playerNameInputField;
    public Button startButton;
    string playerName;
    public PlayerSpawner playerSpawner;
    public void OnEnable()
    {
        startButton.onClick.AddListener(OnStartButtonClicked);
    }
    public void OnDisable()
    {
        startButton.onClick.RemoveListener(OnStartButtonClicked);
    }
    public void OnStartButtonClicked()
    {
        JoinGame(GameMode.AutoHostOrClient);
    }

    public void JoinGame(GameMode gameMode)
    {
        playerName = playerNameInputField.text;
        if (string.IsNullOrEmpty(playerName))
        {
            Debug.LogError("Player Name is empty");
            return;
        }
        GameManager.Instance.playerName = playerName;
        playerSpawner.StartGame(gameMode);
        gameObject.SetActive(false);
    }

}
