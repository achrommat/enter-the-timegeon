using Chronos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomHitStop : ChronosMonoBehaviour
{
	bool waiting = false;
	public void Stop(float duration, float timeScale)
	{
		if (waiting)
			return;

		GlobalClock clock = Timekeeper.instance.Clock("Root");
		clock.localTimeScale = timeScale;
		GlobalClock clock2 = Timekeeper.instance.Clock("Player");
		clock2.localTimeScale = timeScale;
		StartCoroutine(Wait(duration));
	}
	public void Stop(float duration)
	{
		Stop(duration, 0.0f);
	}
	IEnumerator Wait(float duration)
	{
		//waiting = true;
		yield return ChronosTime.WaitForSeconds(duration);
		GlobalClock clock = Timekeeper.instance.Clock("Root");
		clock.localTimeScale = 1.0f;
		GlobalClock clock2 = Timekeeper.instance.Clock("Player");
		clock2.localTimeScale = 1.0f;
		waiting = false;
	}
}
