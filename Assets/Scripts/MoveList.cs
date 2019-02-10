using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct moveData
{
    Vector3 knockBack;
    float damage;
    float hitStun;
    string name;

    public moveData(Vector3 in1, float in2, float in3, string in4) {
        knockBack = in1;
        damage    = in2;
        hitStun   = in3;
        name      = in4;
    }   
}

public class MoveList : MonoBehaviour
{

    //Define a default movelist
    IDictionary<string, moveData> moveList = new Dictionary<string, moveData>() {

        {"Jab",             new moveData(Vector3.zero, 1.0f, 1.0f, "default")},
        {"Forward-Normal",  new moveData(Vector3.zero, 1.0f, 1.0f, "default")},
        {"Down-Normal",     new moveData(Vector3.zero, 1.0f, 1.0f, "default")},
        {"Up-Normal",       new moveData(Vector3.zero, 1.0f, 1.0f, "default")},

        {"Neutral-Air",     new moveData(Vector3.zero, 1.0f, 1.0f, "default")},
        {"Forward-Air",     new moveData(Vector3.zero, 1.0f, 1.0f, "default")},
        {"Back-Air",        new moveData(Vector3.zero, 1.0f, 1.0f, "default")},
        {"Up-Air",          new moveData(Vector3.zero, 1.0f, 1.0f, "default")},
        {"Down-Air",        new moveData(Vector3.zero, 1.0f, 1.0f, "default")},

        {"Neutral-Special", new moveData(Vector3.zero, 1.0f, 1.0f, "default")},
        {"Forward-Special", new moveData(Vector3.zero, 1.0f, 1.0f, "default")},
        {"Down-Special",    new moveData(Vector3.zero, 1.0f, 1.0f, "default")},
        {"Up-Special",      new moveData(Vector3.zero, 1.0f, 1.0f, "default")},
    };

    

}
