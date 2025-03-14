// This script was inspired by this tutorial https://www.youtube.com/watch?v=I5rpkvAgueY
// This script is used to send feedback to a Google Form using UnityWebRequest.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class FeedbackForm : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField usernameInputField;
    [SerializeField]
    private TMP_InputField emailInputField;
    [SerializeField]
    private TMP_InputField feedbackInputField;

    private string _username;
    private string _email;
    private string _feedback;

    private static string base_url = "https://docs.google.com/forms/u/0/d/1Vpmm3Mdl4jEBqPcfHYerZ2w12XisrfSVx2IvVtIPTzU/formResponse";

    private static string username_field = "entry.670278920";
    private static string email_field = "entry.2065834128";
    private static string feedback_field = "entry.780128627";

    public void Send()
    {
        _username = usernameInputField.text;
        _email = emailInputField.text;
        _feedback = feedbackInputField.text;

        StartCoroutine(Post());
    }

    private IEnumerator Post()
    {
        WWWForm form = new WWWForm();
        form.AddField(username_field, _username);
        form.AddField(email_field, _email);
        form.AddField(feedback_field, _feedback);

        using UnityWebRequest www = UnityWebRequest.Post(base_url, form);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
        }
    }

}
