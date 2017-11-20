using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace CoreLibrary.Net.Sockets
{
    public class Executor
    {
        private Socket m_socWorker;

        private string _SzIP;
        private int _AlPort;

        public Executor(string szIP = "127.0.0.1",
            int alPort = 6100)
        {
            _SzIP = szIP;
            _AlPort = alPort;
        }

        public bool IsFileLocked(string filename)
        {
            FileInfo FI = new FileInfo(filename);
            FileStream stream = null;

            try
            {
                stream = FI.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                System.Console.WriteLine("locked");
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;

        }


        public void SendCommand(string command, string szIP = "127.0.0.1", int alPort = 6100)
        {
            // connect to a socket
            try
            {
                //create a new client socket ...
                m_socWorker = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                String szIPSelected = (szIP == "127.0.0.1") ? _SzIP : szIP;
                int alPortSelected = (alPort == 6100) ? _AlPort : alPort;

                IPAddress remoteIPAddress = IPAddress.Parse(szIPSelected);
                IPEndPoint remoteEndPoint = new IPEndPoint(remoteIPAddress, alPortSelected);
                m_socWorker.Connect(remoteEndPoint);
            }
            catch (SocketException se)
            {
                throw (se);
            }


            // send data
            try
            {
                //int test = 0;
                Object objData = command;
                char[] mychar = new char[1];
                mychar[0] = '\x0000';
                byte[] byData = System.Text.Encoding.UTF8.GetBytes(objData.ToString());
                byte[] terminator = System.Text.Encoding.UTF8.GetBytes(mychar);

                byte[] bytes = new byte[1024];
                m_socWorker.Send(byData);
                m_socWorker.Send(terminator);
                m_socWorker.ReceiveTimeout = 500000;
                m_socWorker.Receive(bytes);
            }
            catch (SocketException se)
            {
                throw (se);
            }

            m_socWorker.Close();

        }
    }
}
