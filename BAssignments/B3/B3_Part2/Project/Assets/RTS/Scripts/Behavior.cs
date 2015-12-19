using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;
using RootMotion.FinalIK;

public class Behavior : MonoBehaviour
{
	public GameObject Hero;
	public GameObject EvilKnight1;
	public GameObject EvilKnight2;
	public GameObject EvilKnight3;
	public GameObject EvilKnight4;
	public GameObject EvilKnight5;
	public GameObject EvilKnight6;
	public GameObject EvilKnight7;
	public GameObject EvilKnight8;
	public GameObject EvilKnight9;
	public GameObject EvilKnight10;
	public GameObject EvilKnight11;
	public GameObject EvilKnight12;
	public GameObject Skeleton1;
	public GameObject Skeleton2;
	public GameObject Skeleton3;
	public GameObject Skeleton4;
	public GameObject Skeleton5;
	public GameObject Goblin;

	public GameObject Magic;

	public GameObject FleeTarget;
	public GameObject sk1;
	public GameObject sk2;
	public GameObject sk3;

	public GameObject k1;
	public GameObject k2;
	public GameObject k3;

	public GameObject Rock;
	public InteractionObject RockInter;
	public GameObject Rock2;
	public InteractionObject RockInter2;
	public GameObject Gold;
	public InteractionObject GoldInter;

	public Transform  Stage2;
	public Transform  Stage3;

	private GameObject protagonist;
	private GameObject antagonist;
	private int selectedProtagonist = -1;
	private int selectedAntagonist = -1;
	private int fightOrScare = -1;
	private int storyArc = -1;
	int[] aliveStatus = new int[11];

	public Camera CameraView;
	private int cameraSelect = 1;

    //public Transform goalPosition;
    

    // Full Body Biped Effectors
    public FullBodyBipedEffector Effector;
    //public FullBodyBipedEffector eff_handshake;

    private BehaviorAgent BehaviorAgent;
    // Use this for initialization
    void Start()
    {
		for (int i = 0; i < 11; i++) {
			aliveStatus [i] = 1;
		}
		BehaviorAgent = new BehaviorAgent(this.BuildTreeRoot());
		BehaviorManager.Instance.Register(BehaviorAgent);
		BehaviorAgent.StartBehavior();
    }

    // Update is called once per frame
    void Update()
	{

		protagonist = Hero;
		selectedProtagonist = 0;

		if (Input.GetKeyDown (KeyCode.F)) {
			fightOrScare = 0;
		}

		if (Input.GetKeyDown (KeyCode.B)) {
			fightOrScare = 1;
		}
			
			if (Input.GetKeyDown (KeyCode.Alpha1) || Input.GetKeyDown (KeyCode.Keypad1)) {
				antagonist = EvilKnight1;
				selectedAntagonist = 1;
			}
			if (Input.GetKeyDown (KeyCode.Alpha2) || Input.GetKeyDown (KeyCode.Keypad2)) {
				antagonist = EvilKnight2;
				selectedAntagonist = 2;
			}
			if (Input.GetKeyDown (KeyCode.Alpha3) || Input.GetKeyDown (KeyCode.Keypad3)) {
				antagonist = EvilKnight3;
				selectedAntagonist = 3;
			}
			if (Input.GetKeyDown (KeyCode.Alpha4) || Input.GetKeyDown (KeyCode.Keypad4)) {
				antagonist = EvilKnight4;
				selectedAntagonist = 4;
			}
			if (Input.GetKeyDown (KeyCode.Alpha5) || Input.GetKeyDown (KeyCode.Keypad5)) {
				antagonist = EvilKnight9;
				selectedAntagonist = 5;
			}
			if (Input.GetKeyDown (KeyCode.Alpha6) || Input.GetKeyDown (KeyCode.Keypad6)) {
				antagonist = EvilKnight10;
				selectedAntagonist = 6;
			}
			if (Input.GetKeyDown (KeyCode.Alpha7) || Input.GetKeyDown (KeyCode.Keypad7)) {
				antagonist = EvilKnight11;
				selectedAntagonist = 7;
			}
			if (Input.GetKeyDown (KeyCode.Alpha8) || Input.GetKeyDown (KeyCode.Keypad8)) {
				antagonist = EvilKnight12;
				selectedAntagonist = 8;
			}
			if (Input.GetKeyDown (KeyCode.G)) {
				antagonist = Goblin;
				selectedAntagonist = 9;
			}
			if (Input.GetKeyDown (KeyCode.S)) {
				antagonist = Skeleton1;
				selectedAntagonist = 10;
			}

		if (Input.GetKeyDown (KeyCode.C)) {
			if (cameraSelect == 0) {
				cameraSelect = 1;
			} else {
				cameraSelect = 0;
			}
		}

		if (cameraSelect == 0) {
			CameraView.transform.position = new Vector3 (250, 600, -200);
			CameraView.transform.eulerAngles = new Vector3 (60, 0, 0);
		} else {
			CameraView.transform.position = protagonist.transform.position + new Vector3 (0, 200, 0);
			CameraView.transform.eulerAngles = protagonist.transform.eulerAngles + new Vector3 (90, 0, 0);
		} 

		if (Input.GetKeyDown(KeyCode.Return))
		{
			BehaviorAgent.StopBehavior();
			BehaviorAgent = new BehaviorAgent(this.BuildTreeRoot());
			BehaviorManager.Instance.Register(BehaviorAgent);
			BehaviorAgent.StartBehavior();
		}
			
    }

