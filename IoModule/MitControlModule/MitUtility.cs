using IoModule.Root;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IoModule.MitControlModule
{
    public class MitUtility
    {
        private static MitUtility _instance;
        protected LogManager _logManager;
        private static object _lock = new object();
        private MitUtility()
        {
            _logManager = new LogManager();
        }
        public static MitUtility getInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new MitUtility();
                    }
                }
            }
            return _instance;
        }
        public int HexStringToInt(string hex) => Convert.ToInt32(hex, 16);
        public List<Int16> StringToASCII(string str)
        {
            List<Int16> ASCIIs = new List<Int16>(); //1個Int代表兩個Char
            try
            {
                bool odd = str.Length % 2 != 0 ? true : false;
                int index = 0;
                while (index != str.Length)
                {
                    int _StrNum = 2;
                    if (odd && index == str.Length - 1) _StrNum = 1;

                    ASCIIs.Add(StringHiLowReversal(str.Substring(index, _StrNum)));
                    index += _StrNum;

                }
            }
            catch (Exception)
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"String to ASCII failed.{str}");
            }
            return ASCIIs;
        }
        public int CalculateStringLength(string str) => (int)Math.Ceiling((double)str.Length / 2);
        public Int16 StringHiLowReversal(string str)
        {
            Int16 ASCIICode = 0;
            try
            {
                if (str.Length == 0 || str.Length > 2) throw new Exception("String length is not correct.");

                byte[] asciiBytes = Encoding.ASCII.GetBytes(new string(str.Reverse().ToArray()));
                string ASCIICodeStr = asciiBytes.Length == 1 ?
                    asciiBytes[0].ToString("X2") + "00" : asciiBytes[0].ToString("X2") + asciiBytes[1].ToString("X2");
                ASCIICode = Convert.ToInt16(ASCIICodeStr, 16);
                //I hope asciiCodeStr is always 4 digits and 4300 => 17152
            }
            catch (Exception)
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"StringHiLowReversal failed.{str}");
            }
            return ASCIICode;
        }
    }

}
