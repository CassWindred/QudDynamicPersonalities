
using System;
using System.Collections.Generic;
using UnityEngine;
using XRL.Core;
using XRL.Messages;
using XRL.World.AI;

namespace XRL.World.Parts
{
    class Fyrefly_DynamicPersonality : IPart
    {
        public static void logE(string text) => Debug.LogError(text);
        public static void log(string text) => Debug.Log(text);

        public class Fyrefly_DynamicRelationship : ObjectOpinion
        {
            public Fyrefly_DynamicRelationship(int Feeling) : base(Feeling)
            {
            }

            public Fyrefly_DynamicRelationship(ObjectOpinion src) : base(src)
            {
            }

        }

        public class MethodConversationNode : ConversationNode
        {

            public delegate void VisitMethod(GameObject speaker, GameObject listener);
            public VisitMethod OnVisit;
            public MethodConversationNode(string text, string id, VisitMethod onVisit)
            {

                Text = text;
                ID = id;
                OnVisit = onVisit;

            }

            public MethodConversationNode() { }

            public override void Visit(GameObject speaker, GameObject listener)
            {
                try
                {
                    OnVisit(speaker, listener);
                }
                catch (Exception ex)
                {
                    //$@ is an interpolated string and a verbatim string (it works with multiple lines)
                    logE($@"{(ex is NullReferenceException ? "Null Reference Exception" : "Unknown Exception")} in Method Conversation Node 
                    ID: {ID}
                    OnVisit is {(OnVisit != null ? "not" : "")} null
                    Exception: {ex}");
                    base.Visit(speaker, listener);
                }
            }
        }

        public class FeelingConversationNode : MethodConversationNode
        {
            public FeelingConversationNode(string text, string id, int feelingAdjust)
            {
                Text = text;
                ID = id;
                OnVisit = (GameObject speaker, GameObject listener) =>
                {
                    log($"Adjusting {speaker.DebugName}'s feeling of {listener.DebugName} by {feelingAdjust}");
                    speaker.pBrain.AdjustFeeling(listener, feelingAdjust);
                };
            }
        }

        public Dictionary<GameObject, Fyrefly_DynamicRelationship> relationships;

        public string name;

        private GameObject player = XRLCore.Core.Game.Player.Body;
        private Brain brain => ParentObject.pBrain;

        public void HandleBeginConversation(Event E)
        {
            log("Getting partner");
            GameObject conversationPartner = E.GetGameObjectParameter("With");

            log("Getting Conversation");
            Conversation conversation = E.GetParameter("Conversation") as Conversation;

            if (conversation == null) { logE("Conversation is null!"); return; }
            if (conversationPartner == null) { logE("ConversationPartner is null!"); return; }

            log("Getting startNode");
            if (!conversation.NodesByID.TryGetValue("Start", out ConversationNode startnode))
            {
                logE("Exception uncovered!");
                logE("Conversation " + conversation.ID + " has no usable Start node");
                return;
            }


            log("Getting DebugFeelingText");
            string debugFeelingText = $"Feeling Value: {brain.GetFeeling(conversationPartner)}\n";
            log("Debug Feeling Text got");
            conversation.Introduction = string.IsNullOrEmpty(conversation.Introduction) ? debugFeelingText : "\n" + debugFeelingText;

            MethodConversationNode AddOpinionNode = new FeelingConversationNode("Wow, I really do like you now!", "likeMeNode", 50);
            AddOpinionNode.AddChoice("To Main", "Start");
            AddOpinionNode.AddChoice("End Me Daddy", "End");
            conversation.AddNode(AddOpinionNode);
            startnode.AddChoice("Like me please", "likeMeNode");

        }

        public void HandleShowConversationChoices(Event E)
        {

        }

        public override bool HandleEvent(ObjectCreatedEvent E)
        {
            Debug.Log("Creating Dynamic Personality Part");
            if (ParentObject == null)
            {
                Debug.LogError("No parent object while creating DynamicPersonalityPart!");
                return false;
            }
            if (ParentObject.pBrain == null)
            {
                Debug.LogError("Dynamic Personality is on an object with no brain, this aint supposed to happen!");
                return false;
            }

            MessageQueue.AddPlayerMessage($"Added Dynamic Personality to {ParentObject.DisplayName}");

            return true;
        }

        public override void Register(GameObject Object)
        {
            Object.RegisterPartEvent(this, "BeginConversation");

            Object.RegisterPartEvent(this, "ShowConversationChoices");

            base.Register(Object);
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