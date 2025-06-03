using UnityEngine;
using UnityEngine.SceneManagement;

namespace SlimUI.ModernMenu{


    public class ExtraLinks : MonoBehaviour
    {
        

        public void OpenIntro()
        {
            
            SceneManager.LoadScene("Intro");
        }
        
        public void OpenDockThing()
        {
            SceneManager.LoadScene("DockThing");
        }

    }
}
