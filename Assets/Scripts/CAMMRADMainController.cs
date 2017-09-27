using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using System.Text;

#if !UNITY_EDITOR && UNITY_WSA
using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Text;
#endif

public class CAMMRADMainController : MonoBehaviour {


	public GameObject CAMMRADPM, Timer, TaskList, cutOffMarker, centerText;
	public bool isWorkflowInit = false;


	public CAMMRADTask currentTask;
	public CAMMRADWorkflow workflow;

	public int testableTaskCount = 0;

	void Start(){

		Debug.Log ("Screen width: " + Screen.width);
		Debug.Log ("Cut off x position: " + cutOffMarker.transform.localPosition.x);
		Debug.Log ("Distance needed to move " + ((Screen.width / 2) - cutOffMarker.transform.localPosition.x));

		//Calculate correct position for PM (half of circle hidden)
		CAMMRADPM.GetComponent<RectTransform> ().localPosition = new Vector3 (((Screen.width / 2) - cutOffMarker.transform.localPosition.x), CAMMRADPM.GetComponent<RectTransform> ().localPosition.y, CAMMRADPM.GetComponent<RectTransform> ().localPosition.z);

		Debug.Log ("Data path:" + Application.persistentDataPath);

		CAMMRADPM.SetActive (false);
		Timer.SetActive (false);
		centerText.SetActive (false);

		initWorkflow ();

		showTaskList ();

	}
	void Update(){

		/*
		if ((Input.deviceOrientation != deviceOrientation))
		{
			Debug.Log ("Device Orientation: " + Input.deviceOrientation);
			CAMMRADPM.GetComponent<RectTransform>().localPosition = new Vector3(((Screen.width / 2) - cutOffMarker.transform.localPosition.x), CAMMRADPM.GetComponent<RectTransform>().localPosition.y, CAMMRADPM.GetComponent<RectTransform>().localPosition.z) ;
		}
		*/


		if (Input.GetKeyDown (KeyCode.J)) {
			setCurrentTask ("0");
		}

		if (Input.GetKeyDown (KeyCode.K)) {
			setCurrentTask ("1");
		}


		if (Input.GetKeyDown (KeyCode.L)) {
			setCurrentTask ("2");
		}

		if (Input.GetKeyDown (KeyCode.P)) {
			setCurrentTask ("3");
		}



	}

	//Called by android activity to store Workflow data at beginning of application
	public void initWorkflow() {

		String json_string = "";

		
#if !UNITY_EDITOR && UNITY_WSA

		try {
			using (Stream stream = OpenFileForRead(ApplicationData.Current.RoamingFolder.Path, "all_workflow.json")) {
				byte[] data = new byte[stream.Length];
				stream.Read(data, 0, data.Length);
				json_string = Encoding.ASCII.GetString(data);
			}
		}
		catch (Exception e) {
			Debug.Log(e);
		}
#else

		string JsonPath = Application.streamingAssetsPath + "/all_workflow.json";

		json_string = File.ReadAllText( JsonPath );

		//Debug.Log( "You entered the non-android section: " + JsonPath );


#endif

        Debug.Log(json_string);


        workflow = CAMMRADWorkflowParserFactory.getParser(CAMMRADWorkflowParserFactory.JSON).parse(json_string);

		testableTaskCount = workflow.tasks.Count;

		isWorkflowInit = true;

		Debug.Log ("Tasks parsed:" + workflow.tasks.Count);
	}


	private static Stream OpenFileForRead( string folderName, string fileName ) {
		Stream stream = null;
		bool taskFinish = false;
		#if !UNITY_EDITOR && UNITY_METRO
		Task task = new Task(
		async () => {
		try {
		StorageFolder folder = await StorageFolder.GetFolderFromPathAsync( folderName );
		var item = await folder.TryGetItemAsync( fileName );
		if( item != null ) {
		StorageFile file = await folder.GetFileAsync( fileName );
		if( file != null ) {
		stream = await file.OpenStreamForReadAsync();
		}
		}
		}
		catch( Exception ) { }
		finally { taskFinish = true; }

		} );
		task.Start();
		while( !taskFinish ) {
		task.Wait();
		}
		#endif
		return stream;
	}


	public void showTaskList(){
		TaskList.SetActive(true);
	}

	public void hideTaskList(){
		TaskList.SetActive(false);
	}

