ko.bindingHandlers.bootstrapSwitchOn = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        $elem = $(element);
        $elem.bootstrapSwitch();
        $elem.bootstrapSwitch('state', ko.utils.unwrapObservable(valueAccessor().state())); // Set intial state
        $elem.on('switchChange.bootstrapSwitch', function (e, data) {
            var context = ko.contextFor(this);
            context.$root[valueAccessor().fnctn](context.$data);
        });
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