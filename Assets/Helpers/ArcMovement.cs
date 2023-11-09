using System.Collections;
using UnityEngine;

public class ArcMovement : MonoBehaviour
{
    public AnimationCurve curve;

    public float duration = 1.0f;

    public float maxHeightY = 3.0f;

    public IEnumerator Curve(Vector3 start, Vector2 finish)
    {
        float timePast = 0f;

        //temp vars
        while (timePast < duration)
        {
            timePast += Time.deltaTime;

            float linearTime = timePast / duration;//0 to 1 time
            float heightTime = curve.Evaluate(linearTime);//value from curve

            float height = Mathf.Lerp(0f, maxHeightY, heightTime);//clamped between the max height and 0

            transform.position = Vector2.Lerp(start, finish, linearTime) + new Vector2(0f, height);//adding values on y axis

            yield return null;
        }
    }
}
