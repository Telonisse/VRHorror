using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class PhoneStorySequence : MonoBehaviour
{
    [Header("Spelare & Telefon")]
    public Transform playerCamera; // din ovrcamera (headsetet) för att kolla om de rör sig
    public XRGrabInteractable phoneGrabInteractable;

    [Header("Ljudkällor")]
    public AudioSource phoneAudioSource; // sitter på telefonen
    public AudioSource whisperAudioSource; // sitter på/nära spelarens huvud

    [Header("Ljudklipp")]
    public AudioClip whisperClip;
    public AudioClip ringClip;
    public AudioClip breathingClip;
    public AudioClip hangupClip;

    [Header("Inställningar")]
    public float timeToCheckMovement = 3f; // hur många sekunder vi väntar för att se om de går
    public float movementThreshold = 0.3f; // hur långt (i meter) de måste gå för att det inte ska räknas som att de står still

    private Vector3 startPosition;
    private bool isRinging = false;
    private bool hasAnswered = false;
    private Coroutine ignoreTimerCoroutine;

    void Start()
    {
        // spara var spelaren står exakt när spelet/scenen börjar
        startPosition = playerCamera.position;

        // börja kolla om de rör på sig eller står still
        StartCoroutine(CheckPlayerMovementSequence());

        // säg till scriptet att lyssna på när spelaren plockar upp telefonen
        phoneGrabInteractable.selectEntered.AddListener(OnPhoneAnswered);
    }

    IEnumerator CheckPlayerMovementSequence()
    {
        // vänta i X sekunder i början av spelet
        yield return new WaitForSeconds(timeToCheckMovement);

        // räkna ut hur långt headsetet har flyttat sig från startpunkten
        float distanceMoved = Vector3.Distance(startPosition, playerCamera.position);

        if (distanceMoved > movementThreshold)
        {
            // de började gå! strunta i viskningen och ring direkt.
            StartPhoneRing();
        }
        else
        {
            // de stod helt stilla i mörkret!
            StartCoroutine(PlayWhisperThenRing());
        }
    }

    IEnumerator PlayWhisperThenRing()
    {
        whisperAudioSource.clip = whisperClip;
        whisperAudioSource.Play();

        // vänta exakt så länge som visknings-ljudklippet är
        yield return new WaitForSeconds(whisperClip.length);

        StartPhoneRing();
    }

    void StartPhoneRing()
    {
        isRinging = true;
        phoneAudioSource.clip = ringClip;
        phoneAudioSource.loop = true; // telefonen ska ringa om och om igen
        phoneAudioSource.Play();

        // starta 10-sekunderstimern ifall de ignorerar den
        ignoreTimerCoroutine = StartCoroutine(IgnorePhoneTimer());
    }

    IEnumerator IgnorePhoneTimer()
    {
        yield return new WaitForSeconds(10f);

        // om 10 sekunder har gått och de fortfarande inte har svarat
        if (isRinging && !hasAnswered)
        {
            isRinging = false;
            phoneAudioSource.Stop();

            // här kan din kompis lägga in sin kod för haptic suit-rysningen senare!
            Debug.Log("Spelaren ignorerade telefonen. Rysning triggas här!");
        }
    }

    private void OnPhoneAnswered(SelectEnterEventArgs args)
    {
        // om de plockar upp telefonen men den inte ringer, gör ingenting
        if (!isRinging || hasAnswered) return;

        hasAnswered = true;
        isRinging = false;

        // avbryt 10-sekunderstimern så den inte slumpmässigt lägger på medan de lyssnar
        if (ignoreTimerCoroutine != null)
        {
            StopCoroutine(ignoreTimerCoroutine);
        }

        // starta andnings-sekvensen
        StartCoroutine(PlayBreathingAndHangup());
    }

    IEnumerator PlayBreathingAndHangup()
    {
        // sluta ringa
        phoneAudioSource.Stop();
        phoneAudioSource.loop = false;

        // spela upp andningen
        phoneAudioSource.clip = breathingClip;
        phoneAudioSource.Play();

        // vänta exakt 3 sekunder
        yield return new WaitForSeconds(3f);

        // lägg på!
        phoneAudioSource.Stop();
        if (hangupClip != null)
        {
            phoneAudioSource.PlayOneShot(hangupClip);
        }
    }

    void OnDestroy()
    {
        // städa upp lyssnaren om objektet förstörs
        if (phoneGrabInteractable != null)
        {
            phoneGrabInteractable.selectEntered.RemoveListener(OnPhoneAnswered);
        }
    }
}