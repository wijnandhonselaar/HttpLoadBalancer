using System.Windows.Forms;
using HttpLoadBalancer.Controller;
using HttpLoadBalancer.Models;

namespace HttpLoadBalancer.View
{
    public partial class Gui : Form
    {
        private readonly GuiController controller;
        private bool _running = false;
        public Gui()
        {
            InitializeComponent();
            controller = new GuiController(this);
        }

        private void btnToggleLoadBalancer_Click(object sender, System.EventArgs e)
        {
            _running = _running ? controller.StopServer() : controller.StartServer();
            _running = !_running;
            btnToggleLoadBalancer.Text = _running ? "Stop" : "Start";
        }

        private void btnAddServer_Click(object sender, System.EventArgs e)
        {
            if(txtServerAdrress.Text != null && numServerPort.Value != -1)
                controller.AddServer(txtServerAdrress.Text, (int) numServerPort.Value);
        }

        private void btnRemoveServer_Click(object sender, System.EventArgs e)
        {
            controller.RemoveServer((Server)lstServers.SelectedItem);
        }
    }
}