	//Called by android activity to restart PM showing a new task
	public void setCurrentTask(string index){

		int i = int.Parse (index);

		if(i < workflow.tasks.Count && i >= 0) {
			currentTask = workflow.tasks[i];
		}

		GetComponent<LogFileCreator> ().openNewFile (Application.persistentDataPath + "/" + currentTask.Name + (System.DateTime.Now.Hour + "." + System.DateTime.Now.Minute) + ".txt");

		GetComponent<LogFileCreator> ().writeToFile ("TASK: " + currentTask.Name);
		GetComponent<LogFileCreator> ().writeToFile ("DATE: " + (System.DateTime.Now.ToString()));


		//Restart PM
		CAMMRADPM.SetActive(false);
		CAMMRADPM.SetActive(true);
		centerText.SetActive (true);


		//Restart Timer
		Timer.SetActive (true);
		Timer.GetComponent<timer_controller>().startTask();

		//Hide task list
		hideTaskList();
	}


	public void finishTask(){

		//Check whether or not we are on the last step
		if (CAMMRADPM.GetComponent<CAMMRADPMController> ().currentStep < currentTask.steps.Count - 2) {
			//return;
		}

		CAMMRADPM.SetActive(false);
		centerText.SetActive (true);


		float total_time = Timer.GetComponent<timer_controller> ().endTask();

		GetComponent<LogFileCreator> ().writeToFile ("TOTAL TASK TIME: " + total_time);
		GetComponent<LogFileCreator> ().closeFile ();


		Timer.SetActive (false);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

//		AndroidJavaClass unityPlayer = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		//AndroidJavaObject mainActivity = unityPlayer.GetStatic<AndroidJavaObject> ("currentActivity");
		//mainActivity.Call ("OnFinishedTask", total_time + "");


	}



	//Factory
	public class CAMMRADWorkflowParserFactory {
		
		public const int JSON =  0, XML = 1;

		public static CAMMRADWorkflowParser getParser(int type){

			switch (type) {

			case JSON:

				return new CAMMRADWorkflowJSONParser();

				break;

			case XML:

				return new CAMMRADWorkflowXMLParser();

				break;

			default:

				return new CAMMRADWorkflowJSONParser();

			}

		}

	}


	public class CAMMRADWorkflowJSONParser : CAMMRADWorkflowParser {

		public CAMMRADWorkflow parse(string workflow_string){

			return JsonUtility.FromJson<CAMMRADWorkflow>(workflow_string); 

		}

	}


	public class CAMMRADWorkflowXMLParser : CAMMRADWorkflowParser {

		public CAMMRADWorkflow parse(string workflow_string){

			return new CAMMRADWorkflow ();

		}

	}


	public interface CAMMRADWorkflowParser{

		CAMMRADWorkflow parse(string workflow_string);

	}


	/*
	[System.Serializable]
	public class CAMMRADWorkflow {
		 public string name, type;
		 public CAMMRADWorkflowTask[] tasks;
		 public CAMMRADWorkflow() {}
	}

	[System.Serializable]
	public class CAMMRADWorkflowTask {
		  public string name;
		  public string dir_name;
		  public CAMMRADWorkflowStep[] steps;
		  public CAMMRADWorkflowTask() {
			  }
	}
	[System.Serializable]
	public class CAMMRADWorkflowStep {
		 public string StepDescription, StepImageAid, StepVideoAid;
		 public CAMMRADWorkflowStep() {
			 }
}

*/





	[System.Serializable]
	public class CAMMRADWorkflow
	{
		public List<CAMMRADTask> tasks;
	}

	[System.Serializable]
	public class CAMMRADTask
	{
		public double Version;
		public string Name;
		public string dir_name;
		public string ResourcesPath;
		public List<CAMMRADStep> steps;
	}

	[System.Serializable]
	public class CAMMRADStep
	{
		public string Name;
		public double Version;
		public int Duration;
		public List<FeatureAnchoredObject> FeatureAnchoredObjects;
		public List<DisplayAnchoredImage> DisplayAnchoredImage;
		public List<DisplayAnchoredText> DisplayAnchoredText;
		public Wheel Wheel;
	}

	[System.Serializable]
	public class FeatureAnchoredObject
	{
		public string Image_Path;
		public List<double> Position;
		public string MarkerName;
		public List<AnimationPath> AnimationPath;
	}

	[System.Serializable]
	public class DisplayAnchoredImage
	{
		public string Image_Path;
		public List<double> Position;
		public float Scale;
		public List<AnimationPath> AnimationPath;

	}

	[System.Serializable]
	public class AnimationPath
	{
		public List<double> Position;
		public double Time;
	}


	[System.Serializable]
	public class DisplayAnchoredText
	{
		public string Text;
		public List<double> Position;
		public float Scale;
	}

	[System.Serializable]
	public class Wheel
	{
		public string Text;
		public string Icon;
	}









}
