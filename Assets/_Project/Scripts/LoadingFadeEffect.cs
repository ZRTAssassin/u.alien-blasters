using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LoadingFadeEffect : SingletonPersistent<LoadingFadeEffect>
{
    public static bool _canLoad; // boolean to determinate when the actual loading can happen

    [SerializeField] Image _loadingBackgroundImage; // The background image to change alpha for effect

    [SerializeField] [Range(0f, 0.5f)]
    float _loadingStepTime; // A range of time to wait for every repetition on the effect

    [SerializeField] [Range(0f, 0.5f)] float _loadingStepValue; // The value to modify the alpha every steo time

    // Run the complete effect, use for the clients
    IEnumerator FadeAllEffect()
    {
        // Do the fade in effect 
        yield return StartCoroutine(FadeInEffect());

        // Wait a x time to call
        yield return new WaitForSeconds(1);

        // Do the fadeout effect
        yield return StartCoroutine(FadeOutEffect());
    }

    IEnumerator FadeInEffect()
    {
        // Get the background color
        Color backgroundColor = _loadingBackgroundImage.color;

        // Set the background  color alpha to 0
        backgroundColor.a = 0;

        // Set the modify background color to the background image color
        _loadingBackgroundImage.color = backgroundColor;

        // Turn on the background
        _loadingBackgroundImage.gameObject.SetActive(true);

        // Repeat until the alpha is <= to 1
        while (backgroundColor.a <= 1)
        {
            // Wait 
            yield return new WaitForSeconds(_loadingStepTime);

            // Change the background color bt the step value
            backgroundColor.a += _loadingStepValue;

            // Set the background image to the modify var
            _loadingBackgroundImage.color = backgroundColor;
        }

        // When the fade-in ends you can start loading the scene
        _canLoad = true;
    }

    IEnumerator FadeOutEffect()
    {
        // Set the loading to false, it should be already load the new scene
        _canLoad = false;

        // Get the background image color
        Color backgroundColor = _loadingBackgroundImage.color;

        // Repeat until the alpha is >= 0
        while (backgroundColor.a >= 0)
        {
            // Wait
            yield return new WaitForSeconds(_loadingStepTime);

            // Change the background color bt the step value
            backgroundColor.a -= _loadingStepValue;

            // Set the background image to the modify var
            _loadingBackgroundImage.color = backgroundColor;
        }

        // Turn of the background image
        _loadingBackgroundImage.gameObject.SetActive(false);
    }

    /// <summary>
    /// start the fade in
    /// </summary>
    public void FadeIn()
    {
        StartCoroutine(FadeInEffect());
    }

    /// <summary>
    /// Start the fadeout effect
    /// </summary>
    public void FadeOut()
    {
        StartCoroutine(FadeOutEffect());
    }

    /// <summary>
    /// Start a complete fade effect
    /// </summary>
    public void FadeAll()
    {
        StartCoroutine(FadeAllEffect());
    }
}