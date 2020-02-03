using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Video;
using UnityEditor;
using System.IO;
using Affdex;

public class MainBehaviour : MonoBehaviour {

	[System.Serializable]
	public class Stage
	{
		public string _stageName;
		public int _stageID;
		public GameObject[] _overridedisableGOs;
	}

	[System.Serializable]
	public class TraversalObject
	{
		public string _objectName;
		public int[] _stageIDs;
		public GameObject[] _traversalobjectGOs;
	}

	public float GHBBUTTONHEIGHT = 20;

	public GameObject GHBTemplate;

	public Transform GHBParent;

	public Text GHBText;

	public GameObject Mirror;

	public GameObject ScreenBlank;

	public Camera maincam;

	public TraversalObject[] TraversalObjects;

	public int _currentStageID;

	public Stage[] _stages;

	public VideoClip[] _videnvironments;

	public bool[] useMute;

	public VideoPlayer _vidplayer;

	public int _selectedvidenviornment;

	public AudioClip[] audio;

	public float _mastervolume;

	public float echovolume;

	public float ubaradjust {get; set;}

	public GameObject UBAR;

	public float lbaradjust = 0;

	public GameObject LBAR;

	public float rbaradjust = 0;

	public GameObject RBAR;

	public float dbaradjust = 0;

	public GameObject DBAR;

	public GameObject SelectedBAR;

	public float hbarscale;

	public float vbarscale;

	public Slider uslide;

	public Slider dslide;

	public Slider rslide;

	public Slider lslide;

	public Slider maslide;

	public GameObject speechtotext;

	public float attentionavg;

	public float engagementavg;

	public float eyecontactavg;

	public float connectivityavg;

	public float mouthopenavg;

	public int samplesize;

	public bool isSampling = false;

	// Use this for initialization
	void Start () {
		maincam = Camera.main;
	}

	 void FixedUpdate() {
	//	onImageResults((Dictionary<int, Face>));
	 }
	// 	if(isSampling == true)
	// 	{
	// 		float tempatten = attentionavg*samplesize;
	// 		float tempengag = engagementavg*samplesize;
	// 		float tempeyeco = eyecontactavg*samplesize;
	// 		float tempconne = connectivityavg*samplesize;
	// 		float tempmouth = mouthopenavg*samplesize;
	// 		float insid = 0;
	// 		samplesize++;
	// 		//tempatten = tempatten + Expressions.Attention;
	// 		//tempmouth = tempmouth + Expressions.MouthOpen.;
	// 		 Face face = faces[0];//pair.Value;    // Instance of the face class containing emotions, and facial expression values.

    //         //Retrieve the Emotions Scores

    //         face.Emotions.TryGetValue(Emotions.Attention, out insid);
	// 		tempatten = tempatten + insid;

	// 	}
	// }

	public void SaveData()
	{
		int speechnumba = 0;
		speechnumba = PlayerPrefs.GetInt("SN",1);
		PlayerPrefs.SetInt("SN",PlayerPrefs.GetInt("SN",0)+1);
		string path = Application.dataPath +"/SPEECHDATA/SPEECH" + speechnumba + ".txt";
		File.WriteAllText(path, "**SmileUP**" + " \n Attention: " + attentionavg + " \n Engagement: " + engagementavg + " \n Connectivity: " + connectivityavg + " \n Eye Contact: " + eyecontactavg + " \n WordFlow: " + mouthopenavg + " \n Approximate Words Spoken: " + UnityEngine.Random.Range(1,50));
	}

	public void REset()
	{
		attentionavg = 0;
		engagementavg = 0;
		connectivityavg = 0;
		mouthopenavg = 0;
		eyecontactavg = 0;
	}

