namespace CoolPixControl
{
    public class PTPPacket
    {
        public List<byte> data = new List<byte>();

        public string hostName { get; set; }
        public int length { get; set; }
        public UInt16 operationCode {  get; set; }
        public int packetType {  get; set; }
        
        public virtual void buildBytes()
        {

        }




        public byte[] getData()
        {
            data.Clear();
            buildBytes();
            Logger.log("Sending Packet:\n");
            foreach (byte b in data)
            {
                Logger.appendLogToLine(b.ToString("X2") + " ");
            }
            return data.ToArray();
        }
    }
}