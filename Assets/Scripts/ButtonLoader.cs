using System;
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