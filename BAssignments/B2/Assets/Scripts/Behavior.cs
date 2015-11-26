using UnityEngine;
using System.Collections;
using TreeSharpPlus;
using RootMotion.FinalIK;

public class Behavior : MonoBehaviour
{
    public GameObject Red;
    public Transform  target;
    public GameObject Attendant;
    public GameObject AttendantLookAt;
    public GameObject Black;
    public GameObject Blue;
    public GameObject Green;
    public GameObject Thinker;
    public GameObject ThinkerLookAt;
    public GameObject Ralph;
    public GameObject RalphLookAt;
    public GameObject ThrowAt1;
    public GameObject ThrowAt2;
    public GameObject ThrowAt3;

    // Positions
    public Transform ballEventPosition;
    public Transform ballPosition;
    public Transform ButtonPosition;
    public Transform thinkerPosition;
    public Transform PodiumPosition;
    public GameObject Podium;
    public Transform decide;

    // Interactives
    public InteractionObject Green_RightHand;
    public InteractionObject Thinker_RightHand;
    public InteractionObject Button;
    public InteractionObject Ball;
    public GameObject ballObject;

    // Interaction Waypoints
    public GameObject ThrowBall;
    public Transform ThrowBallPos;
    public Transform InteractPainting;
    public GameObject Painting;
    public Transform InteractGalleryWest;
    public GameObject ShowPiece1;
    public GameObject ShowPiece2;
    public Transform InteractCenterpiece;
    public GameObject CenterPiece;


    // Final Places
    public Transform GreenFinal;
    public Transform ThinkerFinal;
    public Transform BlueFinal;
    public Transform BlackFinal;
    public Transform RalphFinal;
    public Transform AttendantFinal;

