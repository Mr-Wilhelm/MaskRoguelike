using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using Unity.Cinemachine;

public class CameraFadeScrip : MonoBehaviour
{

    public Image fadeImage;
    public float fadeDuration = 0.5f;
    private GameObject player;
    public Vector3 teleportPos = Vector3.zero;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public IEnumerator FadeOutIn()
    {
        
        player.GetComponent<PlayerMovement>().lockedMovement = true;
        yield return Fade(0f, 1f);
        player.transform.position = teleportPos;
        yield return new WaitForSeconds(1f);
        yield return Fade(1f, 0f);
        player.GetComponent<PlayerMovement>().lockedMovement = false;
    }
    public IEnumerator StartGameFade()
    {
        player.transform.position = teleportPos;
        yield return Fade(1f, 0f);
    }

    IEnumerator Fade(float from, float to)
    {
        float time = 0f;
        Color colour = fadeImage.color;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            colour.a = Mathf.Lerp(from, to, time/fadeDuration);
            fadeImage.color = colour;
            yield return null;
        }
        colour.a = to;
        fadeImage.color = colour;
    }
}
