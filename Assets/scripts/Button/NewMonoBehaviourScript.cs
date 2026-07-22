using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public void Button()
    {
        SceneManager.LoadScene("BattleScene");
    }
}
