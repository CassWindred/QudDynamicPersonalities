using System;

using XRL.World;
using XRL.World.Parts;

namespace Fyrefly.UtilityConversationNodes
{
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
                UnityEngine.Debug.LogError($@"{(ex is NullReferenceException ? "Null Reference Exception" : "Unknown Exception")} in Method Conversation Node 
                    ID: {ID}
                    OnVisit is {(OnVisit != null ? "not" : "")} null
                    Exception: {ex}");
                base.Visit(speaker, listener);
            }
        }
    }

    public class PersonalOpinionConversationNode : MethodConversationNode
    {
        public PersonalOpinionConversationNode(string text, string id, int feelingAdjust)
        {
            Text = text;
            ID = id;
            OnVisit = (GameObject speaker, GameObject listener) =>
            {
                UnityEngine.Debug.Log($"Adjusting {speaker.DebugName}'s personal opinion of {listener.DebugName} by {feelingAdjust}");
                DynamicPersonality personality = speaker.GetPart<DynamicPersonality>();
                personality.AdjustPersonalOpinion(listener, feelingAdjust);
            };
        }
    }
}
