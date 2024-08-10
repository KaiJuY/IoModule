using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IoModule.Root;
using Serilog.Events;

namespace IOControlModule.MitControlModule
{
    public abstract class AMitControlModule : IMitControlModule
    {
        protected LogManager _logManager;
        private bool DebugLog;
        public AMitControlModule(bool debugLog = true)
        {
            this.DebugLog = debugLog;
            _logManager = new LogManager();
        }
        #region Implement IMitControlModule
        public void Connect()
        {
            RecordLog($"{nameof(AMitControlModule)} - {nameof(Connect)} : Start");
            try
            {
                connectLogic();
            }
            catch (Exception ex)
            {
                RecordLog($"{nameof(AMitControlModule)} - {nameof(Connect)} : Exception : {ex}");
            }
            finally
            {
                RecordLog($"{nameof(AMitControlModule)} - {nameof(Connect)} : End");
            }
        }
        public void Disconnect()
        {
            RecordLog($"{nameof(AMitControlModule)} - {nameof(Disconnect)} : Start");
            try
            {
                disconnectLogic();
            }
            catch (Exception ex)
            {
                RecordLog($"{nameof(AMitControlModule)} - {nameof(Disconnect)} : Exception : {ex}");
            }
            finally
            {
                RecordLog($"{nameof(AMitControlModule)} - {nameof(Disconnect)} : End");
            }
        }
        public void ReConnect()
        {
            RecordLog($"{nameof(AMitControlModule)} - {nameof(ReConnect)} : Start");
            try
            {
                reconnectLogic();
            }
            catch (Exception ex)
            {
                RecordLog($"{nameof(AMitControlModule)} - {nameof(ReConnect)} : Exception : {ex}");
            }
            finally
            {
                RecordLog($"{nameof(AMitControlModule)} - {nameof(ReConnect)} : End");
            }
        }
        public bool IsConnected()
        {
            bool result = false;
            RecordLog($"{nameof(AMitControlModule)} - {nameof(IsConnected)} : Start");
            try
            {
                result = isConnectedLogic();
            }
            catch (Exception ex)
            {
                RecordLog($"{nameof(AMitControlModule)} - {nameof(IsConnected)} : Exception : {ex}");
            }
            finally
            {
                RecordLog($"{nameof(AMitControlModule)} - {nameof(IsConnected)} : End. Result is {result}");
            }
            return result;
        }
        public bool SetPerporty(string perportyName, object[] values)
        {
            bool result = false;
            RecordLog($"{nameof(AMitControlModule)} - {nameof(SetPerporty)} : Start. Args : {perportyName}, {values}");
            try
            {
                setPerportyLogic(perportyName, values);
            }
            catch (Exception ex)
            {
                RecordLog($"{nameof(AMitControlModule)} - {nameof(SetPerporty)} : Exception : {ex}");
            }
            finally
            {
                RecordLog($"{nameof(AMitControlModule)} - {nameof(SetPerporty)} : End. Result is {result}");
            }
            return result;
        }
        public bool GetPerporty(string perportyName, out object[] values)
        {
            bool result = false;
            values = null;
            RecordLog($"{nameof(AMitControlModule)} - {nameof(GetPerporty)} : Start. Args : {perportyName}");
            try
            {
                getPerportyLogic(perportyName, out values);
            }
            catch (Exception ex)
            {
                RecordLog($"{nameof(AMitControlModule)} - {nameof(GetPerporty)} : Exception : {ex}");
            }
            finally
            {
                RecordLog($"{nameof(AMitControlModule)} - {nameof(GetPerporty)} : End. Result is  {result}, Value is {values}");
            }
            return result;
        }
        public bool WriteDataToPLC(string device, string addr, Int16 data)
        {
            bool result = false;
            RecordLog($"{nameof(AMitControlModule)} - {nameof(WriteDataToPLC)}<{typeof(Int16)}> : Start. Args : {device}, {addr}, {data}");
            try
            {
                writeDataToPLCLogic(device, addr, data);
            }
            catch (Exception ex)
            {
                RecordLog($"{nameof(AMitControlModule)} - {nameof(WriteDataToPLC)}<{typeof(Int16)}> : Exception : {ex}");
            }
            finally
            {
                RecordLog($"{nameof(AMitControlModule)} - {nameof(WriteDataToPLC)}<{typeof(Int16)}> : End. Result is {result}");
            }
            return result;
        }
        public bool WriteDataToPLC(string device, string addr, List<Int16> data)
        {
            bool result = false;
            RecordLog($"{nameof(AMitControlModule)} - {nameof(WriteDataToPLC)}<{typeof(List<Int16>)}> : Start. Args : {device}, {addr}, {data}");
            try
            {
                writeDataToPLCLogic(device, addr, data);
            }
            catch (Exception ex)
            {
                RecordLog($"{nameof(AMitControlModule)} - {nameof(WriteDataToPLC)}<{typeof(List<Int16>)}> : Exception : {ex}");
            }
            finally
            {
                RecordLog($"{nameof(AMitControlModule)} - {nameof(WriteDataToPLC)}<{typeof(List<Int16>)}> : End. Result is {result}");
            }
            return result;
        }
        public bool WriteDataToPLC(string device, string addr, string data)
        {
            bool result = false;
            RecordLog($"{nameof(AMitControlModule)} - {nameof(WriteDataToPLC)}<{typeof(string)}> : Start. Args : {device}, {addr}, {data}");
            try
            {
                writeDataToPLCLogic(device, addr, data);
            }
            catch (Exception ex)
            {
                RecordLog($"{nameof(AMitControlModule)} - {nameof(WriteDataToPLC)}<{typeof(string)}> : Exception : {ex}");
            }
            finally
            {
                RecordLog($"{nameof(AMitControlModule)} - {nameof(WriteDataToPLC)}<{typeof(string)}> : End. Result is  {result}");
            }
            return result;
        }
        public bool WriteDataToPLC(List<string> device, List<string> addr, List<Int16> data)
        {
            bool result = false;
            RecordLog($"{nameof(AMitControlModule)} - {nameof(WriteDataToPLC)}<{typeof(List<string>)}> : Start. Args : {device}, {addr}, {data}");
            try
            {
                writeDataToPLCLogic(device, addr, data);
            }
            catch (Exception ex)
            {
                RecordLog($"{nameof(AMitControlModule)} - {nameof(WriteDataToPLC)}<{typeof(List<string>)}> : Exception : {ex}");
            }
            finally
            {
                RecordLog($"{nameof(AMitControlModule)} - {nameof(WriteDataToPLC)}<{typeof(List<string>)}> : End. Result is {result}");
            }
            return result;
        }
        public bool ReadDataFromPLC(string device, string addr, out Int16 value)
        {
            bool result = false;
            value = 0;
            RecordLog($"{nameof(AMitControlModule)} - {nameof(ReadDataFromPLC)}<{typeof(Int16)}> : Start. Args : {device}, {addr}");
            try
            {
                readDataFromPLCLogic(device, addr, out value);
            }
            catch (Exception ex)
            {
                RecordLog($"{nameof(AMitControlModule)} - {nameof(ReadDataFromPLC)}<{typeof(Int16)}> : Exception : {ex}");
            }
            finally
            {
                RecordLog($"{nameof(AMitControlModule)} - {nameof(ReadDataFromPLC)}<{typeof(Int16)}> : End. Result is {result}, Value is {value}");
            }
            return result;
        }
        public bool ReadDataFromPLC(string device, string addr, int wordlen, out List<Int16> value)
        {
            bool result = false;
            value = null;
            RecordLog($"{nameof(AMitControlModule)} - {nameof(ReadDataFromPLC)}<{typeof(List<Int16>)}> : Start. Args : {device}, {addr}, {wordlen}");
            try
            {
                readDataFromPLCLogic(device, addr, wordlen, out value);
            }
            catch (Exception ex)
            {
                RecordLog($"{nameof(AMitControlModule)} - {nameof(ReadDataFromPLC)}<{typeof(List<Int16>)}> : Exception : {ex}");
            }
            finally
            {
                RecordLog($"{nameof(AMitControlModule)} - {nameof(ReadDataFromPLC)}<{typeof(List<Int16>)}> : End. Result is  {result}, Value is {value}");
            }
            return result;
        }
        public bool ReadDataFromPLC(string device, string addr, int wordlen, out string value)
        {
            bool result = false;
            value = string.Empty;
            RecordLog($"{nameof(AMitControlModule)} - {nameof(ReadDataFromPLC)}<{typeof(string)}> : Start. Args : {device}, {addr}, {wordlen}");
            try
            {
                readDataFromPLCLogic(device, addr, wordlen, out value);
            }
            catch (Exception ex)
            {
                RecordLog($"{nameof(AMitControlModule)} - {nameof(ReadDataFromPLC)}<{typeof(string)}> : Exception : {ex}");
            }
            finally
            {
                RecordLog($"{nameof(AMitControlModule)} - {nameof(ReadDataFromPLC)}<{typeof(string)}> : End. Result is  {result}, Value is {value}");
            }
            return result;
        }
        public bool ReadDataFromPLC(List<string> device, List<string> addr, out List<Int16> value)
        {
            bool result = false;
            value = null;
            RecordLog($"{nameof(AMitControlModule)} - {nameof(ReadDataFromPLC)}<{typeof(string)}> : Start. Args : {device}, {addr}");
            try
            {
                readDataFromPLCLogic(device, addr, out value);
            }
            catch (Exception ex)
            {
                RecordLog($"{nameof(AMitControlModule)} - {nameof(ReadDataFromPLC)}<{typeof(string)}> : Exception : {ex}");
            }
            finally
            {
                RecordLog($"{nameof(AMitControlModule)} - {nameof(ReadDataFromPLC)}<{typeof(string)}> : End. Result is  {result}, Value is {value}");
            }
            return result;
        }
        public bool PrimaryHandshake(string Pdevice, string Paddr, string Sdevice, string Saddr, double Sec = 5)
        {
            bool result = false;
            RecordLog($"{nameof(AMitControlModule)} - {nameof(PrimaryHandshake)} : Start. Args : {Pdevice}, {Paddr}, {Sdevice}, {Saddr}, {Sec}");
            try
            {
                primaryHandshakeLogic(Pdevice, Paddr, Sdevice, Saddr, Sec);
            }
            catch (Exception ex)
            {
                RecordLog($"{nameof(AMitControlModule)} - {nameof(PrimaryHandshake)} : Exception : {ex}");
            }
            finally
            {
                RecordLog($"{nameof(AMitControlModule)} - {nameof(PrimaryHandshake)} : End. Result is {result}");
            }
            return result;
        }
        public bool SecondaryHandshake(string Pdevice, string Paddr, string Sdevice, string Saddr, double Sec = 5)
        {
            bool result = false;
            RecordLog($"{nameof(AMitControlModule)} - {nameof(SecondaryHandshake)} : Start. Args : {Pdevice}, {Paddr}, {Sdevice}, {Saddr}, {Sec}");
            try
            {
                secondaryHandshakeLogic(Pdevice, Paddr, Sdevice, Saddr, Sec);
            }
            catch (Exception ex)
            {
                RecordLog($"{nameof(AMitControlModule)} - {nameof(SecondaryHandshake)} : Exception : {ex}");
            }
            finally
            {
                RecordLog($"{nameof(AMitControlModule)} - {nameof(SecondaryHandshake)} : End. Result is {result}");
            }
            return result;
        }
        public bool MonitorData(string device, string addr, TimeSpan interval, bool rise = true)
        {
            bool result = false;
            RecordLog($"{nameof(AMitControlModule)} - {nameof(MonitorData)} : Start. Args : {device}, {addr}, {interval}, {rise}");
            try
            {
                monitorDataLogic(device, addr, interval, rise);
            }
            catch (Exception ex)
            {
                RecordLog($"{nameof(AMitControlModule)} - {nameof(MonitorData)} : Exception : {ex}");
            }
            finally
            {
                RecordLog($"{nameof(AMitControlModule)} - {nameof(MonitorData)} : End. Result is  {result}");
            }
            return result;
        }
        #endregion
        #region Real Logic
        protected abstract void connectLogic();
        protected abstract void disconnectLogic();
        protected abstract void reconnectLogic();
        protected abstract bool isConnectedLogic();
        protected abstract bool setPerportyLogic(string perportyName, object[] values);
        protected abstract bool getPerportyLogic(string perportyName, out object[] values);
        protected abstract bool writeDataToPLCLogic(string device, string addr, Int16 data);
        protected abstract bool writeDataToPLCLogic(string device, string addr, List<Int16> data);
        protected abstract bool writeDataToPLCLogic(string device, string addr, string data);
        protected abstract bool writeDataToPLCLogic(List<string> device, List<string> addr, List<Int16> data);
        protected abstract bool readDataFromPLCLogic(string device, string addr, out Int16 value);
        protected abstract bool readDataFromPLCLogic(string device, string addr, int wordlen, out List<Int16> value);
        protected abstract bool readDataFromPLCLogic(string device, string addr, int wordlen, out string value);
        protected abstract bool readDataFromPLCLogic(List<string> device, List<string> addr, out List<Int16> value);
        protected abstract bool primaryHandshakeLogic(string Pdevice, string Paddr, string Sdevice, string Saddr, double Sec);
        protected abstract bool secondaryHandshakeLogic(string Pdevice, string Paddr, string Sdevice, string Saddr, double Sec);
        protected abstract bool monitorDataLogic(string device, string addr, TimeSpan interval, bool rise = true);
        #endregion
        protected void RecordLog(string message)
        {
            if (!DebugLog) return;
            _logManager.Trace(LogEventLevel.Debug, message);
        }
    }
}