    protected Node ST_ApproachAndWait(GameObject agent, Transform target)
    {
        Val<Vector3> position = Val.V(() => target.position);
        return new Sequence(
            new LeafTrace("Goingto" + target.position),
            agent.GetComponent<BehaviorMecanim>().Node_GoTo(position), 
            new LeafWait(1000));
    }

	protected Node ST_ApproachAndWait(GameObject agent1, GameObject agent2)
	{
		Vector3 agposition1 = agent2.transform.position;
		agposition1.x = agposition1.x + 1;
		agposition1.z = agposition1.z + 1;
		Val<Vector3> agposition = Val.V(() => agposition1);
		return new Sequence(
			new LeafTrace("Goingto" + agposition),
			agent1.GetComponent<BehaviorMecanim>().Node_GoTo(agposition), 
			new LeafWait(1000));
	}

	protected Node ST_Body_Animation(GameObject agent, string anim, bool check)
    {
        Val<string> animation_name = Val.V(() => anim);
        Val<bool> start = Val.V(() => check);
        return agent.GetComponent<BehaviorMecanim>().Node_BodyAnimation(animation_name, start);
    }

    protected Node ST_Hand_Animation(GameObject agent, string anim, long time)
    {
        Val<string> animation_name = Val.V(() => anim);
        Val<bool> start = Val.V(() => true);
        return new Sequence(
            agent.GetComponent<BehaviorMecanim>().Node_HandAnimation(animation_name, start),
            new LeafWait(time),
            agent.GetComponent<BehaviorMecanim>().Node_HandAnimation(animation_name, false));
    }

    protected Node ST_Face_Animation(GameObject agent, string anim, long time)
    {
        Val<string> Gesture_name = Val.V(() => anim);
        Val<bool> start = Val.V(() => true);
        return new Sequence(
            agent.GetComponent<BehaviorMecanim>().Node_FaceAnimation(Gesture_name, start),
            new LeafWait(time),
            agent.GetComponent<BehaviorMecanim>().Node_FaceAnimation(Gesture_name, false));
    }

    protected Node ST_LookAt(GameObject agent, GameObject subject)
    {
        Val<Vector3> subjectpos = Val.V(() => subject.transform.position);
        return new Sequence(
            agent.GetComponent<BehaviorMecanim>().Node_OrientTowards(subjectpos));
    }

    protected Node ST_Face_Off(GameObject actor1, GameObject actor2)
    {
        Val<Vector3> Agent1 = Val.V(() => actor1.transform.position);
        Val<Vector3> Agent2 = Val.V(() => actor2.transform.position);

        return new SequenceParallel(
            new LeafTrace("face_off"), 
            actor2.GetComponent<BehaviorMecanim>().Node_OrientTowards(Agent1), 
            actor1.GetComponent<BehaviorMecanim>().Node_OrientTowards(Agent2));
    }
		

    protected Node ST_Speak_Pair(GameObject agent1, GameObject agent2)
    {
        return new Sequence(
            this.ST_Face_Off(agent1, agent2),
            new Sequence(
                     new NonLinearSelect(
                    this.ST_Hand_Animation(agent1, "beingcocky", 1500),
                    this.ST_Hand_Animation(agent1, "think", 1500)
                    ),
                     new NonLinearSelect(
                    this.ST_Hand_Animation(agent2, "beingcocky", 1500),
                    this.ST_Hand_Animation(agent2, "think", 1500)
                   )

            ));
    }

