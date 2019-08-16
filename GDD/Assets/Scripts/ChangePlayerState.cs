using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangePlayerState : MonoBehaviour
{
    public Text stateShown = null;

    public void ChangeStateA (string Attack)
    {
        stateShown.text = Attack;
    } 


  

}
