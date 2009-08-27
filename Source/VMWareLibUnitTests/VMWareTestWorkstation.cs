using System;
using System.Collections.Generic;
using System.Text;
using Vestris.VMWareLib;
using System.Configuration;

namespace Vestris.VMWareLibUnitTests
{
    public class TestWorkstation : IVMWareTestProvider
    {
        private VMWareVirtualHost _host = null;
        private VMWareVirtualMachine _virtualMachine = null;
        private VMWareVirtualMachineConfig _config = null;

        public TestWorkstation(VMWareVirtualMachineConfig config)
        {
            _config = config;
        }

        public VMWareVirtualHost Reconnect()
        {
            _virtualMachine = null;
            _host = null;
            return VirtualHost;
        }

        public VMWareVirtualHost VirtualHost
        {
            get
            {
                if (_host == null)
                {
                    VMWareVirtualHost virtualHost = new VMWareVirtualHost();
                    ConsoleOutput.WriteLine("Connecting to local host ...");
                    // connect to a local VM
                    virtualHost.ConnectToVMWareWorkstation();
                    ConsoleOutput.WriteLine("Connection established.");
                    _host = virtualHost;
                }
                return _host;
            }
        }

        public VMWareVirtualMachine VirtualMachine
        {
            get
            {
                if (_virtualMachine == null)
                {
                    ConsoleOutput.WriteLine("Opening: {0}", _config.File);
                    _virtualMachine = VirtualHost.Open(_config.File);
                }
                return _virtualMachine;
            }
        }

        public VMWareVirtualMachine PoweredVirtualMachine
        {
            get
            {
                if (! VirtualMachine.IsRunning)
                {
                    ConsoleOutput.WriteLine("Powering on: {0}", _config.File);
                    VirtualMachine.PowerOn();
                    ConsoleOutput.WriteLine("Waiting for tools ...");
                    VirtualMachine.WaitForToolsInGuest();
                }
                ConsoleOutput.WriteLine("Logging in ...");
                VirtualMachine.LoginInGuest(Username, Password);
                return _virtualMachine;
            }
        }

        public string Username
        {
            get
            {
                return _config.GuestUsername;
            }
        }

        public string Password
        {
            get
            {
                return _config.GuestPassword;
            }
        }
    }
}
