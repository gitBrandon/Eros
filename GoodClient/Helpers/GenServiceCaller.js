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

			    self.call = function(operation, request, callback) {
			        $.ajax({
			            url: endpoint/process,
			            type: "POST",
			            dataType: "application/json utf-8"
			            data: JSON.stringify(request),
			            success: function(response) {
			            	callback(response);
			            };
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