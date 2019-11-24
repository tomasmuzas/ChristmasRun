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

        public void LoadMenu()
        {
            SceneManager.LoadScene(SceneIndexes.Menu);
        }

	    public void ExitGame() 
	    {
 	        Application.Quit ();
 	    }
    }
}
