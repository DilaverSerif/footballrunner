using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private Text _text;
    [SerializeField] private Transform panel;
    public static Action<bool> FinishGame;
    [SerializeField] private Button resetButton;

    private void Start()
    {
        resetButton.onClick.AddListener(()=> SceneManager.LoadScene(0));        
    }

    private void OnEnable()
    {
        FinishGame += FinishTheGame;
    }

    private void OnDisable()
    {
        FinishGame -= FinishTheGame;
    }

    private void FinishTheGame(bool value)
    {
        panel.gameObject.SetActive(true);
        
        if (value)
        {
            _text.text = "YOU WIN!";
        }
        else _text.text = "YOU LOSE!";
    }
}
