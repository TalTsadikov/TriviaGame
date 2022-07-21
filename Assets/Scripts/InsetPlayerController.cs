using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class InsetPlayerController : MonoBehaviour
{
    public QuestionController quesController;

    private string current_json;
    [SerializeField] private InputField playerNameText;
    [SerializeField] private GameObject questionCanvas;

    public void SetPlayerName(string name, float score, int correctAnswer, float totalTime)
    {
        StartCoroutine(GetRequest("https://localhost:44311/api/InsertPlayer?name=" + name + "&score=" + score + "&correctAnswers=" + correctAnswer + "&totalTime=" + totalTime));
    }

    IEnumerator GetRequest(string uri)
    {
        current_json = "";

        UnityWebRequest webRequest = UnityWebRequest.Get(uri);
        {
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Received: " + webRequest.downloadHandler.text);
                current_json = webRequest.downloadHandler.text;

                if (current_json == "1")
                {
                    Debug.Log("Success");
                }
                else
                {
                    Debug.Log("Error");
                }
            }
        }
    }

    public void NameEntered()
    {
        string name = playerNameText.text;

        SetPlayerName(name, quesController.totalScore, quesController.correctAnswer, quesController.TimerSecs);
        quesController.nameEntered = true;
    }
}

