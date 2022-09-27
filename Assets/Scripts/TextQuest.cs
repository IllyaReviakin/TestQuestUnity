using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

[Serializable]
public class ActionInfo
{
    public string ActionText;
    [Range(0,15)]
    public int Index;
}

[Serializable]
public class RoomInfo
{
    public string RoomText;
    public ActionInfo[] Actions;
    public Texture BgImage;
}

public class TextQuest : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _roomText;

    [SerializeField]
    private Button[] _actionButtons;

    [SerializeField]
    private TMP_Text[] _actionButtonsText;

    [SerializeField]
    private RoomInfo[] _roomInfo;

    [SerializeField]
    private RawImage _roomBg;

    private int _currentIndex = 0;

    private void Start()
    {
        CheckData();
        SetRoomInfo();
        
        for (byte i = 0; i < _actionButtons.Length; i++ )
        {
            var copy = i;
            _actionButtons[i].onClick.AddListener(() => ButtonClick(copy));
        }
    }

    private void CheckData()
    {
        for (var i = 0; i < _roomInfo.Length;i++ )
        {
            var roomText    = _roomInfo[i].RoomText;
            var actions     = _roomInfo[i].Actions;
            var actionsLen  = actions.Length;
            
            if (roomText == "" || roomText == null)
                throw new Exception($"В комнате {i} не заполнено поле \"текст\"!");
            if (actionsLen == 0 || actionsLen > _actionButtons.Length)
                throw new Exception($"В комнате {i} не верное количество действий. Должно быть 1 - {_actionButtons.Length}!");
            for (var j = 0; j < actionsLen; j++)
            {
                var actionText = actions[j].ActionText;

                if (actionText  == "" || actionText == null)
                    throw new Exception($"В комнате {i}, деёствие {j} не заполнено поле \"текст\"!");
            }
        }
    }

    private void EndGame()
    {
        _roomText.text = "На данный момент Вы прошли все уровни.";
        for (var i = 0; i < _actionButtons.Length; i++)
        {
            _actionButtons[i].gameObject.SetActive(false);
        }
        _actionButtons[0].gameObject.SetActive(true);
        _actionButtonsText[0].text = "Начать игру сначала";

        _actionButtons[0].onClick.RemoveAllListeners();
        _actionButtons[0].onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name));
    }

    private void ButtonClick(int index)
    {
        var currentRoom = _roomInfo[_currentIndex];
        _currentIndex = currentRoom.Actions[index].Index;

        if (_currentIndex >= _roomInfo.Length)
        {
            EndGame();
        }
        else
        {
            SetRoomInfo();
        }
    }

    private void SetRoomInfo()
    {
        var currentRoom = _roomInfo[_currentIndex];
        var currentRoomActions = currentRoom.Actions;

        _roomText.text = _roomInfo[_currentIndex].RoomText;

        for (var i = 0; i < _actionButtons.Length; i++)
        {
            _actionButtons[i].gameObject.SetActive(false);
        }
        for (var i = 0; i < _roomInfo[_currentIndex].Actions.Length; i++)
        {
            _actionButtons[i].gameObject.SetActive(true);
            _actionButtonsText[i].text = _roomInfo[_currentIndex].Actions[i].ActionText;
        }

        _roomBg.texture = currentRoom.BgImage;
    }
}
