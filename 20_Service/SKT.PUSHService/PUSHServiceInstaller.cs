using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;

namespace SKT.PUSHService
{
    [RunInstaller(true)]
    public partial class PUSHServiceInstaller : System.Configuration.Install.Installer
    {
        public PUSHServiceInstaller()
        {
            InitializeComponent();
        }
    }
}


