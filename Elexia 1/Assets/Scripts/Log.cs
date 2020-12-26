using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log : MonoBehaviour
{
    void OnGUI()
    {
        for (int i = 0; i < Game.inst.slots.Length; i++)
        {
            GUILayout.Label(i + " over " + Game.inst.slots[i].over);
        }
    }

}
