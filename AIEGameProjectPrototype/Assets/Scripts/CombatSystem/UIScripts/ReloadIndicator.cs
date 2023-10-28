using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReloadIndicator : MonoBehaviour
{
    [SerializeField] WeaponHolder weaponHolder;
    RangedWeapon currentWeapon;

    SpriteRenderer reloadBarSprite;
    [SerializeField]TextMeshPro reloadingText;
    [SerializeField] float maxSize = 5;


    float animateDuration = 5;

    Coroutine CR_AnimateBar = null;


    void Awake()
    {
        reloadBarSprite = GetComponent<SpriteRenderer>();
        if(reloadBarSprite)
        {
            reloadBarSprite.enabled = false;
            reloadingText.enabled = false;
        }
    }


    void Start()
    {
        if(weaponHolder)
        {
            currentWeapon = weaponHolder.CurrentWeapon;
            weaponHolder.OnScrollWeapon += ChangeWeapon;
        }


        currentWeapon.OnStartReload += BeginAnimateBar;
    }


    void ChangeWeapon(RangedWeapon weapon)
    {
        CancelAnimation();

        currentWeapon.OnStartReload -= BeginAnimateBar;

        currentWeapon = weapon;

        currentWeapon.OnStartReload += BeginAnimateBar;
    }

    void CancelAnimation()
    {
        if(CR_AnimateBar != null)
        {
            StopCoroutine(CR_AnimateBar);
        }

        reloadBarSprite.enabled = false;
        reloadingText.enabled = false;


    }

    public void BeginAnimateBar()
    {
        animateDuration = currentWeapon.WeaponData.ReloadDuration;
        CR_AnimateBar = StartCoroutine(AnimateBar(animateDuration));
    }


    IEnumerator AnimateBar(float duration)
    {

        reloadBarSprite.enabled = true;
        reloadingText.enabled = true;

        float time = 0;   

        Vector2 initialSize = new Vector2(reloadBarSprite.size.x, 0);
        Vector2 targetSize = new Vector2(reloadBarSprite.size.x, maxSize);

        while(time < duration)
        {
            reloadBarSprite.size = Vector2.Lerp(initialSize, targetSize, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        
        reloadBarSprite.enabled = false;
        reloadingText.enabled = false;

        CR_AnimateBar = null;
    }


}
