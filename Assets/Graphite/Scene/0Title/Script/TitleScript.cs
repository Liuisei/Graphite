using UnityEngine;
using UnityEngine.UI;

public class TitleScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] Button startButton;

    private void Awake()
    {
        startButton.onClick.AddListener(() =>
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
        });
    }
}
