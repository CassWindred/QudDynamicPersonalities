
using System;
using System.Collections.Generic;
using Fyrefly;
using Fyrefly.UtilityConversationNodes;
using UnityEngine;
using XRL.Core;
using XRL.Messages;
using XRL.World.AI;

namespace XRL.World.Parts
{
    public class DynamicPersonality : IPart
    {
        private static void logE(string text) => Debug.LogError(text);
        private static void log(string text) => Debug.Log(text);

        public bool WillingToTrade
        {
            get
            {
                double opinion = GetPersonalOpinion(player).value;
                return (opinion >= boundaries.trade);
            }
        }


        public class BoundarySet
        {
            public int sharename = -20;
            public int trade = -10;
            public int givequest = -5;
            public int receivegift = 10;
            public int converse = 15;
            public int waterbond = 75;
            public int companion = 85;
        }

        public BoundarySet boundaries = new BoundarySet();



        public Fyrefly_PersonalOpinion GetPersonalOpinion(GameObject target)
        {
            log($"GetPersonalOpinion on {ParentObject.DebugName} Begin");
            log($"Getting {ParentObject.DebugName}'s opinion of {target.DebugName}");
            if (!personalOpinions.ContainsKey(target))
            {
                log("No opinion found, creeating a new one");
                Fyrefly_PersonalOpinion newOpinion = new Fyrefly_PersonalOpinion(this, target);
                personalOpinions[target] = newOpinion;
            }
            log($"Returning {personalOpinions[target]}");
            return personalOpinions[target];
        }

        public double AdjustPersonalOpinion(GameObject target, double value)
        {

            Fyrefly_PersonalOpinion opinion = GetPersonalOpinion(target);
            opinion.value += value;
            return opinion.value;
        }




        //Important Fields


        public string name;

        public Dictionary<GameObject, Fyrefly_PersonalOpinion> personalOpinions = new Dictionary<GameObject, Fyrefly_PersonalOpinion>();

        /// <summary>
        /// Determines the minimum random opinion added to all relationships
        /// </summary>
        public int minimumRandomOpinion = 0;
        /// <summary>
        /// Determines the maximum random opinion added to all relationships
        /// </summary>
        public int maximumRandomOpinion = 10;



        //Easy References

        private GameObject player => XRLCore.Core.Game.Player.Body;
        private Brain brain => ParentObject.pBrain;

        public void HandleBeginConversation(Event E)
        {
            log("Getting conversation partner");
            GameObject conversationPartner = E.GetGameObjectParameter("With");

            //log("Getting Conversation");
            Conversation conversation = E.GetParameter("Conversation") as Conversation;

            if (conversation == null) { logE("Conversation is null!"); return; }
            if (conversationPartner == null) { logE("ConversationPartner is null!"); return; }

            //log("Getting startNode");
            if (!conversation.NodesByID.TryGetValue("Start", out ConversationNode startnode))
            {
                logE("Exception uncovered!");
                logE("Conversation " + conversation.ID + " has no usable Start node");
                return;
            }


            //log("Getting DebugFeelingText");
            string debugOpinionText = $"Opinion Value: {GetPersonalOpinion(conversationPartner).value}\n";
            //log("Debug Opinion Text got");
            startnode.Text = debugOpinionText + "\n\n" + startnode.Text;

            MethodConversationNode AddOpinionNode = new PersonalOpinionConversationNode("Wow, I really do like you now!", "likeMeNode", 10);
            AddOpinionNode.AddChoice("To Main", "Start");
            AddOpinionNode.AddChoice("End Me Daddy", "End");
            conversation.AddNode(AddOpinionNode);
            MethodConversationNode SubtractOpinionNode = new PersonalOpinionConversationNode("Wow, I really do hate you now!", "hateMeNode", -10);
            SubtractOpinionNode.AddChoice("To Main", "Start");
            SubtractOpinionNode.AddChoice("End Me Daddy", "End");
            conversation.AddNode(SubtractOpinionNode);
            startnode.AddChoice("Hate me please", "hateMeNode");


        }

        public void HandleShowConversationChoices(Event E)
        {
            List<ConversationChoice> choices = E.GetParameter<List<ConversationChoice>>("Choices");
            ConversationNode firstNode = E.GetParameter<ConversationNode>("FirstNode");
            ConversationNode currentNode = E.GetParameter<ConversationNode>("CurrentNode");




        }



        public override void Register(GameObject Object)
        {
            Object.RegisterPartEvent(this, "BeginConversation");

            Object.RegisterPartEvent(this, "ShowConversationChoices");

            base.Register(Object);

            Debug.Log("Registering Dynamic Personality Part");
            if (ParentObject == null)
            {
                Debug.LogError("No parent object while creating DynamicPersonalityPart!");
                return;
            }
            if (ParentObject.pBrain == null)
            {
                Debug.LogError("Dynamic Personality is on an object with no brain, this aint supposed to happen!");
                return;
            }

            MessageQueue.AddPlayerMessage($"Added Dynamic Personality to {ParentObject.DisplayName}");

            return;

        }



        public override bool FireEvent(Event E)
        {
            if (E.ID == "BeginConversation")
            {
                try
                {
                    HandleBeginConversation(E);
                }
                catch (Exception ex)
                {
                    logE("Dynamic Personality Failed to Handle BeginConversation with exception: " + ex);
                };
            }
            else if (E.ID == "ShowConversationChoices") { HandleShowConversationChoices(E); }
            return true;
        }

    }

}