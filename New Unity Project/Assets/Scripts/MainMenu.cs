using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
  public void PlayGame()
    {

        SceneManager.LoadScene(20);

    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void PotionButton()
    {
        Inventory.PotionUse(); 
    }
    public void StanceButton()

    {
        bool temp = GameController.Stance;

        GameController.Stance = !temp;
    }

    public void Credits()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }
    public void  BackToMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -2);
    }

    private IEnumerator StartGame()
    {
        SceneManager.LoadScene(19);
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(1);
    }
}
