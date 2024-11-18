using System.Runtime.Serialization;

namespace test_rider_app.api;

[DataContract]
public class EpsonTmRequest
{
    [DataMember]
    public string Data { get; set; }
}