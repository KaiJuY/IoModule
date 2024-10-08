using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IoModule.MitControlModule;
using IoModule.Root;
using MCProtocol;
using static MCProtocol.Mitsubishi;

namespace IOControlModule.MitControlModule
{
    /// <summary>
    /// McProtocol Control Module
    /// Fixed using TCP/IP to establish the connection
    /// Fixed using MC3E
    /// </summary>
    public class McControlModule : AMitControlModule
    {
        protected MitUtility _MitUtility = MitUtility.getInstance();
        protected Dictionary<string, object> _Perporty;
        private object _lockObj = new object();
        public McControlModule(string ip, int port) : base()
        {
            _Perporty = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            SetPerporty("IP", new object[] { ip });
            SetPerporty("Port", new object[] { port });
            SetPerporty("Transmission", new object[] { "Tcp" });
            SetPerporty("McFrame", new object[] { "MC3E" });
        }
        protected override async void connectLogic()
        {
            _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(McControlModule)} - {nameof(connectLogic)} : Start");
            try
            {
                PLCData.PLC = new McProtocolTcp(_Perporty["IP"].ToString(), (int)_Perporty["Port"], Mitsubishi.McFrame.MC3E);
                await PLCData.PLC.Open();
            }
            catch (Exception ex)
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Error, $"{nameof(McControlModule)} - {nameof(connectLogic)} Exception : {ex.Message}");
            }
            finally
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(McControlModule)} - {nameof(connectLogic)} : End");
            }
        }
        protected override void disconnectLogic()
        {
            _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(McControlModule)} - {nameof(disconnectLogic)} : Start");
            try
            {
                PLCData.PLC.Close();
            }
            catch (Exception ex)
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Error, $"{nameof(McControlModule)} - {nameof(disconnectLogic)} Exception : {ex.Message}");
            }
            finally
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(McControlModule)} - {nameof(disconnectLogic)} : End");
            }
        }
        protected override void reconnectLogic()
        {
            _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(McControlModule)} - {nameof(reconnectLogic)} : Start");
            try
            {
                disconnectLogic();
                Thread.Sleep(1000);
                connectLogic();
            }
            catch (Exception ex)
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Error, $"{nameof(McControlModule)} - {nameof(reconnectLogic)} Exception : {ex.Message}");
            }
            finally
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(McControlModule)} - {nameof(reconnectLogic)} : End");
            }
        }
        protected override bool isConnectedLogic()
        {
            bool result = false;
            _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(McControlModule)} - {nameof(isConnectedLogic)} : Start");
            try
            {
                result = PLCData.PLC?.Connected ?? false;
            }
            catch (Exception ex)
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Error, $"{nameof(McControlModule)} - {nameof(isConnectedLogic)} Exception : {ex.Message}");
            }
            finally
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(McControlModule)} - {nameof(isConnectedLogic)} : End. Result is {result}");
            }
            return result;
        }
        protected override bool setPerportyLogic(string perportyName, object[] values)
        {
            bool result = false;
            _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(McControlModule)} - {nameof(setPerportyLogic)} : Start. Args : {perportyName}, {values}");
            try
            {
                if (perportyName == "Port" && !(values[0] is int))
                {
                    _logManager.Trace(Serilog.Events.LogEventLevel.Error, $"{nameof(McControlModule)} - {nameof(setPerportyLogic)} : The value is not this perporty expected type.");
                    return false;
                }
                if ((perportyName == "IP" || perportyName == "Transmission" || perportyName == "McFrame") && !(values[0] is string))
                {
                    _logManager.Trace(Serilog.Events.LogEventLevel.Error, $"{nameof(McControlModule)} - {nameof(setPerportyLogic)} : The value is not this perporty expected type");
                    return false;
                }
                _Perporty[perportyName] = values[0];
                result = true;
            }
            catch (Exception ex)
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Error, $"{nameof(McControlModule)} - {nameof(setPerportyLogic)} Exception : {ex.Message}");
            }
            finally
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(McControlModule)} - {nameof(setPerportyLogic)} : End. Result is {result}");
            }
            return result;
        }
        protected override bool getPerportyLogic(string perportyName, out object[] values)
        {
            bool result = false;
            values = null;
            _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(McControlModule)} - {nameof(getPerportyLogic)} : Start. Args : {perportyName}");
            try
            {
                if (!_Perporty.ContainsKey(perportyName))
                {
                    _logManager.Trace(Serilog.Events.LogEventLevel.Error, $"{nameof(McControlModule)} - {nameof(setPerportyLogic)} : The perporty name is not exist.");
                    return false;
                }
                values = new object[] { _Perporty[perportyName] };
                result = true;
            }
            catch (Exception ex)
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Error, $"{nameof(McControlModule)} - {nameof(getPerportyLogic)} Exception : {ex.Message}");
            }
            finally
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(McControlModule)} - {nameof(getPerportyLogic)} : End. Result is {result}, Value is {values}");
            }
            return result;
        }
        protected override bool writeDataToPLCLogic(string device, string addr, Int16 data)
        {
            bool result = false;
            _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(McControlModule)} - {nameof(writeDataToPLCLogic)}<{typeof(Int16)}> : Start. Args : {device}, {addr}, {data}");
            try
            {
                Mitsubishi.PlcDeviceType plcDevice = GetPlcDeviceType(device);
                int plcAddress = GetAddress(plcDevice, addr);
                PLCData<Int16> _data = new PLCData<Int16>(plcDevice, plcAddress, 1); // Add on 5-31
                _data[0] = data;
                lock (_lockObj) _data.WriteData();
                result = true;
            }
            catch (Exception ex)
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Error, $"{nameof(McControlModule)} - {nameof(writeDataToPLCLogic)}<{typeof(Int16)}> Exception : {ex.Message}");
            }
            finally
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(McControlModule)} - {nameof(writeDataToPLCLogic)}<{typeof(Int16)}> : End. Result is {result}");
            }
            return result;
        }
        protected override bool writeDataToPLCLogic(string device, string addr, List<Int16> data)
        {
            bool result = false;
            _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(McControlModule)} - {nameof(writeDataToPLCLogic)}<{typeof(List<Int16>)}> : Start. Args : {device}, {addr}, {data}");
            try
            {
                if (data.Count <= 0) return false;
                Mitsubishi.PlcDeviceType plcDevice = GetPlcDeviceType(device);
                int plcAddress = GetAddress(plcDevice, addr);
                PLCData<Int16> _data = new PLCData<Int16>(plcDevice, plcAddress, data.Count);// Add on 5-31
                for (int i = 0; i < data.Count; i++)
                {
                    _data[i] = data[i];
                }
                lock (_lockObj) _data.WriteData();
                result = true;
            }
            catch (Exception ex)
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Error, $"{nameof(McControlModule)} - {nameof(writeDataToPLCLogic)}<{typeof(List<Int16>)}> Exception : {ex.Message}");
            }
            finally
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(McControlModule)} - {nameof(writeDataToPLCLogic)}<{typeof(List<Int16>)}> : End. Result is {result}");
            }
            return result;
        }
        protected override bool writeDataToPLCLogic(string device, string addr, string data)
        {
            bool result = false;
            _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(McControlModule)} - {nameof(writeDataToPLCLogic)}<{typeof(string)}> : Start. Args : {device}, {addr}, {data}");
            try
            {
                int ws = _MitUtility.CalculateStringLength(data);
                if (ws <= 0) return false;
                List<Int16> writedata = _MitUtility.StringToASCII(data);
                if (writedata.Count <= 0 || writedata.Count != ws) return false;
                Mitsubishi.PlcDeviceType plcDevice = GetPlcDeviceType(device);
                int plcAddress = GetAddress(plcDevice, addr);
                PLCData<Int16> _data = new PLCData<Int16>(plcDevice, plcAddress, ws);// Add on 5-31
                for (int i = 0; i < ws; i++)
                {
                    _data[i] = writedata[i];
                }
                lock (_lockObj) _data.WriteData();
                result = true;
            }
            catch (Exception ex)
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Error, $"{nameof(McControlModule)} - {nameof(writeDataToPLCLogic)}<{typeof(string)}> Exception : {ex.Message}");
            }
            finally
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(McControlModule)} - {nameof(writeDataToPLCLogic)}<{typeof(string)}> : End. Result is {result}");
            }
            return result;
        }
        protected override bool writeDataToPLCLogic(List<string> device, List<string> addr, List<short> data)
        {
            bool result = false;
            _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(McControlModule)} - {nameof(writeDataToPLCLogic)}<{typeof(List<Int16>)}> : Start. Args : {device}, {addr}, {data}");
            try
            {
                if (device.Count <= 0 || addr.Count <= 0 || data.Count <= 0) return false;
                if (device.Count != addr.Count || device.Count != data.Count) return false;
                for (int i = 0; i < device.Count; i++)
                {
                    if (!writeDataToPLCLogic(device[i], addr[i], data[i])) return false;
                }
                result = true;
            }
            catch (Exception ex)
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Error, $"{nameof(McControlModule)} - {nameof(writeDataToPLCLogic)}<{typeof(List<Int16>)}> Exception : {ex.Message}");
            }
            finally
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(McControlModule)} - {nameof(writeDataToPLCLogic)}<{typeof(List<Int16>)}> : End. Result is {result}");
            }
            return result;
        }
        protected override bool readDataFromPLCLogic(string device, string addr, out Int16 value)
        {
            bool result = false;
            value = 0;
            _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(McControlModule)} - {nameof(readDataFromPLCLogic)}<{typeof(Int16)}> : Start. Args : {device}, {addr}");
            try
            {
                Mitsubishi.PlcDeviceType plcDevice = GetPlcDeviceType(device);
                int plcAddress = GetAddress(plcDevice, addr);
                PLCData<Int16> _data = new PLCData<Int16>(plcDevice, plcAddress, 1);// Add on 5-31
                //lock (_lockObj) _data.ReadData();
                _data.ReadData();
                value = _data[0];
                result = true;
            }
            catch (Exception ex)
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Error, $"{nameof(McControlModule)} - {nameof(readDataFromPLCLogic)}<{typeof(Int16)}> Exception : {ex.Message}");
            }
            finally
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(McControlModule)} - {nameof(readDataFromPLCLogic)}<{typeof(Int16)}> : End. Result is {result}, Value is {value}");
            }
            return result;
        }
        protected override bool readDataFromPLCLogic(string device, string addr, int wordlen, out List<Int16> value)
        {
            bool result = false;
            value = null;
            _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(McControlModule)} - {nameof(readDataFromPLCLogic)}<{typeof(List<Int16>)}> : Start. Args : {device}, {addr}");
            try
            {
                if (wordlen <= 0) return false;
                value = new List<Int16>(wordlen);
                Mitsubishi.PlcDeviceType plcDevice = GetPlcDeviceType(device);
                int plcAddress = GetAddress(plcDevice, addr);
                PLCData<Int16> _data = new PLCData<Int16>(plcDevice, plcAddress, wordlen);// Add on 5-31
                //lock (_lockObj) _data.ReadData();
                _data.ReadData();
                for (int i = 0; i < wordlen; i++)
                {
                    value[i] = _data[i];
                }
                result = true;
            }
            catch (Exception ex)
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Error, $"{nameof(McControlModule)} - {nameof(readDataFromPLCLogic)}<{typeof(List<Int16>)}> Exception : {ex.Message}");
            }
            finally
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(McControlModule)} - {nameof(readDataFromPLCLogic)}<{typeof(List<Int16>)}> : End. Result is {result}, Value is {value}");
            }
            return result;
        }
        protected override bool readDataFromPLCLogic(string device, string addr, int wordlen, out string value)
        {
            bool result = false;
            value = null;
            _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(McControlModule)} - {nameof(readDataFromPLCLogic)}<{typeof(string)}> : Start. Args : {device}, {addr}, {wordlen}");
            try
            {
                if (wordlen <= 0) return false;
                Mitsubishi.PlcDeviceType plcDevice = GetPlcDeviceType(device);
                int plcAddress = GetAddress(plcDevice, addr);
                PLCData<Int16> _data = new PLCData<Int16>(plcDevice, plcAddress, wordlen);// Add on 5-31
                //lock (_lockObj) _data.ReadData();
                _data.ReadData();
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
                result = true;
            }
            catch (Exception ex)
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Error, $"{nameof(McControlModule)} - {nameof(readDataFromPLCLogic)}<{typeof(string)}> Exception : {ex.Message}");
            }
            finally
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(McControlModule)} - {nameof(readDataFromPLCLogic)}<{typeof(string)}> : End. Result is {result}, Value is {value}");
            }
            return result;
        }
        protected override bool readDataFromPLCLogic(List<string> device, List<string> addr, out List<short> value)
        {
            bool result = false;
            value = null;
            _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(McControlModule)} - {nameof(readDataFromPLCLogic)}<{typeof(List<Int16>)}> : Start. Args : {device}, {addr}");
            try
            {
                if (device.Count <= 0 || addr.Count <= 0) return false;
                if (device.Count != addr.Count) return false;
                value = new List<Int16>(device.Count);
                for (int i = 0; i < device.Count; i++)
                {
                    Int16 _value = 0;
                    if (!readDataFromPLCLogic(device[i], addr[i], out _value)) return false;
                    value.Add(_value);
                }
                result = true;
            }
            catch (Exception ex)
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Error, $"{nameof(McControlModule)} - {nameof(readDataFromPLCLogic)}<{typeof(List<Int16>)}> Exception : {ex.Message}");
            }
            finally
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(McControlModule)} - {nameof(readDataFromPLCLogic)}<{typeof(List<Int16>)}> : End. Result is {result}, Value is {value}");
            }
            return result;
        }
        protected override bool primaryHandshakeLogic(string Pdevice, string Paddr, string Sdevice, string Saddr, double Sec)
        {
            bool result = false;
            _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(McControlModule)} - {nameof(primaryHandshakeLogic)} : Start. Args : {Pdevice}, {Paddr}, {Sdevice}, {Saddr}, {Sec}");
            try
            {
                if (!WriteDataToPLC(Pdevice, Paddr, (Int16)1)) throw new Exception("Write data to PLC failed in First Step.");
                if (!MonitorData(Sdevice, Saddr, TimeSpan.FromSeconds(Sec))) throw new Exception("Monitor data from PLC failed didn't set on");
                if (!WriteDataToPLC(Pdevice, Paddr, (Int16)0)) throw new Exception("Write data to PLC failed in Second Step.");
                result = true;
            }
            catch (Exception ex)
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Error, $"{nameof(McControlModule)} - {nameof(primaryHandshakeLogic)} Exception : {ex.Message}");
            }
            finally
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(McControlModule)} - {nameof(primaryHandshakeLogic)} : End. Result is  {result}");
            }
            return result;
        }
        protected override bool secondaryHandshakeLogic(string Pdevice, string Paddr, string Sdevice, string Saddr, double Sec)
        {
            bool result = false;
            _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(McControlModule)} - {nameof(secondaryHandshakeLogic)} : Start. Args : {Pdevice}, {Paddr}, {Sdevice}, {Saddr}, {Sec}");
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
                _logManager.Trace(Serilog.Events.LogEventLevel.Error, $"{nameof(McControlModule)} - {nameof(secondaryHandshakeLogic)} Exception : {ex.Message}");
            }
            finally
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(McControlModule)} - {nameof(secondaryHandshakeLogic)} : End. Result is  {result}");
            }
            return result;
        }
        protected override bool monitorDataLogic(string device, string addr, TimeSpan interval, bool rise = true)
        {
            bool result = false;
            _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(McControlModule)} - {nameof(monitorDataLogic)} : Start. Args : {device}, {addr}, {interval}, {rise}");
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
                _logManager.Trace(Serilog.Events.LogEventLevel.Error, $"{nameof(McControlModule)} - {nameof(monitorDataLogic)} Exception : {ex.Message}");
            }
            finally
            {
                _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"{nameof(McControlModule)} - {nameof(monitorDataLogic)} : End. Result is  {result}");
            }
            return result;
        }
        private Mitsubishi.PlcDeviceType GetPlcDeviceType(string Device)
        {
            Mitsubishi.PlcDeviceType _deveice = Mitsubishi.PlcDeviceType.Max;
            try
            {
                if (Enum.IsDefined(typeof(Mitsubishi.PlcDeviceType), Device))
                {
                    _deveice = (Mitsubishi.PlcDeviceType)Enum.Parse(typeof(Mitsubishi.PlcDeviceType), Device);
                }
                else
                {
                    _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"Device type is not correct.{Device}");
                }
            }
            catch (Exception)
            {

                _logManager.Trace(Serilog.Events.LogEventLevel.Information, $"Device type is not correct.{Device}");
            }
            return _deveice;
        }
        private int GetAddress(Mitsubishi.PlcDeviceType plcDevice, string plcAddress)
        {
            try
            {
                if (plcDevice == Mitsubishi.PlcDeviceType.ZR) return int.Parse(plcAddress);
            }
            catch (Exception)
            {

                throw;
            }
            return _MitUtility.HexStringToInt(plcAddress);
        }
    }
}
