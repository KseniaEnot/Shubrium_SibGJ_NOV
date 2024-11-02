using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    [SerializeField] private TaskManager _taskManager;
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
    [Header("Mini Game")]
    [SerializeField] private GameObject _miniGameBar;
    [SerializeField] private Button _miniGameButtonStart;
    [SerializeField] private Button _miniGameButtonEnd;

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
        _summaryOfDayBar.SetActive(false);
        EventBus.NextDay();
    }
    // REQUEST BAR
    public void ShowQuestRequestBar(string text)
    {
        _questRequestText.text = text;
        _questRequestBar.SetActive(true);
        _questRequestButtonAccept.Select();
        // ui actions
    }

    private void OnQuestRequestButtonDenyPressed()
    {
        _questResultText.text = _taskManager.GetCurrentTaskNoGoldReactionText();
        _questRequestBar.SetActive(false);
        _questResultBar.SetActive(true);
        _questResultButtonOkay.Select();
    }

    private void OnQuestRequestButtonAcceptPressed()
    {
        //MiniGameManager.StaticInstance.StartMiniGame();
        _taskManager.MarkCurrentTaskAsStarted();
        _miniGameButtonEnd.gameObject.SetActive(false);
        _miniGameBar.SetActive(true);
        _miniGameButtonStart.gameObject.SetActive(true);
        _miniGameButtonStart.Select();
    }
    // RESULT BAR
    public void ShowQuestResultBar(string text)
    {
        _questRequestText.text = text;
        _questRequestBar.SetActive(true);
        _questRequestButtonAccept.Select();
        // ui actions
    }

    private void OnQuestResultButtonOkayPressed()
    {
        _questResultBar.SetActive(false);
        _taskManager.RemoveCurrentTask();
        EventBus.SendCharacterToExit();
    }
    // MINI GAME
    public void OnMiniGameCompleted(string resultText)
    {
        _miniGameBar.SetActive(false);
        _questResultText.text = resultText;
        _questResultBar.SetActive(true);
        _questResultButtonOkay.Select();
    }

    private void OnMiniGameButtonStartPressed()
    {
        //MiniGameManager.StaticInstance.StartSharing();
        _miniGameButtonStart.gameObject.SetActive(false);
        _miniGameButtonEnd.gameObject.SetActive(true);
        _miniGameButtonEnd.Select();
    }

    private void OnMiniGameButtonEndPressed()
    {
        //MiniGameManager.StaticInstance.StopSharing();
    }
}
