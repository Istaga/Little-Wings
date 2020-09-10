using System;

using UnityEngine;

using UnityEngine.UI;



namespace FrostweepGames.Plugins.GoogleCloud.SpeechRecognition.Examples

{

	public class GCSR_DoCommandsExample : MonoBehaviour

	{

		public GameObject piwi;



		private GCSpeechRecognition _speechRecognition;



		private Button _startRecordButton,

					   _stopRecordButton;



		private Dropdown _languageDropdown;



		private void Start()

		{

			piwi = GameObject.Find("Kiwi");

			_speechRecognition = GCSpeechRecognition.Instance;

			_speechRecognition.RecognizeSuccessEvent += RecognizeSuccessEventHandler;



			_speechRecognition.FinishedRecordEvent += FinishedRecordEventHandler;

			_speechRecognition.RecordFailedEvent += RecordFailedEventHandler;



			_speechRecognition.EndTalkigEvent += EndTalkigEventHandler;



			_startRecordButton = transform.Find("Canvas/Button_StartRecord").GetComponent<Button>();

			_stopRecordButton = transform.Find("Canvas/Button_StopRecord").GetComponent<Button>();



			_languageDropdown = transform.Find("Canvas/Dropdown_Language").GetComponent<Dropdown>();



			_startRecordButton.onClick.AddListener(StartRecordButtonOnClickHandler);

			_stopRecordButton.onClick.AddListener(StopRecordButtonOnClickHandler);



			_startRecordButton.interactable = true;

			_stopRecordButton.interactable = false;



			_languageDropdown.ClearOptions();



			_speechRecognition.RequestMicrophonePermission(null);



			for (int i = 0; i < Enum.GetNames(typeof(Enumerators.LanguageCode)).Length; i++)

			{

				_languageDropdown.options.Add(new Dropdown.OptionData(((Enumerators.LanguageCode)i).Parse()));

			}



			_languageDropdown.value = _languageDropdown.options.IndexOf(_languageDropdown.options.Find(x => x.text == Enumerators.LanguageCode.en_GB.Parse()));





			// select first microphone device

			if (_speechRecognition.HasConnectedMicrophoneDevices())

			{

				_speechRecognition.SetMicrophoneDevice(_speechRecognition.GetMicrophoneDevices()[0]);

			}

		}



		private void OnDestroy()

		{

			_speechRecognition.RecognizeSuccessEvent -= RecognizeSuccessEventHandler;



			_speechRecognition.FinishedRecordEvent -= FinishedRecordEventHandler;

			_speechRecognition.RecordFailedEvent -= RecordFailedEventHandler;



			_speechRecognition.EndTalkigEvent -= EndTalkigEventHandler;

		}



		private void StartRecordButtonOnClickHandler()

		{

			_startRecordButton.interactable = false;

			_stopRecordButton.interactable = true;



			_speechRecognition.StartRecord(false);

		}



		private void StopRecordButtonOnClickHandler()

		{

			_stopRecordButton.interactable = false;

			_startRecordButton.interactable = true;



			_speechRecognition.StopRecord();

		}





		private void RecordFailedEventHandler()

		{

			_stopRecordButton.interactable = false;

			_startRecordButton.interactable = true;

		}



		private void EndTalkigEventHandler(AudioClip clip, float[] raw)

		{

			FinishedRecordEventHandler(clip, raw);

		}



		private void FinishedRecordEventHandler(AudioClip clip, float[] raw)

		{



			if (clip == null)

				return;



			RecognitionConfig config = RecognitionConfig.GetDefault();

			config.languageCode = ((Enumerators.LanguageCode)_languageDropdown.value).Parse();

			config.audioChannelCount = clip.channels;

			// configure other parameters of the config if need



			GeneralRecognitionRequest recognitionRequest = new GeneralRecognitionRequest()

			{

				audio = new RecognitionAudioContent()

				{

					content = raw.ToBase64()

				},

				//audio = new RecognitionAudioUri() // for Google Cloud Storage object

				//{

				//	uri = "gs://bucketName/object_name"

				//},

				config = config

			};



			_speechRecognition.Recognize(recognitionRequest);

		}



		private void RecognizeSuccessEventHandler(RecognitionResponse res)

		{

			string w = res.results[0].alternatives[0].words[0].word;

			DoCommand(w);



			// foreach (var result in res.results)

			// {

			// 	foreach (var alternative in result.alternatives)

			// 	{

			// 		foreach (var command in commands)

			// 		{

			// 			if (command.Trim(' ').ToLowerInvariant() == alternative.transcript.Trim(' ').ToLowerInvariant())

			// 			{

			// 				DoCommand(command.ToLowerInvariant().TrimEnd(' ').TrimStart(' '));

			// 			}

			// 		}

			// 	}

			// }

		}



		private void DoCommand(string req)

		{

			Debug.Log(req);

			Debug.Log(piwi.transform);

		}

	}

}