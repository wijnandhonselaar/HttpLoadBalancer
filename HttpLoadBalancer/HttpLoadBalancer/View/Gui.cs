using System.Windows.Forms;
using HttpLoadBalancer.Controller;
using HttpLoadBalancer.Interfaces;
using HttpLoadBalancer.Models;

namespace HttpLoadBalancer.View
{
    public partial class Gui : Form
    {
        private readonly GuiController _controller;
        private bool _running;
        
        public Gui()
        {
            InitializeComponent();
            _controller = new GuiController(this);
        }

        private void btnToggleLoadBalancer_Click(object sender, System.EventArgs e)
        {
            _running = _running ? _controller.StopServer() : _controller.StartServer();
            _running = !_running;
            btnToggleLoadBalancer.Text = _running ? "Stop" : "Start";
        }

        private void btnAddServer_Click(object sender, System.EventArgs e)
        {
            if(txtServerAdrress.Text != null && numServerPort.Value != -1)
                _controller.AddServer(txtServerAdrress.Text, (int) numServerPort.Value);
        }

        private void btnRemoveServer_Click(object sender, System.EventArgs e)
        {
            _controller.RemoveServer((string)lstServers.SelectedItem);
        }

        private void HealthMonitor_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            _controller.SetHealthMonitor((string)HealthMonitors.SelectedItem);
        }
    }
}
