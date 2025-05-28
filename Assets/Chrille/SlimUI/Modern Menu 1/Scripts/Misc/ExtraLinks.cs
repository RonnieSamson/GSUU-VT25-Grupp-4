using UnityEngine;
using UnityEngine.SceneManagement;

namespace SlimUI.ModernMenu{


    public class ExtraLinks : MonoBehaviour
    {

        
        public void OpenChrille()
        {
            SceneManager.LoadScene("Chrille");
        }
        
        public void OpenDockThing()
        {
            SceneManager.LoadScene("DockThing");
        }

    }
}
