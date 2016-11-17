using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BPS_ADM_ADS_LastLogon
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            getLastLogonUser();
        }

        public void getLastLogonUser()
        { 
            using (var context = new PrincipalContext(ContextType.Domain, "bps.bps-software.de"))
            {
                using (var searcher = new PrincipalSearcher(new ComputerPrincipal(context)))
                {
                    int NumHosts = 0;
                    int CountRows = 0;
                    int CountEmpty = 0;
                    foreach (var result in searcher.FindAll())
                    {
                        var auth = result as AuthenticablePrincipal;
                        if(auth != null)
                        {

                            if (auth.LastLogon != null)
                            {
                                this.dataGridView1.Rows.Add(auth.Name, auth.LastLogon);
                                CountRows++;
                            }
                            else CountEmpty++;
                            NumHosts = dataGridView1.RowCount + CountEmpty;
                            StatusLabel2.Text = "Hosts: " + NumHosts + " | Hosts mit Logon-Datum: " + CountRows + " | Hosts ohne Logon-Datum: " + CountEmpty;
                        }
                    }
                }
            }
            Console.ReadLine();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
