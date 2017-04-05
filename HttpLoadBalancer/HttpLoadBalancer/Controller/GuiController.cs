using System;
using System.CodeDom;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HttpLoadBalancer.Interfaces;
using HttpLoadBalancer.Models;
using HttpLoadBalancer.Service;
using HttpLoadBalancer.View;

namespace HttpLoadBalancer.Controller
{
    public class GuiController
    {
        public Server SelectedServer;

        private readonly ConcurrentBag<Method> _methods;
        private readonly ConnectionService _connectionService;
        private readonly Gui _gui;
        private TcpListener _listener;
        private bool _listening = false;
        private Dictionary<string, string> _healthMonitors;
        private Dictionary<string, string> _persistenceMethods;

        private delegate void SetItemColorHandler(string key, Color color);
        private delegate void AddServerHandler(ListViewItem server);
        /// <summary>
        /// Constructor GuiController
        /// </summary>
        /// <param name="gui"></param>
        public GuiController(Gui gui)
        {
            _gui = gui;
            _methods = new ConcurrentBag<Method>(MethodService.Methods);
            _connectionService = new ConnectionService();
            SelectedServer = _connectionService.SelectedServer;
            // initiate setting up all the data in the interface
            InitGuiData();

            Task.Run(StartPollingServer);
        }

        private Task StartPollingServer()
        {
            Thread.Sleep(2000);
            while (true)
            {
                foreach (var server in SessionService.Servers)
                {
                    server.UpdateStatus();
                    if (server.Status == Status.Online)
                    {
                        SetItemColor($"{server.Address}:{server.Port}", Color.LightGreen);
                    }
                    else
                    {
                        SetItemColor($"{server.Address}:{server.Port}", Color.LightCoral);
                    }
                }
                // only do this every 5 seconds
                Thread.Sleep(5000);
            }
        }

        private void SetItemColor(string key, Color color)
        {
            if (_gui.InvokeRequired)
            {
                var d = new SetItemColorHandler(SetItemColor);
                _gui.Invoke(d, key, color);
            }
            else
            {
                var i = _gui.lstServersView.Items.IndexOfKey(key);
                if (i == -1) return;
                var item = _gui.lstServersView.Items[i];
                item.BackColor = color;
            }
        }

        private void AddServer(ListViewItem server)
        {
            if (_gui.InvokeRequired)
            {
                var d = new AddServerHandler(AddServer);
                _gui.Invoke(d, server);
            }
            else
            {
                _gui.lstServersView.Items.Add(server);
            }
        }

        /// <summary>
        /// Sets all the data for the GUI
        /// </summary>
        private void InitGuiData()
        {
            _gui.lstServersView.View = System.Windows.Forms.View.Details;
            var methods = _methods.Select(x => x.Name).ToList();
            _gui.BalanceMethod.Items.AddRange(methods.Cast<object>().ToArray());

            SetHealthMonitorOptions();

            SetPersistenceMethods();
            
            foreach (var server in _connectionService.GetDefaultServers())
            {
                AddServer(server.Address, server.Port);
            }

        }

        /// <summary>
        /// Adds all instances of class that are derived from the IPersistenceMethod interface to a list
        /// </summary>
        private void SetPersistenceMethods()
        {
            // Set Persistence Methods
            _persistenceMethods = new Dictionary<string, string>();
            var pType = typeof(IPersistenceMethod);
            var methodTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => pType.IsAssignableFrom(p));
            foreach (var method in methodTypes)
            {
                if (method.Name != "IPersistenceMethod")
                    _persistenceMethods.Add(method.Name, method.FullName);
            }
            _gui.PersistenceMethods.Items.AddRange(_persistenceMethods.Select(method => method.Key).ToArray());
        }

        /// <summary>
        /// Adds all instances of class that are derived from the IHealthMonitor interface to a list
        /// </summary>
        private void SetHealthMonitorOptions()
        {
            // Set Health monitors
            _healthMonitors = new Dictionary<string, string>();
            var monitorType = typeof(IHealthMonitor);
            var monitorTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => monitorType.IsAssignableFrom(p));
            foreach (var monitor in monitorTypes)
            {
                if (monitor.Name != "IHealthMonitor")
                    _healthMonitors.Add(monitor.Name, monitor.FullName);
            }
            _gui.HealthMonitors.Items.AddRange(_healthMonitors.Select(monitor => monitor.Key).ToArray());
        }

        public void SetHealthMonitor(string name)
        {
            var type = Type.GetType(_healthMonitors[name]);
            if (type != null)
                MethodService.Monitor = Activator.CreateInstance(type) as IHealthMonitor;
        }

        public void SetPersistenceMethod(string name)
        {
            var type = Type.GetType(_persistenceMethods[name]);
            if (type != null)
                SessionService.SessionManager = Activator.CreateInstance(type) as IPersistenceMethod;
        }

        public bool StartServer()
        {
            var ip = IPAddress.Parse("127.0.0.1");
            _listener = new TcpListener(ip, (int)_gui.numPort.Value);
            SelectMethod(_gui.BalanceMethod.SelectedItem.ToString());
            Listener();
            return true;
        }

        public bool StopServer()
        {
            _listener.Stop();
            return false;
        }

        public void SelectMethod(string name)
        {
            MethodService.SetMethod(name);
        }

        public void AddServer(string address, int port)
        {
            var server = _connectionService.AddServer(address, port);
            if (server == null) return;
            
            var item = new ListViewItem(server.Address, 0);
            item.SubItems.Add(server.Port.ToString());
            item.Name = $"{address}:{port}";
            AddServer(item);
        }

        public async Task Listener()
        {
            _listener.Start();
            _listening = true;
            while (_listening)
            {
                try
                {
                    var client = await _listener.AcceptTcpClientAsync();
                    _connectionService.HandleRequest(client);
                }
                catch (ObjectDisposedException e)
                {
                    Console.WriteLine("Server was stopped.");
                }
            }
        }

        public void RemoveServer(string item)
        {
            var address = item.Split(':')[0];
            var port = int.Parse(item.Split(':')[1]);
            _gui.lstServersView.Items.RemoveByKey($"{address}:{port}");
            _connectionService.RemoveServer(address, port);
        }

        public void SetSessionState(bool @checked)
        {
            _connectionService.SessionsEnabled = @checked;
        }
    }
}