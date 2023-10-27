using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReloadIndicator : MonoBehaviour
{

    public RangedWeapon currentWeapon;

    SpriteRenderer reloadBarSprite;
    [SerializeField]TextMeshPro reloadingText;

    [SerializeField] float maxHeight = 5;

    public float animateDuration = 5;


    void Awake()
    {
        reloadBarSprite = GetComponent<SpriteRenderer>();
        currentWeapon.OnStartReload += BeginAnimateBar;

        reloadBarSprite.enabled = false;
        reloadingText.enabled = false;
    }

    void Start()
    {
        //StartCoroutine(AnimateBar(10));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BeginAnimateBar()
    {
        animateDuration = currentWeapon.WeaponData.ReloadDuration;
        StartCoroutine(AnimateBar(animateDuration));
    }


    IEnumerator AnimateBar(float duration)
    {

        reloadBarSprite.enabled = true;
        reloadingText.enabled = true;

        float time = 0;   

        Vector2 initialSize = new Vector2(reloadBarSprite.size.x, 0);
        Vector2 targetSize = new Vector2(reloadBarSprite.size.x, maxHeight);

        while(time < duration)
        {
            reloadBarSprite.size = Vector2.Lerp(initialSize, targetSize, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        
        reloadBarSprite.enabled = false;
        reloadingText.enabled = false; 

    }


}
