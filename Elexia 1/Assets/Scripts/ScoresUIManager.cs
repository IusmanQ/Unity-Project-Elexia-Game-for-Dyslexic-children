using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoresUIManager : MonoBehaviour
{

    private string HomeMenuScene = "Home";
    private string insertionGameScene = "Scene";
    private string substitutionGameScene = "sub";
    private string omissionGameScene = "Omission";


    public Text scoreText;
    public Text coinsText;
    public Text gemsText;

    public static string rWord;
    public static int levelInsertion;
    public static int levelSubstitution;
    public static int levelOmission;

    public static Sprite replayImage;

    public void Start()
    {
        displayScores();
        
    }


    public void Update()
    {

    }

    public void ReplayButton()
    {
        if (Game.InsertionActive == true)
        {
            Game.isReplay = true;
            rWord = Game.replayWord;
            replayImage = Game.replayImage;
            levelInsertion = GetInsertionGameLevel();
            SceneManager.LoadScene(insertionGameScene);
        }
        else if (Sub_Game.substitutionActive == true)
        {
            Sub_Game.isReplay = true;
            rWord = Sub_Game.replayWord;
            replayImage = Sub_Game.replayImage;
            SceneManager.LoadScene(substitutionGameScene);
        }
        else if(Omi_Game.omissionActive == true)
        {
            Omi_Game.isReplay = true;
            rWord = Omi_Game.replayWord;
            replayImage = Omi_Game.replayImage;
            SceneManager.LoadScene(omissionGameScene);
        }    
    }

    private static int GetInsertionGameLevel()
    {
        return Game.gameLevelInsertion;
    }
    private static int GetSubstitutionGameLevel()
    {
        return Sub_Game.gameLevelSubstitution;
    }
    private static int GetOmissionGameLevel()
    {
        return Omi_Game.gameLevelOmission;
    }

    public void NextButton() {

        if (Game.InsertionActive == true)
        {
            SceneManager.LoadScene(insertionGameScene);
        }
        else if (Sub_Game.substitutionActive == true)
        {
            SceneManager.LoadScene(substitutionGameScene);
        }
        else if (Omi_Game.omissionActive == true)
        {
            SceneManager.LoadScene(omissionGameScene);
        }

    }

    public void HomeButton()
    {
        SceneManager.LoadScene(HomeMenuScene);
    }

    void displayScores()
    {
        //display the score
        if (scoreText != null)
        {
            if(Game.InsertionActive == true)
                scoreText.text = Game.score.ToString("0");
            else if (Sub_Game.substitutionActive == true)
                scoreText.text = Sub_Game.score.ToString("0");
            else if (Omi_Game.omissionActive == true)
                scoreText.text = Omi_Game.score.ToString("0");
        }

        //display the coins
        if(coinsText != null)
        {
            if (Game.InsertionActive == true)
                coinsText.text = Game.coins.ToString("00");
            else if (Sub_Game.substitutionActive == true)
                coinsText.text = Sub_Game.coins.ToString("00");
            else if (Omi_Game.omissionActive == true)
                coinsText.text = Omi_Game.coins.ToString("00");
        }

        //display the Gems
        if(gemsText != null)
        {
            if (Game.InsertionActive == true)
                gemsText.text = Game.gems.ToString("0");
            else if (Sub_Game.substitutionActive == true)
                gemsText.text = Sub_Game.gems.ToString("0");
            else if (Omi_Game.omissionActive == true)
                gemsText.text = Omi_Game.gems.ToString("0");
        }


    }


}