using Chronos;
using Pathfinding;
using UnityEngine;

public class ChronosAIPath : AIPath
{
	public Timeline ChronosTime
	{
		get
		{
			return GetComponent<Timeline>();
		}
	}
	public AIDestinationSetter DestinationSetter
    {
		get
		{
			return GetComponent<AIDestinationSetter>();
		}
	}

    /// <summary>
    /// Called every frame.
    /// If no rigidbodies are used then all movement happens here.
    /// </summary>
    protected override void Update()
	{
		if (shouldRecalculatePath) SearchPath();

		// If gravity is used depends on a lot of things.
		// For example when a non-kinematic rigidbody is used then the rigidbody will apply the gravity itself
		// Note that the gravity can contain NaN's, which is why the comparison uses !(a==b) instead of just a!=b.
		usingGravity = !(gravity == Vector3.zero) && (!updatePosition || ((rigid == null || rigid.isKinematic) && (rigid2D == null || rigid2D.isKinematic)));
		if (rigid == null && rigid2D == null && canMove)
		{
			Vector3 nextPosition;
			Quaternion nextRotation;
			MovementUpdate(ChronosTime.deltaTime, out nextPosition, out nextRotation);
			FinalizeMovement(nextPosition, nextRotation);
		}
	}

	/// <summary>
	/// Called every physics update.
	/// If rigidbodies are used then all movement happens here.
	/// </summary>
	protected override void FixedUpdate()
	{
		if (!(rigid == null && rigid2D == null) && canMove)
		{
			Vector3 nextPosition;
			Quaternion nextRotation;
			MovementUpdate(ChronosTime.fixedDeltaTime, out nextPosition, out nextRotation);
			FinalizeMovement(nextPosition, nextRotation);
		}
	}
}
