using System;
using System.Collections.Generic;
using System.Text;

namespace DESPortal.Widgets.Core.Models
{
    public class Widget
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string ContentServiceId { get; set; }
        
        public int Height { get; set; }

        public int Width { get; set; }

        public int Order { get; set; }

        public int DashboardId { get; set; }
        public Dashboard Dashboard { get; set; }
        
    }
}
