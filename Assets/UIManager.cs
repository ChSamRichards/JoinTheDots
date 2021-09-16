using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Dropdown dropdowncols;
    public Dropdown dropdownrows;
    public Dropdown dropdownplayers;
   
    // Start is called before the first frame update
    
    public void StartGame()
    {
        if (dropdownrows.value == 5  && dropdowncols.value == 4 && dropdownplayers.value == 1)
        {
            SceneManager.LoadScene("MainScene");
        }
        else
        {
            Debug.Log(dropdownrows.value + dropdowncols.value + dropdownplayers.value);
        }
      
       
    }

   






}
