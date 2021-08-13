using UnityEngine;
using UnityEngine.UI;

public class KeyStrokeScr : MonoBehaviour
{
    public CanvasGroup group;
    public Image[] images = new Image[6];
    public Color pressed = new Color(255, 255, 255, 100);
    public Color unpressed = new Color(0, 0, 0, 100);

    // Update is called once per frame
    void FixedUpdate()
    {
        group.alpha = CamScr.isFreecaming ? 0f : 1f;
        for (int i = 0; i < 6; i++)
        {
            images[i].color = getInput(i) ? pressed : unpressed;
        }
    }
    private bool getInput(int index)
    {
        switch(index)
        {
            case 0:
                return Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
            case 1:
                return Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
            case 2:
                return Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
            case 3:
                return Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
            case 4:
                return Input.GetKey(KeyCode.J);
            case 5:
                return Input.GetKey(KeyCode.K);
            default:
                return false;
        }
    }

}
