using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class InsetPlayerController : MonoBehaviour
{
    private string current_json;
    [SerializeField] private InputField playerNameText;

    public void SetPlayerName(string name)
    {
        StartCoroutine(GetRequest("https://localhost:44311/api/InsertPlayer?name=" + name));
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

    }
}

