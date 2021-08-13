using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SwitchChar : MonoBehaviour
{
    public GameObject char1, char2;
    public CinemachineVirtualCamera vcam;
    int whichCharIsOn = 1;

    // Start is called before the first frame update
    void Start()
    {
        char1.gameObject.SetActive(true);
        char2.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        SwitchCharacter();
    }

    void SwitchCharacter(){
        // vcam = GetComponent<CinemachineVirtualCamera>();
        Vector3 pos1 = char1.transform.position;
        Vector3 pos2 = char2.transform.position;
        if(Input.GetKeyDown("2") && whichCharIsOn == 1){
            vcam.Follow = char2.transform;
            char2.transform.position = pos1;
            whichCharIsOn = 2;
            char1.gameObject.SetActive(false);
            char2.gameObject.SetActive(true);
        }
        if(Input.GetKeyDown("1") && whichCharIsOn == 2){
            vcam.Follow = char1.transform;
            char1.transform.position = pos2;
            whichCharIsOn = 1;
            char1.gameObject.SetActive(true);
            char2.gameObject.SetActive(false);
        }
    }
}
