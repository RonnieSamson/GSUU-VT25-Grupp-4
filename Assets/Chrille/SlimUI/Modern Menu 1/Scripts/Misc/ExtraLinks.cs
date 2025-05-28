using UnityEngine;
using UnityEngine.SceneManagement;

namespace SlimUI.ModernMenu{
    
    
    public class ExtraLinks : MonoBehaviour
    {

        public string[] scenes = {"Chrille"};
        public void OpenChrille()
        {
            SceneManager.LoadScene(scenes[0]);
        }

    }
}
