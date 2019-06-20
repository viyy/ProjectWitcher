using System;
using System.Collections.Generic;

namespace Events.Args
{
    public class NpcDieArgs : EventArgs
    {
        public int Id { get; set; }
    }
}