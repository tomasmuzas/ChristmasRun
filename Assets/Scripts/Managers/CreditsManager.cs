using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Managers
{
  public class CreditsManager : MonoBehaviour
  {
     public void LoadMainLevel() 
     {
        EventManager.DisposeAllHandlers();
        SceneManager.LoadScene(0);
     }
  }
}