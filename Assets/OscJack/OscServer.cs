using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace OscJack
{
    public class OscServer
    {
        Thread thread_;
        UdpClient udpClient_;
        IPEndPoint endPoint_;
        OscParser osc_;

        public bool IsRunning {
            get { return thread_ != null && thread_.IsAlive; }
        }

        public int MessageCount {
            get {
                lock (osc_) return osc_.MessageCount;
            }
        }

        public OscMessage PopMessage()
        {
            lock (osc_) return osc_.PopMessage();
        }

        public OscServer(int listenPort = 9000)
        {
            endPoint_ = new IPEndPoint(IPAddress.Any, listenPort);
            udpClient_ = new UdpClient(endPoint_);
            osc_ = new OscParser();
        }

        public void Start()
        {
            if (thread_ == null) {
                thread_ = new Thread(ServerLoop);
                thread_.Start();
            }
        }

        public void Close()
        {
            udpClient_.Close();
        }

        void ServerLoop()
        {
            try {
                while (true) {
                    var data = udpClient_.Receive(ref endPoint_);
                    lock (osc_) osc_.FeedData(data);
                }
            }
            catch (SocketException)
            {
            }
        }
    }
}
