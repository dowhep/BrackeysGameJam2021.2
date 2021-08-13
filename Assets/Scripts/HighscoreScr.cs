using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreScr : MonoBehaviour
{
    float height;
    float dist;
    float speedh;
    float speedv;

    public GameObject player;
    public Text heightTxt;
    public Text distTxt;
    public Text speedhTxt;
    public Text speedvTxt;
    public Vector3 offset = new Vector3(6.35f, -1.7f, 0.6f);


    public GameObject heightInd;
    public GameObject distInd;

    Rigidbody2D rig;
    void Start()
    {
        height = PlayerPrefs.GetFloat("Height", 0f);
        dist = PlayerPrefs.GetFloat("Distance", 0f);
        speedh = PlayerPrefs.GetFloat("SpeedH", 0f);
        speedv = PlayerPrefs.GetFloat("SpeedV", 0f);
        rig = player.GetComponent<Rigidbody2D>();
        heightInd.transform.position = new Vector2(0f, height + offset.y + offset.z);
        distInd.transform.position = new Vector2(dist + offset.x + offset.z, 0f);
    }

    void Update()
    {
        if (rig.velocity.y > 0)
        {
            height = Mathf.Max(height, rig.transform.position.y - offset.y);
            speedv = Mathf.Max(speedv, rig.velocity.y);
            heightInd.transform.position = new Vector2(0f, height + offset.y + offset.z);
        }
        if (rig.velocity.x > 0)
        {
            dist = Mathf.Max(dist, rig.transform.position.x - offset.x);
            speedh = Mathf.Max(speedh, rig.velocity.x);
            distInd.transform.position = new Vector2(dist + offset.x + offset.z, 0f);
        }
    }
    void FixedUpdate()
    {
        heightTxt.text = "Highest Height: " + height.ToString("F") + " m";
        distTxt.text = "Highest Distance: " + dist.ToString("F") + " m";
        speedhTxt.text = "Highest Horizontal Speed: " + speedh.ToString("F") + " m/s";
        speedvTxt.text = "Highest Vertical Speed: " + speedv.ToString("F") + " m/s";
    }
    private void OnDestroy()
    {
        PlayerPrefs.SetFloat("Height", height);
        PlayerPrefs.SetFloat("Distance", dist);
        PlayerPrefs.SetFloat("SpeedH", speedh);
        PlayerPrefs.SetFloat("SpeedV", speedv);
    }
    public void ResetHighscores()
    {
        PlayerPrefs.SetFloat("Height", 0f);
        PlayerPrefs.SetFloat("Distance", 0f);
        PlayerPrefs.SetFloat("SpeedH", 0f);
        PlayerPrefs.SetFloat("SpeedV", 0f);
        Start();
    }
}
