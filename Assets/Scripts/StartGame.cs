using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public GameObject foreGround;
    public GameObject backGround;


    public void ChangeScene()
    {
 
        StartCoroutine(CharacterSelector());
    }

   private IEnumerator CharacterSelector()
    {
        backGround.GetComponent<Animator>().SetTrigger("backStart");
        yield return new WaitForSeconds(0.2f);
        foreGround.GetComponent<Animator>().SetTrigger("ForeStart");
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene("Beta_v1");
    }

}