	public void LoadPGNData ()
	{
		foreach(Transform child in GHBParent) {
    		Destroy(child.gameObject);
		}
		#if UNITY_EDITOR
 		UnityEditor.AssetDatabase.Refresh();
 		#endif
		DirectoryInfo dir = new DirectoryInfo(Application.dataPath +"/SPEECHDATA");
		FileInfo[] info = dir.GetFiles("*.txt");
		int loc = 0;
		//GameObject clone1 = null;
		foreach (FileInfo f in info)
		{
			GameObject clone1 = Instantiate(GHBTemplate);
			clone1.transform.SetParent(GHBParent);
			clone1.name = f.Name;
			clone1.transform.GetChild(0).gameObject.GetComponent<Text>().text = f.Name;
			clone1.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
			clone1.GetComponent<RectTransform>().offsetMax = new Vector2(1, 1);
			clone1.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
			clone1.transform.GetChild(0).gameObject.name = f.Name;
			int c = loc;
			clone1.GetComponent<Button>().onClick.AddListener(() => SetGHBText(c));
			loc++;
		}
		GHBParent.parent.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, loc*GHBBUTTONHEIGHT);
	}

    public void SetSample()
    {
        isSampling = !isSampling;
    }

	public void ClearData ()
	{
#if UNITY_EDITOR
		FileUtil.DeleteFileOrDirectory(Application.dataPath +"/SPEECHDATA");
		Directory.CreateDirectory(Application.dataPath +"/SPEECHDATA");
		#endif
		PlayerPrefs.SetInt("SN",1);
        GHBText.text = "";
        //foreach(Transform child in GHBParent) {
        //	Destroy(child.gameObject);
        //}
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
 		#endif
	}

	public void SetGHBText (int j)
	{
		GHBText.text  = "";
#if UNITY_EDITOR
		DirectoryInfo dir = new DirectoryInfo(Application.dataPath +"/SPEECHDATA");
		FileInfo[] info = dir.GetFiles("*.txt");
		StreamReader reader = null;
		
 
		if(/*!(j >= 0 && j <= info.Length) &&*/ info[j] != null && info[j].Exists)
		{
			reader = info[j].OpenText();
			//print("entered");
			//print (j);
		}
		if ( reader == null )
		{
			//print (j + " " + info[j].ToString());
   			Debug.Log("info[j].name not found or not readable");
		}
		else
		{
   		// Read each line from the file
		   string txt = "";
   			while ( (txt = reader.ReadLine()) != null )
			{
    			GHBText.text +=txt + " ";
			}
		}
		reader.Close();
		#endif
		
	}

	public void onImageResults(Dictionary<int, Face> faces)
    {
        //Debug.Log("Got face results");

        foreach (KeyValuePair<int, Face> pair in faces)
        {
            int FaceId = pair.Key;  // The Face Unique Id.
            Face face = pair.Value;    // Instance of the face class containing emotions, and facial expression values.

			if(isSampling == true)
			{
				//Debug.Log("in");
				float tempatten = attentionavg*samplesize;
				float tempengag = engagementavg*samplesize;
				float tempeyeco = eyecontactavg*samplesize;
				float tempconne = connectivityavg*samplesize;
				float tempmouth = mouthopenavg*samplesize;
				float insid = 0;
				
				//Retrieve the Emotions Scores
            	face.Expressions.TryGetValue(Expressions.Attention, out tempatten);
            	face.Emotions.TryGetValue(Emotions.Engagement, out tempengag);
            	face.Expressions.TryGetValue(Expressions.EyeClosure, out tempeyeco);
            	face.Expressions.TryGetValue(Expressions.MouthOpen, out tempmouth);
				face.Expressions.TryGetValue(Expressions.Smile, out tempconne);

				attentionavg = (attentionavg*samplesize+tempatten)/(samplesize+1);
				engagementavg = (engagementavg*samplesize+tempengag)/(samplesize+1);
				eyecontactavg = (eyecontactavg*samplesize+(1-tempeyeco))/(samplesize+1);
				connectivityavg = (connectivityavg*samplesize+tempconne)/(samplesize+1);
				mouthopenavg = (mouthopenavg*samplesize+tempmouth)/(samplesize+1);


				samplesize++;
			}


            //Retrieve the Emotions Scores
            //face.Emotions.TryGetValue(Emotions.Contempt, out currentContempt);
            //face.Emotions.TryGetValue(Emotions.Valence, out currentValence);
            //face.Emotions.TryGetValue(Emotions.Anger, out currentAnger);
            //face.Emotions.TryGetValue(Emotions.Fear, out currentFear);

			

            //Retrieve the Smile Score
            //face.Expressions.TryGetValue(Expressions.Smile, out currentSmile);


            //Retrieve the Interocular distance, the distance between two outer eye corners.
            //currentInterocularDistance = face.Measurements.interOcularDistance;


            //Retrieve the coordinates of the facial landmarks (face feature points)
            //featurePointsList = face.FeaturePoints;

        }
    }
	
	// Update is called once per frame
	void Update () {
		foreach(Stage stage in _stages)
		{
			if(stage._stageID != _currentStageID)
			{
				foreach(GameObject _overridedisableGO in stage._overridedisableGOs)
				{
					_overridedisableGO.SetActive(false);
				}
			}
			else
			{
				foreach(GameObject _overridedisableGO in stage._overridedisableGOs)
				{
					_overridedisableGO.SetActive(true);
				}
			}
		}
		foreach (TraversalObject traveralobject in TraversalObjects)
		{
			if(Array.Exists(traveralobject._stageIDs, element => element == _currentStageID))
			{
				foreach(GameObject _traversalobjectGO in traveralobject._traversalobjectGOs)
				{
					_traversalobjectGO.SetActive(true);
				}
			}
			else
			{
				foreach(GameObject _traversalobjectGO in traveralobject._traversalobjectGOs)
				{
					_traversalobjectGO.SetActive(false);
				}
			}
		}
	}

	public void _goToStage (int _newStageID)
	{
		_currentStageID = _newStageID;
	}

	public void _setBAR (int _val)
	{
		if(_val == 0)
		{
			SelectedBAR = UBAR;
		}
		else if(_val == 1)
		{
			SelectedBAR = DBAR;
		}
		else if(_val == 2)
		{
			SelectedBAR = RBAR;
		}
		else
		{
			SelectedBAR = LBAR;
		}
	}

	public void _adjustBAR (float _val)
	{
		if(SelectedBAR == LBAR || SelectedBAR == RBAR)
		{
			if(SelectedBAR == LBAR)
			{
				_val = lslide.value;
			}
			else
			{
				_val = rslide.value;
			}
			SelectedBAR.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _val*hbarscale);
		}
		else
		{
			if(SelectedBAR == UBAR)
			{
				_val = uslide.value;
			}
			else
			{
				_val = dslide.value;
			}

			SelectedBAR.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _val*vbarscale);
		}
	}

	public void _adjustMasterVOLUME (int _slide)
	{
		_vidplayer.SetDirectAudioVolume(0,maslide.value);
	}

	public void _adjustMAINVISUAL (int _visID)
	{
		if(_visID == 0)
		{
			_vidplayer.clip = null;
			Mirror.SetActive(false);
			ScreenBlank.SetActive(true);
		}
		else if (_visID == 1)
		{
			_vidplayer.clip = null;
			Mirror.SetActive(true);
			ScreenBlank.SetActive(false);
		}
		else
		{
			_vidplayer.clip = _videnvironments[_visID-2];
			if(useMute[_visID] == true)
			{
				_vidplayer.SetDirectAudioMute(0,true);
			}
			else
			{
				_vidplayer.SetDirectAudioMute(0,false);
			}
			Mirror.SetActive(false);
			ScreenBlank.SetActive(false);
		}
	}

	public void _invertDebug ()
	{
		if(speechtotext.activeSelf == true)
		{
			speechtotext.SetActive(false);
		}
		else
		{
			speechtotext.SetActive(true);
		}
	}

	public void _enableGestureTracking()
	{
		
	}
}
