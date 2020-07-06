using UnityEngine;
using UnityEngine.Playables;
using Game;

public class CinematicTrigger : MonoBehaviour
{
    private PlayableDirector timeline;

    private ThirdPersonController playerController;

    void Start()
    {
        timeline = GetComponent<PlayableDirector>();
        timeline.Stop();
        timeline.stopped += OnPlayableDirectorStopped;
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag == "Player")
        {
            Debug.Log("Player timeline trigger");
            timeline.Play();
            playerController = c.gameObject.GetComponent<ThirdPersonController>();
            playerController.Disable(); // Disable player movements
        }
    }

    void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {
        if (timeline == aDirector)
        {
            Debug.Log("PlayableDirector named " + aDirector.name + " is now stopped.");
            playerController.Enable(); // Enabled player movements back again
            GetComponent<Collider>().enabled = false; // Play the cutscene once
        }
    }

    void OnDisable()
    {
        timeline.stopped -= OnPlayableDirectorStopped;
    }
}