	protected Node ST_Fight(GameObject agent1, GameObject agent2)
	{
		aliveStatus [selectedAntagonist] = 0;
		return new Sequence (
			this.ST_Face_Off(agent1, agent2),
			this.ST_Body_Animation (agent2, "FIGHT", true),
			this.ST_Body_Animation (agent1, "SWORD", true),
			new LeafWait(4000),
			this.ST_Body_Animation (agent1, "SWORD", false),
			this.ST_Body_Animation (agent2, "FIGHT", false),
			this.ST_Body_Animation (agent2, "DYING", true)
		);
	}

	protected Node ST_Fight_MartialArts(GameObject agent1, GameObject agent2)
	{
		aliveStatus [selectedAntagonist] = 0;
		return new Sequence (
			this.ST_Face_Off(agent1, agent2),
			this.ST_Body_Animation (agent2, "FIGHT", true),
			this.ST_Body_Animation (agent1, "BREAKDANCE", true),
			new LeafWait(9000),
			this.ST_Body_Animation (agent1, "BREAKDANCE", false),
			this.ST_Body_Animation (agent2, "FIGHT", false),
			this.ST_Body_Animation (agent2, "DYING", true)
		);
	}

	protected Node ST_Scare(GameObject agent1, GameObject agent2)
	{
		aliveStatus [selectedAntagonist] = 0;
		return new Sequence (
			this.ST_Face_Off (agent1, agent2),
			this.ST_Face_Animation (agent1, "FIREBREATH", 1500),
			this.ST_Flee (agent2,agent1)
		);
	}

	protected Node ST_Scare_Monster(GameObject agent1, GameObject agent2)
	{
		aliveStatus [selectedProtagonist] = 0;
		return new Sequence (
			this.ST_Face_Off (agent1, agent2),
			this.ST_Face_Animation (agent1, "FIREBREATH", 1500),
			this.ST_Face_Animation (agent2, "FIREBREATH", 1500),
			this.ST_Flee (agent1,agent2)
		);
	}

	protected Node ST_Applaud(GameObject agent1, GameObject agent2)
	{
		return new Sequence (
			this.ST_Face_Off (agent1, agent2),
			this.ST_Face_Animation (agent1, "CLAP", 1500)
		);
	}

	protected Node ST_Fight2(GameObject agent1, GameObject agent2)
	{
		aliveStatus [selectedAntagonist] = 0;
		return new Sequence (
			this.ST_Face_Off(agent1, agent2),
			this.ST_Body_Animation (agent2, "DUCK", true),
			this.ST_Body_Animation (agent1, "SWORD", true),
			new LeafWait(4000),
			this.ST_Body_Animation (agent1, "SWORD", false),
			this.ST_Body_Animation (agent2, "DUCK", false),
			this.ST_Body_Animation (agent2, "FIGHT", true),
			new LeafWait(1000),
			this.ST_Body_Animation (agent2, "FIGHT", false),
			this.ST_Body_Animation (agent1, "DUCK", true),
			this.ST_Body_Animation (agent2, "BREAKDANCE", true),
			new LeafWait(9000),
			this.ST_Body_Animation (agent2, "BREAKDANCE", false),
			this.ST_Body_Animation (agent1, "DUCK", false),
			this.ST_Body_Animation (agent1, "THROW", true),
			new LeafWait(4000),
			this.ST_Body_Animation (agent1, "THROW", false),
			this.ST_Body_Animation (agent2, "DYING", true)
		);
	}

	protected Node ST_Flee(GameObject agent1, GameObject agent2)
	{
		return new Sequence (
			this.ST_Hand_Animation(agent1, "handsup", 1500),
			this.ST_Face_Animation(agent1, "headshake", 1500),
			this.ST_LookAt(agent1, agent2),
			this.ST_Hand_Animation(agent1, "wave", 1500),
			this.ST_ApproachAndWait(agent1, FleeTarget)
		);
	}

