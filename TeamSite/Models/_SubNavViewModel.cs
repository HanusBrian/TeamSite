using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamSite.Areas.Admin.Model
{
    using global::TeamSite.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    namespace TeamSite.Areas.Admin.Model
    {
        public class _SubNavViewModel
        {
            public string Title { get; internal set; }
            public IEnumerable<string> Tabs { get; set; }

            public _SubNavViewModel()
            {
                List<Tab> Tabs = new List<Tab>
                {
                    new Tab
                    {
                        Name = "Upload",
                        Area = "Admin",
                        Controller = "User",
                        Action = "Upload"
                    },
                    new Tab
                    {
                        Name = "Team Alignment",
                        Area = "Admin",
                        Controller = "User",
                        Action = "TeamAlignment"
                    }
                };

            }
        }
    }
}
