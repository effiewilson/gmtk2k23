using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndLevel : MonoBehaviour
{
    public TMP_Text text1;
    public TMP_Text text2;


    void Start()
    {
        text1.text = ""; 
        text2.text = "";
    }

    // Start is called before the first frame update
    public void EndLevelNow(float damageLevel)
    {
        string scoreString1 = string.Format("{0}% of you was cut off by this Renaissance piss artist.", (int)(damageLevel * 100));
        string scoreString2;
        if (damageLevel > 0.6)
        {
            scoreString2 = "expensive gravel";
        }
        else if(damageLevel > 0.5)
        {
                scoreString2 = "an upmarket rockery";
        }
        else if (damageLevel > 0.3)
        {
            scoreString2 = "the Belevedere Torso";
        }
        else if (damageLevel > 0.15)
        {
            scoreString2 = "the Venus de Milo";
        }
        else
        {
            scoreString2 = "Michaelangelo's David";
        }

        SetEndLevelText(scoreString1, string.Format("You are {0}.",scoreString2));
    }

    private void SetEndLevelText(string s1, string s2)
    {
        text1.text = s1;
        text2.text = s2;
    }
}
