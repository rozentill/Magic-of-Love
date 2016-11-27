using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class SceneManager : BehaviorSingleton<SceneManager> {
    
    public void Load()
    {
        print(Global.story[Global.currentStory]);

        UnityEngine.SceneManagement.SceneManager.LoadScene(Global.story[Global.currentStory]);
    }

}
