using UnityEngine;
using UnityEngine.UI;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] private RawImage backgroundImage;
    private float XoffsetBackground = 0.1f;
    private float YoffsetBackground = 0.1f;

    // Update is called once per frame
    void Update()
    {
        backgroundImage.uvRect=new Rect(backgroundImage.uvRect.position + new Vector2( XoffsetBackground * Time.deltaTime , YoffsetBackground * Time.deltaTime), backgroundImage.uvRect.size);
    }
}
