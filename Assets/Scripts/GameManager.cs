using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public Phrases[] phrases;
    private static List<Phrases> unansweredPhrases;

    private Phrases currentPhrase;

    [SerializeField]
    private Text phraseText;

    [SerializeField]
    private float delayBetweenQuestions = 1f;

    public SceneTransitions sT;

    public Animator newWordAnim;
    public Animator scoreAnim;

    public int currTime = 150;
    public Text timeTxt;

    [SerializeField]
    private static int scorePoints;
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text highscoreText;

    void Start()
    {
        if (unansweredPhrases == null || unansweredPhrases.Count == 0)
        {
            unansweredPhrases = phrases.ToList<Phrases>();
        }

        SetCurrentPhrase();

        phraseText.color = currentPhrase.textColor; // Setting the Color of words 

        UpdateScore();

        highscoreText.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
    }

    void Update()
    {
        // Timing and conditions after time is at 0
        currTime --;
        timeTxt.text = ("" + currTime);

        if (currTime <= 0) {
            currTime = 0;
            timeTxt.text = ("" + currTime);
            sT.LoadSce();
            scorePoints = 0;
        }
    }

    void SetCurrentPhrase()
    {
        int randomPhraseIndex = Random.Range(0, unansweredPhrases.Count);
        currentPhrase = unansweredPhrases[randomPhraseIndex];

        phraseText.text=currentPhrase.phrase;

        unansweredPhrases.RemoveAt(randomPhraseIndex);
    }

    IEnumerator TransitionToNextPhrase()
    {
        unansweredPhrases.Remove(currentPhrase);

        yield return new WaitForSeconds(delayBetweenQuestions);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        newWordAnim.SetTrigger("start");

        if (scorePoints > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", scorePoints);
            highscoreText.text = scorePoints.ToString();
        }
    }

    public void UserSelectTrue()
    {
        if (currentPhrase.isGood)
        {
            AddScore(2);
            //GOOD
        }
        else
        {
            AddScore(-8);
            //BAD
        }
        StartCoroutine(TransitionToNextPhrase());
    }

    public void UserSelectFalse()
    {
        if (!currentPhrase.isGood)
        {
            AddScore(2);
            //GOOD
        }
        else
        {
            AddScore(-8);
            //BAD
        }
        StartCoroutine(TransitionToNextPhrase());
    }

    public void AddScore(int newScore)
    {
        scorePoints += newScore;
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = "" + scorePoints;
        scoreAnim.SetTrigger("scoreCfm");
    }
}
