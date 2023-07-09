using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class ButtonLoader : MonoBehaviour
    {
        public String sceneToLoad;

        public void Load()
        {
            SceneManager.LoadScene(sceneToLoad);
        }


        
    }
}