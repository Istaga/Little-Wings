using System.Collections;
using System.Collections.Generic;
using TextSpeech;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class VoiceController : MonoBehaviour
{
    const string LANG_CODE = "en-US";

    [SerializeField]
    Text uiText;


    private void Start(){
        Setup(LANG_CODE);

         

        SpeechToText.instance.onResultCallback = OnFinalSpeechResult;
        CheckPermission();
    }


    void CheckPermission(){
#if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone)){
            Permission.RequestUserPermission(Permission.Microphone);
        }
#endif
    }

    #region Speech to Text

    public void StartListening(){
        SpeechToText.instance.StartRecording();
    }

    public void StopListening(){
        SpeechToText.instance.StopRecording();
    }

    void OnFinalSpeechResult(string res){
        uiText.text = res;
    }

    #endregion

    void Setup(string code){
        SpeechToText.instance.Setting(code);
    }
}
