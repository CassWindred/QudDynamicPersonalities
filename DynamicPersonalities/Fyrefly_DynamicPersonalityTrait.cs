using System.Collections.Generic;

namespace XRL.World.Parts
{
    public abstract class Fyrefly_DynamicPersonalityTrait : IPart
    {

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

        public abstract void MutatePersonality();


    }
}