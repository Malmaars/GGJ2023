using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    private void OnMouseDown()
    {
        //reload scene
        SceneManager.LoadScene(1);
    }
}
