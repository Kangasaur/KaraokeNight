using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class ButtonScript : MonoBehaviour
{
   public void RestartGame (){

        SceneManager.LoadScene("GameScene");

    }

    public void GoToTitleScreen() {

        SceneManager.LoadScene("Title Screen");
    }
}
