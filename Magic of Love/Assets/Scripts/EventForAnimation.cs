using UnityEngine;
using System.Collections;

public class EventForAnimation : MonoBehaviour {

    public void LoadScene()
    {
        Global.Init();
        SceneManager.Instance.Load();
    }

	public void EnableInput()
    {
        Global.enableInput = true;
    }

    public void DisableInput()
    {
        Global.enableInput = false;
    }

    public void PlaySound(SE se)
    {
        AudioManager.Instance.Play(se);
    }
}
