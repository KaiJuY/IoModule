using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IoModule.Root;
using IoModule.MitControlModule;
using ActProgTypeLib;

namespace IOControlModule.MitControlModule
{
    /// <summary>
    /// This class is used to control the Mx control module.
    /// You should install the MxComponent and reference the 'ActProgTypeLib' from MELSEC.
    /// https://www.mitsubishielectric.com/dl/fa/software/update/plc/0000000239/sw4dnc-act-e_22y.zip
    /// </summary>
    public class MxControlModule : AMitControlModule
    {
        protected MitUtility _MitUtility = MitUtility.getInstance();
        protected ActProgTypeClass _ActMLProgTypeClass;
        protected Dictionary<string, object> _Perporty;
        protected bool _isConnected = false;
        private object _lockObj = new object();
        /// <summary>
        /// MxControlModule constructor
        /// PlcType : The type of the PLC. provided QCPU, SIM now
        /// ip : The ip address of the PLC
        /// port : The port of the PLC. Default is 5007 for PLC
        /// </summary>
        /// <param name="PlcType"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public MxControlModule(string PlcType, string ip, int port = 5007) : base()
        {
            _ActMLProgTypeClass = new ActProgTypeClass();
            _Perporty = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            SetPerporty("IP", new object[] { ip });
            SetPerporty("Port", new object[] { port });
            SetPerporty("PlcType", new object[] { PlcType });
        }
        protected override async void connectLogic()
        {
            _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(MxControlModule)} - {nameof(connectLogic)} : Start");
            try
            {
                if (_isConnected) throw new Exception("The connection is already established");
                if (!GetPerporty("IP", out object[] ip)) throw new Exception("The IP is not set");
                if (!GetPerporty("Port", out object[] port)) throw new Exception("The Port is not set");
                if (!GetPerporty("PlcType", out object[] plcType)) throw new Exception("The PlcType is not set");
                SetCpuType(plcType[0].ToString());
                _ActMLProgTypeClass.ActHostAddress = ip[0].ToString();
                _ActMLProgTypeClass.ActPortNumber = (int)port[0];
                if ((int)_ActMLProgTypeClass.Open() != 0) throw new Exception("The connection is failed");
                _isConnected = true;

            }
            catch (Exception ex)
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Error, $"{nameof(MxControlModule)} - {nameof(connectLogic)} Exception : {ex.Message}");
            }
            finally
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(MxControlModule)} - {nameof(connectLogic)} : End");
            }
        }
        protected override void disconnectLogic()
        {
            _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(MxControlModule)} - {nameof(disconnectLogic)} : Start");
            try
            {
                if (!_isConnected) throw new Exception("The connection is already disconnected");
                _ActMLProgTypeClass.Close();
            }
            catch (Exception ex)
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Error, $"{nameof(MxControlModule)} - {nameof(disconnectLogic)} Exception : {ex.Message}");
            }
            finally
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(MxControlModule)} - {nameof(disconnectLogic)} : End");
            }
        }
        protected override void reconnectLogic()
        {
            _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(MxControlModule)} - {nameof(reconnectLogic)} : Start");
            try
            {
                disconnectLogic();
                Thread.Sleep(1000);
                connectLogic();
            }
            catch (Exception ex)
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Error, $"{nameof(MxControlModule)} - {nameof(reconnectLogic)} Exception : {ex.Message}");
            }
            finally
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(MxControlModule)} - {nameof(reconnectLogic)} : End");
            }
        }
        protected override bool isConnectedLogic()
        {
            bool result = false;
            _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(MxControlModule)} - {nameof(isConnectedLogic)} : Start");
            try
            {
                result = this._isConnected;
            }
            catch (Exception ex)
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Error, $"{nameof(MxControlModule)} - {nameof(isConnectedLogic)} Exception : {ex.Message}");
            }
            finally
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(MxControlModule)} - {nameof(isConnectedLogic)} : End. Result is {result}");
            }
            return result;
        }
        protected override bool setPerportyLogic(string perportyName, object[] values)
        {
            bool result = false;
            _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(MxControlModule)} - {nameof(setPerportyLogic)} : Start. Args : {perportyName}, {values}");
            try
            {
                if (perportyName == "Port" && !(values[0] is int))
                {
                    _logManager.Trace(Serilog.Events.LogEventLevel.Error, $"{nameof(MxControlModule)} - {nameof(setPerportyLogic)} : The value is not this perporty expected type.");
                    return false;
                }
                if ((perportyName == "IP" || perportyName == "PlcType") && !(values[0] is string))
                {
                    _logManager.Trace(Serilog.Events.LogEventLevel.Error, $"{nameof(MxControlModule)} - {nameof(setPerportyLogic)} : The value is not this perporty expected type");
                    return false;
                }
                _Perporty[perportyName] = values[0];
                result = true;
            }
            catch (Exception ex)
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Error, $"{nameof(MxControlModule)} - {nameof(setPerportyLogic)} Exception : {ex.Message}");
            }
            finally
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(MxControlModule)} - {nameof(setPerportyLogic)} : End. Result is {result}");
            }
            return result;
        }
        protected override bool getPerportyLogic(string perportyName, out object[] values)
        {
            bool result = false;
            values = null;
            _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(MxControlModule)} - {nameof(getPerportyLogic)} : Start. Args : {perportyName}");
            try
            {
                if (!_Perporty.ContainsKey(perportyName))
                {
                    _logManager.Trace(Serilog.Events.LogEventLevel.Error, $"{nameof(MxControlModule)} - {nameof(setPerportyLogic)} : The perporty name is not exist.");
                    return false;
                }
                values = new object[] { _Perporty[perportyName] };
                result = true;
            }
            catch (Exception ex)
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Error, $"{nameof(MxControlModule)} - {nameof(getPerportyLogic)} Exception : {ex.Message}");
            }
            finally
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(MxControlModule)} - {nameof(getPerportyLogic)} : End. Result is {result}, Value is {values}");
            }
            return result;
        }
        protected override bool writeDataToPLCLogic(string device, string addr, Int16 data)
        {
            bool result = false;
            _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(MxControlModule)} - {nameof(writeDataToPLCLogic)}<{typeof(Int16)}> : Start. Args : {device}, {addr}, {data}");
            try
            {
                lock (_lockObj) result = (FunctionResult)_ActMLProgTypeClass.SetDevice2(PrepartDevice(device, addr), data) == FunctionResult.Success;
            }
            catch (Exception ex)
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Error, $"{nameof(MxControlModule)} - {nameof(writeDataToPLCLogic)}<{typeof(Int16)}> Exception : {ex.Message}");
            }
            finally
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(MxControlModule)} - {nameof(writeDataToPLCLogic)}<{typeof(Int16)}> : End. Result is {result}");
            }
            return result;
        }
        protected override bool writeDataToPLCLogic(string device, string addr, List<Int16> data)
        {
            bool result = false;
            _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(MxControlModule)} - {nameof(writeDataToPLCLogic)}<{typeof(List<Int16>)}> : Start. Args : {device}, {addr}, {data}");
            try
            {
                Int16[] wdata = data.ToArray();
                lock (_lockObj) result = (FunctionResult)_ActMLProgTypeClass.WriteDeviceBlock2(PrepartDevice(device, addr), wdata.Length, ref wdata[0]) == FunctionResult.Success;
            }
            catch (Exception ex)
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Error, $"{nameof(MxControlModule)} - {nameof(writeDataToPLCLogic)}<{typeof(List<Int16>)}> Exception : {ex.Message}");
            }
            finally
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(MxControlModule)} - {nameof(writeDataToPLCLogic)}<{typeof(List<Int16>)}> : End. Result is {result}");
            }
            return result;
        }
        protected override bool writeDataToPLCLogic(string device, string addr, string data)
        {
            bool result = false;
            _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(MxControlModule)} - {nameof(writeDataToPLCLogic)}<{typeof(string)}> : Start. Args : {device}, {addr}, {data}");
            try
            {
                int ws = _MitUtility.CalculateStringLength(data);
                if (ws <= 0) return false;
                Int16[] writedata = _MitUtility.StringToASCII(data).ToArray();
                if (writedata.Length <= 0 || writedata.Length != ws) return false; //it should be the same length, both of them are the data/2
                lock (_lockObj) result = (FunctionResult)_ActMLProgTypeClass.WriteDeviceBlock2(PrepartDevice(device, addr), writedata.Length, ref writedata[0]) == FunctionResult.Success;
            }
            catch (Exception ex)
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Error, $"{nameof(MxControlModule)} - {nameof(writeDataToPLCLogic)}<{typeof(string)}> Exception : {ex.Message}");
            }
            finally
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(MxControlModule)} - {nameof(writeDataToPLCLogic)}<{typeof(string)}> : End. Result is {result}");
            }
            return result;
        }
        protected override bool writeDataToPLCLogic(List<string> device, List<string> addr, List<Int16> data)
        {
            bool result = false;
            _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(MxControlModule)} - {nameof(writeDataToPLCLogic)}<{typeof(List<string>)}<{typeof(List<short>)}> : Start. Args : {device}, {addr}, {data}");
            try
            {
                if (device.Count != addr.Count || device.Count != data.Count) return false;
                string deviceCollection = string.Empty;
                for (int i = 0; i < device.Count; i++)
                {
                    //combine the device and addr also separate by \n
                    deviceCollection += PrepartDevice(device[i], addr[i]);
                    if (i == device.Count - 1) break; // the last one should not add \n
                    deviceCollection += "\n";
                }
                Int16[] wdata = data.ToArray();
                lock (_lockObj) result = (FunctionResult)_ActMLProgTypeClass.WriteDeviceRandom2(deviceCollection, wdata.Length, ref wdata[0]) == FunctionResult.Success;
            }
            catch (Exception ex)
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Error, $"{nameof(MxControlModule)} - {nameof(writeDataToPLCLogic)}<{typeof(List<string>)}<{typeof(List<short>)}> Exception : {ex.Message}");
            }
            finally
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(MxControlModule)} - {nameof(writeDataToPLCLogic)}<{typeof(List<string>)}<{typeof(List<short>)}> : End. Result is {result}");
            }
            return result;
        }
        protected override bool readDataFromPLCLogic(string device, string addr, out Int16 value)
        {
            bool result = false;
            value = 0;
            _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(MxControlModule)} - {nameof(readDataFromPLCLogic)}<{typeof(Int16)}> : Start. Args : {device}, {addr}");
            try
            {
                lock (_lockObj) result = (FunctionResult)_ActMLProgTypeClass.GetDevice2(PrepartDevice(device, addr), out value) == FunctionResult.Success;
            }
            catch (Exception ex)
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Error, $"{nameof(MxControlModule)} - {nameof(readDataFromPLCLogic)}<{typeof(Int16)}> Exception : {ex.Message}");
            }
            finally
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(MxControlModule)} - {nameof(readDataFromPLCLogic)}<{typeof(Int16)}> : End. Result is {result}, Value is {value}");
            }
            return result;
        }
        protected override bool readDataFromPLCLogic(string device, string addr, int wordlen, out List<Int16> value)
        {
            bool result = false;
            value = null;
            _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(MxControlModule)} - {nameof(readDataFromPLCLogic)}<{typeof(List<Int16>)}> : Start. Args : {device}, {addr}");
            try
            {
                Int16[] rdata = new Int16[wordlen];
                lock (_lockObj) result = (FunctionResult)_ActMLProgTypeClass.ReadDeviceBlock2(PrepartDevice(device, addr), wordlen, out rdata[0]) == FunctionResult.Success;
                if (result) value = rdata.ToList();
            }
            catch (Exception ex)
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Error, $"{nameof(MxControlModule)} - {nameof(readDataFromPLCLogic)}<{typeof(List<Int16>)}> Exception : {ex.Message}");
            }
            finally
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(MxControlModule)} - {nameof(readDataFromPLCLogic)}<{typeof(List<Int16>)}> : End. Result is {result}, Value is {value}");
            }
            return result;
        }
        protected override bool readDataFromPLCLogic(string device, string addr, int wordlen, out string value)
        {
            bool result = false;
            value = null;
            _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(MxControlModule)} - {nameof(readDataFromPLCLogic)}<{typeof(string)}> : Start. Args : {device}, {addr}, {wordlen}");
            try
            {
                if (wordlen <= 0) return false;
                Int16[] _data = new Int16[wordlen];
                lock (_lockObj) result = (FunctionResult)_ActMLProgTypeClass.ReadDeviceBlock2(PrepartDevice(device, addr), wordlen, out _data[0]) == FunctionResult.Success;
                if (!result) return false;
                string _value = string.Empty;
                for (int i = 0; i < wordlen; i++)
                {
                    string hexStr = _data[i].ToString("X4");
                    hexStr = hexStr.Substring(2, 2) + hexStr.Substring(0, 2);
                    if (hexStr.Contains("00")) hexStr = hexStr.Replace("00", "");
                    _value += hexStr;
                }
                byte[] bytes = Enumerable.Range(0, _value.Length / 2)
                         .Select(i => Convert.ToByte(_value.Substring(i * 2, 2), 16))
                         .ToArray();

                value = Encoding.ASCII.GetString(bytes);
            }
            catch (Exception ex)
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Error, $"{nameof(MxControlModule)} - {nameof(readDataFromPLCLogic)}<{typeof(string)}> Exception : {ex.Message}");
            }
            finally
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(MxControlModule)} - {nameof(readDataFromPLCLogic)}<{typeof(string)}> : End. Result is {result}, Value is {value}");
            }
            return result;
        }
        protected override bool readDataFromPLCLogic(List<string> device, List<string> addr, out List<short> value)
        {
            bool result = false;
            value = null;
            _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(MxControlModule)} - {nameof(readDataFromPLCLogic)}<{typeof(List<string>)}<{typeof(List<short>)}> : Start. Args : {device}, {addr}");
            try
            {
                if (device.Count != addr.Count) return false;
                Int16[] rdata = new Int16[device.Count];
                string deviceCollection = string.Empty;
                for (int i = 0; i < device.Count; i++)
                {
                    //combine the device and addr also separate by \n
                    deviceCollection += PrepartDevice(device[i], addr[i]);
                    if (i == device.Count - 1) break; // the last one should not add \n
                    deviceCollection += "\n";
                }
                lock (_lockObj) result = (FunctionResult)_ActMLProgTypeClass.WriteDeviceRandom2(deviceCollection, rdata.Length, ref rdata[0]) == FunctionResult.Success;
                if (result) value = rdata.ToList();
            }
            catch (Exception ex)
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Error, $"{nameof(MxControlModule)} - {nameof(readDataFromPLCLogic)}<{typeof(List<string>)}<{typeof(List<short>)}> Exception : {ex.Message}");
            }
            finally
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(MxControlModule)} - {nameof(readDataFromPLCLogic)}<{typeof(List<string>)}<{typeof(List<short>)}> : End. Result is {result}, Value is {value}");
            }
            return result;
        }
        protected override bool primaryHandshakeLogic(string Pdevice, string Paddr, string Sdevice, string Saddr, double Sec)
        {
            bool result = false;
            _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(MxControlModule)} - {nameof(primaryHandshakeLogic)} : Start. Args : {Pdevice}, {Paddr}, {Sdevice}, {Saddr}, {Sec}");
            try
            {
                if (!WriteDataToPLC(Pdevice, Paddr, (Int16)1)) throw new Exception("Write data to PLC failed in First Step.");
                if (!MonitorData(Sdevice, Saddr, TimeSpan.FromSeconds(Sec))) throw new Exception("Monitor data from PLC failed didn't set on");
                if (!WriteDataToPLC(Pdevice, Paddr, (Int16)0)) throw new Exception("Write data to PLC failed in Second Step.");
                result = true;
            }
            catch (Exception ex)
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Error, $"{nameof(MxControlModule)} - {nameof(primaryHandshakeLogic)} Exception : {ex.Message}");
            }
            finally
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(MxControlModule)} - {nameof(primaryHandshakeLogic)} : End. Result is  {result}");
            }
            return result;
        }
        protected override bool secondaryHandshakeLogic(string Pdevice, string Paddr, string Sdevice, string Saddr, double Sec)
        {
            bool result = false;
            _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(MxControlModule)} - {nameof(secondaryHandshakeLogic)} : Start. Args : {Pdevice}, {Paddr}, {Sdevice}, {Saddr}, {Sec}");
            try
            {
                Int16 currentValue = 0;
                if (!ReadDataFromPLC(Pdevice, Paddr, out currentValue) || currentValue == 0) throw new Exception("Read data from PLC failed or not set on.");
                if (!WriteDataToPLC(Sdevice, Saddr, (Int16)1)) throw new Exception("Write data to PLC failed in First Step.");
                if (!MonitorData(Pdevice, Paddr, TimeSpan.FromSeconds(Sec), false)) throw new Exception("Monitor data from PLC failed didn't set off");
                if (!WriteDataToPLC(Sdevice, Saddr, (Int16)0)) throw new Exception("Write data to PLC failed in Second Step.");
                result = true;
            }
            catch (Exception ex)
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Error, $"{nameof(MxControlModule)} - {nameof(secondaryHandshakeLogic)} Exception : {ex.Message}");
            }
            finally
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(MxControlModule)} - {nameof(secondaryHandshakeLogic)} : End. Result is  {result}");
            }
            return result;
        }
        protected override bool monitorDataLogic(string device, string addr, TimeSpan interval, bool rise = true)
        {
            bool result = false;
            _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(MxControlModule)} - {nameof(monitorDataLogic)} : Start. Args : {device}, {addr}, {interval}, {rise}");
            try
            {
                DateTime startTime = DateTime.Now;
                Int16 currentValue = 0, targetValue = rise ? (Int16)1 : (Int16)0;
                while (DateTime.Now - startTime < interval)
                {
                    if (!ReadDataFromPLC(device, addr, out currentValue)) throw new Exception("Read data from PLC failed.");
                    if (currentValue == targetValue) return true;
                    Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Error, $"{nameof(MxControlModule)} - {nameof(monitorDataLogic)} Exception : {ex.Message}");
            }
            finally
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(MxControlModule)} - {nameof(monitorDataLogic)} : End. Result is  {result}");
            }
            return result;
        }
        private void SetCpuType(string cpuType)
        {
            try
            {
                switch (cpuType)
                {
                    case "QCPU":
                        _ActMLProgTypeClass.ActUnitType = 0x2c;
                        _ActMLProgTypeClass.ActProtocolType = 0x05;
                        _ActMLProgTypeClass.ActCpuType = 0x90;
                        break;
                    case "SIM":
                        _ActMLProgTypeClass.ActUnitType = 0x30;
                        _ActMLProgTypeClass.ActProtocolType = 0x00;
                        _ActMLProgTypeClass.ActCpuType = 0x00;
                        break;
                    default:
                        throw new Exception($"The CPU type is not supported : {cpuType}");
                }
            }
            catch (Exception ex)
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Error, $"{nameof(MxControlModule)} - {nameof(monitorDataLogic)} Exception : {ex.Message}");
            }
        }
        private string PrepartDevice(string device, string addr) => $"{device}{addr}";
        public enum FunctionResult
        {
            Success
        }
    }
}
