function TableDetailServiceCaller() {
    var self = this;
    var _serviceCaller = new ServiceCaller();
    self.GetAll = function(callback) {
        var request = { "Action": 0 };
        Call(request, callback);
    };
    self.GetSingle = function(id, callback) {
        var request = { "Action": 1, "CustomField": "ID", "CustomValue": id };
        Call(request, callback);
    };
    self.Create = function(request, callback) {
        var request = { "Action": 2, TableDetailItem: JSON.parse(ko.toJSON(request)) };
        Call(request, callback);
    };
    self.Modify = function(request, callback) {
        var request = { "Action": 4, TableDetailItem: JSON.parse(ko.toJSON(request)) };
        Call(request, callback);
    };
    self.Delete = function(id, callback) {
        var request = { "Action": 6, "CustomField": "ID", "CustomValue": id };
        Call(request, callback);
    };
    self.FlagDelete = function(id, callback) {
        var request = { "Action": 7, "CustomField": "ID", "CustomValue": id };
        Call(request, callback);
    };

    function Call(request, callback) { _serviceCaller.Call('Process' + 'TableDetail', request, callback); };
};