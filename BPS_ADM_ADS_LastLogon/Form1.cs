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
                foreach (var result in searcher.FindAll())
                {
                    var auth = result as AuthenticablePrincipal;
                    if(auth != null)
                    {
                       dataGridView1.Rows.Add(dataGridView1.Rows.auth.Name
                                auth.LastLogon
                    }
                }
            }
        }
        Console.ReadLine();
        }

        public DateTime findlastlogon(string userName)

        {
            DirectoryContext context = new DirectoryContext(DirectoryContextType.Domain, "domainName");
            DateTime latestLogon = DateTime.MinValue;
            DomainControllerCollection dcc = DomainController.FindAll(context);
            Parallel.ForEach(dcc.Cast<object>(), dc1 =>
            {


                DirectorySearcher ds;
                DomainController dc = (DomainController)dc1;
                using (ds = dc.GetDirectorySearcher())
                {
                    try
                    {
                        ds.Filter = String.Format(
                          "(sAMAccountName={0})",
                          userName
                          );
                        ds.PropertiesToLoad.Add("lastLogon");
                        ds.SizeLimit = 1;
                        SearchResult sr = ds.FindOne();

                        if (sr != null)
                        {
                            DateTime lastLogon = DateTime.MinValue;
                            if (sr.Properties.Contains("lastLogon"))
                            {
                                lastLogon = DateTime.FromFileTime(
                                  (long)sr.Properties["lastLogon"][0]
                                  );
                            }

                            if (DateTime.Compare(lastLogon, latestLogon) > 0)
                            {
                                latestLogon = lastLogon;
                                //servername = dc1.Name;
                            }
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            });
            return latestLogon;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
