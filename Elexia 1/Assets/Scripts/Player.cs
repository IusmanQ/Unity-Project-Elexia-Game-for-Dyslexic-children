using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player
{
    public static int playerInsertionLevel;
    public static int playerSubstitutionLevel;
    public static int playerOmissionLevel;

    public static int playerTotalGems;
    public static int playerTotalCoins;
    public static int wordsPlayed;
    public static int playerTotalScore;
    public static int playerLevelScore;
    public static int playerLevelScoreSub;
    public static int playerLevelScoreOmi;
    public static string playerGameType;
    public static List <WordStat> words = new List<WordStat>();
    public static Guid id = Guid.NewGuid();
    public static string playerId = id.ToString();

    public static void Save(string word, int gems, int coins,int score,int totalScore,int totalScoreSub, int totalScoreOmi, float time, string gameType)
    {
       

        Debug.Log("level Insertion: " + playerInsertionLevel);
        Debug.Log("level Substitution: " + playerSubstitutionLevel);
        Debug.Log("level Omission: " + playerOmissionLevel);
        Debug.Log("gems: " + gems);
        Debug.Log("coins: " + coins);
        Debug.Log("score: " + score);
        Debug.Log("time: " + time);
        Debug.Log("Game Type: " + gameType);

        PlayerPrefs.SetInt("playerInsertionLevel", playerInsertionLevel);
        PlayerPrefs.SetInt("playerSubstitutionLevel", playerSubstitutionLevel);
        PlayerPrefs.SetInt("playerOmissionLevel", playerOmissionLevel);
        if (Game.IncrementCoinsGems == true)
        {
            PlayerPrefs.SetInt("PlayerTotalGems", playerTotalGems + gems);
            PlayerPrefs.SetInt("PlayerTotalCoins", playerTotalCoins + coins);
            PlayerPrefs.SetInt("PlayerTotalScore", playerTotalScore + score);
            //if (Game.InsertionActive == true)
            //{
                PlayerPrefs.SetInt("PlayerLevelScore", playerLevelScore + totalScore);
            //}
            //if (Sub_Game.substitutionActive == true)
            //{
                PlayerPrefs.SetInt("PlayerLevelScoreSub", playerLevelScoreSub + totalScoreSub);
            //}
            //if (Omi_Game.omissionActive == true)
            //{
                PlayerPrefs.SetInt("PlayerLevelScoreOmi", playerLevelScoreOmi + totalScoreOmi);
            //}

        }
        PlayerPrefs.SetString("gameType", gameType);
        PlayerPrefs.SetString("word" + wordsPlayed, word);
        PlayerPrefs.SetFloat("time" + wordsPlayed, time);
        PlayerPrefs.SetInt("WordsPlayed", ++wordsPlayed);
    }


    public static void SaveSubstitution(string word, int gems, int coins, int score, int totalScore, float time, string gameType)
    {
        Debug.Log("level Substitution: " + playerSubstitutionLevel);
        Debug.Log("gems: " + gems);
        Debug.Log("coins: " + coins);
        Debug.Log("score: " + score);
        Debug.Log("time: " + time);
        Debug.Log("Game Type: " + gameType);

        PlayerPrefs.SetInt("playerSubstitutionLevel", playerSubstitutionLevel);
        if (Game.IncrementCoinsGems == true)
        {
            PlayerPrefs.SetInt("PlayerTotalGems", playerTotalGems + gems);
            PlayerPrefs.SetInt("PlayerTotalCoins", playerTotalCoins + coins);
            PlayerPrefs.SetInt("PlayerTotalScore", playerTotalScore + score);
            PlayerPrefs.SetInt("PlayerLevelScoreSub", playerLevelScoreSub + totalScore);
        }
        PlayerPrefs.SetString("gameType", gameType);
        PlayerPrefs.SetString("word" + wordsPlayed, word);
        PlayerPrefs.SetFloat("time" + wordsPlayed, time);
        PlayerPrefs.SetInt("WordsPlayed", ++wordsPlayed);
    }

    public static void Load()
    {
        playerInsertionLevel = PlayerPrefs.GetInt("playerInsertionLevel", 0);
        playerSubstitutionLevel = PlayerPrefs.GetInt("playerSubstitutionLevel", 0);
        playerOmissionLevel = PlayerPrefs.GetInt("playerOmissionLevel", 0);

        playerTotalGems = PlayerPrefs.GetInt("PlayerTotalGems", 0);
        playerTotalCoins = PlayerPrefs.GetInt("PlayerTotalCoins", 0);
        playerTotalScore = PlayerPrefs.GetInt("PlayerTotalScore", 0);
        playerLevelScore = PlayerPrefs.GetInt("PlayerLevelScore", 0);
        playerLevelScoreSub = PlayerPrefs.GetInt("PlayerLevelScoreSub", 0);
        playerLevelScoreOmi = PlayerPrefs.GetInt("PlayerLevelScoreOmi", 0);
        wordsPlayed = PlayerPrefs.GetInt("WordsPlayed", 0);
        playerGameType = PlayerPrefs.GetString("gameType");
        DetermineLevel();
        DetermineLevelSub();
        DetermineLevelOmi();
        Debug.Log("level: "+playerInsertionLevel);
        Debug.Log("playerTotalGems: " + playerTotalGems);
        Debug.Log("playerTotalCoins: " + playerTotalCoins);
        Debug.Log("wordsPlayed: " + wordsPlayed);
        Debug.Log("playerTotalScore: " + playerTotalScore);
        Debug.Log("playerLevelScore: " + playerLevelScore);
        Debug.Log("playerLevelScoreSub: " + playerLevelScoreSub);
        Debug.Log("playerLevelScoreOmi: " + playerLevelScoreOmi);
        Debug.Log("gameType: " + playerGameType);
    }

    static void DetermineLevel()
    {
        if (playerLevelScore <= 3)
        {
            playerInsertionLevel = 0;
        }
        else if (playerLevelScore > 3 && playerLevelScore <= 10)
        {
            playerInsertionLevel = 1;
        }
        else
        {
            playerInsertionLevel = 2;
        }
    }

    static void DetermineLevelSub()
    {
        if (playerLevelScoreSub <= 3)
        {
            playerSubstitutionLevel = 0;
        }
        else if (playerLevelScoreSub > 3 && playerLevelScoreSub <= 10)
        {
            playerSubstitutionLevel = 1;
        }
        else
        {
            playerSubstitutionLevel = 2;
        }
    }

    static void DetermineLevelOmi()
    {
        if (playerLevelScoreOmi <= 3)
        {
            playerOmissionLevel = 0;
        }
        else if (playerLevelScoreOmi > 3 && playerLevelScoreOmi <= 10)
        {
            playerOmissionLevel = 1;
        }
        else
        {
            playerOmissionLevel = 2;
        }
    }

    public static void Delete()
    {
        PlayerPrefs.DeleteAll();
    }
}

public class WordStat
{
    string word;
    float time;

    public WordStat (string word, float time)
    {
        this.word = word;
        this.time = time;
    }
}
