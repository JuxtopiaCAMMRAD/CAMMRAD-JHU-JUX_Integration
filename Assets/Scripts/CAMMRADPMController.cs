using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




/*
 * Create by Saboor Salaam 
 * Last edited 9/19/2017
 * 
 * 
 * CAMMRADPMController
 * 
 * Purpose: To serve as controller for the Pictoral Mnemonic 
 * 
 * 
 * METHODS: 
 * __________________________________________________________
 * 
 * Start() 
 * Inherited from MonoBehaviour
 * Called once in the GameObjects lifetime when it is initialized 
 * 
 * Update()
 * Inherited from MonoBehaviour
 * Called once every frame
 * 
 * 
 * next()
 * Used to increment the currently highlighted step
 * 
 * 
 * back()
 * Used to decrement the currently highlighted step
 * 
 * 
 * changeCurrentItem ( int index)
 * Updates currentStep, changes centerText, updates the indexes of each shown item, updates  
 * 
 * parameters: 
 * 
 * int index 
 * the current step we are changing to 

 * 
 * 
 * 
 * 
 */


public class CAMMRADPMController : MonoBehaviour {


	bool isRotating = false;
	bool isRotatingForward = false;
	float degreesRotatedAlready = 0;
	float distanceBetweenPoints = 30f;

	public float startTime, totalStepTime;

	public CAMMRADMainController mainController;

	public Text centerText;

	public int index1, index2, index3, index4, index5;

	public List<GameObject> stepObjs, iconObjs;

	public GameObject iconsCircle, stepsCircle, readWorkFlow;


	public int currentStep = 0;

	public float rotationSpeed = 5f;


	void OnEnable () {

		//If workflow data is present they PM should deactivate it self
		if (!mainController.isWorkflowInit) {
			gameObject.SetActive(false);
			return;
		}
			
		index1 = 1;
		index2 = 2; 
		index3 = 3;
		index4 = 4;
		index5 = 5;

		changeCurrentItem(0);

		readWorkFlow.GetComponent<ReadWorkFlow> ().DisplayStep (mainController.currentTask.steps[0]);



		startTime = Time.time;

	
		stepObjs[index3].GetComponent<Image>().color = new Color32(255,205,11,255); //yellow
		stepObjs[index3].GetComponent<Image>().rectTransform.sizeDelta = new Vector2(80f, 80f);
		stepObjs[index3].GetComponent<Animator>().enabled = true;

		//stepObjs [index3].GetComponentInChildren<Text> ().fontSize = 40;
		//stepObjs [index3].GetComponentInChildren<Text> ().rectTransform.sizeDelta = new Vector2(50f, 50f);

	}

	void Update ()
	{

		if(Input.GetKeyDown(KeyCode.F) && !isRotating){
			mainController.finishTask ();
		} 

		if(Input.GetKeyDown(KeyCode.LeftArrow) && !isRotating){
			back();
		}

		if(Input.GetKeyDown(KeyCode.RightArrow) && !isRotating){
			next();
		}

		if(Input.GetKeyDown(KeyCode.F) && !isRotating){
			mainController.finishTask ();
		}


		if (isRotating && !isRotatingForward) {

			stepsCircle.transform.Rotate(Vector3.forward, 1 * rotationSpeed );
			iconsCircle.transform.Rotate(Vector3.forward, 1 * rotationSpeed );

			degreesRotatedAlready += 1 * rotationSpeed;  

			if (degreesRotatedAlready >= distanceBetweenPoints) { //Stop rotating

				isRotating = false;
				isRotatingForward = false;
				degreesRotatedAlready = 0;

				for (int i = 0; i < stepObjs.Count; i++) {

					stepObjs [i].GetComponentInChildren<Text> ().enabled = true;

				}


				for (int i = 0; i < stepObjs.Count; i++) {

					if (i != index3) {
						stepObjs [i].GetComponent<Image> ().color = new Color32(80,145,242,252); //blue;
						stepObjs[i].GetComponent<Image>().rectTransform.sizeDelta = new Vector2(50f, 50f);
						stepObjs[i].GetComponent<Animator>().enabled = false;
						//stepObjs [i].GetComponentInChildren<Text> ().fontSize = 25;
						//stepObjs [i].GetComponentInChildren<Text> ().rectTransform.sizeDelta = new Vector2(30f, 30f);
					} else {

						stepObjs[index3].GetComponent<Image>().color = new Color32(255,205,11,255); //yellow;
						stepObjs[index3].GetComponent<Image>().rectTransform.sizeDelta = new Vector2(80f, 80f);
						stepObjs[index3].GetComponent<Animator>().enabled = true;


						//stepObjs [index3].GetComponentInChildren<Text> ().fontSize = 40;
						//stepObjs [index3].GetComponentInChildren<Text> ().rectTransform.sizeDelta = new Vector2(50f, 50f);

					}

				}

			}

		} else if (isRotating && isRotatingForward) {

			stepsCircle.transform.Rotate(Vector3.back, 1 * rotationSpeed );
			iconsCircle.transform.Rotate(Vector3.back, 1 * rotationSpeed );


				degreesRotatedAlready += 1 * rotationSpeed;  


				if (degreesRotatedAlready >= distanceBetweenPoints) { //Stop rotating

					isRotating = false;
					isRotatingForward = false;
					degreesRotatedAlready = 0;

					for (int i = 0; i < stepObjs.Count; i++) {

						stepObjs [i].GetComponentInChildren<Text> ().enabled = true;

					}


				for (int i = 0; i < stepObjs.Count; i++) {

					if (i != index3) {
						stepObjs [i].GetComponent<Image> ().color = new Color32(80,145,242,252); //blue;
						stepObjs[i].GetComponent<Image>().rectTransform.sizeDelta = new Vector2(50f, 50f);
						stepObjs[i].GetComponent<Animator>().enabled = false;
						//stepObjs [i].GetComponentInChildren<Text> ().fontSize = 25;
						//stepObjs [i].GetComponentInChildren<Text> ().rectTransform.sizeDelta = new Vector2(30f, 30f);

					} else {

						stepObjs[index3].GetComponent<Image>().color = new Color32(255,205,11,255); //yellow;
						stepObjs[index3].GetComponent<Image>().rectTransform.sizeDelta = new Vector2(80f, 80f);
						stepObjs[index3].GetComponent<Animator>().enabled = true;
						//stepObjs [index3].GetComponentInChildren<Text> ().fontSize = 40;
						//stepObjs [index3].GetComponentInChildren<Text> ().rectTransform.sizeDelta = new Vector2(50f, 50f);


					}

				}

				}
		}



		for (int i = 0; i < stepObjs.Count; i++) {

			stepObjs[i].GetComponentInChildren<Text>().transform.rotation = Quaternion.identity;
			iconObjs[i].transform.rotation = Quaternion.identity;

		}


	}

