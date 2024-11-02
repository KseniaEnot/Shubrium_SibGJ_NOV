using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("New Game")]
    [SerializeField] private GameObject _newGameNotification;
    [SerializeField] private Button _newGameButtonOkay;
    [Header("Summary Of Day")]
    [SerializeField] private GameObject _summaryOfDayBar;
    [SerializeField] private TextMeshProUGUI _summaryOfDayText;
    [SerializeField] private Button _summaryOfDayButtonStartNewDay;
    [Header("Quest Request")]
    [SerializeField] private GameObject _questRequestBar;
    [SerializeField] private TextMeshProUGUI _questRequestText;
    [SerializeField] private Button _questRequestButtonDeny;
    [SerializeField] private Button _questRequestButtonAccept;
    [Header("Quest Result")]
    [SerializeField] private GameObject _questResultBar;
    [SerializeField] private TextMeshProUGUI _questResultText;
    [SerializeField] private Button _questResultButtonOkay;
    [SerializeField, Range(0f, 0.49f)] private float _maxPercentToLowResultReaction = 0.33f;
    [SerializeField, Range(0.51f, 1f)] private float _minPercentToHighResultReaction = 0.66f;
    [Header("Mini Game")]
    [SerializeField] private GameObject _miniGameBar;
    [Header("Waypoints")]
    [SerializeField] private Transform _enterPoint;
    [SerializeField] private Transform _exitPoint;
    [Header("Characters/Quests Settings")]
    [SerializeField] private List<CharacterConfig> _characterConfigs = new();
    [SerializeField, Range(1, 10)] private int _minlNewQuestsPerDay = 4;
    [SerializeField, Range(1, 10)] private int _maxNewQuestsPerDay = 6;
    private Dictionary<CharacterConfig, bool> _characterVisited = new();
    private List<CharacterConfig> _charactersWithoutQuest = new();
    private List<TaskData> _tasks = new();
    private CharacterHolder _currentCharacterOnScreen;
    private ConversationState _currentConversationState;

    public List<TaskData> Tasks => _tasks;

    private void Awake()
    {
        for (int i = 0; i < _characterConfigs.Count; i++)
        {
            _characterVisited.Add(_characterConfigs[i], false);// create pool
        }
    }

    private void OnEnable()
    {
        _newGameNotification.SetActive(true);
        _newGameButtonOkay.Select();
        _newGameButtonOkay.onClick.AddListener(OnNewGameButtonOkayPressed);
        _summaryOfDayButtonStartNewDay.onClick.AddListener(OnSummaryOfDayButtonStartNewDayPressed);
        _questRequestButtonDeny.onClick.AddListener(OnQuestRequestButtonDenyPressed);
        _questRequestButtonAccept.onClick.AddListener(OnQuestRequestButtonAcceptPressed);
        _questResultButtonOkay.onClick.AddListener(OnQuestResultButtonOkayPressed);
    }

    private void OnDisable()
    {
        _newGameButtonOkay.onClick.RemoveListener(OnNewGameButtonOkayPressed);
        _summaryOfDayButtonStartNewDay.onClick.RemoveListener(OnSummaryOfDayButtonStartNewDayPressed);
        _questRequestButtonDeny.onClick.RemoveListener(OnQuestRequestButtonDenyPressed);
        _questRequestButtonAccept.onClick.RemoveListener(OnQuestRequestButtonAcceptPressed);
        _questResultButtonOkay.onClick.RemoveListener(OnQuestResultButtonOkayPressed);
    }

    private void OnNewGameButtonOkayPressed()
    {
        _newGameNotification.SetActive(false);
        OnSummaryOfDayButtonStartNewDayPressed();
    }

    private void OnSummaryOfDayButtonStartNewDayPressed()
    {
        _summaryOfDayBar.SetActive(false);
        _charactersWithoutQuest.Clear();// clear pool
        for (int i = 0; i < _characterVisited.Count; i++)
        {
            _characterVisited[_characterConfigs[i]] = false;// toogle visited pool
            _charactersWithoutQuest.Add(_characterConfigs[i]);// add characters to pool
        }
        for (int i = _tasks.Count - 1; i <= 0; i--)
        {
            _charactersWithoutQuest.Remove(_tasks[i].CurrentCharacter);// remove character from pool
        }
        int questCount = Random.Range(_minlNewQuestsPerDay, _maxNewQuestsPerDay + 1);// how many new quest appear today
        CharacterConfig tempCharacter;
        for (int i = 0; i < questCount; i++)
        {
            tempCharacter = _charactersWithoutQuest[Random.Range(0, _charactersWithoutQuest.Count)];// get character without quest
            _charactersWithoutQuest.Remove(tempCharacter);// remove from pool
            _characterVisited[tempCharacter] = true;// toggle visited pool
            _tasks.Add(new(tempCharacter, tempCharacter.GetRandomQuest(), false));// add random quest to task pool
        }
        CheckTasks();
    }

    private void ShowSummaryOfDay()
    {
        _summaryOfDayText.text = $"ÍÈÕÅÐÀ ÒÛ ÍÅ ÏÎËÓ×ÈË ÄÅÍÅÃ, ÑÌÅÐÄ";
        _summaryOfDayBar.SetActive(true);
        _summaryOfDayButtonStartNewDay.Select();
    }

    private void CheckTasks()
    {
        if (_currentCharacterOnScreen != null)
        {
            Destroy(_currentCharacterOnScreen.gameObject);
        }
        if (_tasks.Count > 0)
        {
            SendCharacterToEnter();
        }
        else
        {
            ShowSummaryOfDay();
        }
    }

    private void SendCharacterToEnter()
    {
        _currentCharacterOnScreen = Instantiate(_tasks[0].CurrentCharacter.Model).GetComponent<CharacterHolder>();
        _currentCharacterOnScreen.SetDestination(_enterPoint, false, true);
        _currentConversationState = ConversationState.Entering;
        _currentCharacterOnScreen.OnReachedDestination += OnCharacterReachedDestination;
    }

    private void ShowQuestRequestBar()
    {
        _currentConversationState = ConversationState.Request;
        _questRequestText.text = $"{_tasks[0].CurrentQuest.Description}\n{_tasks[0].CurrentQuest.RequestText} {_tasks[0].CurrentQuest.RequestedGold} çîëîòà.";
        _questRequestBar.SetActive(true);
        _questRequestButtonAccept.Select();
        // ui actions
    }

    private void OnQuestRequestButtonDenyPressed()
    {
        // show negative message
        _currentConversationState = ConversationState.Result;
        _questResultText.text = _tasks[0].CurrentCharacter.GetNoGoldReaction();
        _questRequestBar.SetActive(false);
        _questResultBar.SetActive(true);
        _questResultButtonOkay.Select();
    }

    private void OnQuestRequestButtonAcceptPressed()
    {
        // start minigame
        _currentConversationState = ConversationState.Minigame;
    }

    private void OnQuestResultButtonOkayPressed()
    {
        _tasks.RemoveAt(0);// remove task
        _questResultBar.SetActive(false);
        SendCharacterToExit();
    }

    private void MiniGameCompleted()
    {
        int minGold = 0;
        int maxGold = _tasks[0].CurrentQuest.RequestedGold * 2;
        int miniGameGold = 10;// calculate
        float result = Mathf.InverseLerp(minGold, maxGold, miniGameGold);
        _currentConversationState = ConversationState.Result;
        _miniGameBar.SetActive(false);
        if (result < _maxPercentToLowResultReaction)
        {
            _questRequestText.text = _tasks[0].CurrentCharacter.GetLowGoldReaction();
        }
        else if (result > _minPercentToHighResultReaction)
        {
            _questRequestText.text = _tasks[0].CurrentCharacter.GetHighGoldReaction();
        }
        else
        {
            _questRequestText.text = _tasks[0].CurrentCharacter.GetNormalGoldReaction();
        }
        _questRequestBar.SetActive(true);
        _questResultButtonOkay.Select();
        // remove gold from player (miniGameGold)
    }

    private void SendCharacterToExit()
    {
        _currentCharacterOnScreen.SetDestination(_exitPoint, true, false);
        _currentConversationState = ConversationState.Exiting;
        _currentCharacterOnScreen.OnReachedDestination += OnCharacterReachedDestination;
    }

    private void OnCharacterReachedDestination()// or remove subscribe and call this method inside character???
    {
        _currentCharacterOnScreen.OnReachedDestination -= OnCharacterReachedDestination;
        if (_currentConversationState == ConversationState.Entering)
        {
            ShowQuestRequestBar();
        }
        else if (_currentConversationState == ConversationState.Exiting)
        {
            CheckTasks();
        }
    }
}
