using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PlayerResult
{
    public int id;
    public string player_name;
    public float score;
    public int correct_ans;
    public float total_time;
}

[System.Serializable]
public class PlayerResultList
{
    public PlayerResult[] playerResultList;
}

public class HighScoreController : MonoBehaviour
{
    private string current_json;
    public bool leaderboardActive = false;

    [SerializeField] TextMeshProUGUI[] playerNameText;
    [SerializeField] TextMeshProUGUI[] playerScoreText;
    [SerializeField] TextMeshProUGUI[] correctAnswerText;
    [SerializeField] TextMeshProUGUI[] totalTimeText;

    [SerializeField] GameObject Leaderboard;

    public void GetHighScore()
    {
        StartCoroutine(GetRequest("https://localhost:44311/api/Result"));
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

                if (current_json != null && current_json.Length > 0)
                {
                    PlayerResultList playerResultHigh = JsonUtility.FromJson<PlayerResultList>("{\"playerResultList\":" + current_json + "}");

                    for (int i = 0; i < playerResultHigh.playerResultList.Length; i++)
                    {
                        playerNameText[i].text = playerResultHigh.playerResultList[i].player_name;
                        playerScoreText[i].text = playerResultHigh.playerResultList[i].score.ToString();
                        correctAnswerText[i].text = playerResultHigh.playerResultList[i].correct_ans.ToString();
                        totalTimeText[i].text = playerResultHigh.playerResultList[i].total_time.ToString();
                    }
                }
            }
        }
    }

    public void ScoreBoard()
    {
        leaderboardActive = true;
        GetHighScore();
        Leaderboard.SetActive(true);
    }

    public void RetryButton()
    {
        SceneManager.LoadScene("Game Scene");
    }
}
