using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalSignature
{
    public class OutageDocument
    {
        public string ApplicantName { get; set; }

        public string AreaOfResponsibility { get; set; }

        public string Substation { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }
    }
}
