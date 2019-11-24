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

    	public void OpenCredits()
    	{
    	    SceneManager.LoadScene(SceneIndexes.Credits);
    	}

        public void OpenMenu()
        {
            SceneManager.LoadScene(SceneIndexes.Menu);
        }

        public void OpenSkinStore()
        {
            SceneManager.LoadScene(SceneIndexes.SkinStore);
        }

        public void ExitGame() 
	    {
 	        Application.Quit ();
 	    }

        public void ResetGameState()
        {
            PlayerPrefs.DeleteAll();
            StartGame();
        }
    }
}
