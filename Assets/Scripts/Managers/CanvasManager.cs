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

    public GameObject GameOverPanel;

    private readonly EventHandler<GiftCollectedEvent> _giftHandler = new EventHandler<GiftCollectedEvent>();
    private readonly EventHandler<GameOverEvent> _gameOverHandler = new EventHandler<GameOverEvent>();

    public void Start()
    {
        GiftsText.text = _gifts.ToString();
        _giftHandler.EventAction += HandleGiftCollected;
        _gameOverHandler.EventAction += HandleGameOver;
        _highScore = PlayerPrefs.GetInt("highscore", _highScore);
        _totalGifts = PlayerPrefs.GetInt("totalgifts", _totalGifts);
        StartCoroutine(AddPoints());
    }

    void Update()
    {
        SetPointsText();
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
            yield return new WaitForSeconds(0.01f / GameManager.GameSpeed);
        }
    }

    void SetAllGifts()
    {
        _totalGifts += _gifts;
        PlayerPrefs.SetInt("totalgifts", _totalGifts);
        GiftsTotalText.text = $"Total gifts: {_totalGifts}";
    }
}
