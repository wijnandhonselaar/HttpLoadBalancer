using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using HttpLoadBalancer.Model;
using HttpLoadBalancer.Models;
using HttpLoadBalancer.Service;
using HttpLoadBalancer.View;

namespace HttpLoadBalancer.Controller
{
    public class GuiController
    {
        private readonly ConcurrentBag<Method> _methods;
        private readonly ConnectionService _connectionService;
        private readonly Gui _gui;
        private TcpListener _listener;
        private bool _listening = false;

        /// <summary>
        /// Constructor GuiController
        /// </summary>
        /// <param name="gui"></param>
        public GuiController(Gui gui)
        {
            _gui = gui;
            _methods = new ConcurrentBag<Method>(MethodService.Methods);
            _connectionService = new ConnectionService();
            // initiate setting up all the data in the interface
            InitGuiData();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitGuiData()
        {
            var methods = _methods.Select(x => x.Name).ToList();
            _gui.BalanceMethod.Items.AddRange(methods.Cast<object>().ToArray());
            _gui.BalanceMethod.SelectedIndex = 0;
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
            _gui.lstServers.Items.Add(server);
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
            }
        }

        public void RemoveServer(Server server)
        {
            if(_connectionService.RemoveServer(server))
                _gui.lstServers.Items.Remove(server);
        }
    }
}