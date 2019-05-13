using UnityEngine;
using System.Collections.Generic;

namespace EZR
{
    public class MeasureLine : MonoBehaviour
    {
        [HideInInspector]
        public int Index = 0;

        public static List<MeasureLine> MeasureLines = new List<MeasureLine>();

        Animator anim;

        void Start()
        {
            anim = GetComponent<Animator>();
            MeasureLines.Add(this);
        }

        void Update()
        {
            if ((Index * PatternUtils.Pattern.MeasureLength) - PlayManager.Position < -(JudgmentDelta.Miss + 1))
            {
                Destroy(gameObject);
                return;
            }

            if (PlayManager.IsAutoPlay || !PlayManager.IsSimVsync)
            {
                transform.localPosition = new Vector3(
                    0,
                    (Index * PatternUtils.Pattern.MeasureLength) * PlayManager.GetSpeed(),
                    0
                );
            }
            else
            {
                transform.localPosition = new Vector3(
                    0,
                    (float)(((Index * PatternUtils.Pattern.MeasureLength) + PlayManager.SimVsyncDelta) * PlayManager.GetSpeed()),
                    0
                );
            }
        }

        public void PlayAnim()
        {
            anim.Play("MeasureLine", 0, 0);
        }

        void OnDestroy()
        {
            if (MeasureLines.Contains(this))
                MeasureLines.Remove(this);
        }
    }
}