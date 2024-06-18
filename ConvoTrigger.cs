using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;
using UnityEngine.ProBuilder;


public class ConvoTrigger : MonoBehaviour
{
    public NPCConversation startConvo;
    public NPCConversation FuzeConvo;
    public NPCConversation PackageConvo;
    public NPCConversation HelmetConvo;
    public NPCConversation OutsideConvo;
    public NPCConversation WalkedToShipConvo;
    public NPCConversation ScannerConvo;
    public NPCConversation AllDoneConvo;
    
    public AudioSource audioSource;
    public bool CheckingAudio = false;
    
    public bool HasNotDonePackageTask;
    //public GameObject FirstConvo;


// Called when the script instance is being loaded.
void Start() {
    // Initiates a coroutine to start the narrator delay.
    StartCoroutine(NaratorStartDelay());
}

// Coroutine that introduces a delay before starting a conversation.
public IEnumerator NaratorStartDelay() {
    // Waits for 4 seconds before continuing.
    yield return new WaitForSeconds(4);

    // Starts a conversation using the ConversationManager singleton.
    ConversationManager.Instance.StartConversation(startConvo);

    // Logs a message to the console for debugging purposes.
    Debug.Log("Working");
}

// Method to signal that the Fuze task is done and start checking audio status.
public void FuzeTaskDone() {
    // Sets the flag to start checking audio.
    CheckingAudio = true;

    // Starts the coroutine to check the audio status for the Fuze task.
    StartCoroutine(FuzeAudioStatus());
}

// Method to signal that the Package task is done and start checking audio status.
public void PackageTaskDone() {
    // If the package task has not been done before, skip the audio check.
    if (HasNotDonePackageTask) {
        // Log statement commented out for no narrator scenario.
        // Debug.Log("no narator");
        return;
    } else {
        // Sets the flag to start checking audio.
        CheckingAudio = true;

        // Starts the coroutine to check the audio status for the Package task.
        StartCoroutine(PackageAudioStatus());

        // Marks the Package task as done to avoid repeated checks.
        HasNotDonePackageTask = true;
    }
}

// Method to signal that the Helmet task is done and start checking audio status.
public void HelmetTaskDone() {
    // Sets the flag to start checking audio.
    CheckingAudio = true;

    // Starts the coroutine to check the audio status for the Helmet task.
    StartCoroutine(HelmetAudioStatus());
}

// Method to signal that the Outside task is done and start checking audio status.
public void OutsideTaskDone() {
    // Sets the flag to start checking audio.
    CheckingAudio = true;

    // Starts the coroutine to check the audio status for the Outside task.
    StartCoroutine(OutsideAudioStatus());
}

// Method to signal that the character has walked to the ship and start checking audio status.
public void WalkedToShip() {
    // Sets the flag to start checking audio.
    CheckingAudio = true;

    // Starts the coroutine to check the audio status for walking to the ship.
    StartCoroutine(WalkedToShipStatus());
}

// Method to signal that the Scanner task is done and start checking audio status.
public void ScannerTaskDone() {
    // Sets the flag to start checking audio.
    CheckingAudio = true;

    // Starts the coroutine to check the audio status for the Scanner task.
    StartCoroutine(ScannerAudioStatus());
}

// Method to signal that all tasks are done and start checking audio status.
public void AllTasksDone() {
    // Sets the flag to start checking audio.
    CheckingAudio = true;

    // Starts the coroutine to check the audio status for all tasks done.
    StartCoroutine(AllDoneAudioStatus());
}

// Private method to stop checking the audio status.
private void StopAudioCheck() {
    // Resets the flag to stop checking audio.
    CheckingAudio = false;
}

// Coroutine to check the audio status for the Fuze task.
private IEnumerator FuzeAudioStatus() {
    // Continues looping while CheckingAudio is true.
    while (CheckingAudio) {
        // Checks if audio is currently playing.
        if (audioSource.isPlaying) {
            // Logs that the audio is playing.
            Debug.Log("Audio playing");
        } else {
            // Ends the current conversation.
            ConversationManager.Instance.EndConversation();

            // Waits for 1 second before starting a new conversation.
            yield return new WaitForSeconds(1f);

            // Logs that audio is not playing.
            Debug.Log("Audio not playing");

            // Starts a new conversation related to the Fuze task.
            ConversationManager.Instance.StartConversation(FuzeConvo);

            // Stops the audio check loop.
            StopAudioCheck();
        }

        // Waits for 1 second before checking the audio status again.
        yield return new WaitForSeconds(1f);
    }
}

// Coroutine to check the audio status for the Package task.
private IEnumerator PackageAudioStatus() {
    // Continues looping while CheckingAudio is true.
    while (CheckingAudio) {
        // Checks if audio is currently playing.
        if (audioSource.isPlaying) {
            // Logs that the audio is playing.
            Debug.Log("Audio playing");
        } else {
            // Ends the current conversation.
            ConversationManager.Instance.EndConversation();

            // Waits for 1 second before starting a new conversation.
            yield return new WaitForSeconds(1f);

            // Logs that audio is not playing.
            Debug.Log("Audio not playing");

            // Starts a new conversation related to the Package task.
            ConversationManager.Instance.StartConversation(PackageConvo);

            // Stops the audio check loop.
            StopAudioCheck();
        }

        // Waits for 1 second before checking the audio status again.
        yield return new WaitForSeconds(1f);
    }
}

// Coroutine to check the audio status for the Helmet task.
private IEnumerator HelmetAudioStatus() {
    // Continues looping while CheckingAudio is true.
    while (CheckingAudio) {
        // Checks if audio is currently playing.
        if (audioSource.isPlaying) {
            // Logs that the audio is playing.
            Debug.Log("Audio playing");
        } else {
            // Ends the current conversation.
            ConversationManager.Instance.EndConversation();

            // Waits for 1 second before starting a new conversation.
            yield return new WaitForSeconds(1f);

            // Logs that audio is not playing.
            Debug.Log("Audio not playing");

            // Starts a new conversation related to the Helmet task.
            ConversationManager.Instance.StartConversation(HelmetConvo);

            // Stops the audio check loop.
            StopAudioCheck();
        }

        // Waits for 1 second before checking the audio status again.
        yield return new WaitForSeconds(1f);
    }
}

// Coroutine to check the audio status for the Outside task.
private IEnumerator OutsideAudioStatus() {
    // Continues looping while CheckingAudio is true.
    while (CheckingAudio) {
        // Checks if audio is currently playing.
        if (audioSource.isPlaying) {
            // Logs that the audio is playing.
            Debug.Log("Audio playing");
        } else {
            // Ends the current conversation.
            ConversationManager.Instance.EndConversation();

            // Waits for 1 second before starting a new conversation.
            yield return new WaitForSeconds(1f);

            // Logs that audio is not playing.
            Debug.Log("Audio not playing");

            // Starts a new conversation related to the Outside task.
            ConversationManager.Instance.StartConversation(OutsideConvo);

            // Stops the audio check loop.
            StopAudioCheck();
        }

        // Waits for 1 second before checking the audio status again.
        yield return new WaitForSeconds(1f);
    }
}

// Coroutine to check the audio status for walking to the ship.
private IEnumerator WalkedToShipStatus() {
    // Continues looping while CheckingAudio is true.
    while (CheckingAudio) {
        // Checks if audio is currently playing.
        if (audioSource.isPlaying) {
            // Logs that the audio is playing.
            Debug.Log("Audio playing");
        } else {
            // Ends the current conversation.
            ConversationManager.Instance.EndConversation();

            // Waits for 1 second before starting a new conversation.
            yield return new WaitForSeconds(1f);

            // Logs that audio is not playing.
            Debug.Log("Audio not playing");

            // Starts a new conversation related to walking to the ship.
            ConversationManager.Instance.StartConversation(WalkedToShipConvo);

            // Stops the audio check loop.
            StopAudioCheck();
        }

        // Waits for 1 second before checking the audio status again.
        yield return new WaitForSeconds(1f);
    }
}

// Coroutine to check the audio status for the Scanner task.
private IEnumerator ScannerAudioStatus() {
    // Continues looping while CheckingAudio is true.
    while (CheckingAudio) {
        // Checks if audio is currently playing.
        if (audioSource.isPlaying) {
            // Logs that the audio is playing.
            Debug.Log("Audio playing");
        } else {
            // Ends the current conversation.
            ConversationManager.Instance.EndConversation();

            // Waits for 1 second before starting a new conversation.
            yield return new WaitForSeconds(1f);

            // Logs that audio is not playing.
            Debug.Log("Audio not playing");

            // Starts a new conversation related to the Scanner task.
            ConversationManager.Instance.StartConversation(ScannerConvo);

            // Stops the audio check loop.
            StopAudioCheck();
        }

        // Waits for 1 second before checking the audio status again.
        yield return new WaitForSeconds(1f);
    }
}

// Coroutine to check the audio status for when all tasks are done.
private IEnumerator AllDoneAudioStatus() {
    // Continues looping while CheckingAudio is true.
    while (CheckingAudio) {
        // Checks if audio is currently playing.
        if (audioSource.isPlaying) {
            // Logs that the audio is playing.
            Debug.Log("Audio playing");
        } else {
            // Ends the current conversation.
            ConversationManager.Instance.EndConversation();

            // Waits for 1 second before starting a new conversation.
            yield return new WaitForSeconds(1f);

            // Logs that audio is not playing.
            Debug.Log("Audio not playing");

            // Starts a new conversation related to the completion of all tasks.
            ConversationManager.Instance.StartConversation(AllDoneConvo);

            // Stops the audio check loop.
            StopAudioCheck();
        }

        // Waits for 1 second before checking the audio status again.
        yield return new WaitForSeconds(1f);
    }
}
}
