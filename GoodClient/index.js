function Index() {

    var _vm;
    var _serviceEndPoint;
    var _configHelper = new ConfigHelper();
    // Component
    var _mainGen = new GenMain();
    var _serviceGen = new ServiceGen();
    // Common
    var _serviceCallerGen = new GenServiceCaller();

    function Initialise() {
        _vm = new MainViewModel();
        ko.applyBindings(_vm);
        GetItems();
        GetServiceEndpoint();
    }

    function MainViewModel() {
        var self = this;
        self.Items = ko.observableArray([]);
        self.Generate = function() {
            $(self.Items()).each(function(idx, el) {
                if (el.IsChecked()) {
                    Call(el.Name, function(response) {
                        GenViewModel(el, response);
                        _mainGen.Gen(el, response);
                        _serviceGen.Gen(el, response);
                    });
                }
            })

            _serviceCallerGen.Gen(el, response);
        }
    }

    function GetItems() {
        _configHelper.GetItems(function(response) {
            $(response).each(function(idx, name) {
                _vm.Items.push(new Selector(name));
            })
        })
    }

    function Selector(name) {
        var self = this;
        self.Name = name;
        self.IsChecked = ko.observable(true);
    }

    function GetServiceEndpoint() {
        _configHelper.GetServiceEndPoint(function(response) {
            _serviceEndPoint = response;
        })
    }

    function Call(operation, callback) {
        var request = {
            "Action": 99
        };

        $.ajax({
            url: _serviceEndPoint + "/Process" + operation,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "JSON",
            data: JSON.stringify(request),
            success: function(response) {
                callback(response);
            }
        })
    }

    function GenViewModel(el, response) {
        var item = response[el.Name + "List"][0];
        var strVMData = "";

        strVMData += "function " + el.Name + "ViewModel() {";
        strVMData += "    var self = this";
        for (var prop in item) {
            strVMData += "    self." + prop + " = ko.observable()";
        }
        strVMData += "}"
    }

    function GenMainJS(el, response) {
        var item = response[el.Name + "List"][0];
        var strMainData = "";
    }

    return {
        init: function() {
            Initialise();
        }
    }
}

$(document).ready(function() {
    var idxObj = new Index();
    idxObj.init();
})