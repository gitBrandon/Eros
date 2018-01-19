function Index() {

    var _vm;
    var _serviceEndPoint;
    var _includeTableAsID;
    var _configHelper = new ConfigHelper();
    // Component
    var _mainGen = new GenMain();
    var _serviceGen = new ServiceGen();
    // Common
    var _serviceCallerGen = new GenServiceCaller();

    var _fw = new FileWriter();

    var fs = require('fs');

    var _filePath;

    function Initialise() {
        _vm = new MainViewModel();
        ko.applyBindings(_vm);
        GetItems();
        GetServiceEndpoint();
        GetFilePath();
        GetIncludeTableAsID();
    }

    function MainViewModel() {
        var self = this;
        self.Items = ko.observableArray([]);
        self.Generate = function() {
            $(self.Items()).each(function(idx, el) {
                if (el.IsChecked()) {
                    Call(el.Name, function(response) {
                        if (!fs.existsSync(_filePath + "/" + el.Name)) {
                            fs.mkdirSync(_filePath + "/" + el.Name);
                        }
                        if (!fs.existsSync(_filePath + "/" + el.Name + "/js")) {
                            fs.mkdirSync(_filePath + "/" + el.Name + "/js");
                        }
                        GenViewModel(el, response);
                        _mainGen.Gen(el, response);
                        _serviceGen.Gen(el, response);
                    });
                }
            })
            _serviceCallerGen.Gen();
        }

        self.Load = function() {
            $(".container").load("app/TableDetail/TableDetail.html")
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

    function GetIncludeTableAsID() {
        _configHelper.GetIncludeTableAsID(function(response) {
            _includeTableAsID = response;
        })
    }

    function GetFilePath() {
        _configHelper.GetFilePath(function(response) {
            _filePath = response;
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

        strVMData += "function " + el.Name + "ViewModel(data, fn_modify, fn_delete) {";
        strVMData += "    var self = this;";
        strVMData += "    if(data === undefined || data === null) {"
        for (var prop in item) {
            if (_includeTableAsID) {
                if (prop == el.Name + "ID") {
                    strVMData += "        self." + prop + " = ko.observable(0);";
                } else {
                    strVMData += "        self." + prop + " = ko.observable();";
                }
            } else {
                if (prop == "ID") {
                    strVMData += "        self." + prop + " = ko.observable(0);";
                } else {
                    strVMData += "        self." + prop + " = ko.observable();";
                }
            }
        }
        strVMData += "    } "
        strVMData += "    else {"
        for (var prop in item) {

            if (_includeTableAsID) {
                if (prop == el.Name + "ID") {
                    strVMData += "        if(data." + prop + " === undefined || data." + prop + " === null) {";
                    strVMData += "            self." + prop + " = ko.observable(0);";
                    strVMData += "        } else {";
                    strVMData += "            self." + prop + " = ko.observable(data." + prop + ");";
                    strVMData += "        }"
                } else {
                    strVMData += "        self." + prop + " = ko.observable(data." + prop + ");";
                }
            } else {
                if (prop == "ID") {
                    strVMData += "        if(data." + prop + " === undefined || data." + prop + " === null) {";
                    strVMData += "            self." + prop + " = ko.observable(0);";
                    strVMData += "        } else {";
                    strVMData += "            self." + prop + " = ko.observable(data." + prop + ");";
                    strVMData += "        }"
                } else {
                    strVMData += "        self." + prop + " = ko.observable(data." + prop + ");";
                }
            }
        }
        strVMData += "    }"

        strVMData += "    self.Modify = function() {"
        strVMData += "        if(fn_modify !== undefined)"
        strVMData += "          fn_modify(self);"
        strVMData += "    };"
        strVMData += "    self.Delete = function() {"
        strVMData += "        if(fn_delete !== undefined)"
        strVMData += "          fn_delete(self);"
        strVMData += "    };"
        strVMData += "};"

        _fw.Write(el.Name + "/js", "" + el.Name + "ViewModel", strVMData);
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