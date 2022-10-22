using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public TMPro.TMP_Text levelText;
    public GameObject clickToPlayTextObject;
    public GameObject youLostTextObject;
    public GameObject youWinTextObject;


    public void LevelTextUpdate()
    {
        levelText.text = "Level " + Constants.level.ToString();
    }

    public void ShowClickToPlay(bool show)
    {
        clickToPlayTextObject.SetActive(show);
    }

    public void ShowYouLost()
    {
        youLostTextObject.SetActive(true);
        youLostTextObject.transform.localScale = new Vector3(0, 0, 0);
        LeanTween.scale(youLostTextObject, new Vector3(1, 1, 1), 3f).setEase(LeanTweenType.punch).setOnComplete(() => {
            youLostTextObject.SetActive(false);
        });
    }

    public void ShowYouWin()
    {
        youWinTextObject.SetActive(true);
        youWinTextObject.transform.localScale = new Vector3(0, 0, 0);
        LeanTween.scale(youWinTextObject, new Vector3(1, 1, 1), 3.5f).setEase(LeanTweenType.easeOutQuad).setOnComplete(() => {
            youWinTextObject.SetActive(false);
        });
    }

}
