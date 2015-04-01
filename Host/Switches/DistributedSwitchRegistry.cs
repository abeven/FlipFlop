using System;
using System.Collections.Generic;
using System.Linq;
using Host.Web;
using NanoCluster.Pipeline;

namespace Host.Switches
{
    public class DistributedSwitchRegistry : DistributedTransactionLog
    {
        private readonly Dictionary<string, SwitchState> _switches = new Dictionary<string, SwitchState>();

        public void Handle(AddSwithCommand cmd)
        {
            if (_switches.Any(@switch => cmd.Name == @switch.Value.Name))
                return;

            Apply(new SwithAdded()
            {
                Id = cmd.Id,  
                Name = cmd.Name,
                State = cmd.State
            });
        }

        public void Handle(FlipSwitchCommand cmd)
        {
            if (_switches.Any(@switch => cmd.Id == @switch.Key) == false)
                return;

            Apply(new SwitchFlipped() { Id = cmd.Id });
        }

        public void When(SwithAdded addedEvt)
        {
            _switches.Add(addedEvt.Id, new SwitchState()
            {
                State = addedEvt.State,
                Name = addedEvt.Name
            });
        }

        public void When(SwitchFlipped flippedEvt)
        {
            _switches[flippedEvt.Id].State = !_switches[flippedEvt.Id].State;
        }

        public bool Get(string key)
        {
            return _switches[key].State;
        }

        public IEnumerable<FeatureResource> All()
        {
            return _switches.Select(@switch => new FeatureResource()
            {
                Id = @switch.Key,
                On = @switch.Value.State,
                Name = @switch.Value.Name
            });
        }
    }

    public class SwitchState
    {
        public string Name { get; set; }
        public bool State { get; set; }
    }

    [Serializable]
    public class AddSwithCommand
    {
        public string Name { get; set; }
        public bool State { get; set; }
        public string Id { get; set; }
    }

    [Serializable]
    public class FlipSwitchCommand
    {
        public string Id { get; set; }
    }

    [Serializable]
    public class SwitchFlipped
    {
        public string Id { get; set; }
    }

    [Serializable]
    public class SwithAdded
    {
        public string Name { get; set; }
        public bool State { get; set; }
        public string Id { get; set; }
    }
}