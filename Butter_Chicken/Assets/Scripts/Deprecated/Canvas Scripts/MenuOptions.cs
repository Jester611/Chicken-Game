using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuOptions : MonoBehaviour
{
    public void ExitOptions()
    {
        SceneManager.LoadScene(0);
    }
}