using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skyscraper
{
    class MouseConstraint
    {
        [JsonIgnore]
        public string LastUpdatedBy { get; set; }
        [JsonProperty("constraint")]
        public Constraint Constraint { get; set; }

        public Body DragBody { get; set; }
    }
}
