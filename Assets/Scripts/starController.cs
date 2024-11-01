using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class starController : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    private float time = 0.0f;
    private bool is_active = false;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(is_active);
    }

    // Update is called once per frame
    void Update()
    {
        if (is_active)
            stunAnimation();
    }

    private void stunAnimation()
    {
        time += Time.deltaTime;
        int spriteIndex = Mathf.FloorToInt(time * 10);
        spriteRenderer.sprite = sprites[spriteIndex % sprites.Length];
    }

    public void setStarActive(){
        is_active = true;
        gameObject.SetActive(true);
    }
}
