using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    public string sceneToLoad; 

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button7))
                SceneManager.LoadScene(sceneToLoad);
    }
}
