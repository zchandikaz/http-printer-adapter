using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using CoreWCF;

namespace test_rider_app.api;



[MessageContract(WrapperName = "epos-print", WrapperNamespace = "http://www.epson-pos.com/schemas/2011/03/epos-print", IsWrapped = true)]
public class EposPrintRequest
{
    [MessageHeader(Name = "parameters")]
    public ParameterHeader Parameters { get; set; }
    
    [MessageBodyMember(Name = "image",  Namespace = "http://www.epson-pos.com/schemas/2011/03/epos-print")]
    public ImageElement Image { get; set; }

    [MessageBodyMember(Name = "cut")]
    public CutElement Cut { get; set; }
}

// [DataContract(Name = "image",Namespace = "http://www.epson-pos.com/schemas/2011/03/epos-print")]
public class ImageElement : IXmlSerializable
{
    [DataMember(Name = "width", Order = 1)]
    public int Width { get; set; }

    [DataMember(Name = "height", Order = 2)]
    public int Height { get; set; }

    [DataMember(Name = "align", Order = 3)]
    public string Align { get; set; }

    [DataMember(Order = 4, IsRequired = false, EmitDefaultValue = false)]
    public string Value { get; set; }

    public XmlSchema GetSchema() => null;

    public void ReadXml(XmlReader reader)
    {
        reader.MoveToContent();
            
        Width = int.Parse(reader.GetAttribute("width"));
        Height = int.Parse(reader.GetAttribute("height"));
        Align = reader.GetAttribute("align");

        // Move to the element content and read the inner text
        Value = reader.ReadElementContentAsString();
    }

    public void WriteXml(XmlWriter writer)
    {
        writer.WriteAttributeString("width", Width.ToString());
        writer.WriteAttributeString("height", Height.ToString());
        writer.WriteAttributeString("align", Align);
            
        if (!string.IsNullOrEmpty(Value))
        {
            writer.WriteString(Value);
        }
    }
}

[DataContract(Name = "cut")]
public class CutElement
{
    [DataMember(Name = "type")]
    public string Type { get; set; }
}

[MessageContract]
public class EposPrintResponse
{
    [MessageBodyMember(Name = "success")]
    public bool Success { get; set; }

    [MessageBodyMember(Name = "code")]
    public string Code { get; set; }

    [MessageBodyMember(Name = "status")]
    public string Status { get; set; }

    [MessageBodyMember(Name = "battery")]
    public int Battery { get; set; }
}

[DataContract(Name = "parameter")]
public class ParameterHeader
{
    [DataMember(Name = "devid")]
    public string DevId { get; set; }

    [DataMember(Name = "timeout")]
    public int Timeout { get; set; }

    [DataMember(Name = "printjobid")]
    public string PrintJobId { get; set; }
}


[ServiceContract(Namespace = "http://www.epson-pos.com/schemas/2011/03/epos-print" )]
public interface IEpsonTmService
{
    [OperationContract(Action = "*", ReplyAction = "*", Name = "epos-print")]
    EposPrintResponse EposPrint( EposPrintRequest request);
}
