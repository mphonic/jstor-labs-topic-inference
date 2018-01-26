using System;
using System.Runtime.Serialization;

namespace JstorLabs.DataContracts
{
    // Classes for returned JSON objects
    [DataContract(Name = "ExtractedText")]
    public class ExtractedText
    {
        [DataMember(Name = "text")]
        private string[] TextParts { get; set; }

        public string Text
        {
            get
            {
                if (this.TextParts.Length < 1)
                    return null;
                return String.Join(" ", this.TextParts);
            }
        }
    }

    [DataContract(Name = "Topic")]
    public class TopicWithWeight
    {
        [DataMember(Name = "weight")]
        public int Weight { get; set; }

        [DataMember(Name = "topic")]
        public string Topic { get; set; }
    }

    [DataContract(Name = "Topics")]
    public class InferredTopics
    {
        [DataMember(Name = "model")]
        public string Model { get; set; }

        [DataMember(Name = "topics")]
        public TopicWithWeight[] Topics { get; set; }

        [DataMember(Name = "id")]
        public string Id { get; set; }
    }
}