	protected Node ST_SkeletonWander()
	{
		Node Skeleton1Node = ST_ApproachAndWait (Skeleton2, sk1);
		Node Skeleton2Node = ST_ApproachAndWait (Skeleton3, sk1);
		Node Skeleton3Node = ST_ApproachAndWait (Skeleton4, sk1);
		Node Skeleton4Node =  ST_ApproachAndWait (Skeleton5, sk1);
		System.Random rnd = new System.Random();

		int choice = rnd.Next(1, 4);

		switch(choice){
		case 1:
			Skeleton1Node = ST_ApproachAndWait (Skeleton2, sk1);
			break;
		case 2:
			Skeleton1Node = ST_ApproachAndWait (Skeleton2, sk2);
			break;
		case 3:
			Skeleton1Node = ST_ApproachAndWait (Skeleton2, sk3);
			break;
		}

		choice = rnd.Next(1, 4);

		switch(choice){
		case 1:
			Skeleton2Node = ST_ApproachAndWait (Skeleton3, sk1);
			break;
		case 2:
			Skeleton2Node = ST_ApproachAndWait (Skeleton3, sk2);
			break;
		case 3:
			Skeleton2Node = ST_ApproachAndWait (Skeleton3, sk3);
			break;
		}

		choice = rnd.Next(1, 4);

		switch(choice){
		case 1:
			Skeleton3Node = ST_ApproachAndWait (Skeleton4, sk1);
			break;
		case 2:
			Skeleton3Node = ST_ApproachAndWait (Skeleton4, sk2);
			break;
		case 3:
			Skeleton3Node = ST_ApproachAndWait (Skeleton4, sk3);
			break;
		}

		choice = rnd.Next(1, 4);

		switch(choice){
		case 1:
			Skeleton4Node = ST_ApproachAndWait (Skeleton5, sk1);
			break;
		case 2:
			Skeleton4Node = ST_ApproachAndWait (Skeleton5, sk2);
			break;
		case 3:
			Skeleton4Node = ST_ApproachAndWait (Skeleton5, sk3);
			break;
		}

		return new DecoratorLoop (-1,
			new SequenceParallel (
				Skeleton1Node,
				Skeleton2Node,
				Skeleton3Node,
				Skeleton4Node
			)
		);
	}

	protected Node ST_KnightWander()
	{
		Node Knight1Node = ST_ApproachAndWait (EvilKnight5, k1);
		Node Knight2Node = ST_ApproachAndWait (EvilKnight6, k1);
		Node Knight3Node = ST_ApproachAndWait (EvilKnight7, k1);
		Node Knight4Node =  ST_ApproachAndWait (EvilKnight8, k1);
		System.Random rnd = new System.Random();

		int choice = rnd.Next(1, 4);

		switch(choice){
		case 1:
			Knight1Node = ST_ApproachAndWait (EvilKnight5, k1);
			break;
		case 2:
			Knight1Node = ST_ApproachAndWait (EvilKnight5, k2);
			break;
		case 3:
			Knight1Node = ST_ApproachAndWait (EvilKnight5, k3);
			break;
		}

		choice = rnd.Next(1, 4);

		switch(choice){
		case 1:
			Knight2Node = ST_ApproachAndWait (EvilKnight6, k1);
			break;
		case 2:
			Knight2Node = ST_ApproachAndWait (EvilKnight6, k2);
			break;
		case 3:
			Knight2Node = ST_ApproachAndWait (EvilKnight6, k3);
			break;
		}

		choice = rnd.Next(1, 4);

		switch(choice){
		case 1:
			Knight3Node = ST_ApproachAndWait (EvilKnight7, k1);
			break;
		case 2:
			Knight3Node = ST_ApproachAndWait (EvilKnight7, k2);
			break;
		case 3:
			Knight3Node = ST_ApproachAndWait (EvilKnight7, k3);
			break;
		}

		choice = rnd.Next(1, 4);

		switch(choice){
		case 1:
			Knight4Node = ST_ApproachAndWait (EvilKnight8, k1);
			break;
		case 2:
			Knight4Node = ST_ApproachAndWait (EvilKnight8, k2);
			break;
		case 3:
			Knight4Node = ST_ApproachAndWait (EvilKnight8, k3);
			break;
		}

		return new DecoratorLoop (-1,
			new SequenceParallel (
				new Sequence(
					Knight1Node,
					this.ST_Hand_Animation(EvilKnight5,"Pointing",1500)
				),
				new Sequence(
					Knight2Node,
					this.ST_Hand_Animation(EvilKnight6,"Pointing",1500)
				),
				new Sequence(
					Knight3Node,
					this.ST_Hand_Animation(EvilKnight7,"Pointing",1500)
				),
				new Sequence(
					Knight4Node,
					this.ST_Hand_Animation(EvilKnight8,"Pointing",1500)
				)
			)
		);
	}

