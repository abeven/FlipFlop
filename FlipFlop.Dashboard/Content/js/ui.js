//source: http://stackoverflow.com/a/25552273/139046
ko.bindingHandlers.bootstrapSwitchOn = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
        $elem = $(element);
        $elem.bootstrapSwitch();
        $elem.bootstrapSwitch('state', ko.utils.unwrapObservable(valueAccessor())); // Set intial state
        $elem.on('switchChange.bootstrapSwitch', function (event, state) {
            valueAccessor()(state);
            bindingContext.$parent.siwtchFeature(viewModel);
        });
    },
    update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var vStatus = $(element).bootstrapSwitch('state');
        var vmStatus = ko.utils.unwrapObservable(valueAccessor());
        if (vStatus != vmStatus) {
            $(element).bootstrapSwitch('state', vmStatus);
        }
    }
};

function Feature(data) {
    this.name = ko.observable(data.name);
    this.enabled = ko.observable(data.enabled);
    this.id = ko.observable(data.id);
}

function FeaturesViewModel() {
    var t = this;
    t.features = ko.observableArray([]);
    
    $.getJSON("/features", function(raw) {
        var features = $.map(raw, function (item) { return new Feature(item) });
        t.features(features);
    });

    t.newFeatureName = ko.observable("");

    t.addFeature = function () {
        var name = t.newFeatureName();
        if ((name != "") && (t.features.indexOf(name) < 0)) {

            var feature = new Feature({ name: name, enabled: false });

            t.features.push(feature);

            $.ajax({
                url: "/features",
                type: "POST",
                data: ko.toJS(feature)
            }).done(function (data) {
                feature.id(data.id);
            });
        }

        t.newFeatureName("");
    };

    t.siwtchFeature = function (feature) {
        $.ajax({
            url: '/features/' + feature.id(),
            type: 'PUT',
            data: ko.toJS(feature),
            success: function (data) {
                console.log("put completed " + data);
            }
        });
    };
}

$(document).ready(function () {
    ko.applyBindings(new FeaturesViewModel());
});