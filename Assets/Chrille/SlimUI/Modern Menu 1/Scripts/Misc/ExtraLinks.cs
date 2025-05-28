using UnityEngine;
using UnityEngine.SceneManagement;

namespace SlimUI.ModernMenu{


    public class ExtraLinks : MonoBehaviour
    {

        public string[] scenes = { "Chrille", "Dock Thing" };
        public void OpenChrille()
        {
            SceneManager.LoadScene(scenes[0]);
        }
        
        public void OpenDockThing()
        {
            SceneManager.LoadScene(scenes[1]);
        }

    }
}