    protected Node BuildTreeRoot()
    {
        return
                new Sequence(
					this.ST_MonitorStoryState(),
					this.ST_Story());
    }

	protected Node ST_Story()
	{
		return new SequenceParallel
			(
				this.ST_KnightWander(),
				this.ST_SkeletonWander(),
				new Selector (
					this.ST_Hero_Story (),
					this.ST_Guard_Story (),
					this.ST_Skeleton_Story (),
					this.ST_Goblin_Story ()
				)
		);
	}

	protected Node ST_Hero_Story()
	{
		Func<bool> act = () => (false);
		if (selectedProtagonist != 0 || (aliveStatus [selectedProtagonist] == 0) || (aliveStatus [selectedAntagonist] == 0) || selectedAntagonist == selectedProtagonist) {
			return new LeafAssert (act);
		}
		if (storyArc == 0 && selectedAntagonist > 2) {
			return new LeafAssert (act);
		}
		if (storyArc == 1 && (selectedAntagonist > 4 && selectedAntagonist < 10)) {
			return new LeafAssert (act);
		}
		if (fightOrScare == 0) {
			if ((selectedAntagonist <= 8) || (selectedAntagonist == 10)) {
				return new Sequence(
					this.ST_ApproachAndWait(protagonist, antagonist),
					this.ST_Fight (protagonist, antagonist)
				);
			}
			if (selectedAntagonist == 9) {
				return new Sequence(
					this.ST_ApproachAndWait(protagonist, antagonist),
					this.ST_Fight2 (protagonist, antagonist)
				);
			}
		}
		if (fightOrScare == 1) {
			if (selectedAntagonist <= 8) {
				return new Sequence(
					this.ST_ApproachAndWait(protagonist, antagonist),
					this.ST_Scare (protagonist, antagonist)
				);
			}
			if (selectedAntagonist >= 9) {
				return new Sequence(
					this.ST_ApproachAndWait(protagonist, antagonist),
					this.ST_Scare_Monster(protagonist, antagonist)
				);
			}
		}
		return new LeafAssert (act);

	}

	protected Node ST_Guard_Story()
	{
		Func<bool> act = () => (false);
		if (selectedProtagonist < 0 || selectedProtagonist > 8 || (aliveStatus [selectedProtagonist] == 0) || (aliveStatus [selectedAntagonist] == 0) || selectedAntagonist == selectedProtagonist) {
			return new LeafAssert (act);
		}	
		if (selectedProtagonist <= 2 && selectedAntagonist > 2) {
			return new LeafAssert (act);
		}
		if ((selectedProtagonist > 2 && selectedProtagonist <=4) && !(selectedAntagonist > 2 && selectedAntagonist <=4)) {
			return new LeafAssert (act);
		}
		if ((selectedProtagonist > 2 && selectedProtagonist <=4) && (selectedAntagonist == 0) && storyArc < 1) {
			return new LeafAssert (act);
		}
		if ((selectedProtagonist > 4 && selectedProtagonist <=8) && !(selectedAntagonist > 4 && selectedAntagonist <=8)) {
			return new LeafAssert (act);
		}
		if ((selectedProtagonist > 4 && selectedProtagonist <=8) && (selectedAntagonist == 0) && storyArc < 2){
			return new LeafAssert (act);
		}
		if (selectedAntagonist >= 1 && selectedAntagonist < 9) {
			return new Sequence(
				this.ST_ApproachAndWait(protagonist, antagonist),
				this.ST_Speak_Pair(protagonist, antagonist)
			);
		}
		if (fightOrScare == 0) {
			if (selectedAntagonist == 0) {
				int temp = selectedProtagonist;
				selectedProtagonist = selectedAntagonist;
				selectedAntagonist = temp;
				Node temp2 = new Sequence(
					this.ST_ApproachAndWait(protagonist, antagonist),
					this.ST_Fight (antagonist, protagonist)
				);
				temp = selectedProtagonist;
				selectedProtagonist = selectedAntagonist;
				selectedAntagonist = temp;
				return temp2;
			}
		}
		if (fightOrScare == 1) {
			if (selectedAntagonist == 0) {
				int temp = selectedProtagonist;
				selectedProtagonist = selectedAntagonist;
				selectedAntagonist = temp;
				Node temp2 = new Sequence(
					this.ST_ApproachAndWait(protagonist, antagonist),
					this.ST_Scare_Monster (protagonist, antagonist)
				);
				temp = selectedProtagonist;
				selectedProtagonist = selectedAntagonist;
				selectedAntagonist = temp;
				return temp2;
			}
		}
		return new LeafAssert (act);
	}

