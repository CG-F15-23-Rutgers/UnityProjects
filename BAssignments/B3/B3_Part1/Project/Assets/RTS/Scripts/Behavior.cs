using UnityEngine;
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

	public Camera CameraView;

	private int cameraOption = 1;

    //public Transform goalPosition;
    

    // Full Body Biped Effectors
    public FullBodyBipedEffector Effector;
    //public FullBodyBipedEffector eff_handshake;

    private BehaviorAgent BehaviorAgent;
    // Use this for initialization
    void Start()
    {
        BehaviorAgent = new BehaviorAgent(this.BuildTreeRoot());
        BehaviorManager.Instance.Register(BehaviorAgent);
        BehaviorAgent.StartBehavior();
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.C)){
			if (cameraOption < 1){
				cameraOption++;
			}
			else{
				cameraOption = 0;
			}
		}
		switch (cameraOption) {
		case 0:
			CameraView.transform.position = new Vector3(250,600,-200);
			CameraView.transform.eulerAngles = new Vector3 (60, 0, 0);
			break;
		case 1:
			CameraView.transform.position = Hero.transform.position + new Vector3 (0, 200, 0);
			CameraView.transform.eulerAngles = Hero.transform.eulerAngles + new Vector3 (90, 0, 0);
			break;
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

    protected Node ST_Get_Ball(GameObject picker, FullBodyBipedEffector effector, InteractionObject ball, string tracer)
    {
        Val<FullBodyBipedEffector> eff = Val.V(() => effector);
        Val<InteractionObject> ba = Val.V(() => ball);
        return new Sequence(
            new LeafTrace(tracer), 
            picker.GetComponent<BehaviorMecanim>().Node_StartInteraction(eff, ba),
            new LeafWait(1000));
    }

	protected Node ST_Throw_Ball(GameObject agent1, GameObject agent2)
    {
        return new SequenceParallel(
            new LeafTrace("throwingBall"),
            new Sequence(
                new LeafWait(1500),
                agent1.GetComponent<Ball>().nodeThrow(agent2.transform.position - agent1.transform.position)
                ),
            new Sequence(
                this.ST_Body_Animation(Hero,"DUCK", true), 
                new LeafWait(1000),
                this.ST_Body_Animation(Hero,"DUCK", false)
                )
        );
    }

	protected Node ST_Fight(GameObject agent1, GameObject agent2)
	{
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

	protected Node ST_Fight2(GameObject agent1, GameObject agent2)
	{
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

	protected Node ST_Augment(bool magic)
	{
		Magic.SetActive (false);
		if (magic) {
			Magic.SetActive (true);
		}
		return new LeafWait (1000);
	}

	protected Node ST_Flee(GameObject agent1)
	{
		return new Sequence (
			this.ST_Hand_Animation(agent1, "handsup", 1500),
            this.ST_LookAt(agent1, Hero),
			this.ST_Face_Animation(agent1, "headshake", 1500),
            this.ST_Hand_Animation(agent1, "wave", 1500),
			this.ST_ApproachAndWait(agent1, FleeTarget)
		);
	}

	protected Node ST_SkeletonWander()
	{
		return new DecoratorLoop (1000,
			new SequenceParallel (
				new NonLinearSelect (
					this.ST_ApproachAndWait (Skeleton1, sk1),
					this.ST_ApproachAndWait (Skeleton1, sk2),
					this.ST_ApproachAndWait (Skeleton1, sk3)
				),
				new NonLinearSelect (
					this.ST_ApproachAndWait (Skeleton2, sk1),
					this.ST_ApproachAndWait (Skeleton2, sk2),
					this.ST_ApproachAndWait (Skeleton2, sk3)
				),
				new NonLinearSelect (
					this.ST_ApproachAndWait (Skeleton3, sk1),
					this.ST_ApproachAndWait (Skeleton3, sk2),
					this.ST_ApproachAndWait (Skeleton3, sk3)
				),
				new NonLinearSelect (
					this.ST_ApproachAndWait (Skeleton4, sk1),
					this.ST_ApproachAndWait (Skeleton4, sk2),
					this.ST_ApproachAndWait (Skeleton4, sk3)
				),
				new NonLinearSelect (
					this.ST_ApproachAndWait (Skeleton5, sk1),
					this.ST_ApproachAndWait (Skeleton5, sk2),
					this.ST_ApproachAndWait (Skeleton5, sk3)
				)
			)
		);
	}

	protected Node ST_KnightWander()
	{
		return new DecoratorLoop (1000,
			new SequenceParallel (
				new Sequence(
				new NonLinearSelect (
					this.ST_ApproachAndWait (EvilKnight5, k1),
					this.ST_ApproachAndWait (EvilKnight5, k2),
					this.ST_ApproachAndWait (EvilKnight5, k3)
					),
					this.ST_Hand_Animation(EvilKnight5,"POINTING",1500)
				),
				new Sequence(
					new NonLinearSelect (
						this.ST_ApproachAndWait (EvilKnight6, k1),
						this.ST_ApproachAndWait (EvilKnight6, k2),
						this.ST_ApproachAndWait (EvilKnight6, k3)
					),
					this.ST_Hand_Animation(EvilKnight6,"POINTING",1500)
				),
				new Sequence(
					new NonLinearSelect (
						this.ST_ApproachAndWait (EvilKnight7, k1),
						this.ST_ApproachAndWait (EvilKnight7, k2),
						this.ST_ApproachAndWait (EvilKnight7, k3)
					),
					this.ST_Hand_Animation(EvilKnight7,"POINTING",1500)
				),
				new Sequence(
					new NonLinearSelect (
						this.ST_ApproachAndWait (EvilKnight8, k1),
						this.ST_ApproachAndWait (EvilKnight8, k2),
						this.ST_ApproachAndWait (EvilKnight8, k3)
					),
					this.ST_Hand_Animation(EvilKnight8,"POINTING",1500)
				)
			)
		);
	}
		
    protected Node BuildTreeRoot()
	{
		return
			new DecoratorLoop(
				new DecoratorForceStatus (RunStatus.Success,
				new SequenceParallel(
					new Sequence(
						// First Knight Battle
						new SequenceShuffle(
							new NonLinearSelect(
								new Sequence(
									this.ST_LookAt(Hero, EvilKnight1),
									this.ST_ApproachAndWait(EvilKnight1, Hero),
									this.ST_Face_Off(EvilKnight1, Hero),
									this.ST_Fight(Hero, EvilKnight1)
								),
								new Sequence(
									this.ST_LookAt(Hero, EvilKnight1),
									this.ST_ApproachAndWait(Hero, EvilKnight1),
									this.ST_Face_Off(EvilKnight1, Hero),
									this.ST_Fight(Hero, EvilKnight1)
								),
								new Sequence(
									this.ST_LookAt(Hero, EvilKnight1),
									this.ST_ApproachAndWait(Hero, EvilKnight1),
									this.ST_Face_Off(EvilKnight1, Hero),
									this.ST_Face_Animation(Hero, "FIREBREATH", 1500),
									this.ST_Flee(EvilKnight1)
								),
								new Sequence(
									this.ST_ApproachAndWait(Hero, Rock),
									this.ST_LookAt(Hero, Rock),
									this.ST_Get_Ball(Hero, Effector, RockInter, "GetBall"),
									this.ST_Throw_Ball(Rock, EvilKnight1),
									this.ST_Body_Animation (EvilKnight1, "DYING", true)
								)
							),
							new NonLinearSelect(
								new Sequence(
									this.ST_LookAt(Hero, EvilKnight2),
									this.ST_ApproachAndWait(EvilKnight2, Hero),
									this.ST_Face_Off(EvilKnight2, Hero),
									this.ST_Fight(Hero, EvilKnight2)
								),
								new Sequence(
									this.ST_LookAt(Hero, EvilKnight2),
									this.ST_ApproachAndWait(Hero, EvilKnight2),
									this.ST_Face_Off(EvilKnight2, Hero),
									this.ST_Fight(Hero, EvilKnight2)
								),
								new Sequence(
									this.ST_LookAt(Hero, EvilKnight2),
									this.ST_ApproachAndWait(Hero, EvilKnight2),
									this.ST_Face_Off(EvilKnight2, Hero),
									this.ST_Face_Animation(Hero, "FIREBREATH", 1500),
									this.ST_Flee(EvilKnight2)
								),
								new Sequence(
									this.ST_ApproachAndWait(Hero, Rock2),
									this.ST_LookAt(Hero, Rock2),
									this.ST_Get_Ball(Hero, Effector, RockInter2, "GetBall"),
									this.ST_Throw_Ball(Rock2, EvilKnight2),
									this.ST_Body_Animation (EvilKnight2, "DYING", true)
								)
							)
						),
						// Move to Stage 2
						this.ST_ApproachAndWait(Hero, Stage2),
						this.ST_ApproachAndWait(EvilKnight3, EvilKnight4),
						this.ST_LookAt(Hero, EvilKnight3),
						this.ST_Speak_Pair(EvilKnight3, EvilKnight4),
						this.ST_LookAt(EvilKnight3, Hero),
						this.ST_Hand_Animation(EvilKnight3, "handsup", 1500),
						// Second Knight Battle / Skeletons
						new SequenceShuffle(
							new NonLinearSelect(
								new Sequence(
									this.ST_LookAt(Hero, EvilKnight3),
									this.ST_ApproachAndWait(Hero, EvilKnight3),
									this.ST_Face_Off(EvilKnight3, Hero),
									this.ST_Fight(Hero, EvilKnight3)
								),
								new Sequence(
									this.ST_LookAt(Hero, EvilKnight3),
									this.ST_ApproachAndWait(Hero, EvilKnight3),
									this.ST_Face_Off(EvilKnight3, Hero),
									this.ST_Face_Animation(Hero, "FIREBREATH", 1500),
									this.ST_Flee(EvilKnight3)
								)
							),
							new NonLinearSelect(
								new Sequence(
									this.ST_LookAt(Hero, EvilKnight4),
									this.ST_ApproachAndWait(Hero, EvilKnight4),
									this.ST_Face_Off(EvilKnight4, Hero),
									this.ST_Fight(Hero, EvilKnight4)
								),
								new Sequence(
									this.ST_LookAt(Hero, EvilKnight4),
									this.ST_ApproachAndWait(Hero, EvilKnight4),
									this.ST_Face_Off(EvilKnight4, Hero),
									this.ST_Face_Animation(Hero, "FIREBREATH", 1500),
									this.ST_Flee(EvilKnight4)
								)
							)
						),
						//Stage 3
						this.ST_ApproachAndWait(Hero, Stage3),
						this.ST_Face_Off(Hero, Goblin),
						this.ST_Fight2(Hero, Goblin),
						new SequenceParallel(
							this.ST_Flee(EvilKnight9),
							this.ST_Flee(EvilKnight10),
							this.ST_Flee(EvilKnight11),
							this.ST_Flee(EvilKnight12)
						)
					),
					this.ST_SkeletonWander(),
					this.ST_KnightWander()
				)
				)
			);
	}
}