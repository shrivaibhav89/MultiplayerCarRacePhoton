using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JoinScreen : MonoBehaviour
{
    public Button joinButton;
    public InputField playerNameInputField;
    public Button hostButton;
    string playerName;
    public PlayerSpawner playerSpawner;
    public void OnEnable()
    {
        joinButton.onClick.AddListener(OnJoinButtonClicked);
        hostButton.onClick.AddListener(OnHostButtonClicked);
    }
    public void OnDisable()
    {
        joinButton.onClick.RemoveListener(OnJoinButtonClicked);
        hostButton.onClick.RemoveListener(OnHostButtonClicked);
    }
    public void OnJoinButtonClicked()
    {
        JoinGame(GameMode.Client);
    }
    public void OnHostButtonClicked()
    {
        JoinGame(GameMode.Host);
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
