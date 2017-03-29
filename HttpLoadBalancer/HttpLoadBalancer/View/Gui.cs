using System.Windows.Forms;
using HttpLoadBalancer.Controller;

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
        }

        private void btnAddServer_Click(object sender, System.EventArgs e)
        {
            
        }

        private void btnRemoveServer_Click(object sender, System.EventArgs e)
        {

        }
    }
}
