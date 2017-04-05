using System.Threading.Tasks;
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
            BalanceMethod.SelectedIndex = 0;
            HealthMonitors.SelectedIndex = 0;
            PersistenceMethods.SelectedIndex = 0;
        }

        private void btnToggleLoadBalancer_Click(object sender, System.EventArgs e)
        {
            _running = _running ? _controller.StopServer() : _controller.StartServer();
            btnToggleLoadBalancer.Text = _running ? "Stop" : "Start";
        }

        private void btnAddServer_Click(object sender, System.EventArgs e)
        {
            if(txtServerAdrress.Text != null && numServerPort.Value != -1)
                Task.Run(() =>_controller.AddServer(txtServerAdrress.Text, (int) numServerPort.Value));
        }

        private void btnRemoveServer_Click(object sender, System.EventArgs e)
        {
            foreach (ListViewItem item in lstServersView.SelectedItems)
            {
                _controller.RemoveServer(item.Name);
            }
        }

        private void HealthMonitor_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            _controller.SetHealthMonitor((string)HealthMonitors.SelectedItem);
        }

        private void comboBox1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            _controller.SetPersistenceMethod((string) PersistenceMethods.SelectedItem);
        }

        private void cbEnablePersistence_CheckedChanged(object sender, System.EventArgs e)
        {
            _controller.SetSessionState(cbEnablePersistence.Checked);
        }
    }
}
