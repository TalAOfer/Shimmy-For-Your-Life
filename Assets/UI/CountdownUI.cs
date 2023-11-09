using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CountdownUI : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    private Vector3 originalScale;
    public AnimationCurve beatCurve;
    private Image image;
    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        image = GetComponent<Image>();
        image.sprite = sprites[4];
    }

    public void OnMarker(Component sender, object data)
    {
        string markerString = (string)data;
        switch (markerString)
        {
            case "3":
                image.sprite = sprites[2];
                break;
            case "2":
                image.sprite = sprites[1];
                break;
            case "1":
                image.sprite = sprites[0];
                break;
            case "GO!":
                image.sprite = sprites[3];
                break;
        }
        anim.Play("Countdown_Vanish", -1, 0f);
    }
}
