using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOControlModule
{
    public interface IMitControlModule : IControlModule
    {
        /// <summary>
        /// According the setting establish the connection
        /// </summary>
        void Connect();
        /// <summary>
        /// Disconnect the connection
        /// </summary>
        void Disconnect();
        /// <summary>
        /// Reconnect the connection
        /// </summary>
        void ReConnect();
        /// <summary>
        /// Check the connection status
        /// </summary>
        /// <returns></returns>
        bool IsConnected();
        /// <summary>
        /// Set the perporty of the Module
        /// return true if the setting is successful
        /// otherwise return false
        /// </summary>
        /// <param name="perportyName"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        bool SetPerporty(string perportyName, object[] values);
        /// <summary>
        /// Get the perporty of the Module
        /// return true if the getting is successful
        /// othewise return false
        /// </summary>
        /// <param name="perportyName"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        bool GetPerporty(string perportyName, out object[] values);
        /// <summary>
        /// Write Int16 data to the PLC
        /// return true if the writing is successful
        /// otherwise return false
        /// </summary>
        /// <param name="device"></param>
        /// <param name="addr"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        bool WriteDataToPLC(string device, string addr, Int16 data);
        /// <summary>
        /// Write List<Int16> data to the PLC
        /// return true if the writing is successful
        /// otherwise return false
        /// </summary>
        /// <param name="device"></param>
        /// <param name="addr"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        bool WriteDataToPLC(string device, string addr, List<Int16> data);
        /// <summary>
        /// Write string data to the PLC
        /// return true if the writing is successful
        /// otherwise return false
        /// </summary>
        /// <param name="device"></param>
        /// <param name="addr"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        bool WriteDataToPLC(string device, string addr, string data);
        /// <summary>
        /// Write List<Int16> data to the PLC, according the device and addr
        /// The device and addr should be the same length as the data,
        /// which means the device[n] and addr[n] is the address of data[n]
        /// this funcction will write the data[n] to the address of device[n] and addr[n]
        /// return true if the writing is successful
        /// otherwise return false, any one of the writing is failed, the function will return false
        /// </summary>
        /// <param name="device"></param>
        /// <param name="addr"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        bool WriteDataToPLC(List<string> device, List<string> addr, List<Int16> data);
        /// <summary>
        /// Read Int16 data from the PLC
        /// return true if the reading is successful
        /// otherwise return false
        /// </summary>
        /// <param name="device"></param>
        /// <param name="addr"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool ReadDataFromPLC(string device, string addr, out Int16 value);
        /// <summary>
        /// Read List<Int16> data from the PLC
        /// return true if the reading is successful
        /// otherwise return false
        /// </summary>
        /// <param name="device"></param>
        /// <param name="addr"></param>
        /// <param name="wordlen"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool ReadDataFromPLC(string device, string addr, int wordlen, out List<Int16> value);
        /// <summary>
        /// Read string data from the PLC
        /// return true if the reading is successful
        /// otherwise return false
        /// </summary>
        /// <param name="device"></param>
        /// <param name="addr"></param>
        /// <param name="wordlen"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool ReadDataFromPLC(string device, string addr, int wordlen, out string value);
        /// <summary>
        /// Read List<Int16> data from the PLC, according the device and addr
        /// The device and addr should be the same length as the data,
        /// which means the device[n] and addr[n] is the address of data[n]
        /// this funcction will read the data[n] from the address of device[n] and addr[n]
        /// return true if the reading is successful
        /// otherwise return false, any one of the reading is failed, the function will return false
        /// </summary>
        /// <param name="device"></param>
        /// <param name="addr"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        bool ReadDataFromPLC(List<string> device, List<string> addr, out List<Int16> data);
        /// <summary>
        /// Do the primary handshake, 
        /// we are the active in the handshake.
        /// return true if the handshake is successful
        /// otherwise return false
        /// </summary>
        /// <param name="Pdevice"></param>
        /// <param name="Paddr"></param>
        /// <param name="Sdevice"></param>
        /// <param name="Saddr"></param>
        /// <returns></returns>
        bool PrimaryHandshake(string Pdevice, string Paddr, string Sdevice, string Saddr, double Sec);
        /// <summary>
        /// Do the secondary handshake,
        /// we are the passive in the handshake.
        /// return true if the handshake is successful
        /// otherwise return false
        /// </summary>
        /// <param name="Pdevice"></param>
        /// <param name="Paddr"></param>
        /// <param name="Sdevice"></param>
        /// <param name="Saddr"></param>
        /// <returns></returns>
        bool SecondaryHandshake(string Pdevice, string Paddr, string Sdevice, string Saddr, double Sec);
        /// <summary>
        /// Monitor the data from the PLC
        /// return true if the monitoring rise from this address and is change in the interval
        /// otherwise return false
        /// </summary>
        /// <param name="device"></param>
        /// <param name="addr"></param>
        /// <param name="interval"></param>
        /// <param name="rise"></param>
        /// <returns></returns>
        bool MonitorData(string device, string addr, TimeSpan interval, bool rise = true);
    }
}
