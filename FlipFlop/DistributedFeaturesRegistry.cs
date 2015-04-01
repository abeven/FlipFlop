using System;
using System.Collections.Generic;
using System.Linq;
using NanoCluster.Pipeline;

namespace FlipFlop
{
    public class DistributedFeaturesRegistry : DistributedTransactionLog
    {
        private readonly Dictionary<string, FeatureState> _features = new Dictionary<string, FeatureState>();

        public void Handle(AddFeatureCommand cmd)
        {
            if (_features.Any(@switch => cmd.Name == @switch.Value.Name))
                return;

            Apply(new FeatureAdded()
            {
                Id = cmd.Id,  
                Name = cmd.Name,
                Enabled = cmd.Enabled
            });
        }

        public void Handle(FlipFeatureCommand cmd)
        {
            if (_features.Any(@switch => cmd.Id == @switch.Key) == false)
                return;

            Apply(new FeatureFlipped() { Id = cmd.Id });
        }

        public void When(FeatureAdded addedEvt)
        {
            _features.Add(addedEvt.Id, new FeatureState()
            {
                Enabled = addedEvt.Enabled,
                Name = addedEvt.Name
            });
        }

        public void When(FeatureFlipped flippedEvt)
        {
            _features[flippedEvt.Id].Enabled = !_features[flippedEvt.Id].Enabled;
        }

        public bool Get(string key)
        {
            return _features.Single(@switch => key == @switch.Value.Name).Value.Enabled;
        }

        public IEnumerable<Feature> All()
        {
            return _features.Select(@switch => new Feature()
            {
                Id = @switch.Key,
                Enabled = @switch.Value.Enabled,
                Name = @switch.Value.Name
            });
        }
    }

    public class FeatureState
    {
        public string Name { get; set; }
        public bool Enabled { get; set; }
    }

    [Serializable]
    public class AddFeatureCommand
    {
        public string Name { get; set; }
        public bool Enabled { get; set; }
        public string Id { get; set; }
    }

    [Serializable]
    public class FlipFeatureCommand
    {
        public string Id { get; set; }
    }

    [Serializable]
    public class FeatureFlipped
    {
        public string Id { get; set; }
    }

    [Serializable]
    public class FeatureAdded
    {
        public string Name { get; set; }
        public bool Enabled { get; set; }
        public string Id { get; set; }
    }
}