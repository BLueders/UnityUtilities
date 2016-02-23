using UnityEngine;

namespace Toolbox
{
    /// <summary>
    /// Provides useful tools for handling animation curves
    /// </summary>
    public class CurveUtilities : MonoBehaviour
    {

        private static Keyframe GetKey(AnimationCurve curve, int i, int I)
        {

            var c = curve.length;

            i -= (I - c) / 2;

            return curve[i >= c ? c - 1 : (i <= 0 ? 0 : i)];
        }

        /// <summary>
        /// Creates a new Animationcurve that an interpolation between two other curves. Thanks to Oliver for creating this
        /// </summary>
        /// <param name="CurveA"></param>
        /// <param name="CurveB"></param>
        /// <param name="step"></param>
        /// <returns>The new interpolated animation curve</returns>
        public static AnimationCurve Interpolate(AnimationCurve CurveA, AnimationCurve CurveB, float step)
        {

            int I = CurveA.length < CurveB.length ? CurveB.length : CurveA.length;

            var keys = new Keyframe[I];

            for (int i = 0; i < I; ++i)
            {

                var a = GetKey(CurveA, i, I);
                var b = GetKey(CurveB, i, I);

                var time = Mathf.SmoothStep(a.time, b.time, step);
                var value = Mathf.SmoothStep(a.value, b.value, step);
                var inTangent = Mathf.SmoothStep(a.inTangent, b.inTangent, step);
                var outTangent = Mathf.SmoothStep(a.outTangent, b.outTangent, step);

                var c = new Keyframe(time, value, inTangent, outTangent);

                keys[i] = c;

            }

            return new AnimationCurve(keys);
        }

        public static AnimationCurve Clone(AnimationCurve c)
        {
            return new AnimationCurve(c.keys);
        }
    }
}