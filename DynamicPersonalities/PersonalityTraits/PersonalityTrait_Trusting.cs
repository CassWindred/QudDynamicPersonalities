using System.Collections.Generic;

namespace XRL.World.Parts
{
    public class PersonalityTrait_Trusting : Fyrefly_DynamicPersonalityTrait
    {


            public override string[] traitRumourDescriptors
    {
        get{
                return new string[] { 
                    "opens up easily",
                    "is very trusting",
                    "isn't at all secretive" 
                    };
            }
    }

        private string _ID = "Trusting";
        public override string ID { get => _ID; set => throw new System.NotImplementedException(); }

        public PersonalityTrait_Trusting() {



        }

        public override void MutatePersonality(){

        }

    }
}