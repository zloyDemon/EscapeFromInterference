using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    private const string LogoAnimationIdle = "logo_anim_idle";
    private const string LogoAnimation = "logo_anim";
    
    [SerializeField] private Button newGameBtn;
    [SerializeField] private Button preferenceBtn;
    [SerializeField] private Image logoImg;

    private Animator logoAnimator;
    private bool canPlayLogoAnimation;
    
    private void Awake()
    {
        newGameBtn.onClick.AddListener(NewGameClick);
        preferenceBtn.onClick.AddListener(PrefOpen);
        StartCoroutine(PlayLogoAnimation());
        UIManager.Instance.SetBlackScreen(true);
        
        UIManager.Instance.BlackScrFadeOut(2f, () => { });
        logoAnimator = logoImg.GetComponent<Animator>();
        
        
    }

    private void NewGameClick()
    {
        UIManager.Instance.BlackScrFadeInOut(2f, type =>
        {
            if(type == EFIEnums.FadeType.FadeIn)
                UIManager.Instance.ShowHideLoadingScreen(true);
            if(type == EFIEnums.FadeType.FadeOut)
                SceneManager.LoadScene(1);
        });
    }

    private void OnEnable()
    {
        canPlayLogoAnimation = true;
    }

    private void OnDisable()
    {
        canPlayLogoAnimation = false;
    }

    private void PrefOpen()
    {
        UIManager.Instance.ShowMenu<PreferenceWindow>(m => {});
    }

    private IEnumerator PlayLogoAnimation()
    {
        yield return new WaitForSeconds(2);
        logoAnimator.Play(LogoAnimation);
        while (canPlayLogoAnimation)
        {
            var timeSeconds = Random.Range(5, 10);
            yield return new WaitForSeconds(timeSeconds);
            logoAnimator.Play(LogoAnimation);
        }
    }
}
