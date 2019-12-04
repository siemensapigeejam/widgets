using System;
using System.Collections.Generic;
using System.Text;

namespace DESPortal.Widgets.Core.Models
{
    public class Dashboard
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string MenuItemType { get; set; }

        public string UserId { get; set; }

        public string UserRole { get; set; }

        public bool IsDefaultDashboard { get; set; }

        public List<Widget> Widgets { get; set; }

    }
}
