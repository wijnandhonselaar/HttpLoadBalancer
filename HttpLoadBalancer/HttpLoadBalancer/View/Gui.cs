using System.Windows.Forms;
using HttpLoadBalancer.Controller;

namespace HttpLoadBalancer.View
{
    public partial class Gui : Form
    {
        private GuiController controller;
        public Gui()
        {
            controller = new GuiController(this);
            InitializeComponent();
        }
    }
}
