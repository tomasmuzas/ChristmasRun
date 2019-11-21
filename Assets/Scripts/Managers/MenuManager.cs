using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Managers
{
    public class MenuManager : MonoBehaviour
    {
        public void StartGame()
        {
            SceneManager.LoadScene(1);
        }
    }
}
