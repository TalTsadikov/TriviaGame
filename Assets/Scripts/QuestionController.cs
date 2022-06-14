using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;

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

    // Start is called before the first frame update
    void Start()
    {
        GetQuestion(1);
    }

    // Update is called once per frame
    void Update()
    {
       
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
}