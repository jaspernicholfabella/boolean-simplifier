using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Goto : MonoBehaviour {


    public void ButtonOnClick(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

}
