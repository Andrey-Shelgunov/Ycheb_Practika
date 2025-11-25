using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Library;
using System.Linq;
using System.Drawing;

namespace IReturnService
{
    public partial class Return : Form
    {

        public Return()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            throw new NotImplementedException();
        }

        public interface IReturnService
        {
            Task<ReturnResult> SubmitReturnAsync(ReturnRequest request);
            Task<bool> CancelReturnAsync(string returnId);
        }

    }
}