    // Full Body Biped Effectors
    public FullBodyBipedEffector Effector;
    public FullBodyBipedEffector eff_handshake;

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
    }

    protected Node ST_ApproachAndWait(GameObject agent, Transform target)
    {
        Val<Vector3> position = Val.V(() => target.position);
        return new Sequence(
            new LeafTrace("Goingto" + target.position),
            agent.GetComponent<BehaviorMecanim>().Node_GoTo(position), 
            new LeafWait(1000));
    }

    protected Node ST_Body_Animation(string anim, bool check)
    {
        Val<string> animation_name = Val.V(() => anim);
        Val<bool> start = Val.V(() => check);
        return Green.GetComponent<BehaviorMecanim>().Node_BodyAnimation(animation_name, start);
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

    protected Node ST_Shake_Hands(GameObject actor1, GameObject actor2)
    {
        Val<Vector3> Agent1 = Val.V(() => actor1.transform.position);
        Val<Vector3> Agent2 = Val.V(() => actor2.transform.position);

        return new Sequence(
            new SequenceParallel(
                actor2.GetComponent<BehaviorMecanim>().Node_OrientTowards(Agent1),
                actor1.GetComponent<BehaviorMecanim>().Node_OrientTowards(Agent2)
                ),
            new SequenceParallel(
                new LeafTrace("shakehands"),
                this.ST_Hand_Animation(Thinker, "pointing", 2000),
                this.ST_Hand_Animation(Green, "pointing", 2000)
                )
            );

    }

    // Looped
    protected Node ST_Speak_Pair(GameObject actor1, GameObject actor2, int loop)
    {
        return new Sequence(
            this.ST_Face_Off(actor1, actor2),
            new DecoratorLoop(loop,
            new Sequence(
                  new NonLinearSelect(
                    this.ST_Hand_Animation(actor1, "surprised", 1500),
                    this.ST_Hand_Animation(actor1, "wonderful", 1500),
                    this.ST_Hand_Animation(actor1, "cry", 1500),
                    this.ST_Hand_Animation(actor1, "think", 1500),
                    this.ST_Hand_Animation(actor1, "beingcocky", 1500),
                    this.ST_Hand_Animation(actor1, "yawn", 2500)
                    ),
                    new NonLinearSelect(
                    this.ST_Hand_Animation(actor2, "surprised", 1500),
                    this.ST_Hand_Animation(actor2, "wonderful", 1500),
                    this.ST_Hand_Animation(actor2, "cry", 1500),
                    this.ST_Hand_Animation(actor2, "think", 1500),
                    this.ST_Hand_Animation(actor2, "beingcocky", 1500),
                    this.ST_Hand_Animation(actor2, "yawn", 2500)
                    )
                   )
                   )

            );
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

    protected Node ST_Speak_Multiple(GameObject person1, GameObject person2, GameObject person3, int loop)
    {
        return new DecoratorLoop(loop,
         new Sequence(
                  new NonLinearSelect(
                  this.ST_Hand_Animation(person1, "beingcocky", 1500),
                  this.ST_Hand_Animation(person1, "think", 1500)
                  ),
                   new NonLinearSelect(
                  this.ST_Hand_Animation(person2, "beingcocky", 1500),
                  this.ST_Hand_Animation(person2, "think", 1500)
                  ),
                    new NonLinearSelect(
                  this.ST_Hand_Animation(person3, "beingcocky", 1500),
                  this.ST_Hand_Animation(person3, "think", 1500)
                  )
                  )
          );
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
                ballObject.GetComponent<Ball>().nodeThrow(agent1.transform.position - agent2.transform.position)
                ),
            new Sequence(
                this.ST_Body_Animation("throw", true), 
                new LeafWait(1000),
                this.ST_Body_Animation("throw", false)
                )
                );
    }


    protected Node ST_Press_Button(GameObject agent, FullBodyBipedEffector effector, InteractionObject button)
    {
        Val<FullBodyBipedEffector> eff = Val.V(() => effector);
        Val<InteractionObject> butt = Val.V(() => button);
        return new Sequence(new LeafTrace("Interaction"), agent.GetComponent<BehaviorMecanim>().Node_StartInteraction(eff, butt), new LeafWait(1000));
    }

    protected Node ST_Use_Ball_Event()
    {
        return new Sequence(
                                this.ST_ApproachAndWait(Green, ballEventPosition),
                                this.ST_Face_Off(Green, Ralph),
                                this.ST_Speak_Multiple(Green, Black, Blue, 1),
                                this.ST_Get_Ball(Green, Effector, Ball, "GetBall"),
                                new NonLinearSelect(
                                    this.ST_Throw_Ball(ThrowAt1, Green),
                                    this.ST_Throw_Ball(ThrowAt2, Green),
                                    this.ST_Throw_Ball(ThrowAt3, Green)
                                    ),
                                this.ST_Get_Ball(Ralph, Effector, Ball, "CatchBall"),
                                this.ST_Speak_Pair(Green, Ralph, 1)
                                );
    }
    protected Node ST_Interaction_Event()
    {
        return new Sequence(
                                new LeafTrace("StartInteraction"),
                                this.ST_Face_Animation(Thinker, "think", 1500),
                                this.ST_ApproachAndWait(Green, thinkerPosition),
                                this.ST_Face_Off(Thinker, Green),
                                this.ST_Speak_Pair(Green, Thinker, 1),
                                this.ST_Shake_Hands(Thinker, Green)
                                );
    }

    protected Node ST_Follow(Transform target)
    {
        Val<Vector3> position = Val.V(() => target.position);
        return new Sequence(
            Red.GetComponent<BehaviorMecanim>().Node_GoTo(position), 
            new LeafWait(1000));
    }

    protected Node BuildTreeRoot()
    {
        return
              new DecoratorLoop(-1,
                new Sequence(
                        // take your places!

                        new SequenceParallel(
                            this.ST_LookAt(Thinker, ThinkerLookAt),
                            this.ST_LookAt(Ralph, ballObject),
                            this.ST_LookAt(Attendant, AttendantLookAt),
                            this.ST_Face_Off(Black, Blue)
                            ),
                        new SequenceParallel(
                            this.ST_Speak_Pair(Black, Blue, 3),
                            this.ST_ApproachAndWait(Green, decide)
                            ),
                        // choose your destiny!
                        new SequenceShuffle(
                            this.ST_Use_Ball_Event(),
                            this.ST_Interaction_Event(),

                            // Randomly choose sequence
                            new NonLinearSelect(
                                new Sequence(
                                    this.ST_ApproachAndWait(Green, InteractPainting),
                                    this.ST_LookAt(Green, Painting)
                                    ),
                                new Sequence(
                                    this.ST_ApproachAndWait(Green, InteractGalleryWest),
                                    this.ST_LookAt(Green, ShowPiece1),
                                    this.ST_LookAt(Green, ShowPiece2)
                                    ),
                                new Sequence(
                                    this.ST_ApproachAndWait(Green, InteractCenterpiece),
                                    this.ST_LookAt(Green, CenterPiece)
                                    ),
                                new LeafWait(120000)                                
                            )
                        ),
                        this.ST_ApproachAndWait(Green, GreenFinal),
                        this.ST_LookAt(Green, Red),
                        this.ST_ApproachAndWait(Green, Red.GetComponent(typeof(Transform)) as Transform),
                        this.ST_LookAt(Green, Green),
                        this.ST_Hand_Animation(Green,"wave",1000),
                        this.ST_LookAt(Green, Podium),
                        new SequenceParallel(
                            this.ST_ApproachAndWait(Thinker, ButtonPosition),
                            this.ST_ApproachAndWait(Green, PodiumPosition)
                        ),
                        this.ST_Press_Button(Thinker, Effector, Button),
                        this.ST_Face_Off(Green,Thinker),                      
                        this.ST_LookAt(Green, Podium),
                        new SequenceParallel(
                            this.ST_ApproachAndWait(Thinker, ThinkerFinal),
                            this.ST_ApproachAndWait(Ralph, RalphFinal),
                            this.ST_ApproachAndWait(Blue, BlueFinal),
                            this.ST_ApproachAndWait(Black, BlackFinal),
                            this.ST_ApproachAndWait(Attendant, AttendantFinal)
                            ),
                        new SequenceParallel(
                            this.ST_LookAt(Thinker, Green),
                            this.ST_LookAt(Ralph, Green),
                            this.ST_LookAt(Blue, Green),
                            this.ST_LookAt(Black, Green),
                            this.ST_LookAt(Attendant, Green)
                            ),

                        new LeafWait(150000)
                        )
                        );
    }
}
