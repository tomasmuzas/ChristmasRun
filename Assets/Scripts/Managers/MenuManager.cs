using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Managers
{
    public class MenuManager : MonoBehaviour
    {
        public void StartGame()
        {
            EventManager.DisposeAllHandlers();
            SceneManager.LoadScene(SceneIndexes.Game);
        }
    }
}
