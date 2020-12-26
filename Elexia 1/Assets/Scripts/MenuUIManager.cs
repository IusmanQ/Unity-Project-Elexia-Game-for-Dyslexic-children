using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MenuUIManager : MonoBehaviour
{
    private string NarrativeScene = "Narrative";

    [SerializeField]
    public GameObject Main_Canvas;

    [SerializeField]
    public GameObject Menu;

    [SerializeField]
    Toggle VolumeToggle;

    [SerializeField]
    Toggle MusicToggle;

    [SerializeField]
    public GameObject Levels_Menu;


    [SerializeField]
    public GameObject Settings_Menu;

    [SerializeField]
    public Image Volume_handler;

    [SerializeField]
    public Image Music_handler;


    [SerializeField]
    public GameObject Quit_Menu;

    public AudioSource menuAudio;

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
        menuAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Basic_Menu Functions

    public void PlayButton()
    {
        
        SceneManager.LoadScene(NarrativeScene);
    }

    public void LevelsButton()
    {
        Menu.SetActive(false);
        Levels_Menu.SetActive(true);
    }

    public void SettingsButton()
    {
        Menu.SetActive(false);
        Settings_Menu.SetActive(true);
    }

    public void QuitButton()
    {
        Menu.SetActive(false);
        Quit_Menu.SetActive(true);
    }

    //Settings_Menu Functions

    public void VolumeOnOff_Button()
    {
        if (VolumeToggle.isOn)
        {
            //VOLUME IS ON
            Volume_handler.transform.localPosition = new Vector2(100.0f, -2.6f);
            menuAudio.Play();

        }
        else if (VolumeToggle.isOn == false)
        {
            //VOLUME IS OFF
            Volume_handler.transform.localPosition = new Vector2(12.0f, -2.6f);
            menuAudio.Pause();
        }
    }

    public void MusicOnOff_Button()
    {
        if (MusicToggle.isOn)
        {
            //Music IS ON
            Music_handler.transform.localPosition = new Vector2(100.0f, 0.24725f);
            menuAudio.Play();
        }
        else if (MusicToggle.isOn == false)
        {
            //Music IS OFF
            Music_handler.transform.localPosition = new Vector2(12.0f, 0.24725f);
            menuAudio.Pause();
        }
    }

    public void Settings_Quit_Button()
    {
        Settings_Menu.SetActive(false);
        Menu.SetActive(true);
    }


    //Exit_Menu Functions

    public void Exit_Yes_Button()
    {
        Application.Quit();
    }

    public void Exit_No_Button()
    {
        Quit_Menu.SetActive(false);
        Main_Canvas.SetActive(true);

    }

    public void Levels_Back_Button()
    {
        Levels_Menu.SetActive(false);
        Menu.SetActive(true);
    }

    public void Display_Menu_Button()
    {
        Main_Canvas.SetActive(false);
        Menu.SetActive(true);
    }

    public void Back_to_Main_Canvas()
    {
        Menu.SetActive(false);
        Main_Canvas.SetActive(true); 
    }

    

}
