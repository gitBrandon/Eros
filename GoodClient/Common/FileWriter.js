function FileWriter() {
    var self = this;
    var _filePath;
    var _configHelper = new ConfigHelper();
    var fs = require('fs');

    self.Write = function(folder, file, data) {
        fs.writeFile(_filePath + folder + "/" + file + ".js", data, function(err) {
            if (err) {
                return console.log(err);
            }

            console.log(file + ".js was saved!");
        });
    }

    function GetFilePath() {
        _configHelper.GetFilePath(function(response) {
            _filePath = response;
        })
    }

    GetFilePath();
}