	protected Node ST_Skeleton_Story()
	{
		Func<bool> act = () => (false);
		if (selectedProtagonist != 10 || (aliveStatus [selectedProtagonist] == 0) || (aliveStatus [selectedAntagonist] == 0) || selectedAntagonist == selectedProtagonist) {
			return new LeafAssert (act);
		}
		if (fightOrScare == 1) {
			if (selectedAntagonist != 9) {
				return new Sequence(
					this.ST_ApproachAndWait(protagonist, antagonist),
					this.ST_Scare (protagonist, antagonist)
				);
			}
			if (selectedAntagonist == 9) {
				return new Sequence(
					this.ST_ApproachAndWait(protagonist, antagonist),
					this.ST_Scare_Monster (protagonist, antagonist)
				);
			}
		}
		return new LeafAssert (act);
	}

	protected Node ST_Goblin_Story()
	{
		Func<bool> act = () => (false);
		if (selectedProtagonist != 9 || (aliveStatus [selectedProtagonist] == 0) || (aliveStatus [selectedAntagonist] == 0) || selectedAntagonist == selectedProtagonist) {
			return new LeafAssert (act);
		}	
		if (selectedAntagonist >= 1 && selectedAntagonist < 8) {
			return new Sequence (
				this.ST_ApproachAndWait (protagonist, antagonist),
				this.ST_Applaud (antagonist, protagonist)
			);
		} else {
			if (fightOrScare == 0) {
				return new Sequence (
					this.ST_ApproachAndWait (protagonist, antagonist),
					this.ST_Fight_MartialArts (protagonist, antagonist)
				);
			}
			if (fightOrScare == 1) {
				return new Sequence (
					this.ST_ApproachAndWait (protagonist, antagonist),
					this.ST_Scare (protagonist, antagonist)
				);
			}
		}
		return new LeafAssert (act);
	}

	protected Node ST_MonitorStoryState()
	{
		storyArc = -1;
		bool isState = true;
		Func<bool> act = () => (isState);
		return new Selector (
			this.ST_CheckHeroArcVanguard (),
			this.ST_CheckHeroArcGraveyard (),
			this.ST_CheckHeroArcGoblin (),
			new LeafAssert (act)
		);
	}

	protected Node ST_CheckHeroArcVanguard()
	{
		bool isState = true;
		isState = isState && (selectedProtagonist == 0);
		bool anyAlive = false;
		for (int i = 0; i <= 2; i++) {
			anyAlive = anyAlive || (aliveStatus[i] == 1);
		}
		isState = isState && anyAlive;

		Func<bool> act = () => (isState);

		Node outcome = new LeafAssert (act);

		if (isState) {
			storyArc = 0;
		}
		return outcome;
	}

	protected Node ST_CheckHeroArcGraveyard()
	{
		bool isState = true;
		isState = isState && (selectedProtagonist == 0) && (aliveStatus[0] == 1);
		bool anyAlive = false;
		bool allDead = true;

		for (int i = 1; i <= 2; i++) {
			allDead = allDead && (aliveStatus[i] == 0);
		}
		isState = isState && allDead;

		for (int i = 3; i <= 4; i++) {
			anyAlive = anyAlive || (aliveStatus[i] == 1);
		}
		isState = isState && anyAlive;

		Func<bool> act = () => (isState);

		Node outcome = new LeafAssert (act);
		if (isState) {
			storyArc = 1;
		}
		return outcome;                              
	}

	protected Node ST_CheckHeroArcGoblin()
	{
		bool isState = true;
		isState = isState && (selectedProtagonist == 0) && (aliveStatus[0] == 1);
		bool allDead = true;

		for (int i = 1; i <= 4; i++) {
			allDead = allDead && (aliveStatus[i] == 0);
		}
		isState = isState && allDead;

		Func<bool> act = () => (isState);

		Node outcome = new LeafAssert (act);
		if (isState) {
			storyArc = 2;
		}
		return outcome;                               
	}
}
