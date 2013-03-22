using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour
{
	Transform stick;
	Transform radarCam;
	
	enum State { LIFT,FLY,SAVE };
	
	State state;
	
	Vector3 rot;
	float flyHeight;
	
	public SkeletonWrapper skelWrap;
	public GUIStyle GUIstyle;
	
	float speed;
	
	float kispeed;
	float kirot;
	
	void Start ()
	{
		stick = transform.FindChild("ControllerStick");
		radarCam = transform.FindChild("RadarCam");
		
		state = State.FLY;
		
		rot = new Vector3(5,0,0);
		flyHeight = 75.0f;
		speed = 0.8f;
	}
	
	void Update ()
	{
		transform.position = new Vector3(transform.position.x, flyHeight, transform.position.z);
		transform.rotation = Quaternion.Euler(rot);
		stick.localRotation = Quaternion.Euler(new Vector3(rot.z,270,-rot.x));
		radarCam.localRotation = Quaternion.Euler(new Vector3(90,180,0));
		
		
		//Controls();
		switch(state)
		{
			case State.FLY :
			{
				//KinectControls();
				Controls();
				break;
			}
			case State.SAVE :
			{
				saveControls();
				break;
			}
		}
	}
	
	void setSaveMode(bool mode)
	{
		if(mode==true)
		{
			state=State.SAVE;
			//this.transform.localRotation = Quaternion.Euler(new Vector3(90,180,0));
			//rotate camera to the back guy with the IR
		}
		else
		{
			state=State.FLY;
			//rotate camera back to pilots view
		}
	}
	
	void saveControls()
	{
		leveling();
		if(Input.GetKey(KeyCode.Return))
		{
			setSaveMode(false);
			//GUI.Label(new Rect(Screen.width-150,Screen.height-150, 300, 300), "Saving in Progress .... Have a nice Day !");
		}
	}
	
	void OnGUI ()
	{
		GUI.Label(new Rect(130,50, 300, 300), "Speed: "+kispeed.ToString(), GUIstyle);	
		GUI.Label(new Rect(130,200, 300, 300), "Rotation: "+kirot.ToString(), GUIstyle);	
	}
	
	void KinectControls()
	{
		//kinect hands check
		float dist = (skelWrap.bonePos[0, 11]-skelWrap.bonePos[0, 7]).magnitude;
		if(dist < 0.3f)
		{
			kispeed = (skelWrap.bonePos[0, 11].y - skelWrap.bonePos[0,2].y) + 0.2f;
			kispeed *= -3.35f;
			
			kirot = (skelWrap.bonePos[0,11].x - skelWrap.bonePos[0,0].x);
			kirot *= 3.0f;
		}
		else
		{
			kispeed = 0;
			kirot = 0;
		}
		
		//moving with ki-vars
		if(rot.x < 25 && rot.x > -15)
			rot.x += kispeed*0.7f;
		
		this.transform.Translate(0,0,kispeed);
		//rotate
		rot.y += kirot;
			
		if(rot.z > -40 && rot.z < 40)
		{
			rot.z -= kirot;
		}
		
		//position heli back to normal again
		leveling();
	}
	
	void leveling()
	{
		if(rot.z > 0)
			rot.z -= 0.45f;
		if(rot.z < 0)
			rot.z += 0.45f;
		
		if(rot.x > 10)
			rot.x -= 0.6f;
		if(rot.x < 10)
			rot.x += 0.6f;
	}
	
	
	void Controls()
	{
		if(Input.GetKey("space") && state == State.FLY)
		{
			setSaveMode(true);
		}
		
		if(Input.GetKey("left") && state == State.FLY)
		{
			turnLeft();
		}
		
		if(Input.GetKey("right") && state == State.FLY)
		{
			turnRight();
		}
		
		if(Input.GetKey("up") && state == State.FLY)
		{
			moveForward();
		}
		
		if(Input.GetKey("down") && state == State.FLY)
		{
			moveBackward();
		}
		
		controlsCheck();
	}
	
	void turnLeft()
	{
		rot.y -= 0.8f;
			
		if(rot.z < 40)
		{
			rot.z += 0.5f;
		}
	}
	
	void turnRight()
	{
		rot.y += 0.8f;
			
		if(rot.z > -40)
		{
			rot.z -= 0.5f;
		}
	}
	
	void moveForward()
	{
		if(rot.x < 25)
			rot.x += 0.4f;
		
		this.transform.Translate(0,0,speed);
	}
	
	void moveBackward()
	{
		if(rot.x > -5)
			rot.x -= 0.4f;
		
		this.transform.Translate(0,0,-(speed*0.5f));
	}
	
	void controlsCheck()
	{
		if(!Input.GetKey("left") && !Input.GetKey("right"))
		{
			if(rot.z > 0)
				rot.z -= 0.45f;
			if(rot.z < 0)
				rot.z += 0.45f;
		}
		
		if(!Input.GetKey("up") && !Input.GetKey("down"))
		{
			if(rot.x > 10)
				rot.x -= 0.6f;
			if(rot.x < 10)
				rot.x += 0.6f;
		}
	}
	
	void liftUp()
	{
		flyHeight += 0.3f;
		if(flyHeight > 75)
			state = State.FLY;
	}
}
