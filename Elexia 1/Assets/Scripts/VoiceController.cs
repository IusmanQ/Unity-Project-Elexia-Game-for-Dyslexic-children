using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TextSpeech;
using UnityEngine.UI;
using UnityEngine.Android;
using UnityEngine.SceneManagement;

public class VoiceController : MonoBehaviour
{
    const string LANG_CODE = "en-US";
    public static string wordSpoken;
    //public Text spokenText;


    public string scoreScreen = "Score_Screen";
    public string levelFailedScreen = "LevelFailedScreen";

    //void OnGUI()
    //{
    //    GUILayout.Label("word to be spoken:" + Voice_Game.vWord.ToUpper());
    //    GUILayout.Label("word spoke:" + wordSpoken);
    //    GUILayout.Label("win: " + win);
    //}

    private void Start()
    {
        print("voice game one: " + Voice_Game.vWord.Length);
        Setup(LANG_CODE);
#if UNITY_ANDROID
        SpeechToText.instance.onPartialResultsCallback = OnPartialSpeechResult;
#endif
        SpeechToText.instance.onResultCallback = OnFinalSpeechResult;
        CheckPermission();
    }
    void CheckPermission()
    {
#if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }
#endif
    }
    [SerializeField]
    Text uiText;
    #region Speech to Text
    public void StartListening()
    {
        SpeechToText.instance.StartRecording();
    }
    public void StopListening()
    {
        SpeechToText.instance.StopRecording();
    }
    void OnFinalSpeechResult(string result)
    {
        uiText.text = result;

    }
    void OnPartialSpeechResult(string result)
    {
        uiText.text = result;
    }
    #endregion

    void Setup(string code)
    {
        SpeechToText.instance.Setting(code); 
    }

    //private bool AreEqual(string val1, string val2)
    //{
    //    if (val1.Length != val2.Length)
    //    {
    //        spokenText.text = "lose (length failed)";
    //        return false;
    //    }

    //    for (int i = 0; i < val1.Length; i++)
    //    {
    //        var c1 = val1[i];
    //        var c2 = val2[i];
    //        if (c1 != c2)
    //        {
    //            spokenText.text = "lose (match failed)";
    //            return false;
    //        }
    //    }
    //    spokenText.text = "win";
    //    return true;
    //}

    void Update()
    {
        if(uiText.text != null && uiText.text != "Listening....") { 
            wordSpoken = uiText.text.ToUpper();

            //spokenText.text = uiText.text.ToUpper();

            if ((Voice_Game.vWord.Length - 1) != uiText.text.Length)
            {
                //spokenText.text = "lose (length failed)";
                print("lose(length)");
                SceneManager.LoadScene(levelFailedScreen);
                return;
            }

            for (int i = 0; i < uiText.text.Length; i++)
            {
                var c1 = Voice_Game.vWord.ToUpper()[i];
                var c2 = wordSpoken.ToUpper()[i];
                if (c1 != c2)
                {
                    //spokenText.text = "lose (match failed)";
                    print("lose(match)");
                    SceneManager.LoadScene(levelFailedScreen);
                    return;
                }

                SceneManager.LoadScene(scoreScreen);
                print("win");
                //spokenText.text = "win";
            }

            //if (AreEqual("table", "table"))
            //{
            //    win = 1;
            //    SceneManager.LoadScene(scoreScreen);
            //}
            //else
            //{
            //    win = 0;
            //    SceneManager.LoadScene(levelFailedScreen);
            //}

        }
    }

   
}
