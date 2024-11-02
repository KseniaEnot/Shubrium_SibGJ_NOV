using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    [SerializeField] private TaskManager _taskManager;
    [SerializeField] private MiniGameManager _miniGameManager;
    [Header("New Game")]
    [SerializeField] private GameObject _newGameNotification;
    [SerializeField] private Button _newGameButtonOkay;
    [Header("Summary Of Day")]
    [SerializeField] private GameObject _summaryOfDayBar;
    [SerializeField] private TextMeshProUGUI _summaryOfDayText;
    [SerializeField] private Button _summaryOfDayButtonStartNewDay;
    [Header("Quest Request")]
    [SerializeField] private GameObject _questRequestBar;
    [SerializeField] private TextMeshProUGUI _questRequestCharacterNameText;
    [SerializeField] private TextMeshProUGUI _questRequestQuestNameText;
    [SerializeField] private TextMeshProUGUI _questRequestDescriptionText;
    [SerializeField] private Button _questRequestButtonDeny;
    [SerializeField] private Button _questRequestButtonAccept;
    [Header("Quest Result")]
    [SerializeField] private GameObject _questResultBar;
    [SerializeField] private TextMeshProUGUI _questResultCharacterNameText;
    [SerializeField] private TextMeshProUGUI _questResultQuestNameText;
    [SerializeField] private TextMeshProUGUI _questResultDescriptionText;
    [SerializeField] private Button _questResultButtonOkay;
    [Header("Mini Game")]
    [SerializeField] private GameObject _miniGameBar;
    [SerializeField] private Button _miniGameButtonStart;
    [SerializeField] private Button _miniGameButtonEnd;

    private bool _deadline;

    private void OnEnable()
    {
        _newGameNotification.SetActive(true);
        _newGameButtonOkay.Select();
        _newGameButtonOkay.onClick.AddListener(OnNewGameButtonOkayPressed);
        _summaryOfDayButtonStartNewDay.onClick.AddListener(OnSummaryOfDayButtonStartNewDayPressed);
        _questRequestButtonDeny.onClick.AddListener(OnQuestRequestButtonDenyPressed);
        _questRequestButtonAccept.onClick.AddListener(OnQuestRequestButtonAcceptPressed);
        _questResultButtonOkay.onClick.AddListener(OnQuestResultButtonOkayPressed);
        _miniGameButtonStart.onClick.AddListener(OnMiniGameButtonStartPressed);
        _miniGameButtonEnd.onClick.AddListener(OnMiniGameButtonEndPressed);
    }

    private void OnDisable()
    {
        _newGameButtonOkay.onClick.RemoveListener(OnNewGameButtonOkayPressed);
        _summaryOfDayButtonStartNewDay.onClick.RemoveListener(OnSummaryOfDayButtonStartNewDayPressed);
        _questRequestButtonDeny.onClick.RemoveListener(OnQuestRequestButtonDenyPressed);
        _questRequestButtonAccept.onClick.RemoveListener(OnQuestRequestButtonAcceptPressed);
        _questResultButtonOkay.onClick.RemoveListener(OnQuestResultButtonOkayPressed);
        _miniGameButtonStart.onClick.RemoveListener(OnMiniGameButtonStartPressed);
        _miniGameButtonEnd.onClick.RemoveListener(OnMiniGameButtonEndPressed);
    }
    // SUMMARY OF DAY
    private void OnNewGameButtonOkayPressed()
    {
        _newGameNotification.SetActive(false);
        CurrencyManager.StaticInstance.ForceUpdateGoldUI();
        OnSummaryOfDayButtonStartNewDayPressed();
    }

    public void ShowSummaryOfDay(string text)
    {
        _summaryOfDayText.text = text;
        _summaryOfDayBar.SetActive(true);
        _summaryOfDayButtonStartNewDay.Select();
    }

    private void OnSummaryOfDayButtonStartNewDayPressed()
    {
        if (_deadline)
        {
            SceneManager.LoadScene(0);// GAME OVER =)
        }
        else
        {
            _summaryOfDayBar.SetActive(false);
            EventBus.NextDay();
        }
    }
    // REQUEST BAR
    public void ShowQuestRequestBar(string characterName, string questName, string descriptionText)
    {
        _questRequestCharacterNameText.text = characterName;
        _questRequestQuestNameText.text = questName;
        _questRequestDescriptionText.text = descriptionText;
        _questRequestBar.SetActive(true);
        _questRequestButtonAccept.Select();
        // ui actions
    }

    private void OnQuestRequestButtonDenyPressed()
    {
        _taskManager.GetCurrentTaskNoGoldReactionText(out string characterName, out string questName, out string descriptionText);
        ShowQuestResultBar(characterName, questName, descriptionText);
    }

    private void OnQuestRequestButtonAcceptPressed()
    {
        if (CurrencyManager.StaticInstance.CurrentGold <= 1f)
        {
            return;
        }
        _questRequestBar.SetActive(false);
        _miniGameButtonEnd.gameObject.SetActive(false);
        _miniGameBar.SetActive(true);
        _miniGameButtonStart.gameObject.SetActive(true);
        _miniGameButtonStart.Select();
        _miniGameManager.StartMiniGame();
        _taskManager.MarkCurrentTaskAsStarted();
    }
    // RESULT BAR
    public void ShowQuestResultBar(string characterName, string questName, string descriptionText)
    {
        _questResultCharacterNameText.text = characterName;
        _questResultQuestNameText.text = questName;
        _questResultDescriptionText.text = descriptionText;
        _questRequestBar.SetActive(false);
        _questResultBar.SetActive(true);
        _questResultButtonOkay.Select();
        // ui actions
    }

    private void OnQuestResultButtonOkayPressed()
    {
        _questResultBar.SetActive(false);
        _taskManager.RemoveCurrentTask();
        EventBus.SendCharacterToExit();
    }
    // MINI GAME
    public void OnMiniGameCompleted(string characterName, string questName, string descriptionText)
    {
        _miniGameBar.SetActive(false);
        _questResultCharacterNameText.text = characterName;
        _questResultQuestNameText.text = questName;
        _questResultDescriptionText.text = descriptionText;
        _questResultBar.SetActive(true);
        _questResultButtonOkay.Select();
    }

    private void OnMiniGameButtonStartPressed()
    {
        _miniGameManager.StartSharing();
        _miniGameButtonStart.gameObject.SetActive(false);
        _miniGameButtonEnd.gameObject.SetActive(true);
        _miniGameButtonEnd.Select();
    }

    private void OnMiniGameButtonEndPressed()
    {
        _miniGameButtonEnd.gameObject.SetActive(false);
        _miniGameManager.StopSharing();
    }
    // DEADLINE
    public void ShowDeadlineNotification()
    {
        _deadline = true;
        ShowSummaryOfDay($"Итоговый доход: {CurrencyManager.StaticInstance.IncomeOverall}\n" +
            $"Итоговый расход: {CurrencyManager.StaticInstance.OutcomeOverall}\n" +
            $"Сумма долга: {CurrencyManager.StaticInstance.RequiredGold}\n" +
            $"{CurrencyManager.StaticInstance.GetDeadLineResultText()}");
    }

    public void ShowGameOverNotification()
    {
        _deadline = true;
        ShowSummaryOfDay($"Вы остались ни с чем! Конец игры.\n" +
            $"Итоговый доход: {CurrencyManager.StaticInstance.IncomeOverall}\n" +
            $"Итоговый расход: {CurrencyManager.StaticInstance.OutcomeOverall}");
    }
}
