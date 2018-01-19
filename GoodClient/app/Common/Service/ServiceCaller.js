
			function ServiceCaller() {
			    var self = this;
			    var endpoint = "http://192.168.1.79/TestGen/ManagementService";

			    self.Call = function(operation, request, callback) {
                    $.ajax({
                        url: endpoint + "/" + operation,
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
		