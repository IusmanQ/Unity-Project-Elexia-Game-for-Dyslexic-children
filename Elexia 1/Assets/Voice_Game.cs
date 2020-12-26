using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Voice_Game : MonoBehaviour
{
    public static string vWord;
    public static Sprite vWordImage;
    private float slotsGap = 0.2f;
    public Slot[] slots;
    public Letter[] letters;

    public static bool GameVoice = false;
    public static bool Sub_GameVoice = false;
    public static bool Omi_GameVoice = false;


    public string scoreScreen = "Score_Screen";
    public string levelFailedScreen = "LevelFailedScreen";

    public static Voice_Game inst;
    void Awake()
    {
        inst = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        getWordForSpeech();
        generateLetters();
        


    }

    // Update is called once per frame
    void Update()
    {

    }

    void getWordForSpeech()
    {
        if (Game.InsertionActive == true)
        {
            vWord = Game.replayWord;
            vWordImage = Game.replayImage;
        }
        else if (Sub_Game.substitutionActive == true)
        {
            vWord = Sub_Game.replayWord;
            vWordImage = Sub_Game.replayImage;
        }
        else if (Omi_Game.omissionActive == true)
        {
            vWord = Omi_Game.replayWord;
            vWordImage = Omi_Game.replayImage;
        }
    }

    void generateLetters()
    {
        GameObject slotPrefab = (GameObject)Resources.Load("Prefabs/Slot");
        float slotSize = slotPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        Vector2 substitutePos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2f, Screen.height / 2)) - new Vector3((slots.Length / 2f) * slotsGap + 6f / 2 - ((slotsGap - letters.Length) / (slotsGap * 10f)), 4f, 0);
        letters = new Letter[vWord.Length];
        for (int i = 0; i < letters.Length; i++)
        {
            letters[i] = ((GameObject)Instantiate(Resources.Load("Prefabs/Letter"), substitutePos, Quaternion.Euler(0, 0, 0))).GetComponent<Letter>(); //GameObject.Instantiate(letterPrefab);
            letters[i].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Alphabets/" + vWord.ToUpper()[i]);
            letters[i].GetComponent<BoxCollider2D>().enabled = false;
            substitutePos += new Vector2(slotsGap + slotSize, 0);
        }
    }
}
