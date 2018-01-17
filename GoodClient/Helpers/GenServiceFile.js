function ServiceGen() {
    var self = this;
    var _fw = new FileWriter();

    self.Gen = function(el, response) {
        var item = response[el.Name + "List"][0];
        var strServiceData = "";

        strServiceData += "function " + el.Name + "ServiceCaller() {"

        strServiceData += "    var self = this;";

        // Get All

        strServiceData += "    self.GetAll(callback) {"
        strServiceData += "         var request = {"
        strServiceData += '             "Action": 0,';
        strServiceData += "         }"
        strServiceData += "         Call(request, callback)";
        strServiceData += "    }"

        // Get Single

        strServiceData += "    self.GetSingle(id, callback) {"
        strServiceData += "         var request = {"
        strServiceData += '             "Action": 1,';
        strServiceData += '             "CustomField": "ID"';
        strServiceData += '             "CustomField": id';
        strServiceData += "         }"
        strServiceData += "         Call(request, callback)";
        strServiceData += "    }"

        // Create

        strServiceData += "    self.Create(request, callback) {"
        strServiceData += "         var request = {"
        strServiceData += '             "Action": 2,';
        strServiceData += '             ' + el.Name + 'Item : JSON.parse(ko.toJSON(request))';
        strServiceData += "         }"
        strServiceData += "         Call(request, callback)";
        strServiceData += "    }"

        // Modify

        strServiceData += "    self.Modify(request, callback) {"
        strServiceData += "         var request = {"
        strServiceData += '             "Action": 4,';
        strServiceData += '             ' + el.Name + 'Item : JSON.parse(ko.toJSON(request))';
        strServiceData += "         }"
        strServiceData += "         Call(request, callback)";
        strServiceData += "    }"

        // Delete 

        strServiceData += "    self.Delete(id, callback) {"
        strServiceData += "         var request = {"
        strServiceData += '             "Action": 6,';
        strServiceData += '             "CustomField": "ID"';
        strServiceData += '             "CustomField": id';
        strServiceData += "         }"
        strServiceData += "         Call(request, callback)";
        strServiceData += "    }"

        // Flag Delete 

        strServiceData += "    self.FlagDelete(id, callback) {"
        strServiceData += "         var request = {"
        strServiceData += '             "Action": 7,';
        strServiceData += '             "CustomField": "ID"';
        strServiceData += '             "CustomField": id';
        strServiceData += "         }"
        strServiceData += "         Call(request, callback)";
        strServiceData += "    }"

        strServiceData += "    function Call(request, callback) {"
        strServiceData += "        _serviceCaller.Call('Process' + " + el.Name + ", request, callback);";
        strServiceData += "    }"

        strServiceData += "}"

        _fw.Write(el.Name + "/js", "Service.js", strServiceData);
    }
}