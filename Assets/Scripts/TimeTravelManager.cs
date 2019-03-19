using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTravelManager : MonoBehaviour
{

    public void UpdatePersistentClone() {

        PlayerManager playerManager = transform.GetComponent<PlayerManager>();
        ControllerHandler controllerHandler = GameObject.Find("ControllerHandler").GetComponent<ControllerHandler>();


        if (playerManager.getCharacterEcho() == null) {
            playerManager.createEcho(new Queue<TBInput>(controllerHandler.getRecording(playerManager.GetWhichPlayer())), playerManager.getRecordPositionList());
        }


    }


}
