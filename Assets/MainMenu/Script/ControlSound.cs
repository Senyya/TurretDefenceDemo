using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlSound : MonoBehaviour
{
    public AudioSource sound;
    public Slider slider;
    public void controlSound()
    {
        sound.volume = slider.value;
    }

}
