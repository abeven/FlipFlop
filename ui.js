//source: http://stackoverflow.com/a/25552273/139046
ko.bindingHandlers.bootstrapSwitchOn = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        $elem = $(element);
        $elem.bootstrapSwitch();
        $elem.bootstrapSwitch('state', ko.utils.unwrapObservable(valueAccessor())); // Set intial state
        $elem.on('switchChange.bootstrapSwitch', function (event, state) {
            valueAccessor()(state);
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

$(document).ready(function () {
    var vm = {
        allItems: ko.observableArray(),
        itemToAdd: {name : ko.observable(""), on: ko.observable(true)}
    };

    vm.updateData = function(arg){
        console.log(arg.name());
    }

    vm.addItem = function () {
        var name = vm.itemToAdd.name(); 
        if ((name != "") && (vm.allItems.indexOf(name) < 0))
            vm.allItems.push({name : ko.observable(name), on: ko.observable(false)});
        
        vm.itemToAdd.name("");
    };

    vm.allItems.push({name : ko.observable("Allow users to register"), on: ko.observable(true)});
    vm.allItems.push({name : ko.observable("Collect analytics"), on: ko.observable(true)});
    vm.allItems.push({name : ko.observable("Deliver notifications"), on: ko.observable(true)});

    ko.applyBindings(vm);
});