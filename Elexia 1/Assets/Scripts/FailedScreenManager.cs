using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FailedScreenManager : MonoBehaviour
{
    private string HomeMenu = "Home";
    private string insertionGameScene = "Scene";
    private string substitutionGameScene = "sub";
    private string omissionGameScene = "Omission";

    public Text coins_Txt;
    public Text gems_Txt;
    public Text score_Txt;

    public static string rWord;
    public static int level;


    // Start is called before the first frame update
    void Start()
    {
        DisplayScores();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void HomeButton()
    {
        SceneManager.LoadScene(HomeMenu);
    }
    public void BackButton()
    {
        if (Game.InsertionActive == true)
        {
            Game.isReplay = true;
            rWord = Game.replayWord;
            SceneManager.LoadScene(insertionGameScene);
        }
        else if (Sub_Game.substitutionActive == true)
        {
            Sub_Game.isReplay = true;
            rWord = Sub_Game.replayWord;
            SceneManager.LoadScene(substitutionGameScene);
        }
        else if (Omi_Game.omissionActive == true)
        {
            Omi_Game.isReplay = true;
            rWord = Omi_Game.replayWord;
            SceneManager.LoadScene(omissionGameScene);
        }
    }

    void DisplayScores()
    {
        if(score_Txt != null)
        {
            score_Txt.text = Game.score.ToString("0");
        }
        if(gems_Txt != null)
        {
            gems_Txt.text = Game.gems.ToString("0");
        }
        if(coins_Txt != null)
        {
            coins_Txt.text = Game.coins.ToString("00");
        }
    }
}
