using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIMenu : MonoBehaviour
{
    private UIDocument _doc;
    private Button _playbutton;
    private Button _optionsbutton;
    private Button _exitbutton;

    private void Awake()
    {
        _doc = GetComponent<UIDocument>();
        _playbutton = _doc.rootVisualElement.Q<Button>("playbutton");
        _optionsbutton = _doc.rootVisualElement.Q<Button>("optionsbutton");
        _exitbutton = _doc.rootVisualElement.Q<Button>("exitbutton");

        _playbutton.clicked += playbuttononclicked;
        _optionsbutton.clicked += optionsbuttononclicked;
        _exitbutton.clicked += exitbuttononclicked;
    }

    private void playbuttononclicked()
    {
        SceneManager.LoadScene("1");
    }

    private void optionsbuttononclicked()
    {
        SceneManager.LoadScene("2");
    }

    private void exitbuttononclicked()
    {
        Application.Quit();
    }

}