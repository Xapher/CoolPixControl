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
            Console.WriteLine("Sending Packet:");
            foreach (byte b in data)
            {
                Console.Write(b.ToString("X2") + " ");
            }
            Console.WriteLine();

            return data.ToArray();
        }
    }
}