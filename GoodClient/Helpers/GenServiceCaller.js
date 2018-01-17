function GenServiceCaller() {
    var self = this;
    var _fw = new FileWriter();
    var _serviceEndPoint;

    self.Gen = function(el, response) {
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
			            }
			        })
			    }
			};
		`;

        _fw.Write(el.Name + "/js", "" + el.Name + ".js", strMainData);
    }

    function GetServiceEndpoint() {
        _configHelper.GetServiceEndPoint(function(response) {
            _serviceEndPoint = response;
        })
    }

    GetServiceEndpoint();
}