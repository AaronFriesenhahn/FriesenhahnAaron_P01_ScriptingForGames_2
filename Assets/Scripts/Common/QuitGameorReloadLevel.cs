using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitGameorReloadLevel : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape key was pressed");
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Debug.Log("Backspace key was pressed");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
