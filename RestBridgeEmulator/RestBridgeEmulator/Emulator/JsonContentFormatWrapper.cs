namespace RestBridgeEmulator.Emulator
{
    using System.ServiceModel.Channels;

    public class JsonContentTypeMapper : WebContentTypeMapper
    {
        public override WebContentFormat
                   GetMessageFormatForContentType(string contentType)
        {
            return WebContentFormat.Json;
        }
    }
}
