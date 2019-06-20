using System;

namespace Events.Args
{
    public class IdArgs : EventArgs
    {
        public int Id { get; set; }
        
        public int TriggeredUnitId { get; set; }
    }
}