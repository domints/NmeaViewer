using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO.Ports;
using Serilog;
using System.Text;
using Newtonsoft.Json;

namespace NmeaViewer
{
    public class NmeaReceiver
    {
        public delegate void SentenceReceivedHandler(NmeaSentence sentence);
        public event SentenceReceivedHandler? SentenceReceived;
        static NmeaReceiver _instance = new();
        public static NmeaReceiver Instance => _instance;
        SerialPort? _port;

        private string _buffer = string.Empty;
        public bool OpenPort(string port, int baudRate)
        {
            if (_port?.IsOpen ?? false)
                return false;

            _port = new(port, baudRate);
            _port.Open();
            try
            {
                _port.ReadTimeout = 2000;
                var foundSentence = false;
                for (int i = 0; i < 3; i++)
                {
                    var sentence = _port.ReadLine();
                    if (NmeaSentence.FromString(sentence).IsCorrect)
                    {
                        foundSentence = true;
                        break;
                    }
                }

                if (!foundSentence)
                {
                    _port.Dispose();
                    _port = null;
                    return false;
                }

                _port.DataReceived += NmeaDataReceived;
            }
            catch (TimeoutException)
            {
                Log.Debug("NMEA Connect timeout occurred");
                _port?.Dispose();
                _port = null;
                return false;
            }
            catch (Exception ex)
            {
                _port?.Dispose();
                _port = null;
                Log.Error(ex, "Something weird happened during connection");
            }

            return true;
        }

        public void NmeaDataReceived(object? sender, SerialDataReceivedEventArgs args)
        {
            if (args.EventType == SerialData.Chars)
            {
                var toRead = _port!.BytesToRead;
                var bfr = new byte[toRead];
                _port.Read(bfr, 0, toRead);
                _buffer += Encoding.ASCII.GetString(bfr);
                var newline = _buffer.IndexOf("\r\n");
                if (newline == -1)
                {
                    return;
                }

                var sentence = _buffer.Substring(0, newline);
                Log.Debug("Found message: {msg}", sentence);
                _buffer = _buffer.Substring(newline + 2);

                NmeaSentence parsed;
                try
                {
                    parsed = NmeaSentence.FromString(sentence);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Failed to parse NMEA sentence.");
                    return;
                }

                if (SentenceReceived != null)
                {
                    Log.Information("Sending message to consumers: {sentence}", JsonConvert.SerializeObject(parsed));
                    SentenceReceived.Invoke(parsed);
                }
            }
        }
    }
}