using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Quest : MonoBehaviour
{
    [SerializeField] private AdsScript _ads;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _rightAnswersText;
    [SerializeField] private TextMeshProUGUI _bestScoreText;
    [SerializeField] private TextMeshProUGUI _questText;
    [SerializeField] private TextMeshProUGUI[] _answerText = new TextMeshProUGUI[4];
    [SerializeField] private float _delayBetweenQuestions;
    [SerializeField] private GameObject _finishUI;
    [SerializeField] private TextMeshProUGUI _finishMessage;
    [SerializeField] private QuestScriptable[] _allQuests;
    private List<QuestScriptable> _availableQuests;
    private int _rightAnswersCount;
    private int _totalQuestsCount;
    private int _currentQuestCount;
    private int _rightAnswerBtnId;
    private int _bestScore = 0;
    private string _rightAnswer;
    private bool _canClick;
    private Save _save;

    public int BestScore { get => _bestScore;}

    private void Awake()
    {
        _save = new Save();
        if (PlayerPrefs.HasKey("SV"))
        {
            _save = JsonUtility.FromJson<Save>(PlayerPrefs.GetString("SV"));
            _bestScore = _save.bestScore;
        }
    }
    private void Start()
    {
        StartNewGame();
    }
    public void StartNewGame()
    {
        _availableQuests = new List<QuestScriptable>();
        _availableQuests.AddRange(_allQuests);
        _rightAnswersCount = 0;
        _totalQuestsCount = _availableQuests.Count;
        _currentQuestCount = 0;
        _finishUI.SetActive(false);
        SetNewQuest();
    }
    private void ShowFinish()
    {

        _finishUI.SetActive(true);
        _finishMessage.text = "Вы ответили правильно на " + _rightAnswersCount + " из " + _totalQuestsCount;
        _ads.ShowNonRewardAd();
    }
    private void SetNewQuest()
    {
        _canClick = true;
        _currentQuestCount++;
        if (_currentQuestCount <= _totalQuestsCount)
        {
            _levelText.text = "Вопрос " + _currentQuestCount + "/" + _totalQuestsCount;
            _rightAnswersText.text = "Правильно: " + _rightAnswersCount;
            _bestScoreText.text = "Рекорд: " + _bestScore;
            QuestScriptable currentQuest = _availableQuests[Random.Range(0, _availableQuests.Count)];
            _questText.text = currentQuest.QuestText;
            List<int> ids = new List<int>()
            {
                0,
                1,
                2,
                3
            };
            for(int i = 0; i < _answerText.Length; i++)
            {
                int id = ids[Random.Range(0, ids.Count)];
                _answerText[i].text = currentQuest.Answers[id];
                if (currentQuest.RightAnswerId == id)
                {
                    _rightAnswerBtnId = i;
                    _rightAnswer = currentQuest.Answers[id];
                }
                ids.Remove(id);
            }
            _availableQuests.Remove(currentQuest);
        }
        else
        {
            ShowFinish();
        }
    }
    public void Answer(int i)
    {
        if (_canClick)
        {
            if (i == _rightAnswerBtnId)
            {
                _questText.text = "Верно, молодец!";
                _rightAnswersCount++;
                _rightAnswersText.text = "Правильно: " + _rightAnswersCount;
                if(_rightAnswersCount > _bestScore)
                {
                    _bestScore = _rightAnswersCount;
                    _bestScoreText.text = "Новый рекорд: " + _bestScore;
                }
            }
            else
            {
                _questText.text = "Не верно, правильный ответ - '" + _rightAnswer + "'";
            }
            Invoke("SetNewQuest", _delayBetweenQuestions);
        }
        _canClick = false;
    }
}
