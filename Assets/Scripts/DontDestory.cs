using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestory : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        GameObject.FindGameObjectsWithTag("MusicSeamless");
        if (GameObject.FindGameObjectsWithTag("MusicSeamless").Length > 1)
            Destroy(this.gameObject);
        DontDestroyOnLoad(this.gameObject);
    }

}
