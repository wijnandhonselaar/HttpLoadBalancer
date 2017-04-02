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
using System.Threading.Tasks;
using HttpLoadBalancer.Interfaces;
using HttpLoadBalancer.Model;
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
        }
        
        private void InitGuiData()
        {
            var methods = _methods.Select(x => x.Name).ToList();
            _gui.BalanceMethod.Items.AddRange(methods.Cast<object>().ToArray());
            _gui.BalanceMethod.SelectedIndex = 0;


            _healthMonitors = new Dictionary<string, string>();
            var type = typeof(IHealthMonitor);
            var monitorTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p));
            foreach (var monitor in monitorTypes)
            {
                if (monitor.Name != "IHealthMonitor")
                    _healthMonitors.Add(monitor.Name, monitor.FullName);
                    
            }
            _gui.HealthMonitors.Items.AddRange(_healthMonitors.Select(monitor => monitor.Key).ToArray());
            SetHealthMonitor((string)_gui.HealthMonitors.Items[0]);
            foreach (var server in _connectionService.GetDefaultServers())
            {
                AddServer(server.Address, server.Port);
            }

        }

        public void SetHealthMonitor(string name)
        {
            var type = Type.GetType(_healthMonitors[name]);
            if (type != null)
                MethodService.Monitor = Activator.CreateInstance(type) as IHealthMonitor;
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
            _gui.lstServers.Items.Add($"{address}:{port}");
            _gui.lstServers.BackColor = Color.Green;
        }

        public async Task Listener()
        {
            _listener.Start();
            _listening = true;
            try
            {
                while (_listening)
                {
                    var client = await _listener.AcceptTcpClientAsync();
                    _connectionService.HandleRequest(client);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw (e);
            }
        }

        public void RemoveServer(string item)
        {

            var server = _connectionService.Servers.FirstOrDefault(x => x.Address == item.Split(':')[0] && x.Port == int.Parse(item.Split(':')[1]));
            if (server == null) return;
            _gui.lstServers.Items.Remove($"{server.Address}:{server.Port}");
            _connectionService.RemoveServer(server);
            _gui.lstServers.BackColor = Color.Red;
        }
    }
}