	public void next(){

		float t = Time.time - startTime;
		string minutes = ((int)t / 60).ToString ();
		string seconds = (t % 60).ToString ("f2");
		string time = minutes + ":" + seconds;

		Debug.Log (time);



		//AndroidJavaClass unityPlayer = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		//AndroidJavaObject mainActivity = unityPlayer.GetStatic<AndroidJavaObject> ("currentActivity");
		//mainActivity.Call ("OnFinishedStep", time + "");

		if (currentStep < mainController.currentTask.steps.Count - 1 ) {
			
			isRotating = true;
			isRotatingForward = true;
			degreesRotatedAlready = 0;		

			resetShownItemIndexes (true);

			for (int i = 0; i < stepObjs.Count; i++) {

				stepObjs [i].GetComponentInChildren<Text> ().enabled = false;

			}


			changeCurrentItem(currentStep+1);

			mainController.GetComponent<LogFileCreator> ().writeToFile ("STEP " +  currentStep + " TIME : " + time);
			startTime = Time.time;

			readWorkFlow.GetComponent<ReadWorkFlow> ().DisplayStep (mainController.currentTask.steps[currentStep]);



		}

	}


	public void back(){

		if (currentStep > 0) {
			
			isRotating = true;
			isRotatingForward = false;


			degreesRotatedAlready = 0;
			//changeCurrentItem (currentItem - 1);


			resetShownItemIndexes (false);

			for (int i = 0; i < stepObjs.Count; i++) {

				stepObjs [i].GetComponentInChildren<Text> ().enabled = false;

			}
			changeCurrentItem(currentStep-1);
			readWorkFlow.GetComponent<ReadWorkFlow> ().DisplayStep (mainController.currentTask.steps[currentStep]);

		}

	}



