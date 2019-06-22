using System;

namespace Events.Args
{
    public class TaskUpdatedArgs : EventArgs
    {
        public TaskUpdatedArgs(string description, int currentAmount, int neededAmount)
        {
            Description = description;
            CurrentAmount = currentAmount;
            NeededAmount = neededAmount;
        }

        public string Description { get; }
        public int CurrentAmount { get; }
        public int NeededAmount { get; }
    }
}