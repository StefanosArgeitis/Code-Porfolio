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


    void Start()
    {
        StartCoroutine(NaratorStartDelay());
    }

    public IEnumerator NaratorStartDelay(){
        yield return new WaitForSeconds(4);
        ConversationManager.Instance.StartConversation(startConvo);
        Debug.Log("Working");
    }

    public void FuzeTaskDone(){
        CheckingAudio = true;
        StartCoroutine(FuzeAudioStatus());
    }

     public void PackageTaskDone(){

        if (HasNotDonePackageTask){
            //Debug.Log("no narator");
            return;
        } else {
            CheckingAudio = true;
            StartCoroutine(PackageAudioStatus());
            HasNotDonePackageTask = true;
        }
        
    }

    public void HelmetTaskDone(){
        CheckingAudio = true;
        StartCoroutine(HelmetAudioStatus());
    }

    public void OutsideTaskDone(){
        CheckingAudio = true;
        StartCoroutine(OutsideAudioStatus());
    }

    public void WalkedToShip(){
        CheckingAudio = true;
        StartCoroutine(WalkedToShipStatus());
    }

    public void ScannerTaskDone(){
        CheckingAudio = true;
        StartCoroutine(ScannerAudioStatus());
    }

    public void AllTasksDone(){
        CheckingAudio = true;
        StartCoroutine(AllDoneAudioStatus());
    }

    private void StopAudioCheck(){
        CheckingAudio = false;
    }

    private IEnumerator FuzeAudioStatus(){
        while (CheckingAudio) {

            if (audioSource.isPlaying){
                Debug.Log("Audio playing");

            }else{

                ConversationManager.Instance.EndConversation();
                yield return new WaitForSeconds(1f);
                Debug.Log("Audio not playing");
                ConversationManager.Instance.StartConversation(FuzeConvo);
                StopAudioCheck();
            }

            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator PackageAudioStatus(){
        while (CheckingAudio) {

            if (audioSource.isPlaying){
                Debug.Log("Audio playing");

            }else{
                
                ConversationManager.Instance.EndConversation();
                yield return new WaitForSeconds(1f);
                Debug.Log("Audio not plg");
                ConversationManager.Instance.StartConversation(PackageConvo);
                StopAudioCheck();
            }

            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator HelmetAudioStatus(){
        while (CheckingAudio) {

            if (audioSource.isPlaying){
                Debug.Log("Audio playing");

            }else{
                
                ConversationManager.Instance.EndConversation();
                yield return new WaitForSeconds(1f);
                Debug.Log("Audio not g");
                ConversationManager.Instance.StartConversation(HelmetConvo);
                StopAudioCheck();
            }

            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator OutsideAudioStatus(){
        while (CheckingAudio) {

            if (audioSource.isPlaying){
                Debug.Log("Audio playing");

            }else{
                
                ConversationManager.Instance.EndConversation();
                yield return new WaitForSeconds(1f);
                Debug.Log("Audio not g");
                ConversationManager.Instance.StartConversation(OutsideConvo);
                StopAudioCheck();
            }

            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator WalkedToShipStatus(){
        while (CheckingAudio) {

            if (audioSource.isPlaying){
                Debug.Log("Audio playing");

            }else{
                
                ConversationManager.Instance.EndConversation();
                yield return new WaitForSeconds(1f);
                Debug.Log("Audio not g");
                ConversationManager.Instance.StartConversation(WalkedToShipConvo);
                StopAudioCheck();
            }

            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator ScannerAudioStatus(){
        while (CheckingAudio) {

            if (audioSource.isPlaying){
                Debug.Log("Audio playing");

            }else{
                
                ConversationManager.Instance.EndConversation();
                yield return new WaitForSeconds(1f);
                Debug.Log("Audio not g");
                ConversationManager.Instance.StartConversation(ScannerConvo);
                StopAudioCheck();
            }

            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator AllDoneAudioStatus(){
        while (CheckingAudio) {

            if (audioSource.isPlaying){
                Debug.Log("Audio playing");

            }else{
                
                ConversationManager.Instance.EndConversation();
                yield return new WaitForSeconds(1f);
                Debug.Log("Audio not g");
                ConversationManager.Instance.StartConversation(AllDoneConvo);
                StopAudioCheck();
            }

            yield return new WaitForSeconds(1f);
        }
    }

}
