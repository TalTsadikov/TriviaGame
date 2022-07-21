using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public class Question
{
    public string question_text;
    public int id;
    public string ans1_text;
    public string ans2_text;
    public string ans3_text;
    public string ans4_text;
    public int answer_id;
}

public class QuestionController : MonoBehaviour
{
    private string current_json;
    private Question current_question;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private TextMeshProUGUI Ans1Text;
    [SerializeField] private TextMeshProUGUI Ans2Text;
    [SerializeField] private TextMeshProUGUI Ans3Text;
    [SerializeField] private TextMeshProUGUI Ans4Text;
    [SerializeField] private GameObject questionCanvas;
    [SerializeField] private GameObject insertPlayerCanvas;
    [SerializeField] private GameObject resultPanel;

    public int playerScore;
    public int correctAnswer;
    public float timeScore;
    public float totalScore;
    int questionNumber = 1;

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI totalScoreText;
    [SerializeField] TextMeshProUGUI resultsTimerText;
    [SerializeField] public float TimerSecs = 0;
    [SerializeField] private TextMeshProUGUI gameTimerText;
    [SerializeField] private TextMeshProUGUI questionTimerText;

    float questionTimer = 10f;
    bool allowTime = true;
    bool timeOver = true;
    public bool nameEntered = false;

    [SerializeField] HighScoreController hsController;
    // Start is called before the first frame update
    void Start()
    {
        playerScore = 0;
        correctAnswer = 0;
        questionNumber = 1;
        allowTime = true;
        timeOver = false;
        nameEntered = false;
        hsController.leaderboardActive = false;
        TimerSecs = 0f;
        GetQuestion(1);
    }

    void Update()
    {
        if (questionNumber <= 5)
        {
            TimerSecs += Time.deltaTime;
            gameTimerText.text = "Time: " + TimerSecs.ToString("f2");
        }

        if (questionNumber > 5)
        {
            allowTime = false;
            questionCanvas.SetActive(false);
            totalScoreText.text = "Total Score: " + totalScore.ToString("f2");
            scoreText.text = "Correct Answer: " + correctAnswer.ToString();
            resultsTimerText.text = "Total Time: " + TimerSecs.ToString("f2");
            if (nameEntered == false)
            {
                insertPlayerCanvas.SetActive(true);
            }
            else
            {
                insertPlayerCanvas.SetActive(false);
                if (hsController.leaderboardActive == false)
                {
                    resultPanel.SetActive(true);
                }
                else
                {
                    resultPanel.SetActive(false);
                }
            }
        }

        if (allowTime)
        {
            questionTimer -= Time.deltaTime;
        }
        
        questionTimerText.text = "Time Left: " + questionTimer.ToString("f2");

        if (questionTimer <= 0)
        {
            timeOver = true;
        }

        if (timeOver)
        {
            timeOver = false;
            questionTimer = 10f;
            StartCoroutine(ChangeQuestionDelay());
        }
    }

    private float ScoreCalculation(float questionTimer)
    {
        float responeTime = 10 - questionTimer;
        timeScore = (1 -((responeTime / 10) / 2 )) * 1000;
        return timeScore;
    }

    public void GetQuestion(int QID)
    {
        StartCoroutine(GetRequest("https://localhost:44311/Api/Question/" + QID));
    }

    IEnumerator GetRequest(string uri)
    {
        current_json = "";
        current_question = null;

        UnityWebRequest webRequest = UnityWebRequest.Get(uri);
        {
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Received: " + webRequest.downloadHandler.text);
                current_json = webRequest.downloadHandler.text;

                if (current_json != null && current_json.Length > 0)
                {
                    current_question = JsonUtility.FromJson<Question>(current_json);
                    questionText.text = current_question.question_text;
                    Ans1Text.text = current_question.ans1_text;
                    Ans2Text.text = current_question.ans2_text;
                    Ans3Text.text = current_question.ans3_text;
                    Ans4Text.text = current_question.ans4_text;
                }
                else
                {
                    current_question = null;
                }
            }
        }
    }

    IEnumerator ChangeQuestionDelay()
    {
        allowTime = false;
        yield return new WaitForSeconds(2f);
        questionNumber++;
        GetQuestion(questionNumber);
        allowTime = true;
        questionTimer = 10f;
    }

    public void ChangeQuestion()
    {
        UpdateScore(questionNumber);
        StartCoroutine(ChangeQuestionDelay());
    }

    private void UpdateScore(int questionNumber)
    {
        string buttonName = EventSystem.current.currentSelectedGameObject.name;

        switch (questionNumber)
        {
            case 1:
                if (buttonName == "Answer3 Button")
                {
                    correctAnswer++;
                    totalScore += ScoreCalculation(questionTimer);
                }
                break;
            case 2:
                if (buttonName == "Answer2 Button")
                {
                    correctAnswer++;
                    totalScore += ScoreCalculation(questionTimer);
                }
                break;
            case 3:
                if (buttonName == "Answer4 Button")
                {
                    correctAnswer++;
                    totalScore += ScoreCalculation(questionTimer);
                }
                break;
            case 4:
                if (buttonName == "Answer4 Button")
                {
                    correctAnswer++;
                    totalScore += ScoreCalculation(questionTimer);
                }
                break;
            case 5:
                if (buttonName == "Answer1 Button")
                {
                    correctAnswer++;
                    totalScore += ScoreCalculation(questionTimer);
                }
                break;
        }
    }
}