	void changeCurrentItem(int index) {


		currentStep = index;

		if (centerText != null) {


			if (mainController.currentTask.steps [currentStep].Wheel.Text.Length > 150) {
				centerText.text = mainController.currentTask.steps [currentStep].Wheel.Text.Substring (0, 149) + "...";
			} else {
				centerText.text =   mainController.currentTask.steps[currentStep].Wheel.Text; 
			}
				
		}

		for (int i = 0; i < stepObjs.Count; i++) {

			stepObjs [i].GetComponent<Image> ().color = new Color32(80,145,242,252); //blue;
			stepObjs[i].GetComponent<Image>().rectTransform.sizeDelta = new Vector2(50f, 50f);
			//stepObjs [i].GetComponentInChildren<Text> ().fontSize = 25;
			//stepObjs [i].GetComponentInChildren<Text> ().rectTransform.sizeDelta = new Vector2(30f, 30f);

		}


		stepObjs[index3].GetComponentInChildren<Text> ().text = index + 1 + "";
		iconObjs[index3].GetComponent<RawImage>().texture = GetComponent<CAMMRADImageLoader> ().LoadImage (CAMMRADImageLoader.PM ,mainController.currentTask.dir_name,  mainController.currentTask.steps[index].Wheel.Icon);




		if (index - 1 < 0) { //If index before current is less than 0 show first to last step in place       2

			stepObjs[index2].GetComponentInChildren<Text> ().text = (mainController.currentTask.steps.Count - 1) + 1 + "";
			iconObjs[index2].GetComponent<RawImage>().texture = GetComponent<CAMMRADImageLoader> ().LoadImage (CAMMRADImageLoader.PM ,mainController.currentTask.dir_name,  mainController.currentTask.steps[mainController.currentTask.steps.Count - 1].Wheel.Icon);


			stepObjs[index1].GetComponentInChildren<Text> ().text = (mainController.currentTask.steps.Count - 2) + 1 + "";
			iconObjs[index1].GetComponent<RawImage>().texture = GetComponent<CAMMRADImageLoader> ().LoadImage (CAMMRADImageLoader.PM ,mainController.currentTask.dir_name,  mainController.currentTask.steps[mainController.currentTask.steps.Count - 2].Wheel.Icon);


		} else {

			stepObjs[index2].GetComponentInChildren<Text> ().text = (index - 1) + 1 + "";
			iconObjs[index2].GetComponent<RawImage>().texture = GetComponent<CAMMRADImageLoader> ().LoadImage (CAMMRADImageLoader.PM ,mainController.currentTask.dir_name,  mainController.currentTask.steps[index - 1].Wheel.Icon);


			if (index - 2 < 0) { //If index before current is less than 0 show second to last step in place   1
				 
				stepObjs[index1].GetComponentInChildren<Text> ().text = (mainController.currentTask.steps.Count - 1) + 1 + "";
				iconObjs[index1].GetComponent<RawImage>().texture = GetComponent<CAMMRADImageLoader> ().LoadImage (CAMMRADImageLoader.PM ,mainController.currentTask.dir_name,  mainController.currentTask.steps[mainController.currentTask.steps.Count - 1].Wheel.Icon);


			} else {

				stepObjs[index1].GetComponentInChildren<Text> ().text = (index - 2) + 1 + "";
				iconObjs[index1].GetComponent<RawImage>().texture = GetComponent<CAMMRADImageLoader> ().LoadImage (CAMMRADImageLoader.PM ,mainController.currentTask.dir_name,  mainController.currentTask.steps[index - 2].Wheel.Icon);


			}


		}




		//*********************************************




		if (index + 1 > mainController.currentTask.steps.Count - 1) { //If index before current is less than 0 show first to last step in place

			stepObjs[index4].GetComponentInChildren<Text> ().text = (0) + 1+  "";
			iconObjs[index4].GetComponent<RawImage>().texture = GetComponent<CAMMRADImageLoader> ().LoadImage (CAMMRADImageLoader.PM ,mainController.currentTask.dir_name,  mainController.currentTask.steps[0].Wheel.Icon);


		} else {

			stepObjs[index4].GetComponentInChildren<Text> ().text = (index + 1) + 1 + "";
			iconObjs[index4].GetComponent<RawImage>().texture = GetComponent<CAMMRADImageLoader> ().LoadImage (CAMMRADImageLoader.PM ,mainController.currentTask.dir_name,  mainController.currentTask.steps[index + 1].Wheel.Icon);

		}



		if (index + 2 > mainController.currentTask.steps.Count - 1) { //If index before current is less than 0 show second to last step in place

			stepObjs[index5].GetComponentInChildren<Text> ().text = (0 + 1) + + 1 +"";
			iconObjs[index5].GetComponent<RawImage>().texture = GetComponent<CAMMRADImageLoader> ().LoadImage (CAMMRADImageLoader.PM ,mainController.currentTask.dir_name,  mainController.currentTask.steps[0+ 1].Wheel.Icon);




		} else {

			stepObjs[index5].GetComponentInChildren<Text> ().text = (index + 2) + 1 + "";
			iconObjs[index5].GetComponent<RawImage>().texture = GetComponent<CAMMRADImageLoader> ().LoadImage (CAMMRADImageLoader.PM ,mainController.currentTask.dir_name,  mainController.currentTask.steps[index + 2].Wheel.Icon);


		}




	}


	void resetShownItemIndexes( bool IsNext){

		if (IsNext) {


			if (index1  < stepObjs.Count - 1){

				index1++;

			} else {

				index1 = 0;

			}



			if (index2  < stepObjs.Count - 1){

				index2++;

			} else {

				index2 = 0;

			}


			if (index3  < stepObjs.Count - 1){

				index3++;

			} else {

				index3 = 0;

			}


			if (index4  < stepObjs.Count - 1){

				index4++;

			} else {

				index4 = 0;

			}

			if (index5  < stepObjs.Count - 1){

				index5++;

			} else {

				index5 = 0;

			}





		} else {


			if (index1 > 0){

				index1--;

			} else {

				index1 = stepObjs.Count - 1;

			}


			if (index2 > 0){

				index2--;

			} else {

				index2 = stepObjs.Count - 1;

			}


			if (index3 > 0){

				index3--;

			} else {

				index3 = stepObjs.Count - 1;

			}


			if (index4 > 0){

				index4--;

			} else {

				index4 = stepObjs.Count - 1;

			}


			if (index5 > 0){

				index5--;

			} else {

				index5 = stepObjs.Count - 1;

			}


		}

	}


}
