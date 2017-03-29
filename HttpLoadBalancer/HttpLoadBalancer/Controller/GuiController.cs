using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using HttpLoadBalancer.Model;
using HttpLoadBalancer.Models;
using HttpLoadBalancer.Service;
using HttpLoadBalancer.View;

namespace HttpLoadBalancer.Controller
{
    public class GuiController
    {
        private readonly ConcurrentBag<Method> _methods;
        private readonly Gui _gui;
        /// <summary>
        /// Constructor GuiController
        /// </summary>
        /// <param name="gui"></param>
        public GuiController(Gui gui)
        {
            _gui = gui;
            _methods = new ConcurrentBag<Method>(MethodService.Methods);

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
            return true;
        }

        public bool StopServer()
        {
            return false;
        }

        public void SelectMethod(string name)
        {
            throw new System.NotImplementedException();
        }
    }
}