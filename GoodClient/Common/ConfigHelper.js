function ConfigHelper() {
    var self = this;

    self.GetItems = function(callback) {
        var items;

        Get(function(response) {
            items = JSON.parse(response).Items;
            callback(items);
        });
    }

    self.GetServiceEndPoint = function(callback) {
        var ep;

        Get(function(response) {
            ep = JSON.parse(response).ServiceEndpoint;
            callback(ep);
        });
    }

    self.GetFilePath = function(callback) {
        var fp;

        Get(function(response) {
            fp = JSON.parse(response).FilePath;
            callback(fp);
        });
    }

    self.GetIncludeTableAsID = function(callback) {
        var itaid;

        Get(function(response) {
            itaid = JSON.parse(response).IncludeTableAsID;
            callback(itaid);
        });
    }

    function Get(callback) {
        $.ajax({
            url: "config.json",
            success: function(response) {
                callback(response);
            }
        })
    }
}