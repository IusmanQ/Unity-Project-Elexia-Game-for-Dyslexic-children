using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class levelsManager : MonoBehaviour
{

    //board1
    public GameObject board1_level2_lock;
    public GameObject board1_level3_lock;

    //board2
    public GameObject board2_level2_lock;
    public GameObject board2_level3_lock;

    //board3
    public GameObject board3_level2_lock;
    public GameObject board3_level3_lock;

    // Start is called before the first frame update
    void Start()
    {
        Player.Load();
        determineBoard1Level();
        determineBoard2Level();
        determineBoard3Level();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void determineBoard1Level() //insertion
    {
        if (Player.playerInsertionLevel == 1)
        {
            board1_level2_lock.SetActive(false);
            board1_level3_lock.SetActive(true);
        }
        else if (Player.playerInsertionLevel == 2)
        {
            board1_level2_lock.SetActive(false);
            board1_level3_lock.SetActive(false);
        }
        print("Insertion level: "+ Player.playerInsertionLevel);
    }

    public void determineBoard2Level()  //omission
    {
        if (Player.playerOmissionLevel == 1)
        {
            board2_level2_lock.SetActive(false);
            board2_level3_lock.SetActive(true);
        }
        else if (Player.playerOmissionLevel == 2)
        {
            board2_level2_lock.SetActive(false);
            board2_level3_lock.SetActive(false);
        }

        print("Omission level: " + Player.playerOmissionLevel);
    }

    public void determineBoard3Level() //substitution
    {
        if (Player.playerSubstitutionLevel == 1)
        {
            board3_level2_lock.SetActive(false);
            board3_level3_lock.SetActive(true);
        }
        else if (Player.playerSubstitutionLevel == 2)
        {
            board3_level2_lock.SetActive(false);
            board3_level3_lock.SetActive(false);
        }

        print("substitution level: " + Player.playerSubstitutionLevel);
    }
}
