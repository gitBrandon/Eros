function GenServiceCaller() {
    var self = this;
    var _fw = new FileWriter();
    var _serviceEndPoint;
    var _configHelper = new ConfigHelper();
    var fs = require('fs');
    var _filePath;

    self.Gen = function(el) {
        var strServiceCallerData = `
			function ServiceCaller() {
			    var self = this;
			    var endpoint = "";

			    self.Call = function(operation, request, callback) {
                    $.ajax({
                        url: endpoint + operation,
                        type: "POST",
                        dataType: "JSON",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(request),
                        success: function(response) {
                            if (response != null) {
                                if (response.Success) {
                                    callback(response);
                                } else {
                                    alert("Error:" + response.ErrorHint);
                                }
                            } else {
                                alert("Response is null. " + operation);
                            }
                        },
                        error: function(xhr, textStatus, errorThrown) {
                            alert("Error: " + textStatus);
                        }
                    });
			    };
			};
		`;
        if (!fs.existsSync(_filePath + "/Common")) {
            fs.mkdirSync(_filePath + "/Common");
        }
        if (!fs.existsSync(_filePath + "/Common/Service")) {
            fs.mkdirSync(_filePath + "/Common/Service");
        }
        _fw.Write("Common/Service", "ServiceCaller", strServiceCallerData);
    }

    function GetServiceEndpoint() {
        _configHelper.GetServiceEndPoint(function(response) {
            _serviceEndPoint = response;
        })
    }

    function GetFilePath() {
        _configHelper.GetFilePath(function(response) {
            _filePath = response;
        })
    }

    GetFilePath();
    GetServiceEndpoint();
}