using System.Collections;
using Assets.Scripts.EventHandling;
using Assets.Scripts.Events;
using UnityEngine;
using Text = TMPro.TextMeshProUGUI;

public class CanvasManager : MonoBehaviour
{
    private int _points;
    public Text PointsText;
    private int _gifts;
    public Text GiftsText;
    private int _highScore;
    public Text HighScoreText;
    private int _totalGifts;
    public Text GiftsTotalText;
    private bool _highScoreReached;
    public Text AddGiftsText;
    private bool _showAddGiftsText;
    private int _addGiftsCount;
    private Vector3 _addGiftsTextScale;
    public float AddGiftsTextScaleSpeed = 10f;
    public float AddGiftsTextStayTime = 0.5f;
    private float countDown = 3f;
    public Text CountDownText;
    private bool gameStarted;
    public GameObject GameOverPanel;

    private readonly EventHandler<GiftCollectedEvent> _giftHandler = new EventHandler<GiftCollectedEvent>();
    private readonly EventHandler<GameOverEvent> _gameOverHandler = new EventHandler<GameOverEvent>();

    public void Start()
    {
        GiftsText.text = _gifts.ToString();
        _addGiftsTextScale = AddGiftsText.transform.localScale;
        AddGiftsText.transform.localScale = Vector3.zero;
        _giftHandler.EventAction += HandleGiftCollected;
        _gameOverHandler.EventAction += HandleGameOver;
        _highScore = PlayerPrefs.GetInt("highscore", _highScore);
        _totalGifts = PlayerPrefs.GetInt("totalgifts", _totalGifts);
        //StartCoroutine(AddPoints());
    }

    void Update()
    {
        if (countDown > 0 && !gameStarted)
        {
            countDown -= Time.deltaTime;
            CountDownText.text = $"{Mathf.Round(countDown)}";
        }
        else if (countDown <= 0 && !gameStarted)
        {
            var gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            gameStarted = true;
            CountDownText.text = "";
            StartCoroutine(AddPoints());
            gameManager.StartGame();
        }
        SetPointsText();
        if (_showAddGiftsText)
        {
            StartCoroutine(AddGiftsTextEffect());
            _showAddGiftsText = false;
        }
    }

    IEnumerator AddGiftsTextEffect()
    {
        var startScale = AddGiftsText.transform.localScale;
        while (_addGiftsTextScale.x * 0.99f >= AddGiftsText.transform.localScale.x)
        {
            AddGiftsText.transform.localScale = Vector3.Lerp(AddGiftsText.transform.localScale, _addGiftsTextScale, AddGiftsTextScaleSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(AddGiftsTextStayTime);

        while (AddGiftsText.transform.localScale.x >= 0.01)
        {
            AddGiftsText.transform.localScale = Vector3.Lerp(AddGiftsText.transform.localScale, Vector3.zero, AddGiftsTextScaleSpeed * Time.deltaTime);
            yield return null;
        }
    }

    void HandleGameOver(GameOverEvent @event)
    {
        SetAllGifts();
        GameOverPanel.SetActive(true);
    }

    void HandleGiftCollected(GiftCollectedEvent @event)
    {
        _gifts += @event.Value;
        GiftsText.text = _gifts.ToString();
        _addGiftsCount = @event.Value;
        _showAddGiftsText = true;
        AddGiftsText.text = $"+{_addGiftsCount}";
    }

    void SetPointsText()
    {
        PointsText.text = _points.ToString();
        if (_points > _highScore)
        {
            _highScore = _points;
            PlayerPrefs.SetInt("highscore", _highScore);
            PlayerPrefs.Save();
            if (!_highScoreReached)
                StartCoroutine("SetHighScoreText");
        }
    }

    IEnumerator SetHighScoreText()
    {
        HighScoreText.text = "Highscore!";
        HighScoreText.enabled = true;
        yield return new WaitForSeconds(2f);
        HighScoreText.enabled = false;
        _highScoreReached = true;
    }

    IEnumerator AddPoints()
    {
        while (GameManager.Instance.GameRunning)
        {
            _points++;
            yield return new WaitForSeconds(0.1f / GameManager.GameSpeed);
        }
    }

    void SetAllGifts()
    {
        _totalGifts += _gifts;
        PlayerPrefs.SetInt("totalgifts", _totalGifts);
        GiftsTotalText.text = $"Total gifts: {_totalGifts}";
    }
}
