using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class AudioManager :MonoBehaviour
{
    public  AudioSource successSound;
     int combo = 0;

    public void ResetCombo()
    {
        combo = 0;
    }
    public void PlaySuccessSound()
    {
        successSound.pitch = 1f + (combo * 0.05f);
        successSound.Play();
        combo++;
    }
}
