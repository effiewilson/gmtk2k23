using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scenes
{
    public class ActiveLoader : MonoBehaviour
    {
        
        public String sceneToLoad;
        public Slider loadMeter;

        private void Awake()
        {
            StartCoroutine(LoadScene());
            // SceneManager.LoadScene(sceneToLoad);
        }


        IEnumerator LoadScene()
        {
            yield return null;

            //Begin to load the Scene you specify
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneToLoad);
            //Don't let the Scene activate until you allow it to
            asyncOperation.allowSceneActivation = false;
            Debug.Log("Pro :" + asyncOperation.progress);
            //When the load is still in progress, output the Text and progress bar
            while (!asyncOperation.isDone)
            {
                loadMeter.value = asyncOperation.progress;

                if (asyncOperation.progress >= 0.9f)
                {
                    asyncOperation.allowSceneActivation = true;
                }

                yield return null;
            }

            if (asyncOperation.isDone)
            {
                SceneManager.UnloadSceneAsync(gameObject.scene);
            }
        }
    }
}