using System.Collections.Generic;

namespace XRL.World.Parts
{
    public abstract class Fyrefly_DynamicPersonalityTrait : IPart
    {

        public abstract string ID { get; set; }

        private DynamicPersonality personality => ParentObject.GetPart<DynamicPersonality>();

        /// <summary>
        /// Determines if the trait can be randomly assigned to entities with a brain.
        /// </summary>
        public bool CanRandomlyGenerate = true;

        /// <summary>
        /// Determines if the trait can be randomly assigned to a quest giver
        /// </summary>
        public bool UseForQuestGiver = true;

        /// <summary>
        /// Determines if the trait should be counted towards the maximum number of traits
        /// </summary>
        public bool CountTowardsTotal = true;

        /// <summary>
        /// Used to describe a trait when shared by others.
        /// For instance "I heard Argyve" + trait.traitRumourDescriptors[random.Next(trait.traitRumourDescriptors.Count)]
        /// </summary>
        public abstract string[] traitRumourDescriptors
        {
            get;
        }

        /// <summary>
        /// A list of Personality Trait IDs this trait cannot be randomly generated with. For instance "Trusting" 
        /// </summary>
        public string[] incompatibleWith
        {
            get
            {
                return new string[] { };
            }
        }

        public abstract void MutatePersonality();


    }
}