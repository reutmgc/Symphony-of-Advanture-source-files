using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.Events;
using System;
using Yarn;

public class WalkmanUI : MonoBehaviour
{
    private AudioManager audioManager;
    DialogueManager dialogueManager;
    [SerializeField]
    private InputActionReference nextSongAction, lastSongAction, confirmAction;

    [SerializeField]
    private TMP_Text emotionTxt; // this is the emotion we are asking the player get the character to feel in this specific dialogue;
    [SerializeField]
    Image leftArrow, rightArrow;
    [SerializeField]
    float hightlightArrowDuration = 0.3f;
    [SerializeField]
    Sprite selectorOn, selectorOff;
    [SerializeField]
    Sprite[] cassetteImages;
    [SerializeField]
    Image cassetteTapeLocation;
    int cassetteImageIndex = 0;

    private void Start()
    {
    }
    private void OnEnable()
    {
        if (audioManager == null)
            audioManager = AudioManager.Instance;
        audioManager.PlayCurrentTrack();
        audioManager.OnTrackChanged.AddListener(UpdateDisplay); 
        cassetteImageIndex = 0;
        UpdateDisplay();
    }
    private void OnDisable()
    {
        audioManager.OnTrackChanged.RemoveListener(UpdateDisplay);
    }
    private void Update()
    {
        if(lastSongAction.action.WasPressedThisFrame())
        {
            LastWasPressed();
        }
        if (nextSongAction.action.WasPressedThisFrame())
        {
            NextWasPressed();
        }
        if (confirmAction.action.WasPressedThisFrame())
        {
            ServiceLocator.Instance.Get<DialogueManager>().PlayerLabeledTrack();
        }
    }
    public void NextWasPressed()
    {
        Debug.Log("next was pressed");
        cassetteImageIndex = (cassetteImageIndex + 1) % (cassetteImages.Length - 1);
        audioManager.PlayLastTrack();
        StartCoroutine(HighlightArrow(rightArrow));

    }
    public void LastWasPressed()
    {
        audioManager.PlayNextTrack();
        StartCoroutine(HighlightArrow(leftArrow));
        if (cassetteImageIndex == 0)
            cassetteImageIndex = cassetteImages.Length - 1;
        else
            cassetteImageIndex--;
    }
    IEnumerator HighlightArrow(Image arrow)
    {
        arrow.sprite = selectorOn;
        yield return new WaitForSeconds(hightlightArrowDuration);
        arrow.sprite = selectorOff;

    }
    // this method will be called when a new song is being played 
    void UpdateDisplay()
    {
        //TrackData currentTrack = audioManager.GetCurrentTrack();
        // do something to indicate we are switching songs 
        Debug.Log("updating display " + cassetteImageIndex);
        cassetteTapeLocation.sprite = cassetteImages[cassetteImageIndex];

    }
}
