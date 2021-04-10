using System;
using XRL.World;
using XRL.World.Parts;

namespace Fyrefly {
            public class Fyrefly_PersonalOpinion
        {


            private double _value;
            /// <summary>
            /// The strength of this opinion, ranges from hatred (-100) to adoration (+100).
            /// </summary>
            public double value
            {
                get { return Math.Max(-100, Math.Min(_value, 100)); }
                set { _value = Math.Max(-100, Math.Min(value, 100)); }
            }

            public DynamicPersonality personality;

            public GameObject opinionHaver;
            public GameObject opinionTarget;

            public Fyrefly_PersonalOpinion(DynamicPersonality p, GameObject target)
            {
                personality = p;
                opinionHaver = personality.ParentObject;
                opinionTarget = target;

                var rand = new System.Random();

                if (personality.minimumRandomOpinion <= personality.maximumRandomOpinion)
                {
                    value = rand.Next(personality.minimumRandomOpinion, personality.maximumRandomOpinion);
                };
            }

        